namespace Finance.DataAccess
{
    public interface IRepository<TEntity>
    {
        void Save(TEntity item);
    }
}