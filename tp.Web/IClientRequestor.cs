namespace tp.Web
{
    public interface IClientRequestor : IRequestor
    {
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// </summary> 
        string ClientId { get; set; }
    }
}
