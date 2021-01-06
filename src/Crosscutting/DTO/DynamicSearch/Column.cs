namespace Crosscutting.DTO.DynamicSearch
{
    public class Column
    {
        public string Name { get; set; }

        public string Property { get; set; }

        public bool IsPrimaryKey { get; set; }

        public DataType DataType { get; set; }
    }
}
