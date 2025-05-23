﻿namespace MvcApp.Library
{


    /// <summary>
    /// Base MVC controller of this application.
    /// <para>NOTE: Does NOT require authentication</para>
    /// </summary>
    [AllowAnonymous]
    public class AppControllerMvcBase : ControllerMvc
    {
        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public AppControllerMvcBase()
        {
        }

    }
}
