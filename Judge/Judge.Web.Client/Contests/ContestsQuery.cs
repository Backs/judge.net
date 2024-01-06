using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

public class ContestsQuery
{
    [Range(0, int.MaxValue)] public int Skip { get; set; } = 0;

    [Range(1, 100)] public int Take { get; set; } = 50;
}