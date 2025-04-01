using MediatR;
using Microsoft.AspNetCore.Mvc;
using task06_Currencies.Currencies.GetCurrency;
using task06_Currencies.Currencies.SynchronizeDatabases;
using task06_Currencies.Currencies.UpdateCurrencies;

namespace task06_Currencies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromBody] GetCurrencyQuery request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            if (request.Symbol == "EUR")
            {
                return BadRequest();
            }

            var response = await mediator.Send(request, cancellationToken);

            await mediator.Send(new SynchronizeDatabasesCommand(), cancellationToken);

            if (response)
                return Ok();

            return BadRequest();
        }
    }
}
