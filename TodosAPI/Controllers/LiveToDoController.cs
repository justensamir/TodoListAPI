using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TodosAPI.DTOS;

namespace TodosAPI.Controllers
{
    [Route("api/Todos")]
    [ApiController]
    public class LiveToDoController : ControllerBase
    {
        private string uri = "https://jsonplaceholder.typicode.com/todos";
        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<TodoDTO> todoList = JsonConvert.DeserializeObject<List<TodoDTO>>(content);
                    return Ok(todoList);
                }
            }
            return BadRequest();
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{uri}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    TodoDTO todo = JsonConvert.DeserializeObject<TodoDTO>(content);
                    return Ok(todo);
                }
            }
            return NotFound();
        }
    }
}
