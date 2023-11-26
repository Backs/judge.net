using System;

namespace Judge.Web.Client.Problems
{
    public class ProblemsList
    {
        public ProblemInfo[] Items { get; set; } = Array.Empty<ProblemInfo>();
        
        public int TotalCount { get; set; }
    }
}