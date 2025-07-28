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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MiProyecto.Controllers
{
    public class UsuarioController : Controller
    {
        
    
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioUsuario repositorio;

        public UsuarioController (IConfiguration configuration, IWebHostEnvironment environment, IRepositorioUsuario repositorio)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
           
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Index(int pagina = 1, int tamPagina = 10, string dniFiltro = null)
        {
           var usuarios = string.IsNullOrEmpty(dniFiltro)
                ? repositorio.ObtenerTodos(pagina, tamPagina)
                : repositorio.BuscarPorDNI(dniFiltro);

            var totalPaginas = string.IsNullOrEmpty(dniFiltro)
                ? (int)Math.Ceiling((double)repositorio.ContarTodos() / tamPagina)
                : (int)Math.Ceiling((double)usuarios.Count / tamPagina);

            pagina = Math.Max(1, Math.Min(pagina, totalPaginas > 0 ? totalPaginas : 1));    

            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.PaginaActual = pagina;
            ViewBag.DniFiltro = dniFiltro ?? "";
            return View(usuarios);
        }
           

        [Authorize(Policy = "Administrador")]
        public IActionResult Details(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            return View(e);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.Roles = new List<string> { "Administrador", "Usuario" };
            return View();
        
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public IActionResult Create(Usuario u)
        {
            ViewBag.Roles = new List<string> { "Administrador", "Usuario" };

            if(!ModelState.IsValid)
            {
                return View(u); 
            }

            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: u.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    
                    u.Clave = hashed;
                    u.Rol = User.IsInRole("Administrador") ? u.Rol :u.Rol = "Usuario";

                    int res = repositorio.Alta(u);
                    
                    if (u.AvatarFile != null && u.AvatarFile.Length > 0 && u.IdUsuario > 0)
                    {
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "Uploads");
                        
                        if(!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        
                        string fileName = "avatar_" + u.IdUsuario + Path.GetExtension(u.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        u.Avatar = Path.Combine("/Uploads", fileName);
                        
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            u.AvatarFile.CopyTo(stream);
                        }
                        repositorio.Modificacion(u);
                    }

                    TempData["SuccessMessage"] = "Usuario creado exitosamente";
                    return RedirectToAction(nameof(Index));

            }
            catch (System.Exception ex)
            {
                
                ModelState.AddModelError("", "Error al crear el usuario: " + ex.Message);
                return View(u);
            }
            
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Roles = new List<string> { "Administrador", "Usuario" };
            var u = repositorio.ObtenerPorId(id);

            if(!User.IsInRole("Administrador") )
            {

                var usuarioActual = repositorio.ObtenerPorEmail(User.Identity.Name);  
                if(usuarioActual.IdUsuario != id)  
                {
                    return RedirectToAction(nameof(Perfil));
                }
            }
            ViewBag.CurrentPasswordHash = u.Clave;
            return View(u);

        }
        [Authorize]
        public IActionResult Perfil()
        {
           ViewBag.Roles = new List<string> { "Administrador", "Usuario" };
            var usuarioActual = repositorio.ObtenerPorEmail(User.Identity.Name);
            return View(usuarioActual);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Perfil(Usuario u, IFormFile avatarFile, bool EliminarAvatar = false) 
        {
            try{
                var usuarioDb = repositorio.ObtenerPorEmail(User.Identity.Name);

                if(usuarioDb.IdUsuario != u.IdUsuario)
                {
                    return RedirectToAction(nameof(Perfil));
                }

                u.Nombre = usuarioDb.Nombre;
                u.Apellido = usuarioDb.Apellido;
                u.Email = usuarioDb.Email;
                u.Rol = usuarioDb.Rol;

                if(EliminarAvatar)
                {
                    if(!string.IsNullOrEmpty(usuarioDb.Avatar))
                    {
                        string normalizedPath = usuarioDb.Avatar.Replace('\\', '/');
                        var rutaCompleta = Path.Combine(environment.WebRootPath, normalizedPath.TrimStart('/'));
                        if (System.IO.File.Exists(rutaCompleta))
                        {
                            System.IO.File.Delete(rutaCompleta);
                        }
                    
                    
                    
                    }
                    u.Avatar = null;
                }

                else if (avatarFile != null && avatarFile.Length > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (!string.IsNullOrEmpty(usuarioDb.Avatar))
                    {
                        string oldPath = usuarioDb.Avatar.Replace('\\', '/');
                        var rutaAnterior = Path.Combine(wwwPath, oldPath.TrimStart('/'));
                        if (System.IO.File.Exists(rutaAnterior))
                            System.IO.File.Delete(rutaAnterior);
                    }

                    string fileName = "avatar_" + u.IdUsuario + Path.GetExtension(avatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = Path.Combine("/Uploads", fileName).Replace('\\', '/');

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        avatarFile.CopyTo(stream);
                    }


                    
                }
                else
                {
                    u.Avatar = usuarioDb.Avatar;
                }

                    u.Clave = usuarioDb.Clave;

                    repositorio.Modificacion(u);
                    
                    TempData["SuccessMessage"] = "Perfil actualizado correctamente";
                    return RedirectToAction(nameof(Perfil));





                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error al actualizar el perfil: " + ex.Message;
                    return View(u);
                }


        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
               try
                {
                    var usuario = repositorio.ObtenerPorId(model.Id);
                    if (usuario == null)
                    {
                        return Json(new { success = false, message = "Usuario no encontrado", errorField = "claveActual" });
                    }

                
                    string hashedCurrent = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: model.CurrentPassword,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    if (hashedCurrent != usuario.Clave)
                    {
                        return Json(new { success = false, message = "La contraseña actual es incorrecta", errorField = "claveActual" });
                    }

                    if (model.NewPassword != model.ConfirmPassword)
                    {
                        return Json(new { success = false, message = "Las contraseñas no coinciden", errorField = "confirmarClave" });
                    }

                    usuario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: model.NewPassword,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    repositorio.Modificacion(usuario);

                    return Json(new { success = true, message = "Contraseña cambiada exitosamente" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al cambiar la contraseña: " + ex.Message });
                }
        }

        public class ChangePasswordModel
        {
            public int Id { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario u, IFormFile AvatarFile, bool EliminarAvatar = false, 
            string currentPassword = null, string newPassword = null, string confirmPassword = null)
        {
            try
            {
                var vista = nameof(Index);
                var usuarioDb = repositorio.ObtenerPorId(id);

                if (!User.IsInRole("Administrador"))
                {
                    var usuarioActual = repositorio.ObtenerPorEmail(User.Identity.Name);
                    if (usuarioActual.IdUsuario != id)
                        return RedirectToAction(nameof(Perfil));

                    u.Rol = usuarioActual.Rol;
                    vista = nameof(Perfil);
                }

                
                    u.DNI = usuarioDb.DNI;
                    u.PuntosVirtuales = usuarioDb.PuntosVirtuales;
                    u.Clave = usuarioDb.Clave;
                

                if (EliminarAvatar)
                {
                    if (!string.IsNullOrEmpty(usuarioDb.Avatar))
                    {
                        string normalizedPath = usuarioDb.Avatar.Replace('\\', '/');
                        var rutaCompleta = Path.Combine(environment.WebRootPath, normalizedPath.TrimStart('/'));
                        if (System.IO.File.Exists(rutaCompleta))
                        {
                            System.IO.File.Delete(rutaCompleta);
                        }
                    }
                    u.Avatar = null;
                }
                else if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (!string.IsNullOrEmpty(usuarioDb.Avatar))
                    {
                        string oldPath = usuarioDb.Avatar.Replace('\\', '/');
                        var rutaAnterior = Path.Combine(wwwPath, oldPath.TrimStart('/'));
                        if (System.IO.File.Exists(rutaAnterior))
                            System.IO.File.Delete(rutaAnterior);
                    }

                    string fileName = "avatar_" + id + Path.GetExtension(AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = Path.Combine("/Uploads", fileName).Replace('\\', '/');

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        AvatarFile.CopyTo(stream);
                    }
                }
                else
                {
                    u.Avatar = usuarioDb.Avatar;
                }

                u.IdUsuario = id;
                repositorio.Modificacion(u);

                TempData["SuccessMessage"] = "Usuario actualizado correctamente";
                return RedirectToAction(vista);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar el usuario: " + ex.Message;
                ViewBag.Roles = new List<string> { "Administrador", "Usuario" };
                return View(u);
            }
        }
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            try
            {
                var usuario = repositorio.ObtenerPorId(id);
                if (!string.IsNullOrEmpty(usuario.Avatar))
                {
                    var ruta = Path.Combine(environment.WebRootPath, usuario.Avatar.TrimStart('/'));
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                }

                repositorio.Baja(id);
                TempData["SuccessMessage"] = "Usuario eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (MySqlException ex) when (ex.Number == 1451) 
            {
                TempData["ErrorMessage"] = "No se puede borrar, es informacion importante y utilizada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el usuario: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Usuario");
            }
            
            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Contains("AccessDenied"))
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(new LoginView());
            }
            
            return View(new LoginView());
        }
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login, string returnUrl)
        {
             ModelState.Clear();
            try
            {
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

						

                    var usuario = repositorio.ObtenerPorEmail(login.Usuario);
                    
                    if (usuario == null || usuario.Clave != hashed)
                    {
                       
                        ModelState.AddModelError(string.Empty, "Credenciales incorrectas");
                        ViewBag.ErrorMessage = "Credenciales incorrectas";
                        return View(login);
                    }
                    
                       

                    if (usuario.Clave != hashed || !string.Equals(login.Usuario, usuario.Email, StringComparison.OrdinalIgnoreCase))
                    {
                        
                        ModelState.AddModelError(string.Empty, "Credenciales incorrectas");
                        ViewBag.ErrorMessage = "Credenciales incorrectas";
                        return View(login);
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Email),
                        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                        new Claim("FullName", $"{usuario.Nombre} {usuario.Apellido}"),
                        new Claim(ClaimTypes.Role, usuario.Rol),
                        new Claim("PuntosVirtuales", usuario.PuntosVirtuales.ToString())
                    };

                    

                    if (Request.Headers["Content-Type"].Contains("application/json"))
                        {
                            var token = GenerarTokenJWT(usuario);
                            return Ok(new { Token = token });
                        }

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                         return usuario.Rol == "Administrador"
                            ? RedirectToAction("Index", "Usuario") 
                            : RedirectToAction("Index", "Coleccion");
                        }
                }
                
                return View(login);
            }
              catch (Exception ex)
            {
                
                ModelState.AddModelError(string.Empty, $"Error al iniciar sesion: {ex.Message}");
                ViewBag.ErrorMessage = $"Error al iniciar sesion: {ex.Message}";
                return View(login);
            }
        }

        private string GenerarTokenJWT(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("FullName", $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim("PuntosVirtuales", usuario.PuntosVirtuales.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = configuration["TokenAuthentication:Issuer"];
            var audience = configuration["TokenAuthentication:Audience"];

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult ComprarPuntos(int idUsuario, int cantidad)
        {
            try
            {
                
                var usuarioDb = repositorio.ObtenerPorEmail(User.Identity.Name);
                if (usuarioDb == null || usuarioDb.IdUsuario != idUsuario)
                {
                    TempData["ErrorMessage"] = "Usuario no autorizado";
                    return RedirectToAction(nameof(Perfil));
                }

               
                if (cantidad <= 0)
                {
                    TempData["ErrorMessage"] = "La cantidad debe ser mayor a 0";
                    return RedirectToAction(nameof(Perfil));
                }

              
                usuarioDb.PuntosVirtuales += cantidad;
                repositorio.Modificacion(usuarioDb);

                TempData["SuccessMessage"] = "Puntos cargados exitosamente";
                return RedirectToAction(nameof(Perfil));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar los puntos: " + ex.Message;
                return RedirectToAction(nameof(Perfil));
            }
        }

        [HttpGet]
        [Route("Usuario/Buscar")]
        public IActionResult Buscar(string q)
        {
            try
            {
                var res = repositorio.BuscarPorDNI(q);
                return Json(new { datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

   





        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
        
}
