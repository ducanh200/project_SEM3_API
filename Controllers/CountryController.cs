using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEM3_API.DTOs;
using SEM3_API.Entities;
using SEM3_API.Models;
using SEM3_API.Models.Country;
using System.Net.WebSockets;

namespace SEM3_API.Controllers
{

    [ApiController]
    [Route("api/country")]
    public class CountryController : ControllerBase
    {
        private readonly Sem3ApiContext _context;

        public CountryController(Sem3ApiContext context)
        {
            _context = context;
        }


        [HttpGet]

        public IActionResult Index()
        {
            List<Country> countries = _context.Countries.ToList();
            List<CountryDTO> data = new List<CountryDTO>();
            foreach (Country c in countries)
            {
                data.Add(new CountryDTO { id = c.Id, name = c.Name });
            }
            return Ok(countries);
        }

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                Country c = _context.Countries.Find(id);
                if (c != null)
                {
                    return Ok(new CountryDTO { id = c.Id, name = c.Name });
                }
            }
            catch (Exception ex)
            {
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Create(CreateCountry model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Country data = new Country { Name = model.name };
                    _context.Countries.Add(data);
                    _context.SaveChanges();
                    return Created($"get-by-id?id={data.Id}",
                    new CountryDTO { id = data.Id, name = data.Name });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            var msgs = ModelState.Values.SelectMany(v => v.Errors)
                   .Select(v => v.ErrorMessage);
            return BadRequest(string.Join(", ", msgs));
        }
        [HttpPut]
        public IActionResult Update(EditCountry model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Country country = new Country { Id = model.id, Name = model.name };
                    if (country != null)
                    {
                        _context.Countries.Update(country);
                        _context.SaveChanges();
                        return Ok("Đổi thành công tên danh mục!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                Country country = _context.Countries.Find(id);
                if (country == null)
                    return NotFound();
                _context.Countries.Remove(country);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
