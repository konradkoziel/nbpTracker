﻿namespace nbpTracker.Common
{
    public record Result<T>(bool IsSuccess, T? Value, string? Error)
    {
        public static Result<T> Ok(T value) => new(true, value, null);
        public static Result<T> Fail(string error) => new(false, default, error);
    }
    public record Result(bool IsSuccess, string? Error)
    {
        public static Result Ok() => new(true, null);
        public static Result Fail(string error) => new(false, error);
    }

}
