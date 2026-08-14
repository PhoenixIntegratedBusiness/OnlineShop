using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<bool> AddtocardAsync(int id,int userid);
        Task<Order> GetCartItemsAsync(int userid);
        Task<int> CountShopCardAsync(int userid);
        Task DeleteFromCartAsync(int userid, int productid);

    }
}
