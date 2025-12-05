using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Sudoku.Api.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            var ctx = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = ctx?.Error; // log as needed
            return Problem(detail: ex?.Message, title: "An error occurred");
        }
    }
}
