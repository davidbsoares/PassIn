using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Events.GetById;
using PassIn.Application.UseCases.Events.Register;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;

namespace PassIn.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredEventJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RequestEventJson request) {
            try {
                var useCase = new RegisterEventUseCase();
                var res = useCase.Execute(request);

                return Created(string.Empty, res);
            }
            catch (PassInException ex) {
                return BadRequest(new ResponseErrorJson(ex.Message));
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Unknown error"));
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromRoute] Guid id) {
            try {
                var useCase = new GetEventByIdUseCase();

                var res = useCase.Execute(id);

                return Ok(res);
            }
            catch (PassInException ex) {
                return NotFound(new ResponseErrorJson(ex.Message));
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Unknown error"));
            }
        }
    }
}
