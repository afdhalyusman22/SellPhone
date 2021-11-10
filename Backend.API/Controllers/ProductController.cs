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
    public class ProductController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public ProductController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            try
            {
                var items = await _repository.ListAsync<Product>();

                var result = _mapper.Map<List<ProductDTO>>(items);

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
                var items = await _repository.GetByIdAsync<Product>(id);

                var result = _mapper.Map<ProductDTO>(items);
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
        public async Task<IActionResult> Create([FromBody] ProductDTO itemDTO)
        {
            try
            {
                var item = _mapper.Map<Product>(itemDTO);
                item.Id = 0;
                var (Added, Message) = await _repository.AddAsync<Product>(item);
                return !Added ? Requests.Response(this, new ApiStatus(500), null, Message) : Requests.Response(this, new ApiStatus(200), null, Message);
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Edit([FromBody] ProductDTO itemDTO)
        {
            try
            {
                var existingItems = await _repository.GetByIdAsync<Product>(itemDTO.Id);
                if (existingItems == null)
                {
                    return Requests.Response(this, new ApiStatus(404), null, "Data Not Found");
                }

                var item = _mapper.Map<ProductDTO, Product>(itemDTO, existingItems);
                if (ModelState.IsValid)
                {
                    var (Updated, Message) = await _repository.UpdateAsync<Product>(item);
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
                var existingItems = await _repository.GetByIdAsync<Product>(id);
                if (existingItems == null)
                {
                    return Requests.Response(this, new ApiStatus(404), null, "Data Not Found");
                }
                var (Deleted, Message) = await _repository.DeleteAsync<Product>(id);
                return !Deleted ? Requests.Response(this, new ApiStatus(500), null, Message) : Requests.Response(this, new ApiStatus(200), null, Message);
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }
    }
}
