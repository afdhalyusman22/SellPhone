
using AutoMapper;
using Backend.API.Error;
using Backend.Application.Dto;
using Backend.Core.Entities;
using Backend.Core.Repositories.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public OrderController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            try
            {
                var items = await _repository.ListAsync<Order>();
                var result = _mapper.Map<List<OrderDTO>>(items);

                return Requests.Response(this, new ApiStatus(200), result, "");

            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }

        [HttpGet("detail/{id:int}")]
        public async Task<IActionResult> ListAsyncById(long id)
        {
            try
            {
                var items = await _repository.GetByIdAsync<Order>(id, x => x.OrderDetails);

                var result = _mapper.Map<OrderDTO>(items);
                if (result == null)
                {
                    return Requests.Response(this, new ApiStatus(404), result, "Data Not Found");
                }
                return Requests.Response(this, new ApiStatus(200), result, "");
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromBody] OrderAddDTO itemDTO)
        {
            try
            {
                bool Added = false;
                var Message = "";
                var product = await _repository.GetByIdAsync<Product>(itemDTO.ProductId);
                if (product.Stock < itemDTO.Quantity)
                    Requests.Response(this, new ApiStatus(404), null, "Product stock less than your request quantity");

                var checkOrder = _repository.ListWithWhere<Order>(x => x.CustomerId == itemDTO.CustomerId && x.Status == "draft" && x.Status != "pay").FirstOrDefault();
                if (checkOrder != null)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductId = itemDTO.ProductId;
                    orderDetail.Quantity = itemDTO.Quantity;
                    orderDetail.OrderId = checkOrder.Id;
                    orderDetail.Price = product.Price;
                    (Added, Message) = await _repository.AddAsync<OrderDetail>(orderDetail);

                }
                else
                {
                    var order = new Order();
                    order.CustomerId = itemDTO.CustomerId;
                    order.OrderNo = Guid.NewGuid();
                    order.Status = "draft";
                    (Added, Message) = await _repository.AddAsync<Order>(order);
                    if (!Added)
                        Requests.Response(this, new ApiStatus(500), null, Message);

                    var orderDetail = new OrderDetail();
                    orderDetail.ProductId = itemDTO.ProductId;
                    orderDetail.Quantity = itemDTO.Quantity;
                    orderDetail.OrderId = order.Id;
                    orderDetail.Price = product.Price;
                    (Added, Message) = await _repository.AddAsync<OrderDetail>(orderDetail);
                }


                if (Added)
                {
                    product.Stock = product.Stock - itemDTO.Quantity;
                    (Added, Message) = await _repository.UpdateAsync<Product>(product);
                }

                return !Added ? Requests.Response(this, new ApiStatus(500), null, Message) : Requests.Response(this, new ApiStatus(200), null, "Success add product to cart");
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }

        [HttpPatch("payment/{id:int}")]
        public async Task<IActionResult> Payment([FromBody] OrderPaymentDTO itemDTO)
        {
            try
            {
                var existingItems = _repository.ListWithWhere<Order>(x => x.CustomerId == itemDTO.CustomerId && x.Id == itemDTO.Id && x.Status == "draft" && x.Status != "pay").FirstOrDefault();
                if (existingItems == null)
                {
                    return Requests.Response(this, new ApiStatus(404), null, "Data Not Found");
                }

                existingItems.Status = "pay";
                if (ModelState.IsValid)
                {
                    var (Updated, Message) = await _repository.UpdateAsync<Order>(existingItems);
                    return !Updated ? Requests.Response(this, new ApiStatus(500), null, Message) : Requests.Response(this, new ApiStatus(200), null, "Payment product success");
                }
                else
                {
                    return Requests.Response(this, new ApiStatus(500), ModelState, "");
                }
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }

        [HttpDelete("item/{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var existingItems = await _repository.GetByIdAsync<OrderDetail>(id, x => x.Order);
                if (existingItems == null)
                {
                    return Requests.Response(this, new ApiStatus(404), null, "Data Not Found");
                }

                if(existingItems.Order.Status == "pay")
                    return Requests.Response(this, new ApiStatus(404), null, "Status order product is pay");

                var (Deleted, Message) = await _repository.DeleteAsync<OrderDetail>(id);
                return !Deleted ? Requests.Response(this, new ApiStatus(500), null, Message) : Requests.Response(this, new ApiStatus(200), null, "Success delete product in cart");
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }



    }
}

