using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIVProject
{
    public class Localization
    {
    }

    public checkCulture(HttpRequestBase Request)
    {
        if (!Request["en-GB"].IsEmpty())
        {
            Culture = UICulture = "en-GB";
        }
        else if (!Request["hr-HR"].IsEmpty())
        {
            Culture = UICulture = "hr-HR";
        }
    }
}