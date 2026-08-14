using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

      


        #region DeleteFromCartAsync
        public async Task DeleteFromCartAsync(int userid, int productid)
        {
            await _orderRepository.DeleteProductfromcardAsync(productid, userid);
            await _orderRepository.SaveChangeAsync();
        }
        #endregion

        #region CountShopCardAsync
        public async Task<int> CountShopCardAsync(int userid)
        {
            return await _orderRepository.CountShopCardAsync(userid);
        }
        #endregion

        #region GetCartItemsAsync
        public async Task<Order> GetCartItemsAsync(int userid)
        {
            return await _orderRepository.GetCartItemsAsync(userid);
        }
        #endregion

        #region AddtocardAsync
        public async Task<bool> AddtocardAsync(int id, int userid)
        {
            var order = await _orderRepository.GetUserOrderAsync(userid);

            var productPrice = await _orderRepository.GetProductPriceById(id);

            if (order == null)
            {
                order = new Order
                {
                    CreateDate = DateTime.Now,
                    isDelete = false,
                    UserId = userid,
                    IsFinaly = false
                };

                await _orderRepository.AddOrderAsync(order);
                await _orderRepository.SaveChangeAsync();
            }

            var detail = await _orderRepository.OrdersDetailById(id, userid);

            if (detail == null)
            {
                var orderDetail = new OrderDetail
                {
                    CreateDate = DateTime.Now,
                    Count = 1,
                    isDelete = false,
                    Price = productPrice,
                    ProductId = id,
                    OrderId = order.OrderId
                };

                await _orderRepository.AddOrderDetailAsync(orderDetail);
            }
            else
            {
                detail.Count += 1;
            }
            await _orderRepository.SaveChangeAsync();
            return true;
        }

        #endregion
    }
}
