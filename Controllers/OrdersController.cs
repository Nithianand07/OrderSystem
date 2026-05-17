using Enterprises.Dtos;
using Enterprises.Entities;
using Enterprises.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enterprises.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service) { _service = service; }

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
        public async Task<IActionResult> Create(OrderCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var head = new TrnOrderHead
            {
                OrderDate = dto.OrderDate,
                OrderType = dto.OrderType,
                AccountID = dto.AccountID,
                Reference = dto.Reference,
                Narration = dto.Narration,
                Details = dto.Details.Select(d => new TrnOrderDetail { ItemID = d.ItemID, Quantity = d.Quantity, Rate = d.Rate, ItemAmount = d.Quantity * d.Rate, Discount = d.Discount, TotalAmount = (d.Quantity * d.Rate) - d.Discount }).ToList()
            };

            try
            {
                var created = await _service.CreateAsync(head);
                return CreatedAtAction(nameof(Get), new { id = created.OrderNo }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult>
 Update(
     int id,
     OrderCreateDto dto
 )
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existing =
                await _service.GetAsync(id);

            if (existing == null)
                return NotFound();

            existing.OrderDate =
                dto.OrderDate;

            existing.OrderType =
                dto.OrderType;

            existing.AccountID =
                dto.AccountID;

            existing.Reference =
                dto.Reference;

            existing.Narration =
                dto.Narration;

            foreach (var d in dto.Details)
            {

                if (d.OrderDetailNo > 0)
                {
                    var existingDetail =
                        existing.Details?
                            .FirstOrDefault(x =>
                                x.OrderDetailNo ==
                                d.OrderDetailNo
                            );

                    if (existingDetail != null)
                    {
                        existingDetail.ItemID =
                            d.ItemID;

                        existingDetail.Quantity =
                            d.Quantity;

                        existingDetail.Rate =
                            d.Rate;

                        existingDetail.Discount =
                            d.Discount;

                        existingDetail.ItemAmount =
                            d.Quantity * d.Rate;

                        existingDetail.TotalAmount =
                            (d.Quantity * d.Rate)
                            - d.Discount;
                    }
                }

                else
                {
                    existing.Details?.Add(
                        new TrnOrderDetail
                        {
                            ItemID =
                                d.ItemID,

                            Quantity =
                                d.Quantity,

                            Rate =
                                d.Rate,

                            Discount =
                                d.Discount,

                            ItemAmount =
                                d.Quantity * d.Rate,

                            TotalAmount =
                                (d.Quantity * d.Rate)
                                - d.Discount
                        }
                    );
                }
            }

            // =========================
            // REMOVE DELETED ROWS
            // =========================

            var dtoDetailIds =
                dto.Details
                    .Where(x =>
                        x.OrderDetailNo > 0
                    )
                    .Select(x =>
                        x.OrderDetailNo
                    )
                    .ToList();

            var deletedItems =
                existing.Details?
                    .Where(x =>
                        !dtoDetailIds.Contains(
                            x.OrderDetailNo
                        )
                    )
                    .ToList();

            if (deletedItems != null)
            {
                foreach (var item in deletedItems)
                {
                    item.IsDelete = true;
                }
            }

            // =========================
            // SAVE
            // =========================

            try
            {
                var updated =
                    await _service
                        .UpdateAsync(existing);

                return Ok(updated);
            }
            catch (
                InvalidOperationException ex
            )
            {
                return BadRequest(
                    new
                    {
                        message = ex.Message
                    }
                );
            }
        }
        [HttpDelete("IndividualItemDelete/{OrderDetailNo}")]
        public async Task<IActionResult> DeleteIndividual(int OrderDetailNo)
        {
            var item = await _service.GetIndividualAsync(OrderDetailNo);
            if (item == null) return NotFound();
            await _service.DeleteAsync(OrderDetailNo);
            return NoContent();
        }
        [HttpDelete("DeleteOrder/{orderNo}")]
        public async Task<IActionResult> DeleteOrder(
            int orderNo)
        {
            var item =
                await _service.GetAsync(orderNo);

            if (item == null)
                return NotFound();

            await _service.DeleteAllAsync(orderNo);

            return NoContent();
        }
        [HttpPost("PurchaseSalesReport")]
        public async Task<IActionResult>
        PurchaseSalesReport(
            PurchaseSalesReportDto dto
        )
        {
            try
            {
                var data =
                    await _service
                        .PurchaseSalesReportAsync(dto);

                return Ok(data);
            }
            catch (
                InvalidOperationException ex
            )
            {
                return BadRequest(
                    new
                    {
                        message = ex.Message
                    }
                );
            }
        }
    }
}
