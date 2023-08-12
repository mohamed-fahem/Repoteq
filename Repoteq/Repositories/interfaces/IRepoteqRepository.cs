namespace Repoteq.Repositories.interfaces
{
    public interface IRepoteqRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(int id);
        Task<TEntity> Add(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
        IEnumerable<TEntity> Search(string term);
        IQueryable<TEntity> GetNoTranckingTable();
    }
}
