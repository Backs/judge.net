using Judge.Application.ViewModels.User;

namespace Judge.Application.Interfaces
{
    public interface IUserService
    {
        UserViewModel GetUserInfo(long id);
    }
}
