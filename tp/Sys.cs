[assembly: CLSCompliant(true)]

namespace tp
{
    /// <summary>
    /// Helper
    /// </summary>
    static public class Sys
    {
        /// <summary>
        /// Used when calling async methods as synchronous
        /// </summary>
        static public readonly TaskFactory TaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        // ● construction 
        /// <summary>
        /// Constructor
        /// </summary>
        static Sys()
        {
        }

        // ● Exceptions
        /// <summary>
        /// Throws an Exception
        /// </summary>
        static public void Throw(string Text)
        {
            if (string.IsNullOrWhiteSpace(Text))
                Text = "Unknown error";

            throw new TpException(Text);
        }
        /// <summary>
        /// Throws an Exception
        /// </summary>
        static public void Throw(string Text, params object[] Args)
        {
            if ((Args != null) && (Args.Length > 0))
                Text = string.Format(Text, Args);
            throw new TpException(Text);
        }
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

        // ● strings 
        /// <summary>
        /// Case insensitive string equality.
        /// </summary>
        public static bool IsSameText(string A, string B)
        {
            return A.IsSameText(B);
        }

        /* to/from Base64 */
        /// <summary>
        /// Encodes Value into a Base64 string using the specified Enc.
        /// If End is null, the Encoding.Unicode is used.
        /// </summary>
        static public string StringToBase64(string Value, Encoding Enc)
        {
            if (Enc == null)
                Enc = Encoding.Unicode;

            byte[] Data = Enc.GetBytes(Value);
            return Convert.ToBase64String(Data);
        }
        /// <summary>
        /// Decodes the Base64 string Value into a string using the specified Enc.
        /// If End is null, the Encoding.Unicode is used.
        /// </summary>
        static public string Base64ToString(string Value, Encoding Enc)
        {
            if (Enc == null)
                Enc = Encoding.Unicode;

            byte[] Data = Convert.FromBase64String(Value);
            return Enc.GetString(Data);
        }
    }
}
