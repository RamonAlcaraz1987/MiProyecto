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
    [Authorize(Policy = "Administrador")]
    public class CartaController : Controller
    {

        private readonly IRepositorioCarta repositorio;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioCategoria repositorioCategoria;
        private readonly IRepositorioTipo repositorioTipo;
        

        public CartaController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioCarta repositorio, 
        IRepositorioCategoria repositorioCategoria, IRepositorioTipo repositorioTipo)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
            this.repositorioCategoria = new RepositorioCategoria(configuration);
            this.repositorioTipo = new RepositorioTipo(configuration);
        }
        public IActionResult Index(int pagina = 1, int tamPagina = 10, string nombreFiltro = null)
        {
            var cartas = string.IsNullOrEmpty(nombreFiltro)
                ? repositorio.ObtenerTodos(pagina, tamPagina)
                : repositorio.BuscarPorNombre(nombreFiltro);

            var totalPaginas = string.IsNullOrEmpty(nombreFiltro)
                ? (int)Math.Ceiling((double)repositorio.ContarTodos() / tamPagina)
                : (int)Math.Ceiling((double)cartas.Count / tamPagina);
            
            pagina = Math.Max(1, Math.Min(pagina, totalPaginas > 0 ? totalPaginas : 1));
            
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.PaginaActual = pagina;
            ViewBag.NombreFiltro = nombreFiltro ?? "";

            return View(cartas);
        }

        public IActionResult Detail(int id, int pagina = 1)
        {
            
            var carta = repositorio.ObtenerPorId(id);
            if (carta == null)
            {
                return NotFound();
            }
            ViewBag.PaginaActual = pagina;
            return View(carta);
        }

        public IActionResult Create(int pagina = 1)
        {
            ViewBag.PaginaActual = pagina;
            ViewBag.Categorias = repositorioCategoria.ObtenerTodos(1, 1000);
            ViewBag.Tipos = repositorioTipo.ObtenerTodos(1, 1000);
            return View();
        }

        [HttpPost]
        public IActionResult Create(Carta carta,int pagina = 1)
        {  

            try{
                
                var cartaExistente = repositorio.ObtenerPorNombre(carta.Nombre);
                
                if (ModelState.ContainsKey("Imagen"))
                {
                    ModelState["Imagen"].Errors.Clear();
                    ModelState["Imagen"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                }
                if (ModelState.ContainsKey("ImagenFile"))
                {
                    ModelState["ImagenFile"].Errors.Clear();
                    ModelState["ImagenFile"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                }

                if (cartaExistente != null)
                {
                    ModelState.AddModelError("Nombre", "Ya existe una carta con el mismo nombre.");
                    
                }
                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"Validation error: {error.ErrorMessage}");
                    }
                    ViewBag.PaginaActual = pagina;
                    ViewBag.Categorias = repositorioCategoria.ObtenerTodos(1, 1000);
                    ViewBag.Tipos = repositorioTipo.ObtenerTodos(1, 1000);
                    return View(carta);
                }
                        carta.Imagen = null;
                int IdCarta = repositorio.Alta(carta);


                if(carta.ImagenFile != null && carta.ImagenFile.Length > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "CartasArte");
                    if (!Directory.Exists(path))

                    {
                        
                        Directory.CreateDirectory(path);
                    }

                        string fileName = $"carta_{IdCarta}{Path.GetExtension(carta.ImagenFile.FileName)}";
                        string pathCompleto = Path.Combine(path, fileName);
                        carta.Imagen = $"/CartasArte/{fileName}";
                        using (var stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            carta.ImagenFile.CopyTo(stream);
                        }

                        carta.IdCarta = IdCarta;
                        repositorio.Modificacion(carta);
                        }

                        TempData["SuccessMessage"] = "Carta creada correctamente.";
                
                        return RedirectToAction(nameof(Index), new {pagina});
            
            }
        
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear la carta: {ex.Message}");
                ViewBag.Categorias = repositorioCategoria.ObtenerTodos(1, 1000);
                ViewBag.Tipos = repositorioTipo.ObtenerTodos(1, 1000);
                return View(carta);
            }
        }

        public IActionResult Edit(int id, int pagina = 1)
        {
            var carta = repositorio.ObtenerPorId(id);

            if (carta == null)
            {
                return NotFound();
            }
            ViewBag.PaginaActual = pagina;
            ViewBag.Categorias = repositorioCategoria.ObtenerTodos(1, 1000);
            ViewBag.Tipos = repositorioTipo.ObtenerTodos(1, 1000);
            return View(carta);
        }

        [HttpPost]
        public IActionResult Edit(int id, Carta carta, IFormFile ImagenFile, bool EliminarImagen, int pagina = 1)
        {
            try{

                var cartaDb= repositorio.ObtenerPorId(id);
                if(cartaDb == null)
                {

                        TempData["ErrorMessage"] = "Carta no encontrada.";
                        return RedirectToAction(nameof(Index), new {pagina});

                }


                var cartaExistente = repositorio.ObtenerPorNombre(carta.Nombre);
                if (cartaExistente != null && cartaExistente.IdCarta != id)
                {
                    ModelState.AddModelError("Nombre", "Ya existe una carta con el mismo nombre.");
                }

                if (ModelState.ContainsKey("Imagen"))
                {
                    ModelState["Imagen"].Errors.Clear();
                    ModelState["Imagen"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                }
                if (ModelState.ContainsKey("ImagenFile"))
                {
                    ModelState["ImagenFile"].Errors.Clear();
                    ModelState["ImagenFile"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                }

                if (!ModelState.IsValid)
                {

                    if (!ModelState.IsValid)
                        {
                            foreach (var error in ModelState)
                            {
                                Console.WriteLine($"ModelState Error: Key={error.Key}, Errores={string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                            }
                        }
                    ViewBag.PaginaActual = pagina;
                    ViewBag.Categorias = repositorioCategoria.ObtenerTodos(1, 1000);
                    ViewBag.Tipos = repositorioTipo.ObtenerTodos(1, 1000);
                    return View(carta);


                }

                if (EliminarImagen)
                {
                    if(!String.IsNullOrEmpty(cartaDb.Imagen))
                    {
                        string normalizedPath = cartaDb.Imagen.Replace("/", "\\");
                        var rutaCompleta = Path.Combine(environment.WebRootPath, normalizedPath.TrimStart('\\'));
                        if (System.IO.File.Exists(rutaCompleta))
                        {
                            System.IO.File.Delete(rutaCompleta);
                        }
                        
                        
                    }
                    carta.Imagen = null;
                
                }

                else if (ImagenFile != null && ImagenFile.Length > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "CartasArte");
                    if (!Directory.Exists(path))

                    {
                        Directory.CreateDirectory(path);
                    }

                    if(!string.IsNullOrEmpty(cartaDb.Imagen))
                    {
                        string normalizedPath = cartaDb.Imagen.Replace("/", "\\");
                        var rutaCompleta = Path.Combine(environment.WebRootPath, normalizedPath.TrimStart('\\'));
                        if (System.IO.File.Exists(rutaCompleta))
                        {
                            System.IO.File.Delete(rutaCompleta);
                        }
                    }

                    string fileName = $"carta_{id}{Path.GetExtension(ImagenFile.FileName)}";
                    string pathCompleto = Path.Combine(path, fileName);
                    carta.Imagen = $"/CartasArte/{fileName}";
                    using (var stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        ImagenFile.CopyTo(stream);
                    }
                }
                else
                {
                    carta.Imagen = cartaDb.Imagen;
                }

                carta.IdCarta = id;
                repositorio.Modificacion(carta);
                TempData["SuccessMessage"] = "Carta modificada correctamente.";
                return RedirectToAction(nameof(Index), new {pagina});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al modificar la carta: {ex.Message}");
                ViewBag.PaginaActual = pagina;
                ViewBag.Categorias = repositorioCategoria.ObtenerTodos(1, 1000);
                ViewBag.Tipos = repositorioTipo.ObtenerTodos(1, 1000);
                return View(carta);




            }
        }

        [HttpGet]
        [Route("Carta/Buscar")]
        public IActionResult Buscar(string q)
        {
            try{

                var res = repositorio.BuscarPorNombre(q);
                return Json(new {datos = res});
            }
            catch (Exception ex)
            {
                return Json(new {error = ex.Message});
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, int pagina)
        {
            try
            {
                var carta = repositorio.ObtenerPorId(id);
                
                if (carta == null)
                {
                    TempData["ErrorMessage"] = "Carta no encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                if(!string.IsNullOrEmpty(carta.Imagen))
                {
                        var ruta = Path.Combine(environment.WebRootPath, carta.Imagen.TrimStart('/'));
                        if (System.IO.File.Exists(ruta))
                        {
                            System.IO.File.Delete(ruta);
                        }

                }
                repositorio.Baja(id);
                TempData["SuccessMessage"] = "Carta eliminada correctamente.";
                return RedirectToAction(nameof(Index), new {pagina});
            }
             catch (MySqlException ex) when (ex.Number == 1451) 
            {
                TempData["ErrorMessage"] = "No se puede borrar, es informacion importante y utilizada.";
                return RedirectToAction(nameof(Index), new {pagina});
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el usuario: " + ex.Message;
                return RedirectToAction(nameof(Index), new {pagina});
            }
        }
    }
}