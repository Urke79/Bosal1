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
        private BosalMontazeDbContext _context { get; }

        public JobsRepository(BosalMontazeDbContext context)
        {
            _context = context;
        }

        public IEnumerable<JobsCountForSpecificDate> GetCalendarEventsData(int month, int year)
        {
            var dateCountList = new List<JobsCountForSpecificDate>();

            // Get data from the db
            var query = _context.Montaze.Where(x => x.Datum.Month == month && x.Datum.Year == year).GroupBy(x => x.Datum,
                x => x, (key, group) => new { Date = key, Count = group.Count() });

            var calendarEventsNumberPerDate = query.ToList();

            // put data into domain model
            foreach (var item in calendarEventsNumberPerDate)
            {
                var jobDate = new JobsCountForSpecificDate { Date = string.Format("{0:MM-dd-yyyy}", item.Date), Count = item.Count };
                dateCountList.Add(jobDate);
            }

            return dateCountList;
        }

        public IEnumerable<Montaza> GetCalandearEventsForSpecificDate(DateTime date)
        {
            var calendarEvents = _context.Montaze.Where(m => m.Datum == date).ToList();

            return calendarEvents;
        }

        public Montaza GetJobBy(int id)
        {
            var montaza = _context.Montaze.Find(id);

            return montaza;
        }

        public bool DeleteJob(int id)
        {
            var isDeleted = false;
            var montaza = _context.Montaze.Find(id);

            if (montaza != null)
            {
                _context.Montaze.Remove(montaza);
                isDeleted = Save();
            }

            return isDeleted;
        }

        public bool AddJob(Montaza montaza)
        {
            _context.Montaze.Add(montaza);
            var isAdded = Save();

            return isAdded;
        }

        public bool EditJob(Montaza montaza)
        {
            var isUpdated = false;

            var mon = _context.Montaze.Find(montaza.MontazaId);

            if (mon != null)
            {
                // update properties
                mon.Radnik = montaza.Radnik;
                mon.Adresa = montaza.Adresa;
                mon.Vreme = montaza.Vreme;

                _context.Entry(mon).State = System.Data.Entity.EntityState.Modified;
                isUpdated = Save();
            }

            return isUpdated;
        }

        private bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
