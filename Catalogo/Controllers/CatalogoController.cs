using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogoController : ControllerBase
    {
        List<string> catalogo = new List<string> { "Abacaxi", "Café", "Bolacha" };

        //GET api/catalogo
        [HttpGet]
        public List<string> Get()
        {
            return catalogo;
        }

        //GET api/catalogo/1
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return catalogo[id];
        }
    }
}