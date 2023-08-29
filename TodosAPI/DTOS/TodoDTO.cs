using System.ComponentModel.DataAnnotations.Schema;

namespace TodosAPI.DTOS
{
    public record TodoDTO(int Id, string Title, bool Completed, string UserId);
}
