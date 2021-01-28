
namespace PTMS.Infrastructure.Postgre.Mappers
{
    public static class CategoryMapper
    {
        public static Core.Models.Category AsDomain(this Data.DbEntities.DbCategory category) => new Core.Models.Category
        {
            Id = category.Id,
            Description = category.Description,
            Name = category.Name
        };


        public static Data.DbEntities.DbCategory AsEntity(this Core.Models.Category category) => new Data.DbEntities.DbCategory
        {
            Id = category.Id,
            Description = category.Description,
            Name = category.Name
        };
        
    }
}
