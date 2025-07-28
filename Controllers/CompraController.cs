using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using MiProyecto.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using MySql.Data.MySqlClient;


namespace MiProyecto.Controllers
{

    
    public class CompraController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioCompra repositorio;
        private readonly IRepositorioCarta repositorioCarta;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IRepositorioColeccion repositorioColeccion;
        private readonly IRepositorioPack repositorioPack;
        

        public CompraController (IConfiguration configuration, IWebHostEnvironment environment, IRepositorioCompra repositorio, 
        IRepositorioCarta repositorioCarta, IRepositorioUsuario repositorioUsuario, IRepositorioColeccion repositorioColeccion, IRepositorioPack repositorioPack) 
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
            this.repositorioCarta = repositorioCarta;
            this.repositorioUsuario = repositorioUsuario;
            this.repositorioColeccion = repositorioColeccion;
            this.repositorioPack = repositorioPack;
            

            
        }
        [Authorize(Policy = "Administrador")]

        public IActionResult Index(int pagina = 1, int tamPagina = 10, string dniFiltro = null)
        {
           var compras = string.IsNullOrEmpty(dniFiltro)
                ? repositorio.ObtenerTodos(pagina, tamPagina)
                : repositorio.BuscarPorDNIUsuario(dniFiltro);

            var totalPaginas = string.IsNullOrEmpty(dniFiltro)
                ? (int)Math.Ceiling((double)repositorio.ContarTodos() / tamPagina)
                : (int)Math.Ceiling((double)compras.Count / tamPagina);

            pagina = Math.Max(1, Math.Min(pagina, totalPaginas > 0 ? totalPaginas : 1));

            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.PaginaActual = pagina;
            ViewBag.DniFiltro = dniFiltro ?? "";

            return View(compras);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Detail(int id, int pagina = 1)
        {
            try
            {
                var compra = repositorio.ObtenerPorId(id);
                if (compra == null)
                {
                    TempData["ErrorMessage"] = "La compra no existe.";
                    return RedirectToAction("Index", new { pagina });
                }

                ViewBag.PaginaActual = pagina;
                return View(compra);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrio un error al obtener los detalles de la compra: " + ex.Message;
                return RedirectToAction("Index", new { pagina });
            }
        }

        [HttpGet]
        [Route("Compra/Buscar")]
        [Authorize(Policy = "Administrador")]
        public IActionResult Buscar(string q)
        {
            try
            {
                var res = repositorio.BuscarPorDNIUsuario(q);
                return Json(new { datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        
        [Authorize]
        public IActionResult Create()
        {

            ViewBag.Packs = repositorioPack.ObtenerTodos(1, 4);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult ComprarSobre(int idPack, int IdUsuario)
        {
            try
            {
               int IdUsuarioLogueado = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
               if (IdUsuarioLogueado != IdUsuario)
               {
                    TempData["ErrorMessage"] = "No puedes comprar el pack de otra persona.";
                    return RedirectToAction("Create");

               }
                //obtener usuario
               Usuario usuario = repositorioUsuario.ObtenerPorId(IdUsuario);
               if (usuario == null)
               {
                    TempData["ErrorMessage"] = "El usuario no existe.";
                    return RedirectToAction("Create");
               }

               Pack pack = repositorioPack.ObtenerPorId(idPack);
               if (pack == null)
               {
                    TempData["ErrorMessage"] = "El pack no existe.";
                    return RedirectToAction("Create");
               }

               if(usuario.PuntosVirtuales < pack.Precio)
               {
                    TempData["ErrorMessage"] = "No tienes suficientes puntos para comprar este pack.";
                    return RedirectToAction("Create");
               }

               usuario.PuntosVirtuales -= pack.Precio;
               repositorioUsuario.Modificacion(usuario);


               Compra compra = new Compra{
                    IdUsuario = IdUsuario,
                    IdPack = idPack,
                    Fecha = DateTime.Now,
                    Estado = 1



               };

               int IdCompra = repositorio.Alta(compra);

               List<int> cartasIds = GenerarCartasParaPack(pack);
               repositorio.AgregarCartasACompra(IdCompra, cartasIds);


               Coleccion coleccion = null;

               var colecciones = repositorioColeccion.BuscarPorIdUsuario(IdUsuario);

               if(colecciones.Count > 0)
               {
                    coleccion = colecciones[0];
               }
               else
               {
                    coleccion = new Coleccion{
                        
                        IdUsuario = IdUsuario,
                        Nombre = "Mi coleccion" + " " + usuario.Nombre + " " + usuario.Apellido,
                        EsPublica = 1,
                        Estado = 1
                    };
                    repositorioColeccion.Alta(coleccion);
               }


               repositorioColeccion.AgregarCartasAColeccion(cartasIds, coleccion.IdColeccion);

               var cartas = repositorioCarta.ObtenerPorIds(cartasIds);
               var CardData = new List<(string Image, int IdCarta, int IdCategoria)>();
               foreach (var id in cartasIds)  
               {  
                    bool encontrado = false;
                    foreach (var carta in cartas)  
                    {  
                         if (carta.IdCarta == id)  
                         {  
                              string image = carta.Imagen;
                              if(string.IsNullOrEmpty(image))
                              {
                                  image = "/CartasArte/default.png";
                              }
                              
                              
                              CardData.Add((image, carta.IdCarta, carta.IdCategoria));  
                              encontrado = true;  
                              break;  
                         }  
                    }  
                    if (!encontrado)  
                    {  
                         CardData.Add(("/CartasArte/default.png", id, 1));  
                    }
               }  

               ViewBag.CardData = CardData;
               ViewBag.IdPack = idPack;
               ViewBag.PackName = pack.Nombre;
               TempData["SuccessMessage"] = "Compra realizada con exito.";
               return View("MostrarCartas");  

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] ="Ocurrio un error" + ex.Message;
                return RedirectToAction("Create");
            }  

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete(int id, int pagina)
        {
            try
            {
                var compra = repositorio.ObtenerPorId(id);
                if (compra == null)
                {
                    TempData["ErrorMessage"] = "Compra no encontrada.";
                    return RedirectToAction(nameof(Index), new { pagina });
                }

                repositorio.Baja(id);
                TempData["SuccessMessage"] = "Compra eliminada correctamente.";
                return RedirectToAction(nameof(Index), new { pagina });
            }
            catch (MySqlException ex) when (ex.Number == 1451)
            {
                TempData["ErrorMessage"] = "No se puede borrar, es información importante y utilizada.";
                return RedirectToAction(nameof(Index), new { pagina });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar la compra: " + ex.Message;
                return RedirectToAction(nameof(Index), new { pagina });
            }
        }

       

                

        private List<int> GenerarCartasParaPack(Pack pack)
        {
            var cartasIds = new List<int>();
            Random rand = new Random();
          
            //Agregar cartas Garantizadas
            for ( int i = 0; i < pack.RaraGar; i++)
            {
                var rarasIds = repositorioCarta.ObtenerCartasAleatorias(2,1); //categoria,cantidad
                if(rarasIds.Count > 0) 
                {
                    cartasIds.Add(rarasIds[0]);
                }

            }

            for(int i = 0; i < pack.EpicaGar; i++)
            {
                var epicasIds = repositorioCarta.ObtenerCartasAleatorias(3,1); //categoria,cantidad
                if(epicasIds.Count > 0) 
                {
                    cartasIds.Add(epicasIds[0]);
                }

            }

            for(int i = 0; i < pack.LegGar; i++)
            {
                var legendariasIds = repositorioCarta.ObtenerCartasAleatorias(4,1); //categoria,cantidad
                if(legendariasIds.Count > 0) 
                {
                    cartasIds.Add(legendariasIds[0]);
                }

            }


            //agregar las cartas restantes aleatorias

            int restantes = pack.TotalCartas - cartasIds.Count;
            if(restantes > 0)
            {  
                for (int i = 0; i < restantes; i++)
                {
                    decimal roll = (decimal)rand.NextDouble() * 100m; // numero aleatorio entre 0 y 100
                    int categoria;

                    if (roll < pack.LegendariaChance)
                    {
                        categoria = 4; // Legendaria
                    }
                    else if (roll < pack.EpicaChance)
                    {
                        categoria = 3; // Epica
                    }
                    else if (roll < pack.RaraChance)
                    {
                        categoria = 2; // Rara
                    }
                    else
                    {
                        categoria = 1; // Comun
                    }

                    var ids = repositorioCarta.ObtenerCartasAleatorias(categoria, 1);
                    if (ids.Count > 0) cartasIds.Add(ids[0]);
                    else
                    {
                        //  Comun 
                        var comunIds = repositorioCarta.ObtenerCartasAleatorias(1, 1);
                        if (comunIds.Count > 0) cartasIds.Add(comunIds[0]);
                    }
                }
            }
                
            return cartasIds;
        }
        
    }
}

