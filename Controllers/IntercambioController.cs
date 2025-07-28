
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiProyecto.Models;

namespace MiProyecto.Controllers
{
    [Authorize]
    public class IntercambioController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioIntercambio repositorio;
        private readonly IRepositorioColeccion repositorioColeccion;
        private readonly IRepositorioUsuario repositorioUsuario;

        public IntercambioController(
            IConfiguration configuration,
            IRepositorioIntercambio repositorio,
            IRepositorioColeccion repositorioColeccion,
            IRepositorioUsuario repositorioUsuario)
        {
            this.configuration = configuration;
            this.repositorio = repositorio;
            this.repositorioColeccion = repositorioColeccion;
            this.repositorioUsuario = repositorioUsuario;
        }

        public IActionResult Index(int pagina = 1, int tamPagina = 10)
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var intercambios = User.IsInRole("Administrador") 
                ? repositorio.ObtenerTodos(pagina, tamPagina)
                : repositorio.ObtenerPorUsuario(idUsuario);

            var totalPaginas = User.IsInRole("Administrador")
                ? (int)Math.Ceiling((double)repositorio.ContarTodos() / tamPagina)
                : (int)Math.Ceiling((double)intercambios.Count / tamPagina);

            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.PaginaActual = pagina;

            return View(intercambios);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var intercambio = repositorio.ObtenerPorId(id);
            if (intercambio == null)
            {
                return Json(new { success = false, message = "Intercambio no encontrado" });
            }

            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!User.IsInRole("Administrador") && intercambio.IdUsuarioEmisor != idUsuario && intercambio.IdUsuarioReceptor != idUsuario)
            {
                return Json(new { success = false, message = "No tienes permiso para ver este intercambio" });
            }

            return Json(new { 
                success = true, 
                intercambio = new
                {
                    idIntercambio = intercambio.IdIntercambio,
                    fecha = intercambio.Fecha.ToString("yyyy-MM-dd HH:mm"),
                    emisor = new { nombre = intercambio.Emisor.Nombre, apellido = intercambio.Emisor.Apellido, avatar = intercambio.Emisor.Avatar },
                    receptor = new { nombre = intercambio.Receptor.Nombre, apellido = intercambio.Receptor.Apellido, avatar = intercambio.Receptor.Avatar },
                    coleccionEmisor = new { nombre = intercambio.ColeccionEmisor.Nombre },
                    coleccionReceptor = new { nombre = intercambio.ColeccionReceptor.Nombre },
                    estado = intercambio.Estado,
                    cartas = intercambio.Cartas.Select(c => new
                    {
                        idIntercambioCarta = $"{c.IdIntercambio}_{c.IdCarta}_{c.EsDeEmisor}",
                        idCarta = c.IdCarta,
                        cantidad = c.Cantidad,
                        esDeEmisor = c.EsDeEmisor,
                        carta = new { nombre = c.Carta.Nombre, imagen = c.Carta.Imagen, valorEstimado = c.Carta.ValorEstimado }
                    }).ToList()
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            try
            {
                var intercambio = repositorio.ObtenerPorId(id);
                if (intercambio == null)
                {
                    return Json(new { success = false, message = "Intercambio no encontrado" });
                }

                var filasAfectadas = repositorio.Baja(id);
                if (filasAfectadas > 0)
                {
                    return Json(new { success = true, message = "Intercambio eliminado exitosamente" });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo eliminar el intercambio" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al eliminar el intercambio: {ex.Message}" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var intercambio = repositorio.ObtenerPorId(id);
            if (intercambio == null)
            {
                return Json(new { success = false, message = "Intercambio no encontrado" });
            }

            
            return Json(new { 
                success = true, 
                intercambio = new
                {
                    idIntercambio = intercambio.IdIntercambio,
                    fecha = intercambio.Fecha.ToString("yyyy-MM-ddTHH:mm") // Format for datetime-local
                }
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, [FromBody] IntercambioEditViewModel model)
        {
            

            if (!ModelState.IsValid || model == null)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine($"ModelState errors: {string.Join(", ", errors)}");
                return Json(new { success = false, message = $"Datos inválidos: {string.Join(", ", errors)}" });
            }

            if (model.IdIntercambio != id)
            {
                return Json(new { success = false, message = "ID de intercambio no coincide" });
            }

            var intercambio = repositorio.ObtenerPorId(id);
            if (intercambio == null)
            {
                return Json(new { success = false, message = "Intercambio no encontrado" });
            }

            if (model.Fecha == default(DateTime))
            {
                return Json(new { success = false, message = "La fecha es inválida" });
            }

            
            intercambio.Fecha = DateTime.SpecifyKind(model.Fecha, DateTimeKind.Unspecified);
            try
            {
                var filasAfectadas = repositorio.Modificacion(intercambio);
                
                if (filasAfectadas > 0)
                {
                    return Json(new { success = true, message = "Fecha actualizada exitosamente" });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo actualizar la fecha" });
                }
            }
            catch (Exception ex)
            {
                
                return Json(new { success = false, message = $"Error al actualizar la fecha: {ex.Message}" });
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create(int idColeccion, int idColeccionReceptor)
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            

            if (repositorio.TieneIntercambioPendiente(idUsuario))
            {
                
                TempData["ErrorMessage"] = "Ya tienes un intercambio pendiente. Debes resolverlo antes de crear otro.";
                return RedirectToAction("Index", "Coleccion");
            }

            var coleccionPropia = repositorioColeccion.ObtenerPorId(idColeccion);
            if (coleccionPropia == null)
            {
                
                TempData["ErrorMessage"] = "Coleccion propia no válida";
                return RedirectToAction("Index", "Coleccion");
            }
            if (coleccionPropia.IdUsuario != idUsuario)
            {
                
                TempData["ErrorMessage"] = "Coleccion propia no válida";
                return RedirectToAction("Index", "Coleccion");
            }

            var coleccionesPublicas = repositorioColeccion.ObtenerTodosFiltrados(1, 1000, idUsuario, User.IsInRole("Administrador"))
                .Where(c => c.IdUsuario != idUsuario && c.EsPublica == 1)
                .ToList();

            var coleccionReceptor = repositorioColeccion.ObtenerPorId(idColeccionReceptor);
            if (coleccionReceptor == null || coleccionReceptor.IdUsuario == idUsuario || coleccionReceptor.EsPublica != 1)
            {
                
                TempData["ErrorMessage"] = "Coleccion receptora no válida";
                return RedirectToAction("Index", "Coleccion");
            }
            if (!coleccionesPublicas.Any(c => c.IdColeccion == idColeccionReceptor))
            {
                coleccionesPublicas.Add(coleccionReceptor);
            }

            // Ensure Cartas is initialized
            coleccionPropia.Cartas = coleccionPropia.Cartas ?? new List<Carta>();
            foreach (var col in coleccionesPublicas)
            {
                col.Cartas = col.Cartas ?? new List<Carta>();
            }

            
            foreach (var col in coleccionesPublicas)
            {
                Console.WriteLine($"coleccionPublica: Id={col.IdColeccion}, Nombre={col.Nombre}, Usuario={col.Usuario.Nombre}, Avatar={col.Usuario.Avatar}, CartasCount={col.Cartas.Count}");
            }

            ViewBag.ColeccionPropia = coleccionPropia;
            ViewBag.ColeccionesPublicas = coleccionesPublicas;
            ViewBag.IdColeccionReceptor = idColeccionReceptor;

            return View(new IntercambioViewModel
            {
                IdColeccionEmisor = coleccionPropia.IdColeccion,
                IdColeccionReceptor = idColeccionReceptor,
                CartasEmisor = new List<IntercambioCartaViewModel>(),
                CartasReceptor = new List<IntercambioCartaViewModel>()
            });
        }
        [HttpGet]
        [Authorize]
        public IActionResult IntercambioPendiente(int? idIntercambio = null)
        {
            try
            {
                
                
                int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                Intercambio intercambio = null;

                if (idIntercambio.HasValue)
                {
                    
                    intercambio = repositorio.ObtenerPorId(idIntercambio.Value);
                    if (intercambio == null || (intercambio.IdUsuarioEmisor != idUsuario && intercambio.IdUsuarioReceptor != idUsuario && !User.IsInRole("Administrador")))
                    {
                        
                        TempData["ErrorMessage"] = "Intercambio no encontrado o no tienes permiso para verlo.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    
                    var intercambios = repositorio.ObtenerPorUsuario(idUsuario)
                        .Where(i => i.Estado == 0)
                        .OrderByDescending(i => i.Fecha)
                        .ToList();
                    intercambio = intercambios.FirstOrDefault();
                }

                if (intercambio == null)
                {
                    
                    TempData["SuccessMessage"] = "No tienes intercambios pendientes.";
                    return RedirectToAction(nameof(Index));
                }

                
                if (intercambio.Cartas != null && intercambio.Cartas.Any())
                {
                    foreach (var carta in intercambio.Cartas)
                    {
                        Console.WriteLine($"[IntercambioPendiente] Carta: IdCarta={carta.IdCarta}, Nombre={carta.Carta?.Nombre}, Cantidad={carta.Cantidad}, EsDeEmisor={carta.EsDeEmisor}");
                    }
                }
                else
                {
                    Console.WriteLine("[IntercambioPendiente] No hay cartas en intercambio.Cartas");
                }

                return View(intercambio);
            }
            catch (Exception ex)
            {
                
                TempData["ErrorMessage"] = $"Error al cargar el intercambio: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
       [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromBody] IntercambioViewModel model)
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            

            if (!ModelState.IsValid || model == null)
            {
                
                return Json(new { success = false, message = "Datos invalidos" });
            }

            if (model.IdColeccionEmisor <= 0 || model.IdColeccionReceptor <= 0)
            {
                
                return Json(new { success = false, message = "Ids de coleccion no validos" });
            }

            if (model.CartasEmisor == null || !model.CartasEmisor.Any() || model.CartasReceptor == null || !model.CartasReceptor.Any())
            {
                
                return Json(new { success = false, message = "Debes seleccionar al menos una carta para intercambiar de cada lado" });
            }

            if (repositorio.TieneIntercambioPendiente(idUsuario))
            {
                
                return Json(new { success = false, message = "Ya tienes un intercambio pendiente. Debes resolverlo antes de crear otro." });
            }

            var coleccionEmisor = repositorioColeccion.ObtenerPorId(model.IdColeccionEmisor);
            if (coleccionEmisor == null || coleccionEmisor.IdUsuario != idUsuario)
            {
                
                return Json(new { success = false, message = "Coleccion emisora no valida o no pertenece al usuario" });
            }

            var coleccionReceptor = repositorioColeccion.ObtenerPorId(model.IdColeccionReceptor);
            if (coleccionReceptor == null || coleccionReceptor.IdUsuario == idUsuario || coleccionReceptor.EsPublica != 1)
            {
                
                return Json(new { success = false, message = "Coleccion receptora no valida o no es publica" });
            }

           

            var intercambio = new Intercambio
            {
                IdUsuarioEmisor = idUsuario,
                IdUsuarioReceptor = coleccionReceptor.IdUsuario,
                IdColeccionEmisor = model.IdColeccionEmisor,
                IdColeccionReceptor = model.IdColeccionReceptor,
                Fecha = DateTime.Now,
                Estado = 0 // Pendiente
            };

            var cartasEmisor = model.CartasEmisor.Select(c => new IntercambioCarta
            {
                IdCarta = c.IdCarta,
                Cantidad = c.Cantidad,
                EsDeEmisor = 1
            }).ToList();

            var cartasReceptor = model.CartasReceptor.Select(c => new IntercambioCarta
            {
                IdCarta = c.IdCarta,
                Cantidad = c.Cantidad,
                EsDeEmisor = 0
            }).ToList();

            try
            {
                
                int idIntercambio = repositorio.CrearIntercambio(intercambio, cartasEmisor, cartasReceptor);
                
                return Json(new { success = true, message = "Intercambio creado exitosamente", idIntercambio });
            }
            catch (Exception ex)
            {
                
                return Json(new { success = false, message = $"Error al crear el intercambio: {ex.Message}" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AceptarIntercambio(int id)
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var intercambio = repositorio.ObtenerPorId(id);

            if (intercambio == null || intercambio.IdUsuarioReceptor != idUsuario)
            {
                TempData["ErrorMessage"] = "Intercambio no válido";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (repositorio.AceptarIntercambio(id))
                {
                    TempData["SuccessMessage"] = "Intercambio aceptado exitosamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo aceptar el intercambio";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al aceptar el intercambio: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelarIntercambio(int id)
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var intercambio = repositorio.ObtenerPorId(id);

            if (intercambio == null || 
                (intercambio.IdUsuarioEmisor != idUsuario && intercambio.IdUsuarioReceptor != idUsuario))
            {
                TempData["ErrorMessage"] = "Intercambio no valido";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (repositorio.CancelarIntercambio(id))
                {
                    TempData["SuccessMessage"] = "Intercambio cancelado exitosamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo cancelar el intercambio";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cancelar el intercambio: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ObtenerCartasColeccion(int idColeccion)
        {
            try
            {
                int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var coleccion = repositorioColeccion.ObtenerPorId(idColeccion);

                if (coleccion == null || (coleccion.IdUsuario != idUsuario && coleccion.EsPublica != 1))
                {
                    
                    return Json(new { success = false, message = "Coleccion no valida" });
                }

                var cartasAgrupadas = coleccion.Cartas
                    .GroupBy(c => c.IdCarta)
                    .Select(g => new
                    {
                        idCarta = g.Key,
                        nombre = g.First().Nombre,
                        imagen = g.First().Imagen,
                        valorEstimado = g.First().ValorEstimado,
                        cantidadDisponible = g.Count()
                    })
                    .ToList();

                
                return Json(new { success = true, cartas = cartasAgrupadas });
            }
            catch (Exception ex)
            {
                
                return Json(new { success = false, message = $"Error al cargar las cartas: {ex.Message}" });
            }
        }
        [HttpGet]
        public IActionResult TestCartasIntercambio(int idIntercambio)
        {
            try
            {
                var cartas = repositorio.ObtenerCartasIntercambio(idIntercambio);
                return Json(new { success = true, cartasCount = cartas.Count, cartas });
            }
            catch (Exception ex)
            {
                
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}