using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase, iController<Product>
{
    private WebRestOracleContext _context;
    // Create a field to store the mapper object
    private readonly IMapper _mapper;

    public ProductController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {

        List<Product> lst = null;
        lst = await _context.Products.ToListAsync();
        return Ok(lst);
    }


    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.Products.Where(x => x.ProductId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }


    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.Products.Where(x => x.ProductId == ID).FirstOrDefaultAsync();
        _context.Products.Remove(itm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Product _Product)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.Products.AsNoTracking()
            .Where(x => x.ProductId == _Product.ProductId)
            .FirstOrDefaultAsync();


            if (itm != null)
            {
                itm = _mapper.Map<Product>(_Product);

                 /*
                        itm.ProductFirstName = _Product.ProductFirstName;
                        itm.ProductMiddleName = _Product.ProductMiddleName;
                        itm.ProductLastName = _Product.ProductLastName;
                        itm.ProductDateOfBirth = _Product.ProductDateOfBirth;
                        itm.ProductGenderId = _Product.ProductGenderId;
                   */      
                _context.Products.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] Product _Product)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _Product.ProductId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _context.Products.Add(_Product);
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