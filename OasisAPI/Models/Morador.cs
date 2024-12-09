using OasisApi.Models;
public class Morador
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string RG { get; set; } = string.Empty;
    public int Telefone { get; set; }
    public string Endereco { get; set; } = string.Empty;
    public int Idade { get; set; }
    public int Datanascimento { get; set; }
    public string Nacionalidade { get; set; } = string.Empty;
    public string Observacoes { get; set; } = string.Empty;
    public bool Ativo { get; set; }

    // Relacionamento com Alojamento
    public int AlojamentoId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public Alojamento? Alojamento { get; set; }
}
