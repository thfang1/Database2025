using Microsoft.AspNetCore.Mvc;

public interface iController<T>
{
    Task<IActionResult> Get();
    Task<IActionResult> Get(string ID);
    Task<IActionResult> Delete(string ID);
    Task<IActionResult> Put([FromBody] T _T);

    Task<IActionResult> Post([FromBody] T _T);
}