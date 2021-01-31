using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class CategoryService
    {
        private readonly Guid _userId;
        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateCategory(CategoryCreate model)
        {
            var entity =
                new Category()
                {
                   OwnerId = _userId,
                   CategoryName = model.CategoryName,
                   SectionName= model.SectionName

                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Categories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }
        public IEnumerable<CategoryListItems> GetCategories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Categories
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                        new CategoryListItems
                        {
                            CategoryName = e.CategoryName,
                            SectionName = e.SectionName,
                           
                        });
                return query.ToArray();
            }
        }
        public CategoryListItems GetCategoryById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Categories
                    .Where(e => e.OwnerId == _userId)
                    .Single(e => e.CategoryId == id);
                return
                    new CategoryListItems
                    {
                        CategoryID = entity.CategoryId,
                        CategoryName = entity.CategoryName,
                        SectionName = entity.SectionName,
                    };
            }
        }
        public bool UpdateCategory(CategoryEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx
                    .Categories
                    .Where(e => e.OwnerId == _userId)
                    .Single(e => e.CategoryId == model.CategoryId);

                entity.CategoryName = model.CategoryName;
                entity.CategoryId   = model.CategoryId;
                entity.SectionName = model.SectionName;
                return ctx.SaveChanges() == 1;
            }
        }
        public bool DeleteCategory(int categoryid)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                      .Categories
                      .Where(e => e.OwnerId == _userId)
                     .Single(e => e.CategoryId == categoryid);

                ctx.Categories.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }

    }
}
