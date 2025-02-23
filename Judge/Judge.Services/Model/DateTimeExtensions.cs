using System;

namespace Judge.Services.Model;

internal static class DateTimeExtensions
{
    public static DateTime SetUtcKind(this DateTime dateTime)
        => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
}