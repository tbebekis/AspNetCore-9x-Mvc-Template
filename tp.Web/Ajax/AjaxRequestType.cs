namespace tp.Web.Ajax
{

    /// <summary>
    /// The request type.  Ui or Proc.  
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AjaxRequestType
    {
        /// <summary>
        /// Request the execution of a procedure. 
        /// </summary>
        Proc = 1,
        /// <summary>
        /// Requests a Ui. 
        /// </summary>
        Ui = 0,              
    }
}
