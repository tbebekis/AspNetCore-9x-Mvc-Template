namespace tp 
{
    /// <summary>
    /// To be used with a property must be serialized as CDATA section.
    /// <para>Property example: public CData Content { get; set; }</para>
    /// <para>From: https://stackoverflow.com/questions/1379888/how-do-you-serialize-a-string-as-cdata-using-xmlserializer</para>
    /// </summary>
    [Serializable]
    public class CData : IXmlSerializable
    {
        [NonSerialized]
        string fValue;

        // ●  construction
        /// <summary>
        /// Constructor
        /// </summary>
        public CData() : this(string.Empty)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public CData(string Value)
        {
            fValue = Value;
        }

        // ●  public
        /// <summary>
        /// Override
        /// </summary>
        public override string ToString()
        {
            return fValue;
        }

        // ●  IXmlSerializable implementation 
        /// <summary>
        /// IXmlSerializable implementation
        /// </summary>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
        /// <summary>
        /// IXmlSerializable implementation
        /// </summary>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            fValue = reader.ReadElementString();
        }
        /// <summary>
        /// IXmlSerializable implementation
        /// </summary>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteCData(fValue);
        }

        // ●  operators 
        /// <summary>
        /// Allow direct assignment from string:
        /// CData cdata = "abc";
        /// </summary>
        public static implicit operator CData(string value)
        {
            return new CData(value);
        }

        /// <summary>
        /// Allow direct assigment to string
        /// string str = cdata;
        /// </summary>
        public static implicit operator string(CData cdata)
        {
            return cdata == null ? null : cdata.fValue;
        }
    }
}
