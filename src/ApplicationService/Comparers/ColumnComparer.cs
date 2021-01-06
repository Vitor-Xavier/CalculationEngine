using Crosscutting.DTO.DynamicSearch;
using System;
using System.Collections.Generic;

namespace ApplicationService.Comparers
{
    public class ColumnComparer : IEqualityComparer<Column>
    {
        public bool Equals(Column x, Column y) => x is not null && y is not null && x.Name == y.Name;

        public int GetHashCode(Column obj) => HashCode.Combine(obj?.Name);
    }
}
