using Microsoft.AspNetCore.Mvc;
using Enterprises.Services;
using Enterprises.Dtos;
using Enterprises.Entities;

namespace Enterprises.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _service;
        public ItemsController(IItemService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.GetAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemDto dto)
        {
            var entity = new MstItem { ItemCode = dto.ItemCode, ItemName = dto.ItemName, ItemUOM = dto.ItemUOM, PurchasePrice = dto.PurchasePrice, SalePrice = dto.SalePrice, StockQty = dto.StockQty, ActiveFlg = dto.ActiveFlg };
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = created.ItemID }, created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            ItemDto dto)
        {
            var existing =
                await _service.GetAsync(id);

            if (existing == null)
                return NotFound();

            existing.ItemCode =
                dto.ItemCode;

            existing.ItemName =
                dto.ItemName;

            existing.ItemUOM =
                dto.ItemUOM;

            existing.PurchasePrice =
                dto.PurchasePrice;

            existing.SalePrice =
                dto.SalePrice;

            existing.StockQty =
                dto.StockQty;

            existing.ActiveFlg =
                dto.ActiveFlg;

            await _service.UpdateAsync(existing);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.GetAsync(id);
            if (item == null) return NotFound();
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
