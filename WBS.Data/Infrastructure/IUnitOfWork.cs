namespace WBS.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
