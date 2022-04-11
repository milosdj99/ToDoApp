using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.ExtensionMethods
{
    public static class ControllerExtensions
    {
        public static string getUserEmail(this ControllerBase controller)
        {
            return controller.User.Claims.FirstOrDefault(x => x.Type == "http://mynamespace/email").ToString().Split(' ')[1];
        }
        public static string getUserName(this ControllerBase controller)
        {
            return controller.User.Claims.FirstOrDefault(x => x.Type == "http://mynamespace/username").ToString().Split(' ')[1];
        }


    }
}
