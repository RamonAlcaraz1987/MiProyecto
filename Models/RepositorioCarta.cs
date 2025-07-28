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
                VALUES (@Nombre, @IdTipo1, @Imagen, @ValorEstimado, @IdCategoria, @IdTipo2, @Estado);
                SELECT LAST_INSERT_ID()";
            

            using (var command = new MySqlCommand(query, (MySqlConnection)connection))
            { 
                
                command.Parameters.AddWithValue("@Nombre", e.Nombre);
                command.Parameters.AddWithValue("@IdTipo1", e.IdTipo1);
                command.Parameters.AddWithValue("@Imagen", e.Imagen ?? (object)DBNull.Value);
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
                    command.Parameters.AddWithValue("@Imagen", e.Imagen ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ValorEstimado", e.ValorEstimado);
                    command.Parameters.AddWithValue("@IdCategoria", e.IdCategoria);
                    command.Parameters.AddWithValue("@IdTipo2", e.IdTipo2);
                    command.Parameters.AddWithValue("@Estado", e.Estado);
                    return command.ExecuteNonQuery();
                }
            }
        }


        public Carta ObtenerPorId(int id)
        {
            Carta? e = null;

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT IdCarta, carta.Nombre, carta.IdTipo1, Imagen, ValorEstimado, carta.IdCategoria, carta.IdTipo2, Estado, 
                            cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion 
                            FROM carta 
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
                        {
                            e = new Carta
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
                                Estado = reader.GetInt32("Estado")
                            };
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
                var query = @"SELECT IdCarta, carta.Nombre, carta.IdTipo1, Imagen, ValorEstimado, carta.IdCategoria, carta.IdTipo2, Estado, 
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
                                Estado = reader.GetInt32("Estado")
                            });
                        }
                    }
                }
                return cartas;
            }
        }

        public Carta ObtenerPorNombre(string nombre)
        {

            using(var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query =@"SELECT IdCarta, carta.Nombre, carta.IdTipo1, Imagen, ValorEstimado, carta.IdCategoria, carta.IdTipo2, Estado, 
                     cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion 
                     FROM carta 
                     INNER JOIN categoria cat ON carta.IdCategoria = cat.IdCategoria
                     INNER JOIN tipo t1 ON carta.IdTipo1 = t1.IdTipo
                     LEFT JOIN tipo t2 ON carta.IdTipo2 = t2.IdTipo
                     WHERE carta.Nombre = @Nombre";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Carta
                            {
                                IdCarta = reader.GetInt32("IdCarta"),
                                Nombre = reader.GetString("Nombre"),
                                IdTipo1 = reader.GetInt32("IdTipo1"),
                                Tipo1 = new Tipo
                                {
                                    IdTipo = reader.GetInt32("IdTipo1"),
                                    Descripcion = reader.GetString("Tipo1Descripcion")
                                },
                                Imagen = reader.IsDBNull("Imagen") ? null : reader.GetString("Imagen"),
                                ValorEstimado = reader.GetInt32("ValorEstimado"),
                                IdCategoria = reader.GetInt32("IdCategoria"),
                                Categoria = new Categoria
                                {
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    Nombre = reader.GetString("CategoriaNombre")
                                },
                                IdTipo2 = reader.GetInt32("IdTipo2"),
                                Tipo2 = reader.IsDBNull("Tipo2Descripcion") ? null : new Tipo
                                {
                                    IdTipo = reader.GetInt32("IdTipo2"),
                                    Descripcion = reader.GetString("Tipo2Descripcion")
                                },
                                Estado = reader.GetInt32("Estado")
                            };
                        }
                    }
                }
                return null;
            }
            
        }
        public IList<Carta> ObtenerPortada()
        {
            IList<Carta> cartas = new List<Carta>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT IdCarta, carta.Nombre, carta.IdTipo1, Imagen, ValorEstimado, carta.IdCategoria, carta.IdTipo2, Estado, 
                cat.Nombre AS CategoriaNombre, 
                t1.Descripcion AS Tipo1Descripcion, 
                t2.Descripcion AS Tipo2Descripcion
                FROM carta
                INNER JOIN categoria cat ON carta.IdCategoria = cat.IdCategoria
                INNER JOIN tipo t1 ON carta.IdTipo1 = t1.IdTipo
                INNER JOIN tipo t2 ON carta.IdTipo2 = t2.IdTipo
                ORDER BY IdCarta";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    
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
                                Estado = reader.GetInt32("Estado")
                            });
                        }
                    }
                }
                return cartas;
            }
        }

        public IList<Carta> BuscarPorNombre(string nombre)
        {
            List<Carta> cartas = new List<Carta>();
            nombre = "%" + nombre + "%";
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT IdCarta, carta.Nombre, carta.IdTipo1, Imagen, ValorEstimado, carta.IdCategoria, carta.IdTipo2, Estado, 
                cat.Nombre AS CategoriaNombre, 
                t1.Descripcion AS Tipo1Descripcion, 
                t2.Descripcion AS Tipo2Descripcion
                FROM carta
                INNER JOIN categoria cat ON carta.IdCategoria = cat.IdCategoria
                INNER JOIN tipo t1 ON carta.IdTipo1 = t1.IdTipo
                LEFT JOIN tipo t2 ON carta.IdTipo2 = t2.IdTipo
                WHERE carta.Nombre LIKE @Nombre
                ORDER BY IdCarta";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
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
                                Tipo2 = reader.IsDBNull("Tipo2Descripcion") ? null : new Tipo
                                {
                                    IdTipo = reader.GetInt32("IdTipo2"),
                                    Descripcion = reader.GetString("Tipo2Descripcion")
                                },
                                Estado = reader.GetInt32("Estado")
                            });
                        }
                    }
                }
                return cartas;
            }
        }

        public List<int> ObtenerCartasAleatorias(int idCategoria, int cantidad)
        {
            List<int> ids = new List<int>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT IdCarta FROM carta WHERE IdCategoria = @idCategoria AND Estado = 1 ORDER BY RAND() LIMIT @cantidad";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@idCategoria", idCategoria);
                    command.Parameters.AddWithValue("@cantidad", cantidad);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ids.Add(reader.GetInt32("IdCarta"));
                        }
                    }
                }
            }

            return ids;
        }
            
        public IList<Carta> ObtenerPorIds(IList<int> ids)
        {
            var cartas = new List<Carta>();
            if(ids==null || ids.Count==0) return cartas;

            using (var connection = GetConnection())
            {
                connection.Open();

                //crear tabla temporal para ids
                var tablaTemp = @"CREATE TEMPORARY TABLE Ids(IdCarta INT)";
                using (var command = new MySqlCommand(tablaTemp, (MySqlConnection)connection))
                {
                    command.ExecuteNonQuery();
                }

                //insertar ids en la tabla temporal
                foreach (var id in ids)
                {
                    var queryInsert = @"INSERT INTO Ids(IdCarta) VALUES (@id)";
                    using (var command = new MySqlCommand(queryInsert, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

                //obtener cartas
                var query =@"SELECT c.IdCarta, c.Nombre, c.IdTipo1, c.Imagen, c.ValorEstimado, c.IdCategoria, c.IdTipo2, c.Estado, 
                     cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion
                     FROM carta c
                     INNER JOIN categoria cat ON c.IdCategoria = cat.IdCategoria
                     INNER JOIN tipo t1 ON c.IdTipo1 = t1.IdTipo
                     LEFT JOIN tipo t2 ON c.IdTipo2 = t2.IdTipo
                     INNER JOIN ids t ON c.IdCarta = t.IdCarta
                     ORDER BY c.IdCarta";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
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
                                Tipo2 = reader.IsDBNull("Tipo2Descripcion") ? null : new Tipo
                                {
                                    IdTipo = reader.GetInt32("IdTipo2"),
                                    Descripcion = reader.GetString("Tipo2Descripcion")
                                },
                                Estado = reader.GetInt32("Estado")
                            });
                        }
                    }
                }
                   //borrar tabla temporal
            
                var dropTable = @"DROP TEMPORARY TABLE Ids";
                using (var command = new MySqlCommand(dropTable, (MySqlConnection)connection))
                {
                    command.ExecuteNonQuery();
                }  
            }
         

            return cartas;                 
        }

        public IList<Coleccion> BuscarPorNombre(string nombre, int idUsuarioLogueado, bool esAdmin)
        {
            var colecciones = new List<Coleccion>();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return colecciones; 
            }
            nombre = "%" + nombre + "%";
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT DISTINCT c.IdColeccion, c.IdUsuario, c.Nombre, c.EsPublica, c.Estado, 
                        u.Nombre AS UsuarioNombre, u.Apellido AS UsuarioApellido, u.DNI AS UsuarioDNI
                    FROM coleccion c
                    JOIN usuario u ON c.IdUsuario = u.IdUsuario
                    JOIN coleccion_carta cc ON cc.IdColeccion = c.IdColeccion
                    JOIN carta ca ON cc.IdCarta = ca.IdCarta
                    WHERE ca.Nombre LIKE @nombre
                    AND (c.EsPublica = 1 OR c.IdUsuario = @idUsuario OR @esAdmin = true)
                    ORDER BY c.IdColeccion";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@idUsuario", idUsuarioLogueado);
                    command.Parameters.AddWithValue("@esAdmin", esAdmin);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            colecciones.Add(new Coleccion
                            {
                                IdColeccion = reader.GetInt32("IdColeccion"),
                                IdUsuario = reader.GetInt32("IdUsuario"),
                                Nombre = reader.GetString("Nombre"),
                                EsPublica = reader.GetInt32("EsPublica"),
                                Estado = reader.GetInt32("Estado"),
                                Usuario = new Usuario
                                {
                                    Nombre = reader.GetString("UsuarioNombre"),
                                    Apellido = reader.GetString("UsuarioApellido"),
                                    DNI = reader.GetString("UsuarioDNI")
                                },
                                Cartas = new List<Carta>()
                            });
                        }
                    }
                }

                foreach (var coleccion in colecciones)
                {
                    var queryCartas = @"
                        SELECT c.IdCarta, c.Nombre AS CartaNombre, c.Imagen, c.ValorEstimado, c.IdCategoria, c.IdTipo1, c.IdTipo2, c.Estado, 
                            cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion, cc.Cantidad
                        FROM coleccion_carta cc
                        INNER JOIN carta c ON cc.IdCarta = c.IdCarta
                        INNER JOIN categoria cat ON c.IdCategoria = cat.IdCategoria
                        INNER JOIN tipo t1 ON c.IdTipo1 = t1.IdTipo
                        LEFT JOIN tipo t2 ON c.IdTipo2 = t2.IdTipo
                        WHERE cc.IdColeccion = @idColeccion AND c.Nombre LIKE @nombre";
                    using (var command = new MySqlCommand(queryCartas, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@idColeccion", coleccion.IdColeccion);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var carta = new Carta
                                {
                                    IdCarta = reader.GetInt32("IdCarta"),
                                    Imagen = reader.GetString("Imagen"),
                                    Nombre = reader.GetString("CartaNombre"),
                                    ValorEstimado = reader.GetInt32("ValorEstimado"),
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    IdTipo1 = reader.GetInt32("IdTipo1"),
                                    IdTipo2 = reader.IsDBNull("IdTipo2") ? 0 : reader.GetInt32("IdTipo2"),
                                    Estado = reader.GetInt32("Estado"),
                                    Categoria = new Categoria { Nombre = reader.GetString("CategoriaNombre") },
                                    Tipo1 = new Tipo { Descripcion = reader.GetString("Tipo1Descripcion") },
                                    Tipo2 = reader.IsDBNull("Tipo2Descripcion") ? null : new Tipo { Descripcion = reader.GetString("Tipo2Descripcion") }
                                };
                                for (int i = 0; i < reader.GetInt32("Cantidad"); i++)
                                {
                                    coleccion.Cartas.Add(carta);
                                }
                            }
                        }
                    }
                }
                return colecciones;
            }
        }

        public int ContarTodos()
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var query = "SELECT COUNT(*) FROM carta";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
        
    }

    
}