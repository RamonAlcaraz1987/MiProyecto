using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace MiProyecto.Models
{
    public class RepositorioCompra : RepositorioBase, IRepositorioCompra
    {
        public RepositorioCompra(IConfiguration configuration) : base(configuration)
        {

        }


        public int Alta(Compra e)
        {

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"INSERT INTO compra (IdUsuario, IdPack, Fecha, Estado) 
                VALUES (@IdUsuario, @IdPack, @Fecha, @Estado);
                SELECT LAST_INSERT_ID()";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", e.IdUsuario);
               
                    command.Parameters.AddWithValue("@IdPack", e.IdPack);
                    command.Parameters.AddWithValue("@Fecha", e.Fecha);
                    command.Parameters.AddWithValue("@Estado", e.Estado);
                    e.IdCompra = Convert.ToInt32(command.ExecuteScalar());
                    return e.IdCompra;
                }
                
            }
        }

       public int Baja(int id)
        {
            using (var connection = (MySqlConnection)GetConnection())
            {
                connection.Open();
                using (var transaction = (MySqlTransaction)connection.BeginTransaction())
                {
                    try
                    {
                      
                        var query1 = @"DELETE FROM compra_carta WHERE IdCompra = @id";
                        using (var command = new MySqlCommand(query1, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }

                        
                        var query2 = @"DELETE FROM compra WHERE IdCompra = @id";
                        using (var command = new MySqlCommand(query2, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", id);
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

        public int Modificacion(Compra e)
        {
           using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"UPDATE compra SET IdUsuario = @IdUsuario, IdPack = @IdPack, Fecha = @Fecha, Estado = @Estado WHERE IdCompra = @idCompra";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", e.IdUsuario);
                    command.Parameters.AddWithValue("@idCompra", e.IdCompra);
                    command.Parameters.AddWithValue("@IdPack", e.IdPack);
                    command.Parameters.AddWithValue("@Fecha", e.Fecha);
                    command.Parameters.AddWithValue("@Estado", e.Estado);
                    return command.ExecuteNonQuery();
                }
           }
        }

        public Compra ObtenerPorId(int id)
        {
            Compra? e = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT c.IdCompra, c.IdPack, c.IdUsuario, c.Fecha, c.Estado, 
                p.Nombre As Nombre, 
                u.Nombre As UsuarioNombre, 
                u.Apellido As UsuarioApellido,
                u.DNI As UsuarioDNI
                FROM compra c 
                JOIN pack p ON c.IdPack = p.IdPack 
                JOIN usuario u ON c.IdUsuario = u.IdUsuario
                WHERE c.IdCompra = @idCompra";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@idCompra", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            e = new Compra
                            {
                                IdCompra = reader.GetInt32("IdCompra"),
                                IdPack = reader.GetInt32("IdPack"),
                                IdUsuario = reader.GetInt32("IdUsuario"),
                                Fecha = reader.GetDateTime("Fecha"),
                                Estado = reader.GetInt32("Estado"),
                                Pack  = new Pack
                                {   
                                    IdPack = reader.GetInt32("IdPack"),
                                    Nombre = reader.GetString("Nombre")
                                    

                                },
                                Usuario = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("IdUsuario"),
                                    Nombre = reader.GetString("UsuarioNombre"),
                                    Apellido = reader.GetString("UsuarioApellido"),
                                    DNI = reader.GetString("UsuarioDNI")
                                },
                                Cartas = new List<Carta>()
                                

                               
                            };
                        }
                    }
                }

                if(e != null)
                {
                    var queryCartas = @"SELECT c.IdCarta, cc.IdCompra, c.Nombre, c.Imagen, c.ValorEstimado, c.IdCategoria, c.IdTipo1, c.IdTipo2, c.Estado, 
                    cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion, cc.Cantidad
                    FROM compra_carta cc
                    INNER JOIN carta c ON cc.IdCarta = c.IdCarta
                    INNER JOIN categoria cat ON c.IdCategoria = cat.IdCategoria
                    INNER JOIN tipo t1 ON c.IdTipo1 = t1.IdTipo
                    LEFT JOIN tipo t2 ON c.IdTipo2 = t2.IdTipo
                    WHERE cc.IdCompra = @idCompra";
                    using (var command = new MySqlCommand(queryCartas, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@idCompra", id);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var carta = new Carta
                                {
                                    IdCarta = reader.GetInt32("IdCarta"),
                                    Nombre = reader.GetString("Nombre"),
                                    IdTipo1 = reader.GetInt32("IdTipo1"),
                                    Tipo1 = new Tipo
                                    {
                                        IdTipo = reader.GetInt32("IdTipo1"),
                                        Descripcion = reader.GetString("Tipo1Descripcion")
                                    },
                                    IdTipo2 = reader.GetInt32("IdTipo2"),
                                    Tipo2 = new Tipo
                                    {
                                        IdTipo = reader.GetInt32("IdTipo2"),
                                        Descripcion = reader.GetString("Tipo2Descripcion")
                                    },
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    Categoria = new Categoria
                                    {
                                        IdCategoria = reader.GetInt32("IdCategoria"),
                                        Nombre = reader.GetString("CategoriaNombre")
                                    },
                                    Imagen = reader.GetString("Imagen"),
                                    ValorEstimado = reader.GetInt32("ValorEstimado"),
                                    Estado = reader.GetInt32("Estado")
                                };
                                for(int i = 0; i < reader.GetInt32("Cantidad"); i++)
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

        public IList<Compra> ObtenerTodos(int pagina, int TamPagina = 10)
        {   
            IList<Compra> compras = new List<Compra>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT c.IdCompra, c.IdPack, c.IdUsuario, c.Fecha, c.Estado, 
                p.Nombre As Nombre, 
                u.Nombre As UsuarioNombre, 
                u.Apellido As UsuarioApellido,
                u.DNI As UsuarioDNI 
                FROM compra c 
                JOIN pack p ON c.IdPack = p.IdPack 
                JOIN usuario u ON c.IdUsuario = u.IdUsuario
                ORDER BY c.IdCompra LIMIT @tamPagina OFFSET @offset";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@tamPagina", TamPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * TamPagina);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            compras.Add(new Compra
                            {
                                IdCompra = reader.GetInt32("IdCompra"),
                                IdPack = reader.GetInt32("IdPack"),
                                IdUsuario = reader.GetInt32("IdUsuario"),
                                Fecha = reader.GetDateTime("Fecha"),
                                Estado = reader.GetInt32("Estado"),
                                Pack = new Pack
                                {
                                    IdPack = reader.GetInt32("IdPack"),
                                    Nombre = reader.GetString("Nombre")
                                },
                                Usuario = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("IdUsuario"),
                                    Nombre = reader.GetString("UsuarioNombre"),
                                    Apellido = reader.GetString("UsuarioApellido"),
                                    DNI = reader.GetString("UsuarioDNI")
                                },
                                Cartas = new List<Carta>()
                            });
                        }
                    }
                }

                foreach(var compra in compras)
                {
                    var queryCartas = @"SELECT c.IdCarta, cc.IdCompra, c.Nombre, c.Imagen, c.ValorEstimado, c.IdCategoria, c.IdTipo1, c.IdTipo2, c.Estado, 
                    cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion, cc.Cantidad 
                    FROM compra_carta cc
                    INNER JOIN carta c ON cc.IdCarta = c.IdCarta
                    INNER JOIN categoria cat ON c.IdCategoria = cat.IdCategoria
                    INNER JOIN tipo t1 ON c.IdTipo1 = t1.IdTipo
                    LEFT JOIN tipo t2 ON c.IdTipo2 = t2.IdTipo
                    WHERE cc.IdCompra = @idCompra";
                    using (var command = new MySqlCommand(queryCartas, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@idCompra", compra.IdCompra);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var carta = new Carta
                                {
                                    IdCarta = reader.GetInt32("IdCarta"),
                                    Nombre = reader.GetString("Nombre"),
                                    Imagen = reader.IsDBNull("Imagen") ? null : reader.GetString("Imagen"),
                                    ValorEstimado = reader.GetInt32("ValorEstimado"),
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    IdTipo1 = reader.GetInt32("IdTipo1"),
                                    IdTipo2 = reader.GetInt32("IdTipo2"),
                                    Estado = reader.GetInt32("Estado"),
                                    Categoria = new Categoria
                                    {
                                        IdCategoria = reader.GetInt32("IdCategoria"),
                                        Nombre = reader.GetString("CategoriaNombre")
                                    },
                                    Tipo1 = new Tipo
                                    {
                                        IdTipo = reader.GetInt32("IdTipo1"),
                                        Descripcion = reader.GetString("Tipo1Descripcion")
                                    },
                                    Tipo2 = new Tipo
                                    {
                                        IdTipo = reader.GetInt32("IdTipo2"),
                                        Descripcion = reader.GetString("Tipo2Descripcion")
                                    }
                                };

                                for(int i = 0; i < reader.GetInt32("Cantidad"); i++)
                                {
                                    compra.Cartas.Add(carta);
                                }


                            }
                        }
                    }


                }
            }

            return compras;


        }
        
        public IList<Compra> BuscarPorDNIUsuario(string dni)
        {
            var compras = new List<Compra>();
            dni = "%" + dni + "%";
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT c.IdCompra, c.IdPack, c.IdUsuario, c.Fecha, c.Estado, 
                            p.Nombre AS PackNombre, 
                            u.Nombre AS UsuarioNombre, u.Email AS UsuarioEmail, u.DNI AS UsuarioDNI
                     FROM compra c 
                     JOIN pack p ON c.IdPack = p.IdPack 
                     JOIN usuario u ON c.IdUsuario = u.IdUsuario
                     WHERE u.DNI LIKE @dni 
                     ORDER BY c.IdCompra";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@dni", dni);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            compras.Add(new Compra
                            {
                                IdCompra = reader.GetInt32("IdCompra"),
                                IdPack = reader.GetInt32("IdPack"),
                                IdUsuario = reader.GetInt32("IdUsuario"),
                                Fecha = reader.GetDateTime("Fecha"),
                                Estado = reader.GetInt32("Estado"),
                                Pack = new Pack
                                {
                                    IdPack = reader.GetInt32("IdPack"),
                                    Nombre = reader.GetString("PackNombre")
                                },
                                Usuario = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("IdUsuario"),
                                    Nombre = reader.GetString("UsuarioNombre"),
                                    Email = reader.GetString("UsuarioEmail"),
                                    DNI = reader.GetString("UsuarioDNI")
                                },
                                Cartas = new List<Carta>()
                            });
                        }
                    }
                }

                foreach (var compra in compras)
                {
                    var queryCartas = @"SELECT c.IdCarta, c.Imagen, cc.IdCompra, c.Nombre, c.ValorEstimado, c.IdCategoria, c.IdTipo1, c.IdTipo2, c.Estado, 
                                       cat.Nombre AS CategoriaNombre, t1.Descripcion AS Tipo1Descripcion, t2.Descripcion AS Tipo2Descripcion, cc.Cantidad 
                                       FROM compra_carta cc
                                       INNER JOIN carta c ON cc.IdCarta = c.IdCarta
                                       INNER JOIN categoria cat ON c.IdCategoria = cat.IdCategoria
                                       INNER JOIN tipo t1 ON c.IdTipo1 = t1.IdTipo
                                       LEFT JOIN tipo t2 ON c.IdTipo2 = t2.IdTipo
                                       WHERE cc.IdCompra = @idCompra";
                    using (var command = new MySqlCommand(queryCartas, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@idCompra", compra.IdCompra);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var carta = new Carta
                                {
                                    IdCarta = reader.GetInt32("IdCarta"),
                                    Nombre = reader.GetString("Nombre"),
                                    Imagen = reader.IsDBNull("Imagen") ? null : reader.GetString("Imagen"),
                                    ValorEstimado = reader.GetInt32("ValorEstimado"),
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    IdTipo1 = reader.GetInt32("IdTipo1"),
                                    IdTipo2 = reader.GetInt32("IdTipo2"),
                                    Estado = reader.GetInt32("Estado"),
                                    Categoria = new Categoria
                                    {
                                        IdCategoria = reader.GetInt32("IdCategoria"),
                                        Nombre = reader.GetString("CategoriaNombre")
                                    },
                                    Tipo1 = new Tipo
                                    {
                                        IdTipo = reader.GetInt32("IdTipo1"),
                                        Descripcion = reader.GetString("Tipo1Descripcion")
                                    },
                                    Tipo2 = reader.IsDBNull("IdTipo2") ? null : new Tipo
                                    {
                                        IdTipo = reader.GetInt32("IdTipo2"),
                                        Descripcion = reader.GetString("Tipo2Descripcion")
                                    }
                                };

                                for (int i = 0; i < reader.GetInt32("Cantidad"); i++)
                                {
                                    compra.Cartas.Add(carta);
                                }
                            }
                        }
                    }
                }
            }
            return compras;
        }
        
        public int ContarTodos()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM compra";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        
        public void AgregarCartasACompra(int idCompra, List<int> idsCartas)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                foreach (var idCarta in idsCartas)
                {
                    var query = @"INSERT INTO compra_carta (IdCompra, IdCarta, Cantidad) 
                    VALUES (@idCompra, @idCarta, 1) 
                    ON DUPLICATE KEY UPDATE Cantidad = Cantidad + 1;";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@idCompra", idCompra);
                        command.Parameters.AddWithValue("@idCarta", idCarta);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

    }


    
}