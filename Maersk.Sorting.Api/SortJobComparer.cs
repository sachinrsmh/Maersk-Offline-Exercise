using System;
using System.Collections.Generic;

namespace Maersk.Sorting.Api
{
    public class SortJobComparer : IEqualityComparer<SortJob>
    {
        public bool Equals(SortJob? x, SortJob? y)
        {
            if (x == null || y == null)
                return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(SortJob? obj)
        {
            if (obj == null)
                return -1;

            return obj.Id.GetHashCode();

        }
    }
}