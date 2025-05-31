namespace tp.Web
{
    public interface IUserRequestor : IRequestor
    {
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// </summary> 
        string UserName { get; set; }
        /// <summary>
        /// Optional. The requestor email
        /// </summary> 
        string Email { get; set; }
    }
}
