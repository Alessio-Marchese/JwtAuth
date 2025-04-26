namespace JwtAuth.Utilities;

public class Result<T>
{
    public bool IsSuccessful { get; set; }
    public string Reason { get; set; } = string.Empty;
    public T Data { get; set; }
}

public class Result : Result<object>
{

}
