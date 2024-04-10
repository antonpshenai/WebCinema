namespace ITExpertTEST.Models
{
    public class FilmCategory
    {
        public int FilmCategoryId { get; set; }
        public int FilmId { get; set; }
        public int CategoryId { get; set; }

        public Film Film { get; set; }
        public Category Category { get; set; }
    }
}
