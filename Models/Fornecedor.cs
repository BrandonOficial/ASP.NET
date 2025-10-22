/*
 * Models/Fornecedor.cs
 * Entidade principal que representa o Fornecedor.
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CadastroFornecedores.Models
{
    public class Fornecedor
    {
        [cite_start][Key] // Define como Chave Primária [cite: 13]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [cite_start][StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres")] // [cite: 14]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo CNPJ é obrigatório")]
        [cite_start][StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 dígitos")] // [cite: 15]
        [cite_start][RegularExpression("^[0-9]*$", ErrorMessage = "O CNPJ deve conter apenas números")] // [cite: 15]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "O campo Segmento é obrigatório")]
        public Segmento Segmento { get; set; } // 

        [Required(ErrorMessage = "O campo CEP é obrigatório")]
        [cite_start][StringLength(8, MinimumLength = 8, ErrorMessage = "O CEP deve ter 8 dígitos")] // [cite: 17]
        [cite_start][RegularExpression("^[0-9]*$", ErrorMessage = "O CEP deve conter apenas números")] // [cite: 17]
        public string Cep { get; set; }

        [cite_start][StringLength(255, ErrorMessage = "O Endereço deve ter no máximo 255 caracteres")] // [cite: 18]
        public string Endereco { get; set; }

        [cite_start]// Campo para o Desafio Opcional (Foto) 
        // Armazenaremos apenas o caminho da imagem
        [Display(Name = "Caminho da Foto")]
        public string? FotoCaminho { get; set; }

        // Propriedade de navegação (não mapeada) para o upload
        [NotMapped] // Informa ao EF para não criar esta coluna no banco
        [Display(Name = "Foto")]
        public IFormFile? FotoArquivo { get; set; }
    }
}