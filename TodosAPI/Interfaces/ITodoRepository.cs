using TodosAPI.Models;

namespace TodosAPI.Interfaces
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAll(string userId);
        Task<Todo> Get(int id, string userId);
        Task<int> Add(Todo todo);
        Task<int> Update(Todo todo);
        Task<int> Delete(int id, string userId);
    }
}
