namespace FastAndroidAPI.Models
{
    using System;

    public partial class Category
    {
        public int CategoryID { get; set; }
        public int? ParentCategoryID { get; set; }
        public string Name { get; set; }
        public int? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public string Favicon { get; set; }
        public string ImgUrl { get; set; }
    }
}