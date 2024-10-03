using FootballLeague.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace FootballLeague.Filters
{
    public class UserProfileImageFilter : IAsyncActionFilter
    {
        private readonly IUserHelper _userHelper;

        public UserProfileImageFilter(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var email = context.HttpContext.User.Identity.Name;

                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _userHelper.GetUserByEmailAsync(email);
                    
                    context.HttpContext.Items["UserPhoto"] = user?.ImageProfileFullPath ?? "https://footballleague.blob.core.windows.net/default/no-profile.png";
                }
            }          

            await next();
        }
    }
}
