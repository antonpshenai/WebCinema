namespace ITExpertTEST.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int FilmCount { get; set; }
        public int NestingLevel { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
