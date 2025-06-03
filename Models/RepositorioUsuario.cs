using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MiProyecto.Models
{
    public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration) { }
     

            public int Alta(Usuario e)
            {

                using (var connection = GetConnection())
                {
                connection.Open();
                var query = @"INSERT INTO usuario (Nombre, Apellido, DNI, Email, Contraseña, Rol, PuntosVirtuales, Avatar)
                VALUES (@Nombre, @Apellido, @DNI, @Email, @Clave, @Rol, @PuntosVirtuales, @Avatar)
                SELECT LAST_INSERT_ID()";        
                
                using(var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Nombre", e.Nombre);
                    command.Parameters.AddWithValue("@Apellido", e.Apellido);
                    command.Parameters.AddWithValue("@DNI", e.DNI);
                    command.Parameters.AddWithValue("@Email", e.Email);
                    command.Parameters.AddWithValue("@Clave", e.Clave);
                    command.Parameters.AddWithValue("@Rol", e.Rol);
                    command.Parameters.AddWithValue("@PuntosVirtuales", e.PuntosVirtuales);
                    command.Parameters.AddWithValue("@Avatar", string.IsNullOrEmpty(e.Avatar) ? DBNull.Value : (object)e.Avatar);
                    e.IdUsuario = Convert.ToInt32(command.ExecuteScalar());
                    return e.IdUsuario;
                }
                }
            }

            public int Baja(int id)
            {

                using (var connection = GetConnection())
                {
                    connection.Open();
                    var query = @"DELETE FROM usuario WHERE IdUsuario = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery();
                    }
                }
                
            }


            public int Modificacion(Usuario e)
            {
                using (var connection = GetConnection())
                {

                connection.Open();

                var query = @"UPDATE usuario SET Nombre = @Nombre, Apellido = @Apellido, DNI = @DNI, Email = @Email, 
                Rol = @Rol, PuntosVirtuales = @PuntosVirtuales, 
                Avatar = NULLIF(@Avatar, '')" ;

                if(!string.IsNullOrEmpty(e.Clave))
                {
                    query += ", Clave = @Clave";
                }

                query += " WHERE IdUsuario = @id";


                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", e.IdUsuario);
                    command.Parameters.AddWithValue("@Nombre", e.Nombre);
                    command.Parameters.AddWithValue("@Apellido", e.Apellido);
                    command.Parameters.AddWithValue("@DNI", e.DNI);
                    command.Parameters.AddWithValue("@Email", e.Email);
                    command.Parameters.AddWithValue("@Rol", e.Rol);
                    command.Parameters.AddWithValue("@PuntosVirtuales", e.PuntosVirtuales);
                    command.Parameters.AddWithValue("@Avatar", string.IsNullOrEmpty(e.Avatar) ? DBNull.Value : (object)e.Avatar);

                    if (!string.IsNullOrEmpty(e.Clave))
                    {
                        command.Parameters.AddWithValue("@Clave", e.Clave);
                    }

                    return command.ExecuteNonQuery();
                }
                }
            }


            public Usuario ObtenerPorId(int id)
            {
                Usuario? e = null;

                using (var connection = GetConnection())
                {
                    connection.Open();
                    var query = @"SELECT IdUsuario, Nombre, Apellido, DNI, Email, Clave, Rol, PuntosVirtuales, Avatar FROM usuario WHERE IdUsuario = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                e= new Usuario
                                {
                                    IdUsuario = reader.GetInt32("IdUsuario"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),
                                    DNI = reader.GetString("DNI"),
                                    Email = reader.GetString("Email"),
                                    Clave = reader.GetString("Clave"),
                                    Rol = reader.GetString("Rol"),
                                    PuntosVirtuales = reader.GetInt32("PuntosVirtuales"),
                                    Avatar = reader.IsDBNull("Avatar") ? "" : reader.GetString("Avatar"),
                                };
                            }
                        }
                    }
                }

                return e;
            }

            public IList<Usuario> ObtenerTodos(int pagina , int tamPagina = 10)
            {
                IList<Usuario> usuarios = new List<Usuario>();
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var query = @"SELECT IdUsuario, Nombre, Apellido, DNI, Email, Rol, PuntosVirtuales, Avatar 
                    FROM usuario
                    ORDER BY Apellido, Nombre
                    LIMIT @tamPagina OFFSET @offset";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@tamPagina", tamPagina);
                        command.Parameters.AddWithValue("@offset", (pagina - 1) * tamPagina);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usuarios.Add(new Usuario
                                {
                                    IdUsuario = reader.GetInt32("IdUsuario"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),
                                    DNI = reader.GetString("DNI"),
                                    Email = reader.GetString("Email"),
                                    Rol = reader.GetString("Rol"),
                                    PuntosVirtuales = reader.GetInt32("PuntosVirtuales"),
                                    Avatar = reader.IsDBNull("Avatar") ? "" : reader.GetString("Avatar")
                                });
                            }
                        }
                    }
                }
                return usuarios;
            }

            public Usuario ObtenerPorEmail(string email)
            {
                    Usuario? e = null;
                    using (var connection = GetConnection())
                    {

                      connection.Open();
                      var query = @"SELECT IdUsuario, Nombre, Apellido, DNI, Email, Clave, Rol, PuntosVirtuales, Avatar FROM usuario WHERE Email = @email";
                      
                      
                      using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                      {
                        command.Parameters.AddWithValue("@email", email);
                        using(var reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                e = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("IdUsuario"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),
                                    DNI = reader.GetString("DNI"),
                                    Email = reader.GetString("Email"),
                                    Clave = reader.GetString("Clave"),
                                    Rol = reader.GetString("Rol"),
                                    PuntosVirtuales = reader.GetInt32("PuntosVirtuales"),
                                    Avatar = reader.IsDBNull("Avatar") ? "" : reader.GetString("Avatar"),
                                };
                            }
                        }
                    }
                          
                }      



                        
                return e;
                
            }




            
                

    
    
    }

}   