using System.ComponentModel.DataAnnotations;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models.ViewModels
{
    public class PersonaViewModel
    {
        [Display(Name = "Cédula")]
        [Range(1, long.MaxValue, ErrorMessage = "La cédula debe ser un número positivo.")]
        public long Cc { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(45, ErrorMessage = "El nombre admite máximo 45 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(45, ErrorMessage = "El apellido admite máximo 45 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Display(Name = "Género")]
        [Required(ErrorMessage = "El género es obligatorio.")]
        [RegularExpression("^[MF]$", ErrorMessage = "El género debe ser M o F.")]
        public string Genero { get; set; } = string.Empty;

        [Display(Name = "Edad")]
        [Range(0, 150, ErrorMessage = "La edad debe estar entre 0 y 150.")]
        public int? Edad { get; set; }


        public static PersonaViewModel FromEntity(Persona persona) => new()
        {
            Cc = persona.Cc,
            Nombre = persona.Nombre,
            Apellido = persona.Apellido,
            Genero = persona.Genero,
            Edad = persona.Edad
        };

        public Persona ToEntity() => new()
        {
            Cc = Cc,
            Nombre = Nombre,
            Apellido = Apellido,
            Genero = Genero,
            Edad = Edad
        };
    }
}
