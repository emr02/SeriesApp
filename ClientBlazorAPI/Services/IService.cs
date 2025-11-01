namespace ClientBlazorAPI.Services
{
    public interface IService<TEntity>
    {
        Task<List<TEntity>?> GetAllAsync(string nomControleur);
        Task<TEntity?> GetByIdAsync(string nomControleur, int id);
        Task<TEntity?> GetByStringAsync(string nomControleur, string str);
        Task<bool> PostAsync(string nomControleur, TEntity entity);
        Task<bool> PutAsync(string nomControleur, TEntity entity);
        Task<bool> DeleteAsync(string nomControleur, TEntity entity);
    }
}
