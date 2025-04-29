namespace tp
{
    /// <summary>
    /// Helper json static class
    /// <para>SEE: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview </para>
    /// </summary>
    static public class Json
    {

        /// <summary>
        /// Returns true if ClassType implements InterfaceType.  
        /// </summary>
        static public bool ImplementsInterface(Type ClassType, Type InterfaceType)
        {
            return (ClassType != null) && (Array.IndexOf(ClassType.GetInterfaces(), InterfaceType) != -1);
        }
        /// <summary>
        /// Returns trur if Value exists in List, case insensitively.
        /// </summary>
        static public bool ContainsText(IList<string> List, string Value)
        {
            if (List != null)
            {
                for (int i = 0; i < List.Count; i++)
                    if (string.Compare(List[i], Value, StringComparison.InvariantCultureIgnoreCase) == 0)
                        return true;
            }
            return false;
        }

        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        static Json()
        {

        }

        // ● serialize
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

        // ● de-serialize
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
        /// Converts a json text to a Dictionary instance.
        /// </summary>
        static public Dictionary<string, string> ToDictionary(string JsonText)
        {
            return Deserialize<Dictionary<string, string>>(JsonText);
        }
        /// <summary>
        /// Converts json text to a dynamic object
        /// </summary>
        static public dynamic ToDynamic(string JsonText)
        {
            return !string.IsNullOrWhiteSpace(JsonText) ? JsonConvert.DeserializeObject(JsonText) as dynamic : null;
        }

        // ● json streams
        /// <summary>
        /// Converts Instance to a json string using the NewtonSoft json serializer and then to stream.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public MemoryStream ToJsonStream(object Instance, JsonSerializerSettings Settings = null)
        {
            MemoryStream MS = new MemoryStream();
            ToJsonStream(Instance, MS, Settings);
            return MS;
        }
        /// <summary>
        /// Converts Instance to a json string using the NewtonSoft json serializer and then to stream.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public void ToJsonStream(object Instance, Stream Stream, JsonSerializerSettings Settings = null)
        {
            string JsonText = Serialize(Instance, Settings);// ToJson(Instance, Settings);
            JsonTextToStream(JsonText, Stream);
        }

        /// <summary>
        /// Reads the json text from a stream and then deserializes (creates) an object of a specified type.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public object FromJsonStream(Type ClassType, Stream Stream, JsonSerializerSettings Settings = null)
        {
            string JsonText = StreamToJsonText(Stream);
            return Deserialize(JsonText, ClassType, Settings);// FromJson(ClassType, JsonText, Settings);
        }
        /// <summary>
        /// Loads an object's properties from a specified stream, after reading the json text from the stream.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public void FromJsonStream(object Instance, Stream Stream, JsonSerializerSettings Settings = null)
        {
            string JsonText = StreamToJsonText(Stream);
            //FromJson(Instance, JsonText, Settings);
            PopulateObject(Instance, JsonText, Settings);
        }

        /// <summary>
        /// Converts a specified json text to a stream.
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public MemoryStream JsonTextToStream(string JsonText)
        {
            MemoryStream MS = new MemoryStream();
            JsonTextToStream(JsonText, MS);
            return MS;
        }
        /// <summary>
        /// Converts a specified json text to a stream.
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public void JsonTextToStream(string JsonText, Stream Stream)
        {
            byte[] Buffer = Encoding.UTF8.GetBytes(JsonText);
            Stream.Write(Buffer, 0, Buffer.Length);
        }

        /// <summary>
        /// Reads a stream as json text.
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public string StreamToJsonText(Stream Stream)
        {
            string JsonText = string.Empty;
            if (Stream != null && Stream.Length > 0)
            {
                using (StreamReader reader = new StreamReader(Stream, Encoding.UTF8))
                {
                    JsonText = reader.ReadToEnd();
                }
            }

            return JsonText;
        }

        /// <summary>
        /// Returns the text of the input stream of a request (HttpContext.Request.Body) as a Dictionary. To be used when POST-ing json data.
        /// </summary>
        static public Dictionary<string, dynamic> GetRequestDic(Stream RequestBodyStream)
        {
            if (RequestBodyStream != null && RequestBodyStream.CanSeek)
            {
                string Text = StreamToJsonText(RequestBodyStream);
                if (!string.IsNullOrWhiteSpace(Text))
                {
                    return Deserialize<Dictionary<string, dynamic>>(Text); //Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Text);
                }
            }

            return new Dictionary<string, dynamic>();
        }

        // ● load from/save to file
        /// <summary>
        /// Saves an instance as json text in a specified file.
        /// </summary>
        static public void SaveToFile(object Instance, string FilePath, string Encoding = "utf-8")
        {
            string Folder = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrWhiteSpace(Folder))
                Directory.CreateDirectory(Folder);
            string JsonText = Serialize(Instance, DefaultSettings); //ToJson(Instance);
            File.WriteAllText(FilePath, JsonText, System.Text.Encoding.GetEncoding(Encoding));
        }

        /// <summary>
        /// Loads the properties of an instance by reading the json text of a specified file.
        /// </summary>
        static public void LoadFromFile(object Instance, string FilePath, string Encoding = "utf-8")
        {
            if (File.Exists(FilePath))
            {
                string JsonText = File.ReadAllText(FilePath, System.Text.Encoding.GetEncoding(Encoding));
                PopulateObject(Instance, JsonText, DefaultSettings);
            }
        }
        /// <summary>
        ///  Creates and returns an object of ClassType using the json text of a specified file
        /// </summary>
        static public object LoadFromFile(Type ClassType, string FilePath, string Encoding = "utf-8")
        {
            if (File.Exists(FilePath))
            {
                string JsonText = File.ReadAllText(FilePath, System.Text.Encoding.GetEncoding(Encoding));   
                return Deserialize(JsonText, ClassType, DefaultSettings);
            }

            return null;
        }

        // ● miscs
        /// <summary>
        /// Converts an object to JObject
        /// </summary>
        static public JObject ObjectToJObject(object Instance)
        {
            string JsonText = Serialize(Instance, DefaultSettings);
            return JObject.Parse(JsonText);
        }
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

        // ● properties
        /// <summary>
        /// Default settings
        /// </summary>
        static public JsonSerializerSettings DefaultSettings { get; } = CreateDefaultSettings(true);
    }



    /// <summary>
    /// Used in excluding properties when serializing.
    /// <para>E.g. JsonConvert.SerializeObject(Instance, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new JsonNetContractResolver(ExcludeProperties) }) </para>
    /// </summary>
    public class JsonNetContractResolver : DefaultContractResolver
    {

        string[] ExcludeProperties = new string[0];

        /// <summary>
        /// Override.  Creates properties for the given Newtonsoft.Json.Serialization.JsonContract.
        /// </summary>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            properties = properties.Where(p => !Json.ContainsText(ExcludeProperties, p.PropertyName)).ToList();
            return properties;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonNetContractResolver()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonNetContractResolver(string[] ExcludeProperties)
        {
            this.ExcludeProperties = ExcludeProperties;
        }
    }
}
