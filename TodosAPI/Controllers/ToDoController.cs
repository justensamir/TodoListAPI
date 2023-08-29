using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodosAPI.Data;
using TodosAPI.DTOS;
using TodosAPI.Interfaces;
using TodosAPI.Models;

namespace TodosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ITodoRepository todoRepository;
        private readonly IMapper mapper;
        public ToDoController(IMapper _mapper,ITodoRepository _todoRepository)
        {
            todoRepository = _todoRepository;
            mapper = _mapper;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(string userId)
        {
            var todoList = await todoRepository.GetAll(userId);
            List<TodoDTO> result = new List<TodoDTO>();
            todoList.ForEach(item =>
            {
                var todo = mapper.Map<TodoDTO>(item);
                result.Add(todo);
            });
            return result.Count() > 0? Ok(result) : NotFound();
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetById([FromQuery]int todoId,[FromQuery] string userId)
        {
            var todo = await todoRepository.Get(todoId, userId);
            var todoDto = mapper.Map<TodoDTO>(todo);
            return todoDto is null ? NotFound() : Ok(todoDto);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(TodoDTO todoDTO)
        {
            if (ModelState.IsValid) { 
                var todo = mapper.Map<Todo>(todoDTO);
                int result = await todoRepository.Add(todo);
                if (result > 0) return Ok("Todo Created");
            }
            return BadRequest("Todo Not Created");
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(TodoDTO todoDTO)
        {
            if (ModelState.IsValid)
            {
                var todo = mapper.Map<Todo>(todoDTO);
                int result = await todoRepository.Update(todo);
                if (result > 0) return Ok("Todo Updated");
            }
            return BadRequest("Todo Not Updated");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int todoId,[FromQuery] string userId)
        {
            int result = await todoRepository.Delete(todoId, userId);
            return result > 0 ? Ok("Todo Deleted Successfully") : BadRequest("Todo Not Deleted");
        }
    }
}
