using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarritoSosua.Models
{
    public class Turno
    {
        public int Id { get; set; }
        [Display(Name = "Dia de semana")]
        public DayOfWeek DayOfWeek { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm tt}")]
        [Display(Name = "Hora de")]
        public DateTime TimeFrom { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hora hasta")]
        public DateTime TimeTo { get; set; }
        [Required]
        [Display(Name = "Puesto")]
        public int LocalidadId { get; set; }

        public virtual Localidad Localidad { get; set; }
        public virtual ICollection<PublicadorTurno> PublicadorTurnos { get; set; }
    }
}
