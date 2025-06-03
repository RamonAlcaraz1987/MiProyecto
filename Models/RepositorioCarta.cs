using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace MiProyecto.Models
{
    public class RepositorioCarta : RepositorioBase, IRepositorioCarta
    {
        public RepositorioCarta(IConfiguration configuration) : base(configuration)
        {
        }


        public int Alta(Carta e)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"INSERT INTO carta (Nombre, IdTipo1, Imagen, ValorEstimado, IdCategoria, IdTipo2, Estado)
                VALUES (@Nombre, @IdTipo1, @Imagen, @ValorEstimado, @IdCategoria, @IdTipo2, @Estado)
                SELECT LAST_INSERT_ID()";
            

            using (var command = new MySqlCommand(query, (MySqlConnection)connection))
            { 
                command.Parameters.AddWithValue("@Nombre", e.Nombre);
                command.Parameters.AddWithValue("@IdTipo1", e.IdTipo1);
                command.Parameters.AddWithValue("@Imagen", e.Imagen);
                command.Parameters.AddWithValue("@ValorEstimado", e.ValorEstimado);
                command.Parameters.AddWithValue("@IdCategoria", e.IdCategoria);
                command.Parameters.AddWithValue("@IdTipo2", e.IdTipo2);
                command.Parameters.AddWithValue("@Estado", e.Estado);
                e.IdCarta = Convert.ToInt32(command.ExecuteScalar());
                return e.IdCarta;

                
            }
            }


        }

        public int Baja(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"DELETE FROM carta WHERE IdCarta = @id";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int Modificacion (Carta e)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"UPDATE carta SET Nombre = @Nombre, IdTipo1 = @IdTipo1, Imagen = @Imagen, ValorEstimado = @ValorEstimado, IdCategoria = @IdCategoria, IdTipo2 = @IdTipo2, Estado = @Estado WHERE IdCarta = @id";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", e.IdCarta);
                    command.Parameters.AddWithValue("@Nombre", e.Nombre);
                    command.Parameters.AddWithValue("@IdTipo1", e.IdTipo1);
                    command.Parameters.AddWithValue("@Imagen", e.Imagen);
                    command.Parameters.AddWithValue("@ValorEstimado", e.ValorEstimado);
                    command.Parameters.AddWithValue("@IdCategoria", e.IdCategoria);
                    command.Parameters.AddWithValue("@IdTipo2", e.IdTipo2);
                    command.Parameters.AddWithValue("@Estado", e.Estado);
                    return command.ExecuteNonQuery();
                }
            }
        }


        public Carta ObtenerPorId (int id)
        {
            Carta? e = null;

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT IdCarta, Nombre, IdTipo1, Imagen, ValorEstimado, IdCategoria, IdTipo2, Estado, cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion FROM carta 
                INNER JOIN categoria cat ON carta.IdCategoria = cat.IdCategoria
                INNER JOIN tipo t1 ON carta.IdTipo1 = t1.IdTipo
                INNER JOIN tipo t2 ON carta.IdTipo2 = t2.IdTipo
                WHERE IdCarta = @id";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                       
                            e = new Carta();

                        {
                            e.IdCarta = reader.GetInt32("IdCarta");
                            e.Nombre = reader.GetString("Nombre");
                            e.IdTipo1 = reader.GetInt32("IdTipo1");
                            Tipo1 = new Tipo
                            {
                                IdTipo = reader.GetInt32("IdTipo1"),
                                Descripcion = reader.GetString("Tipo1Descripcion")
                            }
                            e.Imagen = reader.GetString("Imagen");
                            e.ValorEstimado = reader.GetInt32("ValorEstimado");
                            e.IdCategoria = reader.GetInt32("IdCategoria");
                            Categoria = new Categoria
                            {
                                IdCategoria = reader.GetInt32("IdCategoria"),
                                Nombre = reader.GetString("CategoriaNombre")
                            }

                            e.IdTipo2 = reader.GetInt32("IdTipo2");

                            Tipo2 = new Tipo
                            {
                                IdTipo = reader.GetInt32("IdTipo2"),
                                Descripcion = reader.GetString("Tipo2Descripcion")
                            }
                            e.Estado = reader.GetString("Estado");
                        }
                    }

                }

            }



            return e;

        }
        
        public IList<Carta> ObtenerTodos(int pagina, int tamPagina = 10)
        {
            IList<Carta> cartas = new List<Carta>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT IdCarta, Nombre, IdTipo1, Imagen, ValorEstimado, IdCategoria, IdTipo2, Estado, 
                cat.Nombre AS CategoriaNombre, 
                t1.Descripcion AS Tipo1Descripcion, 
                t2.Descripcion AS Tipo2Descripcion
                FROM carta
                INNER JOIN categoria cat ON carta.IdCategoria = cat.IdCategoria
                INNER JOIN tipo t1 ON carta.IdTipo1 = t1.IdTipo
                INNER JOIN tipo t2 ON carta.IdTipo2 = t2.IdTipo
                ORDER BY IdCarta
                LIMIT @tamPagina OFFSET @offset";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * tamPagina);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartas.Add(new Carta
                            {
                                IdCarta = reader.GetInt32("IdCarta"),
                                Nombre = reader.GetString("Nombre"),
                                IdTipo1 = reader.GetInt32("IdTipo1"),
                                Tipo1 = new Tipo
                                {
                                    IdTipo = reader.GetInt32("IdTipo1"),
                                    Descripcion = reader.GetString("Tipo1Descripcion")
                                },

                                Imagen = reader.GetString("Imagen"),
                                ValorEstimado = reader.GetInt32("ValorEstimado"),
                                IdCategoria = reader.GetInt32("IdCategoria"),
                                Categoria = new Categoria
                                {
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    Nombre = reader.GetString("CategoriaNombre")
                                },

                                IdTipo2 = reader.GetInt32("IdTipo2"),
                                Tipo2 = new Tipo
                                {
                                    IdTipo = reader.GetInt32("IdTipo2"),
                                    Descripcion = reader.GetString("Tipo2Descripcion")
                                },
                                Estado = reader.GetString("Estado")
                            });
                        }
                    }
                }
                return cartas;
            }
        }
    }


    
}