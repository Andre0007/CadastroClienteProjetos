using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CadastroClienteProjetos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AntiForgeryController : ControllerBase
    {
        private readonly IAntiforgery _antiForgery;
        public AntiForgeryController(IAntiforgery antiForgery)
        {
            _antiForgery = antiForgery;
        }

        [IgnoreAntiforgeryToken]
        public IActionResult GenerateAntiForgeryTokens()
        {
            try
            {
                var tokens = _antiForgery.GetAndStoreTokens(HttpContext);
                Response.Cookies.Append("XSRF-REQUEST-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });
                return NoContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}