namespace RenzGrandWeddingblazor.ph.Data.Entities
{
    public partial class Title
    {
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public string TItleDescription { get; set; }
        public int? ProductLineId { get; set; }
        public ProductLine ProductLine { get; set; }
    }
}
