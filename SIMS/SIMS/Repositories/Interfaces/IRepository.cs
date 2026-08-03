namespace SIMS.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // 1. Get all data in a table
        Task<IEnumerable<T>> GetAllAsync();

        // 2. Get a line data by id
        Task<T?> GetByIdAsync(int id);

        // 3. Add new data
        Task AddAsync(T entity);

        // 4. Update data
        Task UpdateAsync(T entity);

        // 5. Delete data by id
        Task DeleteAsync(int id);
    }
}
