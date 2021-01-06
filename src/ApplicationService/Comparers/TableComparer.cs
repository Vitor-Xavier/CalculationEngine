using Crosscutting.DTO.DynamicSearch;
using System;
using System.Collections.Generic;

namespace ApplicationService.Comparers
{
    public class TableComparer : IEqualityComparer<Table>
    {
        public bool Equals(Table x, Table y) => x is not null && y is not null && x.Name == y.Name;

        public int GetHashCode(Table obj) => HashCode.Combine(obj?.Name);
    }
}
