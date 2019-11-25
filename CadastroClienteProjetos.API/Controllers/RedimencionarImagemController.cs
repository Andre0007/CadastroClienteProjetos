using CadastroClienteProjetos.Domain.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CadastroClienteProjetos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedimencionarImagemController : ControllerBase
    {
        /// <summary>
        /// Redimenciona uma imagem 
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public FileStreamResult RedimencionarImg(IList<IFormFile> files)
        {
            try
            {
                using (Image img = Image.FromStream(files[0].OpenReadStream()))
                {
                    Stream ms = new MemoryStream(img.Resize(100, 100).ToByteArray());
                    return new FileStreamResult(ms, "image/jpg");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}