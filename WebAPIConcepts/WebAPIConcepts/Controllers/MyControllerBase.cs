using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIConcepts.Controllers
{
    /// <summary>
    /// Base Class for all the controllers
    /// </summary>
    //[Route("api/[controller]")] //Base For All Controller with no action
    [ApiController]
    public class MyControllerBase : ControllerBase
    {
    }
}