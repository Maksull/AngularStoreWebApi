﻿using Core.Contracts.Controllers.Orders;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }

        public IEnumerable<Order> GetOrders()
        {
            if (_unitOfWork.Order.Orders.Any())
            {
                return _unitOfWork.Order.Orders
                    .Include(o => o.Lines)!
                        .ThenInclude(l => l.Product);
            }

            return Enumerable.Empty<Order>();
        }

        public async Task<Order?> GetOrder(long id)
        {
            if (_unitOfWork.Order.Orders.Any())
            {
                Order? order = await _unitOfWork.Order.Orders
                    .Include(o => o.Lines)!
                        .ThenInclude(l => l.Product)
                        .FirstOrDefaultAsync(o => o.OrderId == id);

                if (order != null)
                {
                    return order;
                }
            }

            return null;
        }

        public async Task<Order> CreateOrder(CreateOrderRequest createOrder)
        {
            var order = _mapper.Map<Order>(createOrder);

            await _unitOfWork.Order.CreateOrderAsync(order);

            _emailService.Send(new(order.Email, "You have made an order", $"Your orderId is {order.OrderId}"));

            return order;
        }

        public async Task<Order?> UpdateOrder(UpdateOrderRequest updateOrder)
        {
            var order = _mapper.Map<Order>(updateOrder);

            if (await _unitOfWork.Order.Orders.ContainsAsync(order))
            {
                await _unitOfWork.Order.UpdateOrderAsync(order);

                return order;
            }

            return null;
        }

        public async Task<Order?> DeleteOrder(long id)
        {
            if (_unitOfWork.Order.Orders.Any())
            {
                Order? order = await _unitOfWork.Order.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

                if (order != null)
                {
                    await _unitOfWork.Order.DeleteOrderAsync(order);

                    return order;
                }
            }

            return null;
        }
    }
}
