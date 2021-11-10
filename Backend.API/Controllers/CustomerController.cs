using AutoMapper;
using Backend.API.Error;
using Backend.Application.Dto;
using Backend.Core.Entities;
using Backend.Core.Repositories.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CustomerController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            try
            {
                var items = await _repository.ListAsync<Customer>();

                var result = _mapper.Map<List<CustomerDTO>>(items);

                return Requests.Response(this, new ApiStatus(200), result, "");

            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ListAsyncById(long id)
        {
            try
            {
                var items = await _repository.GetByIdAsync<Customer>(id);

                var result = _mapper.Map<CustomerDTO>(items);
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerDTO itemDTO)
        {
            try
            {
                var item = _mapper.Map<Customer>(itemDTO);
                item.Id = 0;
                var (Added, Message) = await _repository.AddAsync<Customer>(item);
                return !Added ? Requests.Response(this, new ApiStatus(500), null, Message) : Requests.Response(this, new ApiStatus(200), null, Message);
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Edit([FromBody] CustomerDTO itemDTO)
        {
            try
            {
                var existingItems = await _repository.GetByIdAsync<Customer>(itemDTO.Id);
                if (existingItems == null)
                {
                    return Requests.Response(this, new ApiStatus(404), null, "Data Not Found");
                }

                var item = _mapper.Map<CustomerDTO, Customer>(itemDTO, existingItems);
                if (ModelState.IsValid)
                {
                    var (Updated, Message) = await _repository.UpdateAsync<Customer>(item);
                    return !Updated ? Requests.Response(this, new ApiStatus(500), null, Message) : Requests.Response(this, new ApiStatus(200), null, Message);
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var existingItems = await _repository.GetByIdAsync<Customer>(id);
                if (existingItems == null)
                {
                    return Requests.Response(this, new ApiStatus(404), null, "Data Not Found");
                }
                var (Deleted, Message) = await _repository.DeleteAsync<Customer>(id);
                return !Deleted ? Requests.Response(this, new ApiStatus(500), null, Message) : Requests.Response(this, new ApiStatus(200), null, Message);
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }
    }
}
