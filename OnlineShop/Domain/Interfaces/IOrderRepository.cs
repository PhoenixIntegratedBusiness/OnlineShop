using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetUserOrderAsync(int userid);
        Task<OrderDetail> OrdersDetailById(int id,int userid);
        Task SaveChangeAsync();
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task AddOrderAsync(Order order);
        Task<int> GetProductPriceById(int id);
        Task<Order> GetCartItemsAsync(int userid);
        Task<int>  CountShopCardAsync(int userid);
        Task DeleteProductfromcardAsync(int ProductId,int userid);




    }
}
