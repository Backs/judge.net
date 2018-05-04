using Microsoft.Owin;

namespace Judge.Application
{
    public class CallContextOwinContextAccessor : IOwinContextAccessor
    {
        public static IOwinContext OwinContext = null;
        public IOwinContext CurrentContext => OwinContext;
    }
}