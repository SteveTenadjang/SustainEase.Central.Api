namespace Central.Application.Common;

public class Result<T>
{
    protected Result(bool isSuccess, T? data, string? error = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }

    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Error { get; }
    public List<string> Errors { get; } = new();

    public static Result<T> Success(T data)
    {
        return new Result<T>(true, data);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default, error);
    }

    public static Result<T> Failure(List<string> errors)
    {
        var result = new Result<T>(false, default);
        result.Errors.AddRange(errors);
        return result;
    }
}

public class Result
{
    protected Result(bool isSuccess, string? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public string? Error { get; }
    public List<string> Errors { get; } = new();

    public static Result Success()
    {
        return new Result(true);
    }

    public static Result Failure(string error)
    {
        return new Result(false, error);
    }

    public static Result Failure(List<string> errors)
    {
        var result = new Result(false);
        result.Errors.AddRange(errors);
        return result;
    }
}