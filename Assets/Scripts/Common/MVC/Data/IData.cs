namespace Common.MVC.Data
{
    public interface IData<T>
    {
        T Data { get; }
        void SetData(T data);
    }
}