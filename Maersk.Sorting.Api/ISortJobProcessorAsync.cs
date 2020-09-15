using System;
using System.Collections.Generic;

namespace Maersk.Sorting.Api
{
    public interface ISortJobProcessorAsync
    {
        public List<SortJob> PendingJobs { get;  }
        public List<SortJob> CompletedJobs { get; }
        public void EnqueueJob(SortJob objJob);
        public List<SortJob> GetJobs();
        public SortJob? GetJob(Guid idGuid);
    }
}
