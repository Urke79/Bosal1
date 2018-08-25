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
        // GET api/Jobs
        [AllowAnonymous]
        [HttpGet]
        [Route("api/Jobs/GetEventsForMonth/{month}/{year}")]
        public IEnumerable<JobsCountForSpecificDate> Get(int month, int year)
        {
           return  _jobRepository.GetCalendarEventsData(month, year);                   
        }

        // GET api/Jobs/date
        [AllowAnonymous]
        [HttpGet]
        [Route ("api/Jobs/{date}")]
        public IEnumerable<Montaza> Get(DateTime date)
        {
            return _jobRepository.GetCalandearEventsForSpecificDate(date);
        }

        // GET api/Jobs/GetSingleJob/id
        [AllowAnonymous]
        [HttpGet]
        [Route("api/Jobs/GetSingleJob/{id}")]
        public Montaza Get(int id)
        {
            return _jobRepository.GetJobBy(id);
        }

        // POST api/Jobs/AddJob
        [AllowAnonymous]
        [HttpPost]
        [Route("api/Jobs/AddJob")]
        public bool Post(Montaza montaza)
        {
            return _jobRepository.AddJob(montaza);
        }

        // PUT api/Jobs/EditJob
        [AllowAnonymous]
        [HttpPut]
        [Route("api/Jobs/EditJob")]
        public bool Put([FromBody]Montaza montaza)
        {
            return _jobRepository.EditJob(montaza);
        }

        // DELETE api/Jobs/DeleteJob/id
        [AllowAnonymous]
        [HttpDelete]
        [Route("api/Jobs/DeleteJob/{id}")]
        public bool Delete(int id)
        {
            return _jobRepository.DeleteJob(id);
        }
    }
}
