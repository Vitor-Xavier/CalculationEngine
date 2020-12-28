namespace Models.Entities
{
    public class RoteiroSelecaoItemFisico : RoteiroSelecaoItem
    {
        public virtual Fisico Fisico { get; set; }
    }
}
