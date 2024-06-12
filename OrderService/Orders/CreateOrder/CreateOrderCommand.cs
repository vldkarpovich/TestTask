using DAL.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Orders.CreateOrder;
    public record CreateOrderCommand(string Email, List<OrderItem> Items) : IRequest<CreateOrderResponse>;
