using System.Web;

namespace Judge.Web.Services
{
    public class SessionService : ISessionService
    {
        public int GetSelectedLanguage()
        {
            var value = HttpContext.Current.Session["SelectedLanguage"];
            if (value != null)
            {
                return (value as int?) ?? 0;
            }
            return 0;
        }

        public void SaveSelectedLanguage(int value)
        {
            HttpContext.Current.Session["SelectedLanguage"] = value;
        }
    }
}