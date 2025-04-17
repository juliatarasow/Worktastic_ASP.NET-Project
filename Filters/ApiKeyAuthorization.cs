using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Worktastic.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorization : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //prüft auf API Authentifizierung
            if (context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var key))
            {
                //holt aus appsettings.json den Value für ApiKey indem er sich dahin verbindet
                var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
                var configApiKey = config.GetValue<string>("ApiKey");
                if (key.Equals(configApiKey) == false)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
