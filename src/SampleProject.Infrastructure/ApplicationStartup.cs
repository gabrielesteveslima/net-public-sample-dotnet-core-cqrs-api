namespace SampleProject.Infrastructure
{
    using System;
    using Application.Configuration;
    using Application.Configuration.Emails;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Autofac.Extras.CommonServiceLocator;
    using Caching;
    using CommonServiceLocator;
    using Database;
    using Domain;
    using Emails;
    using global::Quartz;
    using global::Quartz.Impl;
    using Logging;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Microsoft.Extensions.DependencyInjection;
    using Processing;
    using Processing.InternalCommands;
    using Processing.Outbox;
    using Quartz;
    using SeedWork;
    using Serilog;

    public class ApplicationStartup
    {
        public static IServiceProvider Initialize(
            IServiceCollection services,
            string connectionString,
            ICacheStore cacheStore,
            IEmailSender emailSender,
            EmailsSettings emailsSettings,
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor,
            bool runQuartz = true)
        {
            if (runQuartz)
            {
                StartQuartz(connectionString, emailsSettings, logger, executionContextAccessor);
            }

            services.AddSingleton(cacheStore);

            IServiceProvider serviceProvider = CreateAutofacServiceProvider(
                services,
                connectionString,
                emailSender,
                emailsSettings,
                logger,
                executionContextAccessor);

            return serviceProvider;
        }

        private static IServiceProvider CreateAutofacServiceProvider(
            IServiceCollection services,
            string connectionString,
            IEmailSender emailSender,
            EmailsSettings emailsSettings,
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor)
        {
            ContainerBuilder container = new ContainerBuilder();

            container.Populate(services);

            container.RegisterModule(new LoggingModule(logger));
            container.RegisterModule(new DataAccessModule(connectionString));
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new DomainModule());

            if (emailSender != null)
            {
                container.RegisterModule(new EmailModule(emailSender, emailsSettings));
            }
            else
            {
                container.RegisterModule(new EmailModule(emailsSettings));
            }

            container.RegisterModule(new ProcessingModule());

            container.RegisterInstance(executionContextAccessor);

            IContainer buildContainer = container.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(buildContainer));

            AutofacServiceProvider serviceProvider = new AutofacServiceProvider(buildContainer);

            CompositionRoot.SetContainer(buildContainer);

            return serviceProvider;
        }

        private static void StartQuartz(
            string connectionString,
            EmailsSettings emailsSettings,
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor)
        {
            StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            ContainerBuilder container = new ContainerBuilder();

            container.RegisterModule(new LoggingModule(logger));
            container.RegisterModule(new QuartzModule());
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new DataAccessModule(connectionString));
            container.RegisterModule(new EmailModule(emailsSettings));
            container.RegisterModule(new ProcessingModule());

            container.RegisterInstance(executionContextAccessor);
            container.Register(c =>
            {
                DbContextOptionsBuilder<OrdersContext> dbContextOptionsBuilder =
                    new DbContextOptionsBuilder<OrdersContext>();
                dbContextOptionsBuilder.UseSqlServer(connectionString);

                dbContextOptionsBuilder
                    .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                return new OrdersContext(dbContextOptionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();

            scheduler.JobFactory = new JobFactory(container.Build());

            scheduler.Start().GetAwaiter().GetResult();

            IJobDetail processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
            ITrigger trigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * ? * *")
                    .Build();

            scheduler.ScheduleJob(processOutboxJob, trigger).GetAwaiter().GetResult();

            IJobDetail processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();
            ITrigger triggerCommandsProcessing =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * ? * *")
                    .Build();
            scheduler.ScheduleJob(processInternalCommandsJob, triggerCommandsProcessing).GetAwaiter().GetResult();
        }
    }
}