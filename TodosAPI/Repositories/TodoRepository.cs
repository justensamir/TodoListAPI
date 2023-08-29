using Microsoft.EntityFrameworkCore;
using TodosAPI.Data;
using TodosAPI.Interfaces;
using TodosAPI.Models;

namespace TodosAPI.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodosDbContext context;

        public TodoRepository(TodosDbContext _context)
        {
            context = _context;
        }

        public async Task<int> Add(Todo todo)
        {
            await context.AddAsync(todo);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id, string userId)
        {
            var todo = await Get(id,userId);
            context.Todos.Remove(todo);
            return await context.SaveChangesAsync();
        }

        public async Task<Todo> Get(int id, string userId)
        {
            return await context.Todos.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id);
        }

        public async Task<List<Todo>> GetAll(string userId)
        {
            return await context.Todos.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<int> Update(Todo todo)
        {
            context.Update(todo);
            return await context.SaveChangesAsync();
        }
    }
}
