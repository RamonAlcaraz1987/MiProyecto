using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiProyecto.Models;

namespace MiProyecto.Controllers;
//1

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepositorioCarta car;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {   
        var cartas = car.ObtenerTodos();
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
