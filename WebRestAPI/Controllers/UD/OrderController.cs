using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase, iController<Order>
{
    private WebRestOracleContext _context;
    // Create a field to store the mapper object
    private readonly IMapper _mapper;

    public OrderController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {

        List<Order> lst = null;
        lst = await _context.Orders.ToListAsync();
        return Ok(lst);
    }


    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.Orders.Where(x => x.OrdersId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }


    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.Orders.Where(x => x.OrdersId == ID).FirstOrDefaultAsync();
        _context.Orders.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Order _Order)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.Orders.AsNoTracking()
            .Where(x => x.OrdersId == _Order.OrdersId)
            .FirstOrDefaultAsync();


            if (itm != null)
            {
                itm = _mapper.Map<Order>(_Order);

                 /*
                        itm.OrderFirstName = _Order.OrderFirstName;
                        itm.OrderMiddleName = _Order.OrderMiddleName;
                        itm.OrderLastName = _Order.OrderLastName;
                        itm.OrderDateOfBirth = _Order.OrderDateOfBirth;
                        itm.OrderGenderId = _Order.OrderGenderId;
                   */      
                _context.Orders.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] Order _Order)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _Order.OrdersId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.Orders.Add(_Order);
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