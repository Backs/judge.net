namespace Judge.Application.Interfaces
{
    using System.Collections.Generic;
    using Judge.Application.ViewModels.Problems.ProblemsList;
    using Judge.Application.ViewModels.Problems.Statement;

    public interface IProblemsService
    {
        /// <summary>
        /// Get problems list for one page
        /// </summary>
        /// <param name="page">Page number from 1 to N</param>
        /// <param name="pageSize">Page size from 1 to N</param>
        /// <param name="userId"></param>
        /// <param name="openedOnly"></param>
        ProblemsListViewModel GetProblemsList(int page, int pageSize, long? userId, bool openedOnly);

        StatementViewModel GetStatement(long id, bool isAdmin);

        IReadOnlyCollection<ProblemItem> GetAllProblems();
    }
}
