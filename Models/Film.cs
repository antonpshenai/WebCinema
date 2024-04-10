namespace ITExpertTEST.Models
{
    public class Film
    {
        public int FilmId { get; set; }
        public string FilmName { get; set; }
        public string FilmDirector { get; set; }
        public DateTime Release { get; set; }

        public ICollection<FilmCategory> FilmCategories { get; set; }

    }
}
