namespace SmartPaperEdms.Web.App_Code
{
    public static class SessionHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static string GetUser()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                return httpContext.Session.GetString("user");
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public static string GetMenus()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                return httpContext.Session.GetString("Menus");
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public static string GetLicenceMsg()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                return httpContext.Session.GetString("LicenceMsg");
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
