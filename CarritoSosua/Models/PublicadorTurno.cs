using System;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace CarritoSosua.Models
{
    public class PublicadorTurno
    {
        public int Id { get; set; }
        public int PublicadorId { get; set; }
        public int TurnoId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha")]
        public DateTime Day { get; set; }
        public bool Disponible { get; set; }

        public virtual Publicador Publicador { get; set; }
        public virtual Turno Turno { get; set; }
    }
}
