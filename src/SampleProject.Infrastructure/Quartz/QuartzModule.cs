namespace SampleProject.Infrastructure.Quartz
{
    using System.Reflection;
    using Autofac;
    using global::Quartz;
    using Module = Autofac.Module;

    public class QuartzModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => typeof(IJob).IsAssignableFrom(x)).InstancePerDependency();
        }
    }
}