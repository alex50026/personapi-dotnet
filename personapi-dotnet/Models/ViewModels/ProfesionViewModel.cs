using System.ComponentModel.DataAnnotations;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models.ViewModels
{
    public class ProfesionViewModel
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id debe ser un número mayor que 0.")]
        public int? Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(90, ErrorMessage = "El nombre admite máximo 90 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nom { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string? Des { get; set; }

        public static ProfesionViewModel FromEntity(Profesion p) => new()
        {
            Id = p.Id,
            Nom = p.Nom,
            Des = p.Des
        };

        public Profesion ToEntity() => new()
        {
            Id = Id!.Value,
            Nom = Nom.Trim(),
            Des = string.IsNullOrWhiteSpace(Des) ? null : Des.Trim()
        };
    }
}
