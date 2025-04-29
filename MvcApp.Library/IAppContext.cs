namespace MvcApp.Library
{
    /// <summary>
    /// Represents this application's context
    /// <para>There is a single instance of this object, which is assigned to the <see cref="Lib.AppContext"/> property.</para>
    /// </summary>
    public interface IAppContext: IWebContext
    {
        /// <summary>
        /// The default requestor
        /// </summary>
        IRequestor DefaultRequestor { get; }
    }
}
