using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HttpContextTest.Components
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionName = "_Name";
        private const string SessionAge = "_Age";

        public string Name { get; set; }
        public string Color { get; set; }
        public string Type { get; set; } // e.g., Root, Leafy, etc.
        public decimal Weight { get; set; } // In kg
        public bool IsOrganic { get; set; } // Organic or not
        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetVegetableInSession(Vegetable vegetable)
        {
            var context = _httpContextAccessor.HttpContext;
            context.Session.SetString("Vegetable", JsonSerializer.Serialize(vegetable));
        }

        public Vegetable GetVegetableFromSession()
        {
            var context = _httpContextAccessor.HttpContext;
            var vegetableJson = context.Session.GetString("Vegetable");
            return vegetableJson != null ? JsonSerializer.Deserialize<Vegetable>(vegetableJson) : null;
        }
    }
}
