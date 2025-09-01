using FumicertiApi.Data;
using FumicertiApi.DTOs;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System.Security.Claims;

namespace FumicertiApi.Controllers
{
    [Authorize]
    [ApiController]
   
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public ProductController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        // Helper to get current user ID from JWT or Claims


        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query = FilterByCompany(_context.products.AsNoTracking(), "ProductCompanyId");

            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedProducts = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductUnit = p.ProductUnit,
                    ProductOpening = (decimal)p.ProductOpening,
                    ProductOpeningUnit = p.ProductOpeningUnit,
                    ProductWeightPerUnit = (decimal)p.ProductWeightPerUnit,
                    ProductTotalWt = (decimal)p.ProductTotalWt,
                    ProductCompanyId = p.ProductCompanyId,
                    ProductConsumeQty = (decimal)p.ProductConsumeQty,
                    ProductType = p.ProductType,
                    ProductAlpwtperTablets= p.ProductAlpwtperTablets
                })
                .ToListAsync();

            return Ok(new
            {
                pagination = new
                {
                    page = currentPage,
                    pageSize = pageSize,
                    totalRecords = totalRecords,
                    totalPages = totalPages
                },
                data = pagedProducts
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var product = await FilterByCompany(_context.products, "ProductCompanyId")
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return NotFound();

            var dto = new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductUnit = product.ProductUnit,
                ProductOpening = (decimal)product.ProductOpening,
                ProductOpeningUnit = product.ProductOpeningUnit,
                ProductWeightPerUnit = (decimal)product.ProductWeightPerUnit,
                ProductTotalWt = (decimal)product.ProductTotalWt,
                ProductCompanyId = product.ProductCompanyId,
                ProductConsumeQty = (decimal)product.ProductConsumeQty,
                ProductAlpwtperTablets= product.ProductAlpwtperTablets,
                ProductType = product.ProductType
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            var userId = GetUserId().ToString();

            var product = new Product
            {
                ProductName = dto.ProductName,
                ProductUnit = dto.ProductUnit,
                ProductOpening = dto.ProductOpening,
                ProductOpeningUnit = dto.ProductOpeningUnit,
                ProductWeightPerUnit = dto.ProductWeightPerUnit,
                ProductTotalWt = dto.ProductTotalWt,
                ProductConsumeQty = dto.ProductConsumeQty,
                ProductCreated = DateTime.UtcNow,
                ProductUpdated = DateTime.UtcNow,
                ProductCompanyId = GetCompanyId(),
                ProductCreatedBy = userId,
                ProductEditBy = userId,
                ProductType= dto.ProductType,
                ProductAlpwtperTablets= dto.ProductAlpwtperTablets

            };

            _context.products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(new { product.ProductId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDto dto)
        {
            var userId = GetUserId().ToString();

            var product = await _context.products.FindAsync(id);
            if (product == null) return NotFound();

            product.ProductName = dto.ProductName;
            product.ProductUnit = dto.ProductUnit;
            product.ProductOpening = dto.ProductOpening;
            product.ProductOpeningUnit = dto.ProductOpeningUnit;
            product.ProductWeightPerUnit = dto.ProductWeightPerUnit;
            product.ProductTotalWt = dto.ProductTotalWt;
            product.ProductConsumeQty = dto.ProductConsumeQty;
            product.ProductUpdated = DateTime.UtcNow;
            product.ProductCompanyId = GetCompanyId();
            product.ProductEditBy = userId;
            product.ProductAlpwtperTablets = dto.ProductAlpwtperTablets;
            product.ProductType = dto.ProductType;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await FilterByCompany(_context.products, "ProductCompanyId").FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return NotFound();

            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetByType(string type, [FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query = FilterByCompany( _context.products, "ProductCompanyId")
                .AsNoTracking()
                .Where(p => p.ProductType == type); // Assuming ProductType exists

            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedProducts = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductUnit = p.ProductUnit,
                    ProductCompanyId = p.ProductCompanyId,
                    ProductOpening = (decimal)p.ProductOpening,
                    ProductOpeningUnit = p.ProductOpeningUnit,
                    ProductWeightPerUnit = (decimal)p.ProductWeightPerUnit,
                    ProductTotalWt = (decimal)p.ProductTotalWt,
                    ProductConsumeQty = (decimal)p.ProductConsumeQty,
                    ProductAlpwtperTablets= p.ProductAlpwtperTablets
                })
                .ToListAsync();

            return Ok(new
            {
                pagination = new
                {
                    page = currentPage,
                    pageSize = pageSize,
                    totalRecords = totalRecords,
                    totalPages = totalPages
                },
                data = pagedProducts
            });
        }

    }
}
