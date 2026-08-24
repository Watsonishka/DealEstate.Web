namespace OnlineShopWebApp.Interfaces
{
    public interface IFileProvider
    {
        T? Read<T>(string path);
        void Write<T>(string path, T data);
    }
}
