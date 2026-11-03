using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.Models.Entities;


namespace personapi_dotnet.Models.ViewModels
{
    public class TelefonoViewModel
    {
        [Required(ErrorMessage = "El número es obligatorio.")]
        [StringLength(15, ErrorMessage = "El número admite máximo 15 caracteres.")]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Use solo dígitos (se permite un + al inicio).")]
        [Display(Name = "Número")]
        public string Num { get; set; } = string.Empty;

        [Required(ErrorMessage = "El operador es obligatorio.")]
        [StringLength(45, ErrorMessage = "El operador admite máximo 45 caracteres.")]
        [Display(Name = "Operador")]
        public string Oper { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seleccione un dueño.")]
        [Range(1, long.MaxValue, ErrorMessage = "La cédula del dueño debe ser mayor que 0.")]
        [Display(Name = "Dueño (cédula)")]
        public long? Duenio { get; set; }

        // Solo para mostrar en Index/Details/Delete.
        [ValidateNever]
        [JsonIgnore]
        [Display(Name = "Dueño")]
        public string? DuenioNombre { get; set; }

        // Solo para el desplegable de las vistas Create/Edit. No viaja en la API.
        [ValidateNever]
        [JsonIgnore]
        public IEnumerable<SelectListItem>? Personas { get; set; }

        public static TelefonoViewModel FromEntity(Telefono t) => new()
        {
            Num = t.Num,
            Oper = t.Oper,
            Duenio = t.Duenio,
            DuenioNombre = t.DuenioNavigation is null
                ? null
                : $"{t.DuenioNavigation.Nombre} {t.DuenioNavigation.Apellido}"
        };

        public Telefono ToEntity() => new()
        {
            Num = Num.Trim(),
            Oper = Oper.Trim(),
            Duenio = Duenio!.Value
        };
    }
}
