using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIConcepts.Controllers
{
    /// <summary>
    /// Pizza Express Functions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PizzaController : MyControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Sample Message";
        }

        /// <summary>
        /// Gets all available Pizzas
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllPizzas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAllPizzas()
        {
            return Ok(PizzaService.GetAll());
        }


        /// <summary>
        /// Deletes a Pizza From Pizza Store
        /// </summary>
        /// <param name="id">Id Of Pizza</param>
        /// <returns></returns>
        ///<response code="204"> No Content</response>
        ///<response code="404">No Pizza Found</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult DeletePizza(int id)
        {
            var item = PizzaService.Get(id);
            if (item == null) return NotFound();
            PizzaService.Delete(id);
            //await Task.CompletedTask;
            return NoContent();

        }
    }
}