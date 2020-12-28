namespace Models.Entities
{
    public class RoteiroSelecaoItemContribuinte : RoteiroSelecaoItem
    {
        public virtual Contribuinte Contribuinte { get; set; }
    }
}
