using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace MiProyecto.Models
{
    public class RepositorioColeccion : RepositorioBase, IRepositorioColeccion
    {
        private readonly IConfiguration configuration;
        public RepositorioColeccion(IConfiguration configuration) : base(configuration)
        {
            
        }

        public int Alta(Coleccion coleccion)
        {
            using (var conexion = GetConnection())
            
            {
                conexion.Open();
                var query = @"INSERT INTO coleccion (IdUsuario, Nombre, EsPublica, Estado) VALUES (@IdUsuario, @Nombre, @EsPublica, @Estado); SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(query, (MySqlConnection)conexion))
                {
                    
                    command.Parameters.AddWithValue("@IdUsuario", coleccion.IdUsuario);
                    command.Parameters.AddWithValue("@Nombre", coleccion.Nombre);
                    command.Parameters.AddWithValue("@EsPublica", coleccion.EsPublica);
                    command.Parameters.AddWithValue("@Estado", coleccion.Estado);
                    coleccion.IdColeccion = Convert.ToInt32(command.ExecuteScalar());
                    return coleccion.IdColeccion;;

                }   
            }
        }

        public int Baja(int idColeccion)
        {
            using (var connection = (MySqlConnection)GetConnection())
            {
                connection.Open();
                using (var transaction = (MySqlTransaction)connection.BeginTransaction())
                {
                    try
                    {
                        
                        var query1 = @"DELETE FROM coleccion_carta WHERE IdColeccion = @idColeccion";
                        using (var command = new MySqlCommand(query1, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idColeccion", idColeccion);
                            command.ExecuteNonQuery();
                        }

                        
                        var query2 = @"DELETE FROM coleccion WHERE IdColeccion = @idColeccion";
                        using (var command = new MySqlCommand(query2, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idColeccion", idColeccion);
                            int rowsAffected = command.ExecuteNonQuery();
                            transaction.Commit();
                            return rowsAffected;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public int Modificacion(Coleccion coleccion)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"UPDATE coleccion SET IdUsuario = @IdUsuario, Nombre = @Nombre, EsPublica = @EsPublica, Estado = @Estado WHERE IdColeccion = @idColeccion";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@idColeccion", coleccion.IdColeccion);
                    command.Parameters.AddWithValue("@IdUsuario", coleccion.IdUsuario);
                    command.Parameters.AddWithValue("@Nombre", coleccion.Nombre);
                    command.Parameters.AddWithValue("@EsPublica", coleccion.EsPublica);
                    command.Parameters.AddWithValue("@Estado", coleccion.Estado);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public Coleccion ObtenerPorId(int idColeccion)
        {
            Coleccion? e = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT c.IdColeccion, c.IdUsuario, c.Nombre, c.EsPublica, c.Estado, 
                u.Nombre AS UsuarioNombre, 
                u.Apellido AS UsuarioApellido, 
                u.DNI AS UsuarioDNI,
                u.Avatar
                FROM coleccion c JOIN usuario u 
                ON c.IdUsuario = u.IdUsuario 
                WHERE IdColeccion = @idColeccion";
            
            using (var command = new MySqlCommand(query, (MySqlConnection)connection))
            {
                command.Parameters.AddWithValue("@idColeccion", idColeccion);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        e = new Coleccion
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
                                DNI = reader.GetString("UsuarioDNI"),
                                Avatar = reader.GetString("Avatar")
                            },
                            Cartas = new List<Carta>()
                        };
                    }
                }
            }

            if(e!=null)
            {

                var queryCartas=@"SELECT c.IdCarta, c.Imagen, c.Nombre AS CartaNombre, c.ValorEstimado, c.IdCategoria, c.IdTipo1, c.IdTipo2, c.Estado, 
                    cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion, cc.Cantidad
                    FROM coleccion_carta cc
                    INNER JOIN carta c ON cc.IdCarta = c.IdCarta
                    INNER JOIN categoria cat ON c.IdCategoria = cat.IdCategoria
                    INNER JOIN tipo t1 ON c.IdTipo1 = t1.IdTipo
                    LEFT JOIN tipo t2 ON c.IdTipo2 = t2.IdTipo
                    WHERE cc.IdColeccion = @idColeccion
                    ORDER BY c.IdCarta";

                using (var command = new MySqlCommand(queryCartas, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@idColeccion", idColeccion);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                             var carta = new Carta
                            {
                                IdCarta = reader.GetInt32("IdCarta"),
                                Nombre = reader.GetString("CartaNombre"),
                                Imagen = reader.GetString("Imagen"),
                                ValorEstimado = reader.GetInt32("ValorEstimado"),
                                IdCategoria = reader.GetInt32("IdCategoria"),
                                IdTipo1 = reader.GetInt32("IdTipo1"),
                                IdTipo2 = reader.GetInt32("IdTipo2"),
                                Estado = reader.GetInt32("Estado"),
                                Categoria = new Categoria
                                {
                                    Nombre = reader.GetString("CategoriaNombre")
                                },
                                Tipo1 = new Tipo
                                {
                                    Descripcion = reader.GetString("Tipo1Descripcion")
                                },
                                Tipo2 = new Tipo
                                {
                                    Descripcion = reader.GetString("Tipo2Descripcion")
                                },
                               
                            };

                            for (int i = 0; i < reader.GetInt32("Cantidad"); i++)
                            {
                                e.Cartas.Add(carta);
                            }
                        }
                    }
                }
                
            }
           
           return e;
            }
        }

        
        public IList<Coleccion> ObtenerTodos(int pagina, int TamPagina = 10)
        {
            IList<Coleccion> colecciones = new List<Coleccion>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT c.IdColeccion, c.IdUsuario, c.Nombre, c.EsPublica, c.Estado, 
                u.Nombre AS UsuarioNombre, 
                u.Apellido AS UsuarioApellido, 
                u.DNI AS UsuarioDNI,
                u.Avatar
                FROM coleccion c JOIN usuario u 
                ON c.IdUsuario = u.IdUsuario
                ORDER BY c.IdColeccion LIMIT @TamPagina OFFSET @offset";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@TamPagina", TamPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * TamPagina);
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
                                    DNI = reader.GetString("UsuarioDNI"),
                                    Avatar = reader.GetString("Avatar")
                                },
                                Cartas = new List<Carta>()
                            }); 
                        }
                    }
                }

                foreach (var coleccion in colecciones)
                {
                    var queryCartas = @"SELECT c.IdCarta, c.Imagen, c.Nombre AS CartaNombre, c.ValorEstimado, c.IdCategoria, c.IdTipo1, c.IdTipo2, c.Estado, 
                    cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion, cc.Cantidad
                    FROM coleccion_carta cc
                    INNER JOIN carta c ON cc.IdCarta = c.IdCarta
                    INNER JOIN categoria cat ON c.IdCategoria = cat.IdCategoria
                    INNER JOIN tipo t1 ON c.IdTipo1 = t1.IdTipo
                    LEFT JOIN tipo t2 ON c.IdTipo2 = t2.IdTipo
                    WHERE cc.IdColeccion = @idColeccion";

                    using (var command = new MySqlCommand(queryCartas, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@idColeccion", coleccion.IdColeccion);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var carta = new Carta
                                {
                                    IdCarta = reader.GetInt32("IdCarta"),
                                    Nombre = reader.GetString("CartaNombre"),
                                    Imagen = reader.GetString("Imagen"),
                                    ValorEstimado = reader.GetInt32("ValorEstimado"),
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    IdTipo1 = reader.GetInt32("IdTipo1"),
                                    IdTipo2 = reader.GetInt32("IdTipo2"),
                                    Estado = reader.GetInt32("Estado"),
                                    Categoria = new Categoria
                                    {
                                        Nombre = reader.GetString("CategoriaNombre")
                                    },
                                    Tipo1 = new Tipo
                                    {
                                        Descripcion = reader.GetString("Tipo1Descripcion")
                                    },
                                    Tipo2 = new Tipo
                                    {
                                        Descripcion = reader.GetString("Tipo2Descripcion")
                                    },
                                    
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

        public IList<Coleccion> ObtenerTodosFiltrados(int pagina, int tamPagina, int idUsuarioLogueado, bool esAdmin)
        {
            IList<Coleccion> colecciones = new List<Coleccion>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT c.IdColeccion, c.IdUsuario, c.Nombre, c.EsPublica, c.Estado, 
                    u.Nombre AS UsuarioNombre, 
                    u.Apellido AS UsuarioApellido, 
                    u.DNI AS UsuarioDNI,
                    u.Avatar
                    FROM coleccion c JOIN usuario u 
                    ON c.IdUsuario = u.IdUsuario
                    WHERE c.EsPublica = 1 OR c.IdUsuario = @idUsuario OR @esAdmin = true
                    ORDER BY c.IdColeccion LIMIT @TamPagina OFFSET @offset";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@TamPagina", tamPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * tamPagina);
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
                                    DNI = reader.GetString("UsuarioDNI"),
                                    Avatar = reader.GetString("Avatar")
                                },
                                Cartas = new List<Carta>()
                            }); 
                        }
                    }
                }

                foreach (var coleccion in colecciones)
                {
                    var queryCartas = @"SELECT c.IdCarta, c.Nombre AS CartaNombre, c.Imagen, c.ValorEstimado, c.IdCategoria, c.IdTipo1, c.IdTipo2, c.Estado, 
                        cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion, cc.Cantidad
                        FROM coleccion_carta cc
                        INNER JOIN carta c ON cc.IdCarta = c.IdCarta
                        INNER JOIN categoria cat ON c.IdCategoria = cat.IdCategoria
                        INNER JOIN tipo t1 ON c.IdTipo1 = t1.IdTipo
                        LEFT JOIN tipo t2 ON c.IdTipo2 = t2.IdTipo
                        WHERE cc.IdColeccion = @idColeccion";

                    using (var command = new MySqlCommand(queryCartas, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@idColeccion", coleccion.IdColeccion);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var carta = new Carta
                                {
                                    IdCarta = reader.GetInt32("IdCarta"),
                                    Nombre = reader.GetString("CartaNombre"),
                                    Imagen = reader.GetString("Imagen"),
                                    ValorEstimado = reader.GetInt32("ValorEstimado"),
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    IdTipo1 = reader.GetInt32("IdTipo1"),
                                    IdTipo2 = reader.IsDBNull("IdTipo2") ? 0 : reader.GetInt32("IdTipo2"),
                                    Estado = reader.GetInt32("Estado"),
                                    Categoria = new Categoria
                                    {
                                        Nombre = reader.GetString("CategoriaNombre")
                                    },
                                    Tipo1 = new Tipo
                                    {
                                        Descripcion = reader.GetString("Tipo1Descripcion")
                                    },
                                    Tipo2 = reader.IsDBNull("Tipo2Descripcion") ? null : new Tipo
                                    {
                                        Descripcion = reader.GetString("Tipo2Descripcion")
                                    }
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

        public IList<Coleccion> BuscarPorDNIUsuario(string dni)
        {
            var colecciones = new List<Coleccion>();
            dni = "%" + dni + "%";
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT c.IdColeccion, c.IdUsuario, c.Nombre, c.EsPublica, c.Estado, 
                u.Nombre AS UsuarioNombre, 
                u.Apellido AS UsuarioApellido, 
                u.DNI AS UsuarioDNI,
                u.Avatar 
                FROM coleccion c JOIN usuario u 
                ON c.IdUsuario = u.IdUsuario 
                WHERE u.DNI LIKE @dni
                ORDER BY c.IdColeccion";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@dni", dni);
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
                                    DNI = reader.GetString("UsuarioDNI"),
                                    Avatar = reader.GetString("Avatar")
                                },
                                Cartas = new List<Carta>()
                            });
                        }
                    }
                }

                foreach (var coleccion in colecciones)
                {
                    var queryCartas = @"SELECT c.IdCarta, c.Imagen, c.Nombre AS CartaNombre, c.ValorEstimado, c.IdCategoria, c.IdTipo1, c.IdTipo2, c.Estado, 
                    cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion, cc.Cantidad
                    FROM coleccion_carta cc
                    INNER JOIN carta c ON cc.IdCarta = c.IdCarta
                    INNER JOIN categoria cat ON c.IdCategoria = cat.IdCategoria
                    INNER JOIN tipo t1 ON c.IdTipo1 = t1.IdTipo
                    LEFT JOIN tipo t2 ON c.IdTipo2 = t2.IdTipo
                    WHERE cc.IdColeccion = @idColeccion";

                    using (var command = new MySqlCommand(queryCartas, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@idColeccion", coleccion.IdColeccion);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var carta = new Carta
                                {
                                    IdCarta = reader.GetInt32("IdCarta"),
                                    Nombre = reader.GetString("CartaNombre"),
                                   
                                    Imagen = reader.GetString("Imagen"),
                                    ValorEstimado = reader.GetInt32("ValorEstimado"),
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    IdTipo1 = reader.GetInt32("IdTipo1"),
                                    IdTipo2 = reader.GetInt32("IdTipo2"),
                                    Estado = reader.GetInt32("Estado"),
                                    Categoria = new Categoria
                                    {
                                        Nombre = reader.GetString("CategoriaNombre")
                                    },
                                    Tipo1 = new Tipo
                                    {
                                        Descripcion = reader.GetString("Tipo1Descripcion")
                                    },
                                    Tipo2 = new Tipo
                                    {
                                        Descripcion = reader.GetString("Tipo2Descripcion")
                                    },
                                    
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

        public IList<Coleccion> BuscarPorIdUsuario(int idUsuario)
        {
            var colecciones = new List<Coleccion>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT c.IdColeccion, c.IdUsuario, c.Nombre, c.EsPublica, c.Estado, 
                u.Nombre AS UsuarioNombre, 
                u.Apellido AS UsuarioApellido, 
                u.DNI AS UsuarioDNI,
                u.Avatar 
                FROM coleccion c JOIN usuario u 
                ON c.IdUsuario = u.IdUsuario 
                WHERE c.IdUsuario = @idUsuario
                ORDER BY c.IdColeccion";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
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
                                    DNI = reader.GetString("UsuarioDNI"),
                                    Avatar = reader.GetString("Avatar")
                                },
                                Cartas = new List<Carta>()
                            });
                        }
                    }
                }

                foreach (var coleccion in colecciones)
                {
                    var queryCartas = @"SELECT c.IdCarta, c.Imagen, c.Nombre AS CartaNombre, c.ValorEstimado, c.IdCategoria, c.IdTipo1, c.IdTipo2, c.Estado, 
                    cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion, cc.Cantidad
                    FROM coleccion_carta cc
                    INNER JOIN carta c ON cc.IdCarta = c.IdCarta
                    INNER JOIN categoria cat ON c.IdCategoria = cat.IdCategoria
                    INNER JOIN tipo t1 ON c.IdTipo1 = t1.IdTipo
                    LEFT JOIN tipo t2 ON c.IdTipo2 = t2.IdTipo
                    WHERE cc.IdColeccion = @idColeccion";

                    using (var command = new MySqlCommand(queryCartas, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@idColeccion", coleccion.IdColeccion);
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
                                    IdTipo2 = reader.GetInt32("IdTipo2"),
                                    Estado = reader.GetInt32("Estado"),
                                    Categoria = new Categoria
                                    {
                                        Nombre = reader.GetString("CategoriaNombre")
                                    },
                                    Tipo1 = new Tipo
                                    {
                                        Descripcion = reader.GetString("Tipo1Descripcion")
                                    },
                                    Tipo2 = new Tipo
                                    {
                                        Descripcion = reader.GetString("Tipo2Descripcion")
                                    },
                                    
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

        public int ContarTodos(int idUsuarioLogueado, bool esAdmin)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT COUNT(*) 
                    FROM coleccion c 
                    WHERE c.EsPublica = 1 OR c.IdUsuario = @idUsuario OR @esAdmin = true";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUsuarioLogueado);
                    command.Parameters.AddWithValue("@esAdmin", esAdmin);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }


        public void AgregarCartasAColeccion(List<int> idsCartas, int idColeccion)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                foreach (var idCarta in idsCartas)
                {
                    var query = @"INSERT INTO coleccion_carta (IdColeccion, IdCarta, Cantidad) 
                    VALUES (@idColeccion, @idCarta, 1) 
                    ON DUPLICATE KEY UPDATE Cantidad = Cantidad + 1;";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@idColeccion", idColeccion);
                        command.Parameters.AddWithValue("@idCarta", idCarta);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public bool EsPublica(int idColeccion)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = "SELECT EsPublica FROM coleccion WHERE IdColeccion = @idColeccion";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@idColeccion", idColeccion);
                    return Convert.ToInt32(command.ExecuteScalar()) == 1;
                }
            }
        }


            



    }
}

    