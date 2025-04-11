using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class AddressTypeController : ControllerBase, iController<AddressType>
{
    private WebRestOracleContext _context;
    // Create a field to store the mapper object
    private readonly IMapper _mapper;

    public AddressTypeController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {

        List<AddressType> lst = null;
        lst = await _context.AddressTypes.ToListAsync();
        return Ok(lst);
    }


    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.AddressTypes.Where(x => x.AddressTypeId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }


    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.AddressTypes.Where(x => x.AddressTypeId == ID).FirstOrDefaultAsync();
        _context.AddressTypes.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] AddressType _AddressType)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.AddressTypes.AsNoTracking()
            .Where(x => x.AddressTypeId == _AddressType.AddressTypeId)
            .FirstOrDefaultAsync();


            if (itm != null)
            {
                itm = _mapper.Map<AddressType>(_AddressType);

                 /*
                        itm.AddressTypeFirstName = _AddressType.AddressTypeFirstName;
                        itm.AddressTypeMiddleName = _AddressType.AddressTypeMiddleName;
                        itm.AddressTypeLastName = _AddressType.AddressTypeLastName;
                        itm.AddressTypeDateOfBirth = _AddressType.AddressTypeDateOfBirth;
                        itm.AddressTypeGenderId = _AddressType.AddressTypeGenderId;
                   */      
                _context.AddressTypes.Update(itm);
                await _context.SaveChangesAsync();
                trans.Commit();

            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return Ok();

    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AddressType _AddressType)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _AddressType.AddressTypeId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.AddressTypes.Add(_AddressType);
            await _context.SaveChangesAsync();
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return Ok();
    }

}