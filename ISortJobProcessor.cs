﻿using System;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public interface ISortJobProcessor
    {
        Task<SortJob> Process(SortJob job);
        Task<SortJob> EnqueueJob(int[] jibList);
        Task<SortJob> GetJob();
        Task<SortJob> GetJobByID(Guid id);

    }
}