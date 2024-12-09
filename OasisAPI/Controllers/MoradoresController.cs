using OasisApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using OasisApi.Data;
using Microsoft.EntityFrameworkCore;

namespace OasisApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("[Controller]")]
    public class MoradoresController : ControllerBase
    {
        private readonly DataContext _context;

        public MoradoresController(DataContext context)
        {
            _context = context;
        }

        // GET: api/moradores
        [HttpGet]
        public ActionResult<IEnumerable<Morador>> GetMoradores()
        {
            return _context.Moradores.ToList();
        }

        // GET: api/moradores/{id}
        [HttpGet("MoradorbyId/{id}")]
        public async Task<ActionResult> GetMoradorbyId(int id)
        {
            Morador? morador = await _context.Moradores.FirstOrDefaultAsync(m => m.Id == id);

            if (morador == null)
            {
                return NotFound();
            }

            return Ok(morador);
        }

     
       
        // POST: api/moradores
        [HttpPost]
public async Task<ActionResult> InserirNovoMorador([FromBody] Morador morador)
{
    // Verifique se o objeto recebido não é nulo
    if (morador == null)
    {
        return BadRequest("Os dados do morador não podem ser nulos.");
    }

    // Adiciona o novo morador ao banco de dados
    try
    {
        _context.Moradores.Add(morador);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(InserirNovoMorador), new { id = morador.Id }, morador);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao salvar no banco de dados: {ex.InnerException?.Message ?? ex.Message}");
        return StatusCode(500, $"Erro ao salvar no banco de dados: {ex.Message}");
    }

}


        // PUT: api/moradores/{id}
      [HttpPut("{id}")]
public async Task<IActionResult> AtualizarMoradorbyId(int id, Morador moradorAtualizado)
{
    if (moradorAtualizado == null)
    {
        return BadRequest("Os dados do morador não foram enviados.");
    }

    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var morador = await _context.Moradores.FirstOrDefaultAsync(m => m.Id == id);
    if (morador == null)
    {
        return NotFound("Morador não encontrado.");
    }

    // Atualizar os campos do morador
    morador.Nome = moradorAtualizado.Nome;
    morador.Idade = moradorAtualizado.Idade;
    morador.CPF = moradorAtualizado.CPF;
    morador.RG = moradorAtualizado.RG;
    morador.Endereco = moradorAtualizado.Endereco;
    morador.Ativo = moradorAtualizado.Ativo;
    morador.Observacoes = moradorAtualizado.Observacoes;

    _context.Update(morador);
    await _context.SaveChangesAsync();

    return Ok(morador);
}



        // DELETE: api/moradores/{id}
        [HttpDelete("{Deletar Morador}")]
        public async Task<IActionResult> DeleteMorador(int id)
        {
            Morador? morador = _context.Moradores.FirstOrDefault(m => m.Id == id);
            if (morador == null)
            {
                return NotFound();
            }

            _context.Remove(morador);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
