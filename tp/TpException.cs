namespace tp
{
 

    /// <summary>
    /// An exception class used by this system.
    /// </summary>
    public class TpException : ApplicationException
    {

        // ● construction 
        /// <summary>
        /// constructor
        /// </summary>
        public TpException()
        {
        }
        /// <summary>
        /// constructor
        /// </summary>
        public TpException(string Message)
            : base(Message)
        {
        }
        /// <summary>
        /// constructor
        /// </summary>
        public TpException(string Message, Exception InnerException)
            : base(Message, InnerException)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TpException(string Message, int ErrorCode)
            : base(Message)
        {
            this.ErrorCode = ErrorCode;
        }
        /// <summary>
        /// constructor
        /// </summary>
        public TpException(string Message, int ErrorCode, Exception InnerException)
            : base(Message, InnerException)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TpException(string Message, HttpStatusCode StatusCode)
            : base(Message)
        {
            this.StatusCode = StatusCode;
        }
        /// <summary>
        /// constructor
        /// </summary>
        public TpException(string Message, HttpStatusCode StatusCode, Exception InnerException)
            : base(Message, InnerException)
        {
            this.StatusCode = StatusCode;
        }

        // ● public
        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        public override string ToString()
        {
            StringBuilder SB = new StringBuilder();

            SB.Append(base.ToString());
            this.AddDataDictionaryTo(SB);

            return SB.ToString();
        }
 

        // ● properties
        /// <summary>
        /// Error code
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Http status code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    }
}
