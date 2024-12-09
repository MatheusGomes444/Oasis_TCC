using OasisApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using OasisApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity; // Adicionando o namespace para o PasswordHasher
using Microsoft.Extensions.Configuration; // Adicionando para acessar a configuração

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly PasswordHasher<User> _passwordHasher; // Instância do PasswordHasher
    private readonly IConfiguration _configuration; // Injeção de dependência para IConfiguration

    // Injeção de dependência para DataContext e IConfiguration
    public AuthController(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>(); // Inicializando o PasswordHasher
        _configuration = configuration; // Inicializando a configuração
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Encontre o usuário no banco de dados
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        
        // Verifique se o usuário foi encontrado
        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new { authenticated = false, message = "Credenciais inválidas!" });
        }

        // Se as credenciais estiverem corretas, gera o token
        var claims = new[] 
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, "User"),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])); // Usando _configuration para acessar o SecretKey
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"], // Usando _configuration para acessar o Issuer
            audience: _configuration["JwtSettings:Audience"], // Usando _configuration para acessar o Audience
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { authenticated = true, token = tokenString });
    }

   [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterRequest request)
{
    // Verifica se o email já está cadastrado
    if (await _context.Users.AnyAsync(u => u.Email == request.Email))
    {
        return BadRequest("Usuário já existe.");
    }

    // Criando o novo usuário e fazendo o hash da senha
    var user = new User
    {
        Email = request.Email,
        Username = request.Email,  // Atribuindo o Email como Username (ou qualquer outra lógica)
        PasswordHash = _passwordHasher.HashPassword(null, request.Password) // Hash da senha
    };

    // Adiciona o usuário no banco de dados
    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return Ok("Usuário registrado com sucesso.");
}

}
