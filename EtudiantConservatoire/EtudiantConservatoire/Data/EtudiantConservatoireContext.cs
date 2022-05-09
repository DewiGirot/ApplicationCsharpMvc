#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EtudiantConservatoire.Models;

namespace EtudiantConservatoire.Data
{
    public class EtudiantConservatoireContext : DbContext
    {
        public EtudiantConservatoireContext (DbContextOptions<EtudiantConservatoireContext> options)
            : base(options)
        {
        }

        public DbSet<EtudiantConservatoire.Models.Etudiant> Etudiant { get; set; }
    }
}
