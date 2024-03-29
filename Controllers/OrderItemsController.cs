﻿using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreatEmpty.Data;
using DutchTreatEmpty.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreatEmpty.Controllers
{
    [Route("/api/orders/{orderid}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<OrderItemsController> logger;
        private readonly IMapper mapper;

        public OrderItemsController(IDutchRepository repository,
            ILogger<OrderItemsController> logger,
            IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            var order = repository.GetOrderById(User.Identity.Name, orderId);
            if (order != null)
            {
                return Ok(mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
            }
            return NotFound();
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int orderId, int id)
        {
            var order = repository.GetOrderById(User.Identity.Name, orderId);
            if (order != null)
            {
                var orderItem = order.Items.Where(i => i.Id == id).FirstOrDefault();
                if (orderItem != null)
                {
                    return Ok(mapper.Map<OrderItem, OrderItemViewModel>(orderItem));
                }
            }
            return NotFound();
        }
    }
}
