using ContactAPI.Data;
using ContactAPI.Models;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext _dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
           return Ok( await _dbContext.Contacts.ToListAsync());
            //return View();
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound(); 

            }
            return Ok( contact );
        }
        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                Phone = addContactRequest.Phone,
            };
            await _dbContext.AddAsync(contact);
            await _dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContacts(Guid id,UpdateContactRequest updateContactRequest)
        {
             var contact = await _dbContext.Contacts.FindAsync(id);
            if(contact != null)
            {
                contact.FullName = updateContactRequest.FullName;
                contact.Phone = updateContactRequest.Phone;
                contact.Email = updateContactRequest.Email;
                contact.Address = updateContactRequest.Address;

                await _dbContext.SaveChangesAsync();

                return Ok(contact);

            }
            return NotFound();
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);

            if(contact != null)
            {
                _dbContext.Remove(contact);
                await _dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }
    }
}
