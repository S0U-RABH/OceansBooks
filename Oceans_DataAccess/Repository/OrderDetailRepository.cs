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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetail OrderDetail)
        {
            _db.OrderDetails.Update(OrderDetail);
        }
    }
}
