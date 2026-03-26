namespace Application.Interfaces.UseCases.Categories
{
    public interface IDeleteCategory
    {
        Task ExecuteAsync(Guid categoryId, Guid userId);
    }
}
