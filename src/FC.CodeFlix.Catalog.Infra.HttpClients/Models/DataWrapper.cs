namespace FC.CodeFlix.Catalog.Infra.HttpClients.Models;

public class DataWrapper<T>
    where T : class
{
    public DataWrapper(T data)
    {
        Data = Data;
    }
    public T Data { get; set; }
}