using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace MiProyecto.Models
{
    public class RepositorioCategoria : RepositorioBase, IRepositorioCategoria
    {
        public RepositorioCategoria(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Categoria categoria)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"INSERT INTO categoria (Nombre) VALUES (@Nombre) SELECT LAST_INSERT_ID()";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                    categoria.IdCategoria = Convert.ToInt32(command.ExecuteScalar());
                    return categoria.IdCategoria;
                }
            }
        }

        public int Baja(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"DELETE FROM categoria WHERE IdCategoria = @id";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int Modificacion(Categoria categoria)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"UPDATE categoria SET Nombre = @Nombre WHERE IdCategoria = @id";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", categoria.IdCategoria);
                    command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public Categoria ObtenerPorId(int id)
        {
            Categoria? e = null;
           using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT IdCategoria, Nombre FROM categoria WHERE IdCategoria = @id";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            e = new Categoria
                            {
                                IdCategoria = reader.GetInt32("IdCategoria"),
                                Nombre = reader.GetString("Nombre")
                            };
                        }
                    }
                }
                return e;
               
           }
        }

        public IList<Categoria> ObtenerTodos(int pagina, int tamPagina = 10)
        {
            IList<Categoria> categorias = new List<Categoria>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT IdCategoria, Nombre FROM categoria ORDER BY IdCategoria LIMIT @tamPagina OFFSET @offset";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * tamPagina);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categorias.Add(new Categoria
                            {
                                IdCategoria = reader.GetInt32("IdCategoria"),
                                Nombre = reader.GetString("Nombre")
                            });
                        }
                    }
                }
                return categorias;
            }
        }


        
    }
}