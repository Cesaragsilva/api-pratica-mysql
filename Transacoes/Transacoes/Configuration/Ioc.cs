using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;
using Transacao.API.Repositories.Dapper;

namespace Transacao.API.Configuration
{
    public static class Ioc
    {
        public static void AddInjecaoDependencia(this IServiceCollection service)
        {
            service.AddScoped<IDbConnection>(o=> new MySqlConnection("host=localhost;port=3306;user id=root;password=my-secret-pw;database=teste;"));
            service.AddScoped<IDapperDB, DapperDB>();
        }
    }
}
