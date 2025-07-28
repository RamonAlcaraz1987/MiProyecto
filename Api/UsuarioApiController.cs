using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MiProyecto.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MiProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioApiController : ControllerBase
    {
        private readonly IRepositorioUsuario repositorio;
        private readonly IConfiguration configuration;

        public UsuarioApiController(IRepositorioUsuario repositorio, IConfiguration configuration)
        {
            this.repositorio = repositorio;
            this.configuration = configuration;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginView login)
        {
            try
            {
                if (login == null || string.IsNullOrEmpty(login.Usuario) || string.IsNullOrEmpty(login.Clave))
                {
                    return BadRequest(new { message = "Credenciales inválidas" });
                }

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: login.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                var usuario = repositorio.ObtenerPorEmail(login.Usuario);
                if (usuario == null || usuario.Clave != hashed)
                {
                    return BadRequest(new { message = "Credenciales incorrectas" });
                }

                var token = GenerarTokenJWT(usuario);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al iniciar sesión: " + ex.Message });
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

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));
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

        [HttpGet]
        [Authorize(Policy = "Administrador")]
        public IActionResult GetUsuarios([FromQuery] int pagina = 1, [FromQuery] int tamPagina = 10)
        {
            var usuarios = repositorio.ObtenerTodos(pagina, tamPagina);
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult GetUsuario(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }
    }
}