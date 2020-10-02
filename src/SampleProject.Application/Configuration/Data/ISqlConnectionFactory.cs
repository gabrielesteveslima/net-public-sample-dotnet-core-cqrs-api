namespace SampleProject.Application.Configuration.Data
{
    using System.Data;

    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}