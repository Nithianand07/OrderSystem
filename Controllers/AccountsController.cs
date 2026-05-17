using Microsoft.AspNetCore.Mvc;
using Enterprises.Services;
using Enterprises.Dtos;
using Enterprises.Entities;

namespace Enterprises.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController(IAccountService service) { _service = service; }

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
        public async Task<IActionResult> Create(AccountDto dto)
        {
            var entity = new MstAccount { AccountType = dto.AccountType, AccountName = dto.AccountName, ContactNumber = dto.ContactNumber, EmailAddress = dto.EmailAddress, ActiveFlg = dto.ActiveFlg };
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = created.AccountID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AccountDto dto)
        {
            var existing = await _service.GetAsync(id);
            if (existing == null) return NotFound();
            existing.AccountType = dto.AccountType;
            existing.AccountName = dto.AccountName;
            existing.ContactNumber = dto.ContactNumber;
            existing.EmailAddress = dto.EmailAddress;
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
