using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using CarritoSosua.Models;

namespace CarritoSosua.DAL
{
    public class CarritoSosuaContext : DbContext
    {
        public DbSet<Publicador> Publicadores { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Localidad> Localidades { get; set; }
        public DbSet<PublicadorTurno> PublicadorTurnos { get; set; } 
    }
}
