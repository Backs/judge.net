using System.Threading.Tasks;
using Judge.Web.Client.Submits;

namespace Judge.Services;

public interface ISubmitsService
{
    public Task<SubmitsList> SearchAsync(SubmitsQuery query);
}