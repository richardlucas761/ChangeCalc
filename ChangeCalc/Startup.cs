﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChangeCalc.Startup))]

namespace ChangeCalc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
