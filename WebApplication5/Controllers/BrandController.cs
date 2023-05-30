using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        public readonly DataContext _dbContext;
        public BrandController(DataContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpGet]
        public async Task<List<Brand>> GetBrands()
        {
            return await _dbContext.Brands.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            var brand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }
        [HttpPost]
        public async Task<ActionResult> PostBrand(BrandDTO req)
        {
            var newBrand = new Brand
            {
                Name = req.Name
            };
            _dbContext.Brands.Add(newBrand);
            await _dbContext.SaveChangesAsync();
            return Ok(newBrand);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Brand>> DeleteBrand(int id)
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }

            var brand = await _dbContext.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            _dbContext.Brands.Remove(brand);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
