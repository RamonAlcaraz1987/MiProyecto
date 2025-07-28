using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MiProyecto.Models

{
    public class RepositorioPack : RepositorioBase, IRepositorioPack
    {

        public RepositorioPack(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Pack pack)
        {
            throw new InvalidOperationException("No se pueden agregar nuevos packs. La tabla está restringida a cuatro filas.");
        }

        public int Baja(int id)
        {
            throw new InvalidOperationException("No se pueden eliminar packs. La tabla esta restringida a cuatro filas.");
        }

        public int Modificacion(Pack pack)
        {
            int rowsAffected;
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"UPDATE pack 
                             SET Precio = @Precio, TotalCartas = @TotalCartas, 
                                 RaraChance = @RaraChance, EpicaChance = @EpicaChance, 
                                 LegendariaChance = @LegendariaChance, RaraGar = @RaraGar, 
                                 EpicaGar = @EpicaGar, LegGar = @LegGar, 
                                 Imagen = @Imagen, Leyenda = @Leyenda
                             WHERE IdPack = @IdPack";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@IdPack", pack.IdPack);
                    command.Parameters.AddWithValue("@Precio", pack.Precio);
                    command.Parameters.AddWithValue("@TotalCartas", pack.TotalCartas);
                    command.Parameters.AddWithValue("@RaraChance", pack.RaraChance);
                    command.Parameters.AddWithValue("@EpicaChance", pack.EpicaChance);
                    command.Parameters.AddWithValue("@LegendariaChance", pack.LegendariaChance);
                    command.Parameters.AddWithValue("@RaraGar", pack.RaraGar);
                    command.Parameters.AddWithValue("@EpicaGar", pack.EpicaGar);
                    command.Parameters.AddWithValue("@LegGar", pack.LegGar);
                    command.Parameters.AddWithValue("@Imagen", pack.Imagen);
                    command.Parameters.AddWithValue("@Leyenda", pack.Leyenda);
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }

        public IList<Pack> ObtenerTodos(int pagina, int tamPagina)
        {
            var packs = new List<Pack>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT IdPack, Nombre, Precio, TotalCartas, RaraChance, EpicaChance, 
                             LegendariaChance, RaraGar, EpicaGar, LegGar, Imagen, Leyenda
                             FROM pack 
                             ORDER BY IdPack 
                             LIMIT @tamPagina OFFSET @offset";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * tamPagina);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            packs.Add(new Pack
                            {
                                IdPack = reader.GetInt32("IdPack"),
                                Nombre = reader.GetString("Nombre"),
                                Precio = reader.GetInt32("Precio"),
                                TotalCartas = reader.GetInt32("TotalCartas"),
                                RaraChance = reader.GetDecimal("RaraChance"),
                                EpicaChance = reader.GetDecimal("EpicaChance"),
                                LegendariaChance = reader.GetDecimal("LegendariaChance"),
                                RaraGar = reader.GetInt32("RaraGar"),
                                EpicaGar = reader.GetInt32("EpicaGar"),
                                LegGar = reader.GetInt32("LegGar"),
                                Imagen = reader.IsDBNull("Imagen") ? null : reader.GetString("Imagen"),
                                Leyenda = reader.GetString("Leyenda")
                            });
                        }
                    }
                }
            }
            return packs;
        }

        public Pack ObtenerPorId(int id)
        {
            Pack pack = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT IdPack, Nombre, Precio, TotalCartas, RaraChance, EpicaChance, 
                             LegendariaChance, RaraGar, EpicaGar, LegGar, Imagen, Leyenda
                             FROM pack WHERE IdPack = @id";
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pack = new Pack
                            {
                                IdPack = reader.GetInt32("IdPack"),
                                Nombre = reader.GetString("Nombre"),
                                Precio = reader.GetInt32("Precio"),
                                TotalCartas = reader.GetInt32("TotalCartas"),
                                RaraChance = reader.GetDecimal("RaraChance"),
                                EpicaChance = reader.GetDecimal("EpicaChance"),
                                LegendariaChance = reader.GetDecimal("LegendariaChance"),
                                RaraGar = reader.GetInt32("RaraGar"),
                                EpicaGar = reader.GetInt32("EpicaGar"),
                                LegGar = reader.GetInt32("LegGar"),
                                Imagen = reader.IsDBNull("Imagen") ? null : reader.GetString("Imagen"),
                                Leyenda = reader.GetString("Leyenda")
                            };
                        }
                    }
                }
            }
            return pack;
        }
    
    }
}
       