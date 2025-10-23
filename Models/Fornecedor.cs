/*
 * Models/Fornecedor.cs
 * Entidade principal que representa o Fornecedor.
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Garanta que o namespace é o nome do seu projeto.Models
namespace DESAFIOCRUD.Models
{
    public class Fornecedor
    {
        [Key] // Define como Chave Primária
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo CNPJ é obrigatório")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 dígitos")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O CNPJ deve conter apenas números")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "O campo Segmento é obrigatório")]
        public Segmento Segmento { get; set; } // Referencia o Enum

        [Required(ErrorMessage = "O campo CEP é obrigatório")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O CEP deve ter 8 dígitos")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O CEP deve conter apenas números")]
        public string Cep { get; set; }

        [StringLength(255, ErrorMessage = "O Endereço deve ter no máximo 255 caracteres")]
        public string Endereco { get; set; }

        // Campo para o Desafio Opcional (Foto)
        [Display(Name = "Caminho da Foto")]
        public string? FotoCaminho { get; set; }

        // Propriedade para o Upload
        [NotMapped] // Informa ao EF para não criar esta coluna no banco
        [Display(Name = "Foto")]
        public IFormFile? FotoArquivo { get; set; }
    }
}