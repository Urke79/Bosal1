using DataAccess.Entities;
using Domain;
using System;
using System.Collections.Generic;

namespace DataAccess.Api
{
    public interface IJobsRepository
    {
        IEnumerable<JobsCountForSpecificDate> GetCalendarEventsData(int month, int year);
        IEnumerable<Montaza> GetCalandearEventsForSpecificDate(DateTime date);
        Montaza GetJobBy(int id);

        bool DeleteJob(int id);

        bool AddJob(Montaza montaza);
        bool EditJob(Montaza montaza);
    }
}
