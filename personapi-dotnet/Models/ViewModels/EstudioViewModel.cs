using System.ComponentModel.DataAnnotations;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models.ViewModels
{
    public class EstudioViewModel
    {
        [Display(Name = "Cédula")]
        [Range(1, long.MaxValue, ErrorMessage = "La cédula debe ser un número positivo.")]
        public long CcPer { get; set; }

        [Display(Name = "Profesión (Id)")]
        [Range(1, int.MaxValue, ErrorMessage = "El id de la profesión debe ser un número positivo.")]
        public int IdProf { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Fecha { get; set; }

        [Display(Name = "Universidad")]
        [StringLength(50, ErrorMessage = "La universidad admite máximo 50 caracteres.")]
        public string? Univer { get; set; }


        public static EstudioViewModel FromEntity(Estudio estudio) => new()
        {
            CcPer = estudio.CcPer,
            IdProf = estudio.IdProf,
            Fecha = estudio.Fecha,
            Univer = estudio.Univer
        };

        public Estudio ToEntity() => new()
        {
            CcPer = CcPer,
            IdProf = IdProf,
            Fecha = Fecha,
            Univer = Univer
        };
    }
}
