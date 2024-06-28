namespace Esercizio_M4_W2_D5.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImgCover { get; set; }

        public string ShortDescription
        {
            get
            {
                if (Description.Length > 75)
                    return Description.Substring(0, 75) + "...";
                else
                    return Description;
            }
        }


    }
}
