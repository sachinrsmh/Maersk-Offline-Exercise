﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    
    public class AsyncSortJobProcessor  : ISortJobProcessorAsync
    {
        private readonly List<SortJob>  _pendingJobs = new List<SortJob>();
        private readonly List<SortJob>  _completedJobs = new List<SortJob>();
        private readonly ISortJobProcessor _sortJobProcessor;
        

        public AsyncSortJobProcessor(ISortJobProcessor sortJobProcessor)
        {
            _sortJobProcessor = sortJobProcessor;
        }

        private void ProcessJob(SortJob objJob)
        {
            _completedJobs.Add(_sortJobProcessor.Process(objJob).Result );
        }
        public void EnqueueJob(SortJob objJob)
        {
            _pendingJobs.Add(objJob);
            Task.Factory.StartNew(() =>
                {
                    ProcessJob(objJob);
                }
            );

        }

        public List<SortJob> PendingJobs
        {
            get => _pendingJobs;
         }
        public List<SortJob> CompletedJobs
        {
            get => _completedJobs;
        }

        public List<SortJob> GetJobs()
        {
            SortJobComparer sortJobComparer = new SortJobComparer();

            var resAllJob = CompletedJobs.Union(PendingJobs, sortJobComparer).ToList(); 
            return resAllJob;
        }

        public SortJob? GetJob(Guid idGuid)
        {
            //First loop through the Completed Jobs
            foreach (var jobItem in CompletedJobs)
            {
                if (jobItem.Id == idGuid) return jobItem;
            }

            //Now loop through the Pending Jobs
            foreach (var jobItem in PendingJobs)
            {
                if (jobItem.Id == idGuid) return jobItem;
            }
            return null;
        }
    }


}
