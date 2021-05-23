using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

namespace PhotoGallery.API.Controllers.Base
{
    public abstract class ControllerBaseWithUser : ControllerBase
    {
        protected int UserId => int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
