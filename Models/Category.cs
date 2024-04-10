namespace ITExpertTEST.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }

        public Category ParentCategory { get; set; }
        public ICollection<Category> Subcategories { get; set; }
        public ICollection<FilmCategory> FilmCategories { get; set; }
    }
}
