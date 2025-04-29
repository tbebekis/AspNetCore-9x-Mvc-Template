namespace tp.Logging
{
    /// <summary>
    /// Helper, represents this library.
    /// </summary>
    static internal partial class LogLib
    {
        /// <summary>
        /// Used when calling async methods as synchronous
        /// </summary>
        static public readonly TaskFactory TaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        // ● Exceptions
        /// <summary>
        /// Adds E.Data dictionary information to SB
        /// </summary>
        static public void AddDataDictionaryTo(this Exception E, StringBuilder SB)
        {
            if ((E != null) && (E.Data.Count > 0))
            {
                SB.AppendLine();

                foreach (object Key in E.Data.Keys)
                    SB.AppendLine(string.Format("{0}: {1}", Key, E.Data[Key]));

                SB.AppendLine();
            }

        }
        /// <summary>
        /// Returns a string containing all exception information,
        /// including the Data dictionary and the inner exceptions
        /// </summary>
        static public string GetExceptionText(this Exception Ex)
        {
            StringBuilder SB = new StringBuilder();

            Action<Exception> Proc = null;
            Proc = delegate (Exception E)
            {
                if (E != null)
                {
                    SB.AppendLine(E.ToString());
                    AddDataDictionaryTo(E, SB);

                    if (E.InnerException != null)
                    {
                        SB.AppendLine(" ----------------------------------------------------------------");
                        SB.AppendLine(" ");

                        Proc(E.InnerException);
                    }
                }
            };

            SB.AppendLine(" ");

            Proc(Ex);

            return SB.ToString();

        }

        // ●  json 
        /// <summary>
        /// Creates and returns an instance of default settings for the serializer
        /// </summary>
        static public JsonSerializerSettings CreateDefaultSettings(bool Formatted = true, bool IgnoreNullProperties = false)
        {
            JsonSerializerSettings Result = new JsonSerializerSettings();
            Result.Formatting = Formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;
            Result.NullValueHandling = IgnoreNullProperties ? NullValueHandling.Ignore : NullValueHandling.Include;
            Result.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            return Result;
        }
        /// <summary>
        /// Deserializes (creates) an object of a specified type by deserializing a specified json text.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public T Deserialize<T>(string JsonText, JsonSerializerSettings Settings = null)
        {
            return !string.IsNullOrWhiteSpace(JsonText) ? JsonConvert.DeserializeObject<T>(JsonText, Settings ?? DefaultSettings) : default(T);
        }
        /// <summary>
        /// Deserializes (creates) an object of a specified type by deserializing a specified json text.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public object Deserialize(string JsonText, Type ReturnType, JsonSerializerSettings Settings = null)
        {
            return !string.IsNullOrWhiteSpace(JsonText) ? JsonConvert.DeserializeObject(JsonText, ReturnType, Settings ?? DefaultSettings) : null;
        }
        /// <summary>
        /// Loads an object's properties from a specified json text.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public void PopulateObject(object Instance, string JsonText, JsonSerializerSettings Settings = null)
        {
            if (Instance != null && !string.IsNullOrWhiteSpace(JsonText))
            {
                JsonConvert.PopulateObject(JsonText, Instance, Settings ?? DefaultSettings);
            }
        }

        /// <summary>
        /// Converts Instance to a json string using the NewtonSoft json serializer.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public string Serialize(object Instance, JsonSerializerSettings Settings = null)
        {
            return Instance != null ? JsonConvert.SerializeObject(Instance, Settings ?? DefaultSettings) : string.Empty;
        }
        /// <summary>
        /// Converts Instance to a json string using the NewtonSoft json serializer.
        /// </summary>
        static public string Serialize(object Instance, bool Formatted)
        {
            return Serialize(Instance, CreateDefaultSettings(Formatted));
        }

        /// <summary>
        /// Returns a specified json text as formatted for readability.
        /// </summary>
        static public string Format(string JsonText)
        {
            if (string.IsNullOrWhiteSpace(JsonText))
            {
                var Settings = CreateDefaultSettings(true);
                object Instance = JsonConvert.DeserializeObject(JsonText);
                return Serialize(Instance, Settings);
            }

            return JsonText;
        }

        /// <summary>
        /// Default settings
        /// </summary>
        static public JsonSerializerSettings DefaultSettings { get; } = CreateDefaultSettings(true);

        // ●  miscs  
        /// <summary>
        /// Creates and returns a new Guid.
        /// <para>If UseBrackets is true, the new guid is surrounded by {}</para>
        /// </summary>
        static public string GenId(bool UseBrackets)
        {
            string format = UseBrackets ? "B" : "D";
            return Guid.NewGuid().ToString(format).ToUpper();
        }
        /// <summary>
        /// Creates and returns a new Guid WITHOUT surrounding brackets, i.e. {}
        /// </summary>
        static public string GenId()
        {
            return GenId(false);
        }

        /// <summary>
        /// Creates and returns a file name based on DT.
        /// <para>The returned string has the format </para>
        /// <para><c>yyyy-MM-dd HH_mm_ss__fff</c></para>
        /// </summary>
        public static string ToFileName(this DateTime DT, bool UseMSecs)
        {
            return UseMSecs ? DT.ToString("yyyy-MM-dd HH_mm_ss__fff") : DT.ToString("yyyy-MM-dd HH_mm_ss");
        }

        // ● properties
        /// <summary>
        /// The username of the current user of the local computer
        /// </summary>
        static public string NetUserName { get; private set; } = Environment.UserName;
        /// <summary>
        /// The name of the local computer
        /// </summary>
        static public string HostName { get; private set; } = System.Net.Dns.GetHostName();
    }
}
