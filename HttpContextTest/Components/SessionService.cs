using System.Text.Json;

namespace HttpContextTest.Components
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetVegetable(Vegetable vegetable)
        {
            var context = _httpContextAccessor.HttpContext;
            context.Items["Vegetable"] = vegetable; // Store in HttpContext
        }

        public Vegetable GetVegetable()
        {
            var context = _httpContextAccessor.HttpContext;
            return context.Items["Vegetable"] as Vegetable; // Retrieve from HttpContext
        }
    }
}
