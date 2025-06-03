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

        public CartaController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioCarta repositorio)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
        }
        public IActionResult Index(int pagina = 1)
        {
            int tamPagina = 10;
            var cartas = repositorio.ObtenerTodos(pagina, tamPagina);
            ViewBag.PaginaActual = pagina;
            return View(cartas);
        }
    }
}