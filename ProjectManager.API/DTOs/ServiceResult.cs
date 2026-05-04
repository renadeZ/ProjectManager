namespace ProjectManager.API.DTOs;

public record ServiceResult<T>
{
    public bool IsSuccess { get; private set; }
    public string? Message { get; private set; }
    public T? Data { get; private set; }

    public static ServiceResult<T> Success(T data, string message)
    {
        return new ServiceResult<T>
        {
            IsSuccess = true, 
            Data = data, 
            Message = message
        };
    }

    public static ServiceResult<T> Failure(string error)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false, 
            Message = error
        };
    }
}