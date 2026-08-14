using Domain.Interfaces;
using Domain.Model;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MyContext _context;
        public OrderRepository(MyContext context)
        {
            _context = context;
        }

        #region DeleteProductfromcardAsync
        public async Task DeleteProductfromcardAsync(int productId, int userid)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o =>o.UserId == userid &&!o.IsFinaly);

            if (order == null)
                return;

            var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync
                                                          (od =>od.OrderId == order.OrderId &&
                                                           od.ProductId == productId);

            if (orderDetail == null)
                return;

            _context.OrderDetails.Remove(orderDetail);
        }
        #endregion


        #region CountShopCardAsync
        public async Task<int> CountShopCardAsync(int userid)
        {
            return await _context.Orders
                 .Where(u => u.UserId == userid && !u.IsFinaly)
                 .SelectMany(u => u.OrderDetails)
                 .SumAsync(u => u.Count);
        }
        #endregion

        #region GetCartItemsAsync
        public async Task<Order> GetCartItemsAsync(int userid)
        {
            return await _context.Orders.Include(o => o.OrderDetails).ThenInclude(d => d.Product)
                   .FirstOrDefaultAsync(o => o.UserId == userid && !o.IsFinaly);
        }
        #endregion

        #region GetProductPriceById
        public async Task<int> GetProductPriceById(int id)
        {
            return await _context.Products.Where(p => p.ProductId == id).Select(p => p.Price).FirstOrDefaultAsync();
        }
        #endregion

        #region AddOrderAsync
        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }
        #endregion

        #region AddOrderDetailAsync
        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
        }

        #endregion

        #region GetUserOrderAsync
        public async Task<Order> GetUserOrderAsync(int userid)
        {
            return await _context.Orders.FirstOrDefaultAsync(u => u.UserId == userid && !u.IsFinaly);
        }
        #endregion

        #region OrdersDetailById
        public async Task<OrderDetail> OrdersDetailById(int id, int userid)
        {
            return await _context.OrderDetails.FirstOrDefaultAsync(u => u.ProductId == id && u.Order.UserId == userid);
        }
        #endregion

        #region SaveChangeAsync
        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
