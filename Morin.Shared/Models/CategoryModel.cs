namespace Morin.Shared.Models;

public class CategoryModel : Model
{
    public string? Title { get; set; }
}
public class MovieCategoryModel : CategoryModel { }
public class TVCategoryModel : CategoryModel { }
public class RecordCategoryModel : CategoryModel { }
public class AnimeCategoryModel : CategoryModel { }
public class EntertainmentCategoryModel : CategoryModel { }
public class MarkCategoryModel : CategoryModel { }
public class ReginCategoryModel : CategoryModel { }
public class YearCategoryModel : CategoryModel { }
public class RankCategoryModel : CategoryModel { }
