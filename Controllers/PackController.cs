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

    public class PackController : Controller
    {

        private readonly IRepositorioPack repositorio;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public PackController(IRepositorioPack repositorio, IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.repositorio = repositorio;
            this.configuration = configuration;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var packs = repositorio.ObtenerTodos(1, 4); 
            return View(packs);
        }

        public IActionResult Edit(int id)
        {
            var pack = repositorio.ObtenerPorId(id);
            if (pack == null)
            {
                return NotFound();
            }
            ViewBag.NombresPermitidos = new[] {"Basico","Raro","Epico","Jumbo"};

            return View(pack);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Pack pack, IFormFile ImagenFile, bool EliminarImagen)
        {
            try
            {
                var packDb = repositorio.ObtenerPorId(id);
                if (packDb == null)
                {
                    TempData["ErrorMessage"] = "Pack no encontrado.";
                    return RedirectToAction(nameof(Index));
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
                   
                    foreach (var error in ModelState)
                    {
                        Console.WriteLine($"ModelState Error: Key={error.Key}, Errors={string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                    ViewBag.NombresPermitidos = new[] { "Basico", "Raro", "Epico", "Jumbo" };
                    return View(pack);
                }

                if (EliminarImagen && !string.IsNullOrEmpty(packDb.Imagen))
                {
                    string normalizedPath = packDb.Imagen.Replace("/", "\\").TrimStart('\\');
                    var rutaCompleta = Path.Combine(environment.WebRootPath, normalizedPath);
                    if (System.IO.File.Exists(rutaCompleta))
                    {
                        System.IO.File.Delete(rutaCompleta);
                    }
                    pack.Imagen = null;
                }
                else if (ImagenFile != null && ImagenFile.Length > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "PacksArte");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (!string.IsNullOrEmpty(packDb.Imagen))
                    {
                        string normalizedPath = packDb.Imagen.Replace("/", "\\").TrimStart('\\');
                        var rutaCompleta = Path.Combine(environment.WebRootPath, normalizedPath);
                        if (System.IO.File.Exists(rutaCompleta))
                        {
                            System.IO.File.Delete(rutaCompleta);
                        }
                    }
                    string fileName = $"pack_{id}{Path.GetExtension(ImagenFile.FileName)}";
                    string pathCompleto = Path.Combine(path, fileName);
                    pack.Imagen = $"/PacksArte/{fileName}";
                    using (var stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        ImagenFile.CopyTo(stream);
                    }
                }
                else
                {
                    pack.Imagen = packDb.Imagen; // Keep existing image
                }

                pack.IdPack = id;
                var rowsAffected = repositorio.Modificacion(pack);
                if (rowsAffected == 0)
                {
                    TempData["ErrorMessage"] = "No se actualizo ningun registro en la base de datos.";
                    ViewBag.NombresPermitidos = new[] { "Basico", "Raro", "Epico", "Jumbo" };
                    return View(pack);
                }

                TempData["SuccessMessage"] = "Pack modificado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al modificar el pack: {ex.Message}";
                ViewBag.NombresPermitidos = new[] { "Basico", "Raro", "Epico", "Jumbo" };
                return View(pack);
            }
        }
    }
}
 