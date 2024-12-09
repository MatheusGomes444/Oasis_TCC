using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OasisApi.Models;
using OasisApi.Data;
using Microsoft.EntityFrameworkCore; // Adicione esta linha
using System.Threading.Tasks;

namespace OasisApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlojamentosController : ControllerBase
        {
        private readonly DataContext _context;

        public AlojamentosController(DataContext context)
        {
            _context = context;
        }
     
// GET: api/alojamentos/{id}
[HttpGet("{id}")]
public async Task<ActionResult<Alojamento>> GetAlojamentoById(int id)
{
    // Busca o alojamento pelo ID, incluindo a lista de moradores
    var alojamento = await _context.Alojamentos
                                    .Include(a => a.Moradores) // Inclui os moradores
                                    .FirstOrDefaultAsync(a => a.Id == id);

    if (alojamento == null)
    {
        return NotFound($"Alojamento com ID {id} não encontrado.");
    }

    return Ok(alojamento); // Retorna o alojamento encontrado
}



        // POST: api/Alojamentos
  [HttpPost]
public async Task<IActionResult> InserirNovoAlojamento([FromBody] Alojamento alojamento)
{
    if (alojamento == null)
    {
        return BadRequest("Os dados do alojamento não podem ser nulos.");
    }

    try
    {
        alojamento.Moradores = new List<Morador>(); // Garante que não há moradores ao criar

        _context.Alojamentos.Add(alojamento);
        await _context.SaveChangesAsync();

        return Ok(alojamento);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Erro ao salvar o alojamento: {ex.Message}");
    }
}




        // GET: api/alojamentos
       [HttpGet]
public async Task<ActionResult<IEnumerable<object>>> GetAlojamentos()
{
    var alojamentos = await _context.Alojamentos
        .Include(a => a.Moradores)
        .Select(a => new 
        {
            a.Id,
            a.Nome,
            a.CapacidadeMaxima,
            QuantidadeMoradores = a.Moradores.Count // Conta os moradores associados
        })
        .ToListAsync();

    return Ok(alojamentos);
}


        // PUT: api/alojamentos/Atualizar
        [HttpPut("Atualizar")]
        public async Task<IActionResult> AtualizarAlojamento(int id, Alojamento alojamentoAtualizado)
        {
            Alojamento? alojamento = await _context.Alojamentos.FirstOrDefaultAsync(a => a.Id == id);

            if (alojamento == null)
            {
                return NotFound();
            }

            // Atualização dos campos
            alojamento.Nome = alojamentoAtualizado.Nome;
            alojamento.Equipe = alojamentoAtualizado.Equipe;
            alojamento.Telefone = alojamentoAtualizado.Telefone;
            alojamento.Email = alojamentoAtualizado.Email;
            alojamento.CapacidadeMaxima = alojamento.CapacidadeMaxima;
            alojamento.Pet = alojamento.Pet;
            alojamento.Sexo = alojamento.Sexo;
            alojamento.Pertences = alojamento.Pertences;
            alojamento.Refeicoes = alojamento.Refeicoes;
            await _context.SaveChangesAsync();

            return Ok(alojamento);
        }

        // DELETE: api/alojamentos/DeletarAlojamentobyId
        [HttpDelete("DeletarAlojamentobyId")]
        public async Task<IActionResult> DeleteAlojamento(int id)
        {
            Alojamento? alojamento = await _context.Alojamentos.FirstOrDefaultAsync(a => a.Id == id);

            if (alojamento == null)
            {
                return NotFound();
            }

            _context.Alojamentos.Remove(alojamento);
            await _context.SaveChangesAsync();

            return Ok();
        }
    
    

   [HttpPost("{alojamentoId}/adicionar-morador")]
public async Task<IActionResult> AdicionarMoradorAoAlojamento(int alojamentoId, [FromBody] Morador morador)
{
    // Verifica se o alojamento existe
    var alojamento = await _context.Alojamentos.Include(a => a.Moradores).FirstOrDefaultAsync(a => a.Id == alojamentoId);
    if (alojamento == null)
    {
        return NotFound($"Alojamento com ID {alojamentoId} não encontrado.");
    }

    // Verifica se o alojamento atingiu a capacidade máxima
    if (alojamento.Moradores.Count >= alojamento.CapacidadeMaxima)
    {
        return BadRequest($"Alojamento com ID {alojamentoId} já atingiu sua capacidade máxima.");
    }

    // Verifica se o morador está ativo
    if (!morador.Ativo)
    {
        return BadRequest("Apenas moradores ativos podem ser alocados.");
    }

    // Remove referência circular do Alojamento
    morador.Alojamento = null;

    // Associa o morador ao alojamento
    morador.AlojamentoId = alojamentoId;
    _context.Moradores.Add(morador);
    await _context.SaveChangesAsync();

    return Ok(morador);
}


    // Listar moradores de um alojamento
    [HttpGet("{alojamentoId}/filtro")]
    public async Task<ActionResult<IEnumerable<Morador>>> ListarMoradoresDoAlojamento(int alojamentoId)
    {
        // Verifica se o alojamento existe
        var alojamento = await _context.Alojamentos.Include(a => a.Moradores).FirstOrDefaultAsync(a => a.Id == alojamentoId);
        if (alojamento == null)
        {
            return NotFound($"Alojamento com ID {alojamentoId} não encontrado.");
        }

        // Retorna a lista de moradores do alojamento
        return Ok(alojamento.Moradores);
    }
   [HttpPost("{alojamentoId}/mudarmorador")]
public async Task<IActionResult> MudarAlojamentoOuFilaDeEspera([FromBody] MudancaAlojamentoRequest request)
{
    // Iniciar uma transação para garantir que todas as alterações sejam atômicas
    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        // Buscar o morador com o alojamento associado
        var morador = await _context.Moradores.Include(m => m.Alojamento)
                                               .FirstOrDefaultAsync(m => m.Id == request.MoradorId);

        if (morador == null)
        {
            return NotFound(new { Message = "Morador não encontrado." });
        }

        // Buscar o novo alojamento
        var novoAlojamento = await _context.Alojamentos
                                            .FirstOrDefaultAsync(a => a.Id == request.NovoAlojamentoId);

        if (novoAlojamento == null)
        {
            return NotFound(new { Message = "Alojamento não encontrado." });
        }

        // Verificar se o alojamento tem capacidade
        var moradoresNoAlojamento = await _context.Moradores
                                                  .CountAsync(m => m.AlojamentoId == request.NovoAlojamentoId);

        if (moradoresNoAlojamento >= novoAlojamento.CapacidadeMaxima)
        {
            // Se o alojamento não tem capacidade, colocar na fila de espera
            var filaDeEspera = new FilaDeEspera
            {
                MoradorId = request.MoradorId,
                AlojamentoId = request.NovoAlojamentoId
            };

            // Adicionar o morador à fila de espera
            await _context.FilasDeEspera.AddAsync(filaDeEspera);
            await _context.SaveChangesAsync();

            // Commitar a transação
            await transaction.CommitAsync();

            return Ok(new { Message = "Morador colocado na fila de espera, pois o alojamento está cheio." });
        }
        else
        {
            // Se o alojamento tem capacidade, mover o morador para o novo alojamento
            morador.AlojamentoId = request.NovoAlojamentoId;

            // Atualizar o morador
            _context.Moradores.Update(morador);
            await _context.SaveChangesAsync();

            // Commitar a transação
            await transaction.CommitAsync();

            return Ok(new { Message = "Morador movido para o novo alojamento com sucesso." });
        }
    }
    catch (Exception ex)
    {
        // Se ocorrer algum erro, fazer rollback da transação
        await transaction.RollbackAsync();
        return StatusCode(500, new { Message = "Erro ao mover o morador: " + ex.Message });
    }
}


[HttpGet("{alojamentoId}/fila-de-espera")]
public async Task<IActionResult> ListarFilaDeEspera(int alojamentoId)
{
    var filaDeEspera = await _context.FilasDeEspera
        .Where(f => f.AlojamentoId == alojamentoId)
        .OrderBy(f => f.DataEntrada)
        .Include(f => f.Morador) // Inclui informações do morador, se necessário
        .ToListAsync();

    if (!filaDeEspera.Any())
    {
        return Ok($"Nenhum morador está na fila de espera para o alojamento com ID {alojamentoId}.");
    }

    return Ok(filaDeEspera);
}


}
}
 
