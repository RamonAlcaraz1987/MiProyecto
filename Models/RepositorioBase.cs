using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

using System.Data;

namespace MiProyecto.Models
{
    public abstract class RepositorioBase
    {
        protected readonly string connectionString;
        
        public RepositorioBase (IConfiguration configuration)
        
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        protected IDbConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}

