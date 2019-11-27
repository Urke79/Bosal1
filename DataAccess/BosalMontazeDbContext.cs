using DataAccess.EntityModels;
using System.Data.Entity;
using System.Diagnostics;

namespace DataAccess.Entities
{
    public class BosalMontazeDbContext : DbContext
    {
        public BosalMontazeDbContext() : base("BosalMontazeDb")
        {
            Database.Log = sql => Debug.Write(sql);
        }

        public DbSet<MontazaEntity> Montaze { get; set; }

       
    }
}