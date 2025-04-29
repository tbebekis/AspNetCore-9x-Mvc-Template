using System.Runtime.Serialization;

namespace tp
{
    /// <summary>
    /// Xml utility methods
    /// </summary>
    static public class Xml
    {

        /// <summary>
        /// Converts the name to a valid XML name.
        /// </summary>
        static public string EncodeName(string Name)
        {
            return XmlConvert.EncodeName(Name);
        }
        /// <summary>
        /// Decodes a name.
        /// </summary>
        static public string DecodeName(string Name)
        {
            return XmlConvert.DecodeName(Name);
        }
        /// <summary>
        /// Returns true if Name can be used to name an xml node
        /// </summary>
        static public bool IsValidName(string Name)
        {
            try
            {
                XmlConvert.VerifyName(Name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns formatted xml string (indent and newlines) from unformatted Xml
        /// string for display in eg textboxes.
        /// <para>SOURCE: http://www.codeproject.com/KB/cpp/FormattingXML.aspx</para>
        /// </summary>    
        static public string Format(string Text)
        {
            //load unformatted xml into a dom
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(Text);

            //will hold formatted xml
            StringBuilder SB = new StringBuilder();

            //pumps the formatted xml into the StringBuilder above
            StringWriter SW = new StringWriter(SB);

            //does the formatting
            XmlTextWriter XTW = null;

            try
            {
                //point the XTW at the StringWriter
                XTW = new XmlTextWriter(SW);

                //we want the output formatted
                XTW.Formatting = System.Xml.Formatting.Indented;

                //get the dom to dump its contents into the XTW 
                Doc.WriteTo(XTW);
            }
            finally
            {
                //clean up even if error
                if (XTW != null)
                    XTW.Close();
            }

            //return the formatted xml
            return SB.ToString();
        }

        // ●  Document 
        /// <summary>
        /// Creates an XmlDocument with a given declaration.
        /// </summary>
        static public XmlDocument CreateDoc(string RootName = "root", string Encoding = "utf-8", string Version = "1.0", string StandAlone = "yes")
        {
            XmlDocument Doc = new XmlDocument();

            XmlElement Root = Doc.CreateElement(RootName);
            Doc.AppendChild(Root);

            XmlDeclaration Declaration = Doc.CreateXmlDeclaration(Version, Encoding, StandAlone);
            Doc.InsertBefore(Declaration, Root);

            return Doc;
        }
        /// <summary>
        /// Creates and returns an XmlDocument loaded with Text.
        /// </summary>
        static public XmlDocument LoadXml(string Text)
        {
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(Text);
            return Doc;
        }
        /// <summary>
        /// Returns the XmlNode.OwnerDocument.
        /// <para>WARNING: Throws an exception if OwnerDocument in null.</para>
        /// </summary>
        static public XmlDocument DocumentOf(XmlNode Node)
        {
            if (Node.OwnerDocument == null)
                throw new ApplicationException("Node OwnerDocument is null");
            return Node.OwnerDocument;
        }

        // ●  Node - XmlElement or XmlAttribute 
        /// <summary>
        /// Returns the "text" of Node, if Node is not null, else Default.
        /// <para>Node can be either an XmlElement or an XmlAttribute.</para>
        /// <para>If Node is an XmlAttribute it returns the XmlAttribute.Value.</para>
        /// <para>If Node is an XmlElement it returns the XmlElement.InnerText.</para>
        /// <para>WARNING: If Default is null an exception is thrown.</para>
        /// <para>NOTE: Both XmlAttribute.Value and XmlElement.InnerText are of type string.</para>
        /// <para>CAUTION: The XmlElement.Value property MUST be always null.</para>
        /// </summary>
        static public string AsString(this XmlNode Node, string Default = "")
        {
            if (Node != null)
            {
                if ((Node is XmlElement) && (!string.IsNullOrWhiteSpace(((XmlElement)Node).InnerText)))
                    return ((XmlElement)Node).InnerText;
                else if ((Node is XmlAttribute) && (!string.IsNullOrWhiteSpace(((XmlAttribute)Node).Value)))
                    return ((XmlAttribute)Node).Value;
            }

            return Default;
        }
        /// <summary>
        /// Reads the "value" of a node (XmlElement or XmlAttribute) and returns a value.
        /// </summary>
        static public int AsInteger(this XmlNode Node, int Default = 0)
        {
            string S = AsString(Node, XmlConvert.ToString(Default));
            return XmlConvert.ToInt32(S);
        }
        /// <summary>
        /// Reads the "value" of a node (XmlElement or XmlAttribute) and returns a value.
        /// </summary>
        static public double AsDouble(this XmlNode Node, double Default = 0)
        {
            string S = AsString(Node, XmlConvert.ToString(Default));
            return XmlConvert.ToDouble(S);
        }
        /// <summary>
        /// Reads the "value" of a node (XmlElement or XmlAttribute) and returns a value.
        /// </summary>
        static public decimal AsDecimal(this XmlNode Node, decimal Default = 0)
        {
            string S = AsString(Node, XmlConvert.ToString(Default));
            return XmlConvert.ToDecimal(S);
        }
        /// <summary>
        /// Reads the "value" of a node (XmlElement or XmlAttribute) and returns a value.
        /// </summary>
        static public bool AsBoolean(this XmlNode Node, bool Default = false)
        {
            string S = AsString(Node, XmlConvert.ToString(Default));
            return XmlConvert.ToBoolean(S);
        }
        /// <summary>
        /// Reads the "value" of a node (XmlElement or XmlAttribute) and returns a value.
        /// </summary>
        static public DateTime AsDateTime(this XmlNode Node, string Format = "yyyy-MM-dd HH:mm:ss")
        {
            string S = AsString(Node, string.Empty);
            return XmlConvert.ToDateTime(S, Format);
        }
        /// <summary>
        /// Reads the "value" of a node (XmlElement or XmlAttribute) and returns a value.
        /// </summary>
        static public TimeSpan AsTimeSpan(this XmlNode Node)
        {
            string S = AsString(Node, string.Empty);
            return XmlConvert.ToTimeSpan(S);
        }

        /// <summary>
        /// If Node is not null, then it sets Value as the "text" of the Node.
        /// <para>Node can be either an XmlElement or an XmlAttribute.</para>
        /// <para>If Node is an XmlAttribute it sets the XmlAttribute.Value.</para>
        /// <para>If Node is an XmlElement it sets the XmlElement.InnerText.</para>
        /// <para>WARNING: If Value is null an exception is thrown.</para>
        /// <para>NOTE: Both XmlAttribute.Value and XmlElement.InnerText are of type string.</para>
        /// <para>CAUTION: The XmlElement.Value property MUST be always null.</para>
        /// </summary>
        static public void SetNode(this XmlNode Node, string Value)
        {
            if (Value == null)
                throw new ArgumentNullException("Value");

            if (Node != null)
            {
                if (Node is XmlElement)
                    ((XmlElement)Node).InnerText = Value;
                else if (Node is XmlAttribute)
                    ((XmlAttribute)Node).Value = Value;
            }
        }
        /// <summary>
        /// Sets the "value" of a node (XmlElement or XmlAttribute)
        /// </summary>
        static public void SetNode(this XmlNode Node, int Value)
        {
            string S = XmlConvert.ToString(Value);
            SetNode(Node, S);
        }
        /// <summary>
        /// Sets the "value" of a node (XmlElement or XmlAttribute)
        /// </summary>
        static public void SetNode(this XmlNode Node, double Value)
        {
            string S = XmlConvert.ToString(Value);
            SetNode(Node, S);
        }
        /// <summary>
        /// Sets the "value" of a node (XmlElement or XmlAttribute)
        /// </summary>
        static public void SetNode(this XmlNode Node, decimal Value)
        {
            string S = XmlConvert.ToString(Value);
            SetNode(Node, S);
        }
        /// <summary>
        /// Sets the "value" of a node (XmlElement or XmlAttribute)
        /// </summary>
        static public void SetNode(this XmlNode Node, bool Value)
        {
            string S = XmlConvert.ToString(Value);
            SetNode(Node, S);
        }
        /// <summary>
        /// Sets the "value" of a node (XmlElement or XmlAttribute)
        /// </summary>
        static public void SetNode(this XmlNode Node, DateTime Value, string Format = "yyyy-MM-dd HH:mm:ss")
        {
            string S = XmlConvert.ToString(Value, Format);
            SetNode(Node, S);
        }
        /// <summary>
        /// Sets the "value" of a node (XmlElement or XmlAttribute)
        /// </summary>
        static public void SetNode(this XmlNode Node, TimeSpan Value)
        {
            string S = XmlConvert.ToString(Value);
            SetNode(Node, S);
        }

        // ●  Child XmlElement nodes
        /// <summary>
        /// Gets the child XmlElement with ChildName of ParentNode, if exists, else null.
        /// <para>NOTE: ParentNode can be either an XmlDocument or an XmlElement</para>
        /// </summary>
        static public XmlElement FindChild(XmlNode ParentNode, string ChildName)
        {
            if ((ParentNode is XmlElement) || (ParentNode is XmlDocument))
                //if (ParentNode.HasChildNodes)
                for (int i = 0; i < ParentNode.ChildNodes.Count; i++)
                    if (ParentNode.ChildNodes[i] is XmlElement)
                        if (((XmlElement)ParentNode.ChildNodes[i]).LocalName == ChildName)
                            return (XmlElement)ParentNode.ChildNodes[i];
            return null;
        }
        /// <summary>
        /// Returns true if a parent node (XmlElement) has a child XmlElement under a specified name.
        /// </summary>
        static public bool HasElement(XmlNode ParentNode, string ChildName)
        {
            return FindChild(ParentNode, ChildName) != null;
        }

        /// <summary>
        /// Returns a child XmlElement with ChildName of the ParentNode. If the child XmlElement does not exist
        /// it creates and adds the XmlElement to the ParentNode.
        /// <para>NOTE: ParentNode can be either an XmlDocument or an XmlElement</para>
        /// </summary>
        static public XmlElement GetChild(XmlNode ParentNode, string ChildName)
        {
            XmlElement Result = FindChild(ParentNode, ChildName);
            if (Result == null)
                Result = AddChild(ParentNode, ChildName);

            return Result;
        }
        /// <summary>
        /// Creates a XmlElement with ChildName, adds the newly created XmlElement to the ParentNode
        /// and returns it.
        /// <para>If ParentNode is an XmlDocument with no Root, then the new XmlElement becomes the Root,
        /// otherwise it is added as a child to the Root of the XmlDocument.</para>
        /// <para>WARNING: If ParentNode is null an exception is thrown.</para>
        /// <para>WARNING: If ParentNode is not an XmlDocument or an XmlElement an exception is thrown. </para>
        /// </summary>
        static public XmlElement AddChild(XmlNode ParentNode, string ChildName)
        {
            if (ParentNode == null)
                throw new ApplicationException("ParentNode is null");

            if (ParentNode is XmlElement)
            {
                XmlElement Node = DocumentOf(ParentNode).CreateElement(ChildName);
                ParentNode.AppendChild(Node);
                return Node;
            }
            else if (ParentNode is XmlDocument)
            {
                XmlDocument Doc = (XmlDocument)ParentNode;
                XmlElement Root = Doc.DocumentElement;

                if (Root == null)
                {
                    Root = Doc.CreateElement(ChildName);
                    Doc.AppendChild(Root);
                    return Root;
                }
                else
                {
                    XmlElement Node = Doc.CreateElement(ChildName);
                    Root.AppendChild(Node);
                    return Node;
                }
            }
            else
                throw new ApplicationException("ParentNode is not an XmlElement");
        }
        /// <summary>
        /// Gets the ChildNodes (XmlNodeList) of a child XmlNode (must be XmlElement) with ChildName.
        /// The list may be empty though. If ParentNode has not an XmlNode with ChildName
        /// it returns null.
        /// </summary>
        static public XmlNodeList GetChildChildNodes(XmlNode ParentNode, string ChildName)
        {
            XmlElement ChildNode = FindChild(ParentNode, ChildName);
            if (ChildNode != null)
                return ChildNode.ChildNodes;

            return null;
        }

        // ●  CDATA
        /// <summary>
        /// Gets the Value of the first child of Node which is found to be an XmlNodeType.CDATA node.
        /// If nothing is found it returns Default.
        /// </summary>
        static public string GetNodeTextAsCData(XmlNode Node, string Default)
        {
            if (Node == null)
                return Default;

            if (!Node.HasChildNodes)
                return Default;

            if (Node.ChildNodes.Count == 1)
            {
                if (Node.ChildNodes[0].NodeType != XmlNodeType.CDATA)
                    return Default;

                if (string.IsNullOrWhiteSpace(Node.ChildNodes[0].Value))
                    return Default;

                string S = Node.ChildNodes[0].Value.Trim();
                //if (S.StartsWith(Environment.NewLine))
                //    S.Remove(0, Environment.NewLine.Length);

                return S;
            }
            else
            {
                for (int i = 0; i < Node.ChildNodes.Count; i++)
                {
                    if (Node.ChildNodes[i].NodeType == XmlNodeType.CDATA)
                    {
                        string S = Node.ChildNodes[i].Value.Trim();
                        //if (S.StartsWith(Environment.NewLine))
                        //    S.Remove(0, Environment.NewLine.Length);

                        return S;
                    }

                }
            }

            return Default;

        }
        /// <summary>
        /// Adds Value as a new XmlCDataSection to Node.
        /// <para>WARNING: If Node is not an XmlElement an exception is thrown. </para>
        /// </summary>
        static public void SetNodeTextAsCData(XmlNode Node, string Value)
        {
            if (Node == null)
                throw new ApplicationException("Node is null");

            if (!(Node is XmlElement))
                throw new ApplicationException("ParentNode is not an XmlElement");

            if (!Value.StartsWith(Environment.NewLine))
                Value = Environment.NewLine + Value;


            XmlCDataSection CData;
            var Q = Node.ChildNodes.OfType<XmlCDataSection>();
            if ((Q != null) && (Q.Count() > 0))
            {
                CData = Q.ElementAt(0);
                CData.Value = Value;
            }
            else
            {
                CData = DocumentOf(Node).CreateCDataSection(Value);
                Node.AppendChild(CData);
            }
        }
        /// <summary>
        /// If Node has a child with ChildName and that child has an XmlCDataSection then it returns
        /// the text of that XmlCDataSection, else Default.
        /// </summary>
        static public string GetChildNodeTextAsCData(XmlNode Node, string ChildName, string Default)
        {
            XmlElement ChildNode = FindChild(Node, ChildName);
            if (ChildNode != null)
                return GetNodeTextAsCData(ChildNode, Default);

            return Default;
        }
        /// <summary>
        /// Searches for an XmlElement with ChildName child to Node. If none found it creates one.
        /// Then it creates a new XmlCDataSection to that child XmlElement and adds the Text. 
        /// <para>WARNING: If Node is not an XmlDocument or an XmlElement an exception is thrown. </para>
        /// </summary>
        static public void SetChildNodeTextAsCData(XmlNode Node, string ChildName, string Value)
        {
            if (Value == null)
                throw new ArgumentNullException("Value");

            if (Node == null)
                return;

            XmlElement ChildNode = FindChild(Node, ChildName);

            if (ChildNode == null)
                ChildNode = AddChild(Node, ChildName);

            SetNodeTextAsCData(ChildNode, Value);
        }

        // ●  Attribute
        /// <summary>
        /// Returns true if Node has an Attribute with AttrName
        /// </summary>
        static public bool HasAttribute(XmlNode Node, string AttrName)
        {
            return Node.Attributes.GetNamedItem(AttrName) != null;
        }
        /// <summary>
        /// Returns an Attribute with AttrName of the Node, if exists, else null.
        /// </summary>
        static public XmlAttribute FindAttribute(XmlNode Node, string AttrName)
        {
            return Node.Attributes.GetNamedItem(AttrName) as XmlAttribute;
        }
        /// <summary>
        /// Returns an Attribute with AttrName of the Node. If the Attribute does not exist
        /// it creates and adds the Attribute to the Node.
        /// </summary> 
        static public XmlAttribute GetAttribute(XmlNode Node, string AttrName, bool ForceCreate)
        {
            XmlAttribute Result = Node.Attributes.GetNamedItem(AttrName) as XmlAttribute;
            if ((Result == null) && ForceCreate)
            {
                Result = DocumentOf(Node).CreateAttribute(AttrName);
                Node.Attributes.Append(Result);
            }

            return Result;
        }

        // ●  using the XmlSerializer
        /// <summary>
        /// Converts Instance to xml representation and returns the xml text.
        /// <para>NOTE: uses the XmlSerializer</para>
        /// <para>NOTE: By default XmlSerializer serializes to UTF16. This method serializes by default to UTF8.</para>
        /// </summary>
        static public string ToXml(object Instance, bool AsUTF8 = true)
        {
            if (Instance == null)
                return string.Empty;

            XmlSerializer serializer = new XmlSerializer(Instance.GetType());

            using (StringWriter SW = AsUTF8 ? new StringWriter_UTF8() : new StringWriter())
            {
                serializer.Serialize(SW, Instance);
                return SW.ToString();
            }
        }
        /// <summary>
        /// Creates an object of ClassType using the xml Text
        /// <para>NOTE: uses the XmlSerializer</para>
        /// </summary>
        static public object FromXml(Type ClassType, string XmlText)
        {
            if (string.IsNullOrWhiteSpace(XmlText))
                return null;

            XmlSerializer serializer = new XmlSerializer(ClassType);
            using (StringReader Reader = new StringReader(XmlText))
                return serializer.Deserialize(Reader);
        }
        /* WARNING: warning SYSLIB0050: 'FormatterServices' is obsolete: 'Formatter-based serialization is obsolete and should not be used.'  
        /// <summary>
        /// Loads an object's properties from a specified xml text.
        /// </summary>
        static public void FromXml(object Instance, string XmlText)
        {
            object Temp = FromXml(Instance.GetType(), XmlText);

#pragma warning disable SYSLIB0050
            MemberInfo[] members = FormatterServices.GetSerializableMembers(Instance.GetType());
            FormatterServices.PopulateObjectMembers(Instance, members, FormatterServices.GetObjectData(Temp, members));
#pragma warning restore SYSLIB0050
        }
              */

        /// <summary>
        /// Removes an Attribure from an XmlElement by name, if exists.
        /// </summary>
        static void RemoveAttribute(XmlElement Element, string AttrName)
        {
            if (Element.Attributes != null && Element.Attributes.GetNamedItem(AttrName) != null)
            {
                XmlAttribute Attr = Element.GetAttributeNode(AttrName);
                if (Attr != null)
                    Element.RemoveAttributeNode(Attr);
            }
        }
        /// <summary>
        /// Remove null nodes, i.e. having an Attribute as xsi:nil='true'
        /// </summary>
        static string RemoveNilElements(string XmlText)
        {
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(XmlText);
            XmlElement Root = Doc.DocumentElement;

            // remove null nodes, i.e. having an Attribute as xsi:nil='true'
            if (Root.Attributes != null && Root.Attributes.GetNamedItem("xmlns:xsi") != null)
            {
                XmlNode Attr = Root.Attributes.GetNamedItem("xmlns:xsi");
                if (Attr != null)
                {
                    string XmlNs = Attr.Value;
                    XmlNamespaceManager NsMngr = new XmlNamespaceManager(Doc.NameTable);
                    NsMngr.AddNamespace("xsi", XmlNs);

                    // get null nodes and remove them
                    XmlNodeList NodeList = Root.SelectNodes("//*[@xsi:nil='true']", NsMngr);
                    if (NodeList != null && NodeList.Count > 0)
                    {
                        foreach (XmlNode Node in NodeList)
                            Node.ParentNode.RemoveChild(Node);
                    }
                }
            }

            // remove root namespaces, if exist
            RemoveAttribute(Root, "xmlns:xsi");
            RemoveAttribute(Root, "xmlns:xsd");

            // get the xml text
            using (StringWriter SW = new StringWriter())
            {
                using (XmlTextWriterEx TW = new XmlTextWriterEx(SW))    // removes node namespaces
                {
                    Doc.Save(TW);
                    XmlText = SW.ToString();
                }
            }

            return XmlText;
        }

        /// <summary>
        /// Serializes a specified instance and returns the XML string.
        /// </summary>
        static public string Serialize<T>(T Instance, bool AsUTF8 = true)
        {
            string XmlText;
            XmlSerializer Serializer = new XmlSerializer(typeof(T));

            using (StringWriter Writer = AsUTF8 ? new StringWriter_UTF8() : new StringWriter())
            {
                Serializer.Serialize(Writer, Instance);
                XmlText = Writer.ToString();
            }

            XmlText = RemoveNilElements(XmlText);
            return XmlText;

        }
        /// <summary>
        /// Deserializes an XML string and returns an instance of a specified type.
        /// </summary>
        static public T Deserialize<T>(string XmlText) where T : class
        {
            T Result;
            XmlSerializer Serializer = new XmlSerializer(typeof(T));
            using (StringReader Reader = new StringReader(XmlText))
            {
                Result = Serializer.Deserialize(Reader) as T;
            }

            return Result;
        }
        /// <summary>
        /// Deserializes an XML string and returns an instance of a specified type.
        /// </summary>
        static public object Deserialize(string XmlText, Type type)
        {
            object Result;
            XmlSerializer Serializer = new XmlSerializer(type);
            using (StringReader Reader = new StringReader(XmlText))
            {
                Result = Serializer.Deserialize(Reader);
            }

            return Result;
        }


        /// <summary>
        /// Saves Instance as xml text to FilePath
        /// </summary>
        static public void SaveToFile(object Instance, string FilePath, string Encoding = "utf-8")
        {
            string Folder = Path.GetDirectoryName(FilePath);
            Directory.CreateDirectory(Folder);
            string XmlText = ToXml(Instance);
            File.WriteAllText(FilePath, XmlText, System.Text.Encoding.GetEncoding(Encoding));
        }
        /// <summary>
        /// Loads Instance from xml FilePath
        /// </summary>
        static public T LoadFromFile<T>(string FilePath, string Encoding = "utf-8") where T : class
        {
            if (File.Exists(FilePath))
            {
                string XmlText = File.ReadAllText(FilePath, System.Text.Encoding.GetEncoding(Encoding));
                //FromXml(Instance, XmlText);
                return Deserialize<T>(XmlText);
            }

            return null;
        }


        /// <summary>
        /// Converts an xml string to an XmlElement.
        /// <para>WARNING: In order to import the returned element to an xml document then you have to use the XmlDocument.ImportNode() method.</para>
        /// </summary>
        static public XmlElement ToElement(string XmlText)
        {
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(XmlText);
            return Doc.DocumentElement;
        }
        /// <summary>
        /// Converts an object to an XmlElement.
        /// <para>WARNING: In order to import the returned element to an xml document then you have to use the XmlDocument.ImportNode() method.</para>
        /// </summary>
        static public XmlElement ToElement(object Instance)
        {
            string XmlText = ToXml(Instance);
            return ToElement(XmlText);
        }
        /// <summary>
        /// Converts an object to an XmlDocument.
        /// </summary>
        static public XmlDocument ToDocument(object Instance)
        {
            string XmlText = ToXml(Instance);
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(XmlText);
            return Doc;
        }

        /// <summary>
        /// Converts Instance to xml representation and returns a MemoryStream stream containing the xml text.
        /// </summary>
        static public MemoryStream ToStream(object Instance)
        {
            XmlDocument Doc = ToDocument(Instance);
            MemoryStream Stream = new MemoryStream();
            Doc.Save(Stream);
            Stream.Position = 0;
            return Stream;
        }
        /// <summary>
        /// Converts Instance to xml representation and loads the result text to the Dest stream.
        /// </summary>
        static public void ToStream(object Instance, Stream Dest)
        {
            if ((Dest != null) && (Instance != null))
            {
                Dest.SetLength(0);

                using (MemoryStream Source = ToStream(Instance))
                {
                    Source.WriteTo(Dest);
                }

                Dest.Position = 0;
            }
        }

        /// <summary>
        /// Reads the content of a stream as a string and returns that string.
        /// <para>NOTE: Uses a StreamReader with encoding detection.</para>
        /// </summary>
        static public string ReadFromStream(Stream Stream)
        {
            if (Stream != null && Stream.Length > 0)
            {
                Stream.Position = 0;
                StreamReader SR = new StreamReader(Stream, true);
                string Result = SR.ReadToEnd();
                return Result;
            }

            return string.Empty;
        }
        /// <summary>
        /// Writes a string into a stream using a specified encoding.
        /// <para>If no encoding is specified, then the Encoding.UTF8 is used.</para>
        /// </summary>
        static public void WriteToStream(string XmlText, Stream Stream, Encoding Encoding = null)
        {
            if (!string.IsNullOrWhiteSpace(XmlText))
            {
                if (Encoding == null)
                    Encoding = Encoding.UTF8;

                byte[] Buffer = Encoding.GetBytes(XmlText);
                Stream.Write(Buffer, 0, Buffer.Length);
            }
        }

    }


    /// <summary>
    /// A StringWriter for xml serializer.
    /// </summary>
    public class StringWriter_UTF8 : StringWriter
    {
        /// <summary>
        /// Encoding
        /// </summary>
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }

}
