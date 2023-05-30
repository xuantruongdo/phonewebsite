using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication5.DTO_Put;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly DataContext _dbContext;
        public ProductController(DataContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpGet]
        public async Task<List<Product>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult> PostProduct(ProductDTO req)
        {
            var newProduct = new Product
            {
                Name = req.Name,
                Image = req.Image,
                Description = req.Description,
                ImportPrice = req.ImportPrice,
                OldPrice = req.OldPrice,
                CurrentPrice= req.CurrentPrice,
                BrandId = req.BrandId,
            };
            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();
            return Ok(newProduct);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            if (_dbContext.Products == null)
            {
                return NotFound();
            }

            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }


        [HttpGet("ByBrand/{brandId}")]
        public async Task<ActionResult<List<Product>>> GetProductsByBrand(int brandId)
        {
            var products = await _dbContext.Products
                .Where(p => p.BrandId == brandId)
                .ToListAsync();

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutProduct(int id, ProductDTO req)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = req.Name;
            product.Image = req.Image;
            product.Description = req.Description;
            product.ImportPrice = req.ImportPrice;
            product.OldPrice = req.OldPrice;
            product.CurrentPrice = req.CurrentPrice;
            product.BrandId = req.BrandId;

            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        private bool ProductExists(int id)
        {
            return _dbContext.Users.Any(e => e.Id == id);
        }

    }
}
