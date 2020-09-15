using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Controllers
{
    [ApiController]
    [Route("sort")]
    public class SortController : ControllerBase
    {
        private readonly ISortJobProcessor _sortJobProcessor;
        private readonly ISortJobProcessorAsync _sortJobProcessorAsync; 

        public SortController(ISortJobProcessor sortJobProcessor, ISortJobProcessorAsync sortJobProcessorAsync )
        {
            _sortJobProcessor = sortJobProcessor;
            _sortJobProcessorAsync = sortJobProcessorAsync;
        }

        [HttpPost("run")]
        [Obsolete("This executes the sort job asynchronously. Use the asynchronous 'EnqueueJob' instead.")]
        public async Task<ActionResult<SortJob>> EnqueueAndRunJob([FromForm]int[] values)
        {
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            var completedJob = await  _sortJobProcessor.Process(pendingJob);

            return Ok(completedJob);
        }

        [HttpPost]
        public ActionResult<SortJob> EnqueueJob([FromForm]int[] values)
        {
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            _sortJobProcessorAsync.EnqueueJob(pendingJob);
            return Ok(pendingJob);
        }

        [HttpGet]
        public ActionResult<SortJob[]> GetJobs()
        {
            return _sortJobProcessorAsync.GetJobs() .ToArray() ;

        }

        [HttpGet("{jobId}")]
        public ActionResult<SortJob> GetJob([FromRoute] Guid jobId)
        {
            SortJob?  sortJob = _sortJobProcessorAsync.GetJob(jobId);
            
            if (sortJob == null)
                return NotFound();

            return sortJob;
        }
    }
}
