using System.Collections.Generic;

namespace Crosscutting.DTO.DynamicSearch
{
    public class Table
    {
        public string Name { get; set; }

        public string Class { get; set; }

        public virtual ICollection<Column> Columns { get; set; } = new HashSet<Column>();
    }
}
