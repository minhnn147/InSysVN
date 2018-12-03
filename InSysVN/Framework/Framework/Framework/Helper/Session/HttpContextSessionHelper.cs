using System.Web;

namespace Framework.Helper.Session
{
    public interface ISessionHelper
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
    }

    public class HttpContextSessionHelper : ISessionHelper
    {
        private readonly HttpContext _context;

        public HttpContextSessionHelper(HttpContext context)
        {
            _context = context;
        }

        public T Get<T>(string key)
        {
            object value = _context.Session[key];
            return value == null ? default(T) : (T)value;
        }

        public void Set<T>(string key, T value)
        {
            _context.Session[key] = value;
        }
    }
}
