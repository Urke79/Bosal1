using Domain;
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

        public DbSet<Montaza> Montaze { get; set; }

       
    }
}