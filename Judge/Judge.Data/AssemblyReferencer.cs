using System.Data.Entity.SqlServer;

namespace Judge.Data
{
    public static class AssemblyReferencer
    {
        public static string GetReference()
        {
            return typeof(SqlProviderServices).ToString();
        }
    }
}
