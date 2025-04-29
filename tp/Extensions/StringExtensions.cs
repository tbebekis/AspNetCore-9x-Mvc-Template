using System.Web;

namespace tp
{
    static public class StringExtensions
    {
        /// <summary>
        /// Case insensitive string equality.
        /// <para>Returns true if 1. both are null, 2. both are empty string or 3. they are the same string </para>
        /// </summary>
        static public bool IsSameText(this string A, string B)
        {
            //return (!string.IsNullOrWhiteSpace(A) && !string.IsNullOrWhiteSpace(B))&& (string.Compare(A, B, StringComparison.InvariantCultureIgnoreCase) == 0);

            // Compare() returns true if 1. both are null, 2. both are empty string or 3. they are the same string
            return string.Compare(A, B, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
        /// <summary>
        /// Returns true if Value is contained in the Instance.
        /// Performs a case-insensitive check using the invariant culture.
        /// </summary>
        static public bool ContainsText(this string Instance, string Value)
        {
            if ((Instance != null) && !string.IsNullOrWhiteSpace(Value))
            {
                return Instance.IndexOf(Value, StringComparison.InvariantCultureIgnoreCase) != -1;
            }

            return false;
        }
 
    }
}
