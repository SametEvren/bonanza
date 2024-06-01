namespace Utility
{
    public interface IObjectPoolManager<T>
    {
        public T GetItemFromPool();
        public void ReleaseItemToPool(T item);
    }
}