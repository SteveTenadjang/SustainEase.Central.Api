namespace Central.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Error { get; }
    public List<string> Errors { get; } = new();

    protected Result(bool isSuccess, T? data, string? error = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }

    public static Result<T> Success(T data) => new(true, data);
    public static Result<T> Failure(string error) => new(false, default, error);
    public static Result<T> Failure(List<string> errors)
    {
        var result = new Result<T>(false, default);
        result.Errors.AddRange(errors);
        return result;
    }
}

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public List<string> Errors { get; } = new();

    protected Result(bool isSuccess, string? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true);
    public static Result Failure(string error) => new(false, error);
    public static Result Failure(List<string> errors)
    {
        var result = new Result(false);
        result.Errors.AddRange(errors);
        return result;
    }
}