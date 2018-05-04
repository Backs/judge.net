using Microsoft.Owin;

namespace Judge.Application
{
    public interface IOwinContextAccessor
    {
        IOwinContext CurrentContext { get; }
    }
}