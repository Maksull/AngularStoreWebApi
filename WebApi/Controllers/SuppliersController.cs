using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public sealed class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository _repository;

        public SuppliersController(ISupplierRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSuppliers()
        {
            if (_repository.Suppliers != null)
            {
                IEnumerable<Supplier> suppliers = _repository.Suppliers.Include(s => s.Products);

                foreach (var s in suppliers)
                {
                    foreach (var p in s.Products!)
                    {
                        p.Supplier = null;
                    }
                }

                return Ok(suppliers);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSupplier(long id)
        {
            if (_repository.Suppliers != null)
            {
                Supplier? s = await _repository.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.SupplierId == id);

                if (s != null)
                {
                    foreach (var p in s.Products!)
                    {
                        p.Supplier = null;
                    }
                    return Ok(s);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSupplier(Supplier supplier)
        {
            await _repository.CreateSupplierAsync(supplier);
            return Ok(supplier);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSupplier(Supplier supplier)
        {
            if (await _repository.Suppliers.ContainsAsync(supplier))
            {
                await _repository.UpdateSupplierAsync(supplier);
                return Ok(supplier);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSupplier(long id)
        {
            if (_repository.Suppliers != null)
            {
                Supplier? s = await _repository.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == id);

                if (s != null)
                {
                    await _repository.DeleteSupplierAsync(s);
                    return Ok(s);
                }
            }
            return NotFound();
        }
    }
}
