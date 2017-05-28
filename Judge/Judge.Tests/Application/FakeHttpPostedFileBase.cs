using System.IO;
using System.Web;

namespace Judge.Tests.Application
{
    internal sealed class FakeHttpPostedFileBase : HttpPostedFileBase
    {
        public override Stream InputStream => Stream.Null;
        public override string FileName => "temp";
    }
}
