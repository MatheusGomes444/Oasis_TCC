namespace OasisApi.Models;
public class FilaDeEspera
{
    public int Id { get; set; }
    public int MoradorId { get; set; }
    public int AlojamentoId { get; set; }
    public DateTime DataEntrada { get; set; }

    // Relacionamentos
    public Morador? Morador { get; set; }
    public Alojamento? Alojamento { get; set; }
}
