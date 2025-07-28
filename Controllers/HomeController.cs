using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiProyecto.Models;

namespace MiProyecto.Controllers;
//1

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepositorioCarta car;

    
    public HomeController(ILogger<HomeController> logger, IRepositorioCarta repositorioCarta)
    {
        _logger = logger;
        car = repositorioCarta;
    }

    public IActionResult Index(int pagina=1, int tamPagina=10)
    {   
        var cartas = car.ObtenerPortada();
        return View(cartas);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
     public IActionResult Restringido()
    {
        return View();
    }
}
