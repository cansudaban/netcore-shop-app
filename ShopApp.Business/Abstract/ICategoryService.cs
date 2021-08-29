using ShopApp.Entity;
using System.Collections.Generic;

namespace ShopApp.Business.Abstract
{
    public interface ICategoryService:IValidator<Category>
    {
        Category GetById(int id);
        Category GetByIdWithProducts(int categoryId);
        List<Category> GetAll();
        void Create(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
        void DeleteFromCategory(int productId, int categoryId);
    }
}
