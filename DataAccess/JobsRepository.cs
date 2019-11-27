using DataAccess.Api;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using Logging;
using DataAccess.EntityModels;
using Domain;
using AutoMapper;

namespace DataAccess
{

    public class JobsRepository : IJobsRepository
    {
        private BosalMontazeDbContext _context { get; }
        private IMapper mapper;

        public JobsRepository(BosalMontazeDbContext context)
        {
            InitializeMapper();
            _context = context;
        }

        private void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<MontazaEntity, Montaza>().ReverseMap();            
            });

            mapper = config.CreateMapper();
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

            return mapper.Map<List<Montaza>>(calendarEvents);
        }

        public Montaza GetJobBy(int id)
        {
            var montaza = _context.Montaze.Find(id);

            return mapper.Map<Montaza>(montaza);
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
                var montazaEntity = mapper.Map<MontazaEntity>(montaza);
                _context.Montaze.Add(montazaEntity);
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

            var montazaEntity = _context.Montaze.Find(montaza.MontazaId);

            if (montazaEntity != null)
            {
                try
                {
                    montazaEntity = mapper.Map(montaza, montazaEntity);                   
                    _context.Entry(montazaEntity).State = System.Data.Entity.EntityState.Modified;
                    isUpdated = Save();
                }
                catch (Exception e)
                {
                    Log.Error("Updating failed for montaza with ID: " + montazaEntity.MontazaId + "Error message: " + e.Message, e);
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
