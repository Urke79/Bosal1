using DataAccess.Api;
using DataAccess.Entities;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System;
using Logging;

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

            try
            {
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
            }
            catch (Exception e)
            {
                Log.Error("Error getting the calendar events. Error message: " + e.Message, e);
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
                try
                {
                    _context.Montaze.Remove(montaza);
                    isDeleted = Save();
                }
                catch (Exception e)
                {
                    Log.Error("Deleting of montaza with id of " + id + " failed. Error message: " + e.Message, e);
                }           
            }

            return isDeleted;
        }

        public bool AddJob(Montaza montaza)
        {
            var isAdded = false;

            try
            {
                _context.Montaze.Add(montaza);
                isAdded = Save();
            }
            catch (Exception e)
            {
                Log.Error("Adding of montaza failed. Error message: " + e.Message, e);
            }
            
            return isAdded;
        }

        public bool EditJob(Montaza montaza)
        {
            var isUpdated = false;

            var mon = _context.Montaze.Find(montaza.MontazaId);

            if (mon != null)
            {
                try
                {
                    // update properties
                    mon.Radnik = montaza.Radnik;
                    mon.Adresa = montaza.Adresa;
                    mon.Vreme = montaza.Vreme;
                    throw new Exception("Buljica moja");

                    _context.Entry(mon).State = System.Data.Entity.EntityState.Modified;
                    isUpdated = Save();
                }
                catch (Exception e)
                {
                    Log.Error("Updating failed for montaza with ID: " + mon.MontazaId + "Error message: " + e.Message, e);
                }             
            }

            return isUpdated;
        }

        private bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
