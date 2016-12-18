using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarritoSosua.Models
{
    public class Publicador
    {
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        public virtual ICollection<PublicadorTurno> PublicadorTurnos { get; set; }

    }
}
