namespace Judge.Application.Interfaces
{
    using Judge.Application.ViewModels.User;

    public interface IUserService
    {
        UserViewModel GetUserInfo(long id);
    }
}
