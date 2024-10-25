using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;
using Asp.Versioning;

namespace ToDoApp.Controllers
{
    [ApiVersion(1)]
    [ApiVersion(2)]
    [ApiController]
    // [Route("/api/ToDo/")]
    [Route("/api/v{v:apiVersion}/ToDo/")]

    public class ToDoController : Controller
    {
        private readonly ILogger<ToDoController> logger;
        private readonly IToDoRepository toDoRepository;

        public ToDoController(ILogger<ToDoController> logger, IToDoRepository toDoRepository)
        {
            this.logger = logger;
            this.toDoRepository = toDoRepository;
        }

   

        [HttpGet]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ToDo>))]
        public IActionResult GetAll()
        {
            return Ok(toDoRepository.GetAll());
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDo))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public IActionResult GetSingle([FromRoute] Guid id)
        {
            var obj = toDoRepository.GetSingle(id);
            if (obj is null)
                return NotFound(new ProblemDetails() { Title = $"The ToDo-object with the ID {id} was not found!" });

            return Ok(obj);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] ToDo obj) // model binding
        {
            return Ok(toDoRepository.Add(obj));
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDo))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update([FromBody] ToDo obj)
        {
            var old = toDoRepository.GetSingle(obj.ID);
            if (old is null)
                return NotFound();

            return Ok(toDoRepository.Update(obj));
        }

        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            var obj = toDoRepository.GetSingle(id);
            if (obj is null)
                return NotFound();

            toDoRepository.Delete(id);
            return Ok();
        }

    }
}
