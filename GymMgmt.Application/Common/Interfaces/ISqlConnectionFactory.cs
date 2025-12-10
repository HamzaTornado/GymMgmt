using System.Data;


namespace GymMgmt.Application.Common.Interfaces
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
        IDbConnection CreateNewConnection();
        string GetConnectionString();
    }
}
