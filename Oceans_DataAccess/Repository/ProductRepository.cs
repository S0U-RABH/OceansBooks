using Oceans_DataAccess.Data;
using Oceans_DataAccess.Repository.IRepository;
using Oceans_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Oceans_DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var ObjfromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (ObjfromDb != null)
            {
                ObjfromDb.Title = obj.Title;
                ObjfromDb.Description = obj.Description;
                ObjfromDb.ISBN = obj.ISBN;
                ObjfromDb.Author = obj.Author;
                ObjfromDb.Price = obj.Price;
                ObjfromDb.ListPrice = obj.ListPrice;
                ObjfromDb.Price50 = obj.Price50;
                ObjfromDb.Price100 = obj.Price100;
                ObjfromDb.CategoryId = obj.CategoryId;
                if(obj.ImageUrl != null)
                {
                    ObjfromDb.ImageUrl = obj.ImageUrl;
                }

            }
        }
    }
}
