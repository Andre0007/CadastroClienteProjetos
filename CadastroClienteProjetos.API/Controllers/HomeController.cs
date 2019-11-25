using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CadastroClienteProjetos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet]
        public IEnumerable<int> Get()
        {
            List<int> JsonController = new List<int>();
            JsonController.Add(280);
            JsonController.Add(680);
            JsonController.Add(770);
            JsonController.Add(90);
            JsonController.Add(900);
            JsonController.Add(270);
            JsonController.Add(500);
            return JsonController;
        }
    }
}