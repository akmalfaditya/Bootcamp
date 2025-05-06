using Microsoft.AspNetCore.Mvc;

namespace MyWebAPI.Controllers;

public class ProductController : ControllerBase, IController<string>
{
    public IActionResult Create(string data)
    {
        throw new NotImplementedException();
    }

    public IActionResult Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IActionResult Get()
    {
        throw new NotImplementedException();
    }

    public IActionResult Get(int id)
    {
        throw new NotImplementedException();
    }

    public IActionResult Update(int id, string data)
    {
        throw new NotImplementedException();
    }

}
