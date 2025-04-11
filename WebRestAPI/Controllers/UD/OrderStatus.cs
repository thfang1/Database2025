using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class OrderStatusController : ControllerBase, iController<OrderStatus>
{
    private WebRestOracleContext _context;
    // Create a field to store the mapper object
    private readonly IMapper _mapper;

    public OrderStatusController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {

        List<OrderStatus> lst = null;
        lst = await _context.OrderStatuses.ToListAsync();
        return Ok(lst);
    }


    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.OrderStatuses.Where(x => x.OrderStatusId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }


    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.OrderStatuses.Where(x => x.OrderStatusId == ID).FirstOrDefaultAsync();
        _context.OrderStatuses.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] OrderStatus _OrderStatus)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.OrderStatuses.AsNoTracking()
            .Where(x => x.OrderStatusId == _OrderStatus.OrderStatusId)
            .FirstOrDefaultAsync();


            if (itm != null)
            {
                itm = _mapper.Map<OrderStatus>(_OrderStatus);

                 /*
                        itm.OrderStatusFirstName = _OrderStatus.OrderStatusFirstName;
                        itm.OrderStatusMiddleName = _OrderStatus.OrderStatusMiddleName;
                        itm.OrderStatusLastName = _OrderStatus.OrderStatusLastName;
                        itm.OrderStatusDateOfBirth = _OrderStatus.OrderStatusDateOfBirth;
                        itm.OrderStatusGenderId = _OrderStatus.OrderStatusGenderId;
                   */      
                _context.OrderStatuses.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] OrderStatus _OrderStatus)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _OrderStatus.OrderStatusId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.OrderStatuses.Add(_OrderStatus);
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