using DataAccess.Api;
using DataAccess.Entities;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Diagnostics;

namespace DataAccess
{
    
    public class JobsRepository : IJobsRepository
    {

        public IEnumerable<JobsCountForSpecificDate> GetCalendarEventsData(int month, int year)
        {
            var dateCountList = new List<JobsCountForSpecificDate>();

            using (var db = new BosalMontazeDbContext())
            {
                // Get data from the db
                var query = db.Montaze.Where(x => x.Datum.Month == month && x.Datum.Year == year).GroupBy(x => x.Datum,
                    x => x, (key, group) => new { Date = key, Count = group.Count() });

                var calendarEventsNumberPerDate = query.ToList();

                // put data into domain model
                foreach (var item in calendarEventsNumberPerDate)
                {
                    var jobDate = new JobsCountForSpecificDate { Date = string.Format("{0:MM-dd-yyyy}", item.Date), Count = item.Count };
                    dateCountList.Add(jobDate);
                }
            }
           
            return dateCountList;
        }

        public IEnumerable<Montaza> GetCalandearEventsForSpecificDate(DateTime date)
        {
            var calendarEvents = Enumerable.Empty<Montaza>();

            using (var db = new BosalMontazeDbContext())
            {
                 calendarEvents = db.Montaze.Where(m => m.Datum == date).ToList();
            }

            return calendarEvents;
        }

        public Montaza GetJobBy(int id)
        {
            Montaza montaza;

            using (var db = new BosalMontazeDbContext())
            {
                montaza = db.Montaze.Find(id);
            }
            
            return montaza;
        }

        public bool DeleteJob(int id)
        {
            var isDeleted = false;

            using (var db = new BosalMontazeDbContext())
            {
                var montaza = db.Montaze.Find(id);
                if (montaza != null)
                {
                    db.Montaze.Remove(montaza);
                    db.SaveChanges();
                    isDeleted = true;
                }
            }

            return isDeleted;
        }

        public bool AddJob(Montaza montaza)
        {
            var isSaved = false;

            using (var db = new BosalMontazeDbContext())
            {
                db.Montaze.Add(montaza);
                db.SaveChanges();
                isSaved = true;
            }

            return isSaved;
        }

        public bool EditJob(Montaza montaza)
        {
            var isUpdated = false;

            using (var db = new BosalMontazeDbContext())
            {
                var mon = db.Montaze.Find(montaza.MontazaId);

                if (mon != null)
                {
                    // update properties
                    mon.Radnik = montaza.Radnik;
                    mon.Adresa = montaza.Adresa;
                    mon.Vreme = montaza.Vreme;

                    db.Entry(mon).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    isUpdated = true;
                }                
            }

            return isUpdated;
        }
    }
}
