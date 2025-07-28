using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace MiProyecto.Models
{
    public class RepositorioTipo : RepositorioBase, IRepositorioTipo
    {    
        public RepositorioTipo(IConfiguration configuration) : base(configuration) { }



        public int Alta(Tipo e)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"INSERT INTO tipo (Descripcion) VALUES (@Descripcion) SELECT LAST_INSERT_ID()";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    
                    command.Parameters.AddWithValue("@Descripcion", e.Descripcion);
                    e.IdTipo = Convert.ToInt32(command.ExecuteScalar());
                    
                     return e.IdTipo;
                }
            }
        }

        public int Baja(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"DELETE FROM tipo WHERE IdTipo = @id";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    return command.ExecuteNonQuery();
                }
            }
        }
        
        public int Modificacion(Tipo e)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"UPDATE tipo SET Descripcion = @Descripcion WHERE IdTipo = @id";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", e.IdTipo);
                    command.Parameters.AddWithValue("@Descripcion", e.Descripcion);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public Tipo ObtenerPorId(int id)
        {
            Tipo? e = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT IdTipo, Descripcion FROM tipo WHERE IdTipo = @id";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            e = new Tipo
                            {
                                IdTipo = reader.GetInt32("IdTipo"),
                                Descripcion = reader.GetString("Descripcion")
                            };
                        }
                    }
                }
            }
            return e;
        }

        public IList<Tipo> ObtenerTodos(int pagina, int tamPagina = 10)
        {
            IList<Tipo> tipos = new List<Tipo>();
            using (var connection = GetConnection())            
            {
                connection.Open();
                var query = @"SELECT IdTipo, Descripcion FROM tipo ORDER BY IdTipo LIMIT @tamPagina OFFSET @offset";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * tamPagina);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipos.Add(new Tipo
                            {
                                IdTipo = reader.GetInt32("IdTipo"),
                                Descripcion = reader.GetString("Descripcion")
                            });
                        }
                    }
                }
                return tipos;
            }            
        }
    }


}