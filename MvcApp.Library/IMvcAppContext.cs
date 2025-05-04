namespace MvcApp.Library
{
    /// <summary>
    /// Represents this application's context
    /// <para>There is a single instance of this object, which is assigned to the <see cref="Lib.AppContext"/> property.</para>
    /// </summary>
    public interface IMvcAppContext: IWebContext
    {
        /// <summary>
        /// The default requestor
        /// </summary>
        IRequestor DefaultRequestor { get; }
        /// <summary>
        /// True when the current user/requestor is authenticated
        /// </summary>
        bool IsAuthenticated { get; }
        /// <summary>
        /// Application settings, coming from appsettings.json
        /// </summary>
        AppSettings AppSettings { get; }
    }
}
