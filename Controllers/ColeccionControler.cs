using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Security.Claims;
using MiProyecto.Models;

namespace MiProyecto.Controllers
{
    public class ColeccionController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioColeccion repositorio;
        private readonly IRepositorioCarta repositorioCarta;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IRepositorioIntercambio repositorioIntercambio;

        public ColeccionController(
            IConfiguration configuration,
            IWebHostEnvironment environment,
            IRepositorioColeccion repositorio,
            IRepositorioCarta repositorioCarta,
            IRepositorioUsuario repositorioUsuario,
            IRepositorioIntercambio repositorioIntercambio)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
            this.repositorioCarta = repositorioCarta;
            this.repositorioUsuario = repositorioUsuario;
            this.repositorioIntercambio = repositorioIntercambio;
        }


        [Authorize]
        public IActionResult Index(int pagina = 1, int tamPagina = 10, string cartaFiltro = null)
        {
            int idUsuarioLogueado = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var esAdmin = User.IsInRole("Administrador");

            
            var coleccionPropia = repositorio.BuscarPorIdUsuario(idUsuarioLogueado).FirstOrDefault();

            
            var colecciones = string.IsNullOrEmpty(cartaFiltro)
                ? repositorio.ObtenerTodosFiltrados(pagina, tamPagina, idUsuarioLogueado, esAdmin)
                : repositorioCarta.BuscarPorNombre(cartaFiltro, idUsuarioLogueado, esAdmin);

            
            var totalPaginas = string.IsNullOrEmpty(cartaFiltro)
                ? (int)Math.Ceiling((double)repositorio.ContarTodos(idUsuarioLogueado, esAdmin) / tamPagina)
                : (int)Math.Ceiling((double)colecciones.Count / tamPagina);

            
            pagina = Math.Max(1, Math.Min(pagina, totalPaginas > 0 ? totalPaginas : 1));

            ViewBag.ColeccionPropia = coleccionPropia;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.PaginaActual = pagina;
            ViewBag.CartaFiltro = cartaFiltro ?? "";
            ViewBag.IdUsuarioLogueado = idUsuarioLogueado;
            ViewBag.EsAdmin = esAdmin;

            return View(colecciones);
        }
        
        [Authorize]
        public IActionResult Detail(int id, int pagina = 1, int tamPagina = 25)
        {
            var coleccion = repositorio.ObtenerPorId(id);
            if (coleccion == null)
            {
                TempData["ErrorMessage"] = "La colección no existe.";
                return NotFound();
            }

            int idUsuarioLogueado = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var esAdmin = User.IsInRole("Administrador");

            
          if (coleccion.EsPublica == 0 && coleccion.IdUsuario != idUsuarioLogueado)
            {
                TempData["ErrorMessage"] = "No tienes permiso para ver esta colección.";
                return NotFound();
            }
            
            var cartasConConteo = new List<(Carta Carta, int Cantidad)>();
            var cartasUnicas = new Dictionary<int, int>(); // IdCarta -> Cantidad
            foreach (var carta in coleccion.Cartas)
            {
                if (cartasUnicas.ContainsKey(carta.IdCarta))
                {
                    cartasUnicas[carta.IdCarta]++;
                }
                else
                {
                    cartasUnicas[carta.IdCarta] = 1;
                }
            }

            
            int inicio = (pagina - 1) * tamPagina;
            int indice = 0;
            foreach (var par in cartasUnicas)
            {
                if (indice >= inicio && indice < inicio + tamPagina)
                {
                    
                    Carta cartaSeleccionada = null;
                    foreach (var carta in coleccion.Cartas)
                    {
                        if (carta.IdCarta == par.Key)
                        {
                            cartaSeleccionada = carta;
                            break;
                        }
                    }
                    if (cartaSeleccionada != null)
                    {
                        cartasConConteo.Add((cartaSeleccionada, par.Value));
                    }
                }
                indice++;
            }

            
            int totalPaginas = (int)Math.Ceiling((double)cartasUnicas.Count / tamPagina);

            ViewBag.Coleccion = coleccion;
            ViewBag.CartasConConteo = cartasConConteo;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.TamPagina = tamPagina;

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id, int pagina = 1)
        {
            var coleccion = repositorio.ObtenerPorId(id);
            if (coleccion == null)
            {
                TempData["ErrorMessage"] = "La colección no existe.";
                return NotFound();
            }

            int idUsuarioLogueado = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var esAdmin = User.IsInRole("Administrador");

            if (coleccion.IdUsuario != idUsuarioLogueado && !esAdmin)
            {
                TempData["ErrorMessage"] = "No tienes permiso para editar esta colección.";
                return  NotFound();
            }

            ViewBag.PaginaActual = pagina;
            return View(coleccion);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Coleccion coleccion, int pagina = 1)
        {
            int idUsuarioLogueado = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var esAdmin = User.IsInRole("Administrador");

            var coleccionExistente = repositorio.ObtenerPorId(coleccion.IdColeccion);
            if (coleccionExistente == null)
            {
                TempData["ErrorMessage"] = "La colección no existe.";
                return NotFound();
            }

            if (coleccionExistente.IdUsuario != idUsuarioLogueado && !esAdmin)
            {
                TempData["ErrorMessage"] = "No tienes permiso para editar esta colección.";
                return NotFound();
            }

            if (coleccion.EsPublica == 1 && repositorioIntercambio.TieneIntercambioPendiente(idUsuarioLogueado))
            {
                TempData["ErrorMessage"] = "No puedes hacer pública tu colección mientras tengas un intercambio pendiente.";
                ViewBag.PaginaActual = pagina;
                return View(coleccion);
            }

            ModelState.Remove("Usuario");
            ModelState.Remove("Cartas");
            if (!ModelState.IsValid)
            {
                
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["ErrorMessage"] = "Errores de validación: " + string.Join(", ", errors);
                ViewBag.PaginaActual = pagina;
                return View(coleccion);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Modificacion(coleccion);
                    TempData["SuccessMessage"] = "Colección modificada correctamente.";
                    return RedirectToAction("Index", new { pagina });
                }
                ViewBag.PaginaActual = pagina;
                return View(coleccion);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al modificar la colección: " + ex.Message;
                ViewBag.PaginaActual = pagina;
                return View(coleccion);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, int pagina)
        {
            try
            {
                var coleccion = repositorio.ObtenerPorId(id);
                if (coleccion == null)
                {
                    TempData["ErrorMessage"] = "Colección no encontrada.";
                    return RedirectToAction("Index", new { pagina });
                }

                int idUsuarioLogueado = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var esAdmin = User.IsInRole("Administrador");

                if (coleccion.IdUsuario != idUsuarioLogueado && !esAdmin)
                {
                    TempData["ErrorMessage"] = "No tienes permiso para eliminar esta colección.";
                    return RedirectToAction("Index", new { pagina });
                }

                repositorio.Baja(id);
                TempData["SuccessMessage"] = "Colección eliminada correctamente.";
                return RedirectToAction("Index", new { pagina });
            }
            catch (MySqlException ex) when (ex.Number == 1451)
            {
                TempData["ErrorMessage"] = "No se puede borrar, es información importante y utilizada.";
                return RedirectToAction("Index", new { pagina });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar la colección: " + ex.Message;
                return RedirectToAction("Index", new { pagina });
            }
        }

        [HttpGet]
        [Route("Coleccion/Buscar")]
        [Authorize]
        public IActionResult Buscar(string q)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                {
                    return Json(new { datos = new List<object>() });
                }

                int idUsuarioLogueado = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var esAdmin = User.IsInRole("Administrador");
                var colecciones = repositorioCarta.BuscarPorNombre(q, idUsuarioLogueado, esAdmin);
                
                var nombresCartas = new List<object>();
                var nombresVistos = new List<string>();
                foreach (var coleccion in colecciones)
                {
                    foreach (var carta in coleccion.Cartas)
                    {
                        var nombre = carta.Nombre;
                        var yaExiste = false;
                        foreach (var visto in nombresVistos)
                        {
                            if (string.Equals(visto, nombre, StringComparison.OrdinalIgnoreCase))
                            {
                                yaExiste = true;
                                break;
                            }
                        }
                        if (!yaExiste)
                        {
                            nombresVistos.Add(nombre);
                            nombresCartas.Add(new { id = nombre, text = nombre });
                        }
                    }
                }
                return Json(new { datos = nombresCartas });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}