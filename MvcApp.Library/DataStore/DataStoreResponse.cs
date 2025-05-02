namespace MvcApp.Library
{
    /// <summary>
    /// Data store response
    /// </summary>
    public class DataStoreResponse
    {
        public void AddError(string ErrorText)
        {
            if (Errors == null)
                Errors = new List<string>();

            Errors.Add(ErrorText);
        }
        public void ClearErrors()
        {
            if (Errors != null)
                Errors.Clear();
        }

        [JsonIgnore]
        public bool Succeeded => Errors == null || Errors.Count == 0;
        [JsonIgnore]
        public string Error => Errors != null && Errors.Count > 0 ? Errors[0] : string.Empty;
        public List<string> Errors { get; set; }
    }
 
}
