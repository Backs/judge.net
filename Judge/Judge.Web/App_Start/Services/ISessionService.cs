namespace Judge.Web.Services
{
    public interface ISessionService
    {
        int GetSelectedLanguage();
        void SaveSelectedLanguage(int value);
    }
}