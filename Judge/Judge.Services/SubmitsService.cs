using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.SubmitSolution;
using Judge.Services.Converters;
using Judge.Web.Client.Submits;

namespace Judge.Services;

internal sealed class SubmitsService : ISubmitsService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;

    public SubmitsService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<SubmitsList> SearchAsync(SubmitsQuery query)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();

        var submitResultRepository = unitOfWork.SubmitResultRepository;
        var languageRepository = unitOfWork.LanguageRepository;
        var taskRepository = unitOfWork.TaskRepository;
        var userRepository = unitOfWork.UserRepository;

        var languages = await languageRepository.GetDictionaryAsync(false);
        var specification = new AdminSearchSubmitsSpecification(null, null);
        var submits =
            await submitResultRepository.SearchAsync(specification, query.Skip, query.Take);

        var userSpecification = new UserListSpecification(submits.Select(o => o.Submit.UserId).Distinct());
        var tasks = await taskRepository.GetDictionaryAsync(submits.Select(o => o.Submit.ProblemId).Distinct());
        var users = await userRepository.GetDictionaryAsync(userSpecification);

        var items = submits.Select(o =>
                SubmitsConverter.Convert(o, languages[o.Submit.LanguageId], tasks[o.Submit.ProblemId],
                    users[o.Submit.UserId]))
            .ToArray();

        var totalCount = await submitResultRepository.CountAsync(specification);
        return new SubmitsList
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}