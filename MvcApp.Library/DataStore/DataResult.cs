namespace MvcApp.Library
{
    /// <summary>
    /// Data store response
    /// </summary>
    public class DataResult
    {
        // ● public
        /// <summary>
        /// Adds an error to the error list
        /// </summary>
        /// <param name="ErrorText"></param>
        public void AddError(string ErrorText)
        {
            if (Errors == null)
                Errors = new List<string>();

            Errors.Add(ErrorText);
        }
        /// <summary>
        /// Clears the error list
        /// </summary>
        public void ClearErrors()
        {
            if (Errors != null)
                Errors.Clear();
        }

        // ● properties
        /// <summary>
        /// True if there are no errors
        /// </summary>
        [JsonIgnore]
        public bool Succeeded => Errors == null || Errors.Count == 0;
        /// <summary>
        /// Returns the first error
        /// </summary>
        [JsonIgnore]
        public string Error => Errors != null && Errors.Count > 0 ? Errors[0] : string.Empty;
        /// <summary>
        /// The list of errors
        /// </summary>
        public List<string> Errors { get; set; }
    }
 
}
