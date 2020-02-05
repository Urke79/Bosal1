using DataAccess.Api;
using Domain;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace BosalMontaze.Controllers
{
    [Authorize]
    public class JobsController : ApiController
    {
        private IJobsRepository _jobRepository;

        public JobsController(IJobsRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<JobsCountForSpecificDate> Get(int month, int year)
        {
            return _jobRepository.GetCalendarEventsData(month, year);
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Montaza> Get(DateTime date)
        {
            return _jobRepository.GetCalandearEventsForSpecificDate(date);
        }

        [AllowAnonymous]
        [HttpGet]
        public Montaza Get(int id)
        {
            return _jobRepository.GetJobBy(id);
        }

        [AllowAnonymous]
        [HttpPost]
        public bool Post(Montaza montaza)
        {
            return _jobRepository.AddJob(montaza);
        }

        [AllowAnonymous]
        [HttpPut]
        public bool Put([FromBody]Montaza montaza)
        {
            return _jobRepository.EditJob(montaza);
        }

        [AllowAnonymous]
        [HttpDelete]
        public bool Delete(int id)
        {
            return _jobRepository.DeleteJob(id);
        }
    }
}
