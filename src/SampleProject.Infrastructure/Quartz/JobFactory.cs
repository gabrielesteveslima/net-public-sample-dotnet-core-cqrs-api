namespace SampleProject.Infrastructure.Quartz
{
    using Autofac;
    using global::Quartz;
    using global::Quartz.Spi;

    public class JobFactory : IJobFactory
    {
        private readonly IContainer _container;

        public JobFactory(IContainer container)
        {
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            object job = _container.Resolve(bundle.JobDetail.JobType);

            return job as IJob;
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}