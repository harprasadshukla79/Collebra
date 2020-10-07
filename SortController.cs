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

        public SortController(ISortJobProcessor sortJobProcessor)
        {
            _sortJobProcessor = sortJobProcessor;
        }

        [HttpPost("run")]
        [Obsolete("This executes the sort job asynchronously. Use the asynchronous 'EnqueueJob' instead.")]
        public async Task<ActionResult<SortJob>> EnqueueAndRunJob(int[] values)
        {
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            var completedJob = await _sortJobProcessor.Process(pendingJob);
            if (completedJob != null)
            {
                return Ok(completedJob);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<SortJob>> EnqueueJob(int[] values)
        {
            var completedJob = await _sortJobProcessor.EnqueueJob(values);
            if (completedJob != null)
            {
                return Ok(completedJob);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<SortJob[]>> GetJobs()
        {
            var completedJob = await _sortJobProcessor.GetJob();
            if (completedJob != null)
            {
                return Ok(completedJob);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{jobId}")]
        public async Task<ActionResult<SortJob>> GetJob(Guid jobId)
        {
            var completedJob = await _sortJobProcessor.GetJobByID(jobId);
            if (completedJob != null)
            {
                return Ok(completedJob);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
