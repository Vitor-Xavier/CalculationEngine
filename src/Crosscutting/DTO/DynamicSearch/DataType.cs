using System.ComponentModel;

namespace Crosscutting.DTO.DynamicSearch
{
    public enum DataType
    {
        None,

        [Description("texto")]
        Text,

        [Description("número")]
        Number,

        [Description("decimal")]
        Decimal,

        [Description("data")]
        Date,

        [Description("boleano")]
        Boolean,

        [Description("lista")]
        List,

        [Description("objeto")]
        Object,
    }
}
