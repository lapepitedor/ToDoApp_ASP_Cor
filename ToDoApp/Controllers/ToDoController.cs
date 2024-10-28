using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;
using Asp.Versioning;

namespace ToDoApp.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ILogger<ToDoController> logger;
        private readonly IToDoRepository toDoRepository;

        public ToDoController(ILogger<ToDoController> logger, IToDoRepository toDoRepository)
        {
            this.logger = logger;
            this.toDoRepository = toDoRepository;
        }

        public IActionResult List([FromQuery] ToDoFilter filter)
        {
            var objects = toDoRepository.GetAll(filter);
            return View(objects);
        }

        [HttpGet("/ToDo/Edit/{id}")]
        public IActionResult Edit([FromRoute] Guid id)
        {
            var obj = toDoRepository.GetSingle(id);
            if (obj == null)
                return NotFound();

            return View(obj);
        }

        [HttpGet("/ToDo/New")]
        public IActionResult New()
        {
            return View("Edit", new ToDo());
        }

        [HttpPost("/ToDo/Save/")]
        public IActionResult Save([FromForm] ToDo obj)
        {
            if (obj == null)
                return Redirect("/");

            if (!ModelState.IsValid)
                return View("Edit", obj);

            if (obj.ID == Guid.Empty)
                toDoRepository.Add(obj);
            else
                toDoRepository.Update(obj);

            return Redirect("/");
        }

        public IActionResult Delete([FromRoute] Guid id)
        {
            var obj = toDoRepository.GetSingle(id);
            if (obj == null)
                return NotFound();

            toDoRepository.Delete(id);
            return Redirect("/");
        }

    }
}
