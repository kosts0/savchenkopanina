using practic1.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace practic1.Models
{
    [Authorize]
    public class Chesk
    {
        string [] s = Roles.GetRolesForUser();

    }
}