namespace tp.Web
{

    /// <summary>
    /// Represents an application memory cache.
    /// </summary>
    public interface IWebAppCache
    {
        // ● public
        /// <summary>
        /// Returns true if the key exists.
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        bool ContainsKey(string Key);
        /// <summary>
        /// Removes an entry by a specified key.
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        void Remove(string Key);

        /// <summary>
        /// Sets an entry under a specified key. Creates the entry if not already exists.
        /// <para>If is a new entry it will be removed from the cache after <see cref="DefaultEvictionTimeoutMinutes"/> minutes. </para>
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        void Set<T>(string Key, T Value);
        /// <summary>
        /// Sets an entry under a specified key. Creates the entry if not already exists.
        /// <para>If is a new entry it will be removed from the cache after the specified timeout minutes. </para>
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        void Set<T>(string Key, T Value, int TimeoutMinutes);

        /// <summary>
        /// Returns true if an entry exists under a specified key. Returns the value too as out parameter.
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        bool TryGetValue(string Key, out object Value);
        /// <summary>
        /// Returns true if an entry exists under a specified key. Returns the value too as out parameter.
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        bool TryGetValue<T>(string Key, out T Value);

        /// <summary>
        /// Returns a value found under a specified key, if any, else returns the default value of the specified type argument.
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        T Get<T>(string Key);
        /// <summary>
        /// Returns a value found under a specified key, if any, else returns null.
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        object Get(string Key);


        /// <summary>
        /// Removes and returns a value found under a specified key, if any, else returns the default value of the specified type argument.
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        T Pop<T>(string Key);
        /// <summary>
        /// Removes and returns a value found under a specified key, if any, else returns null.
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        object Pop(string Key);


        /// <summary>
        /// Returns a value found under a specified key.
        /// <para>If the key does not exist, it calls the specified loader call-back function </para>
        /// <para>The loader function should be defined as <c>object LoaderFunc()</c></para>
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        object Get(string Key, Func<object> LoaderFunc);
        /// <summary>
        /// Returns a value found under a specified key.
        /// <para>If the key does not exist, it calls the specified loader call-back function </para>
        /// <para>The loader function should be defined as <c>(int, object) LoaderFunc().</c></para>
        /// <para>The loader function must return a tuple where the first value is the eviction timeout and the second is the result object.</para>
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        object Get(string Key, Func<(int, object)> LoaderFunc);
        /// <summary>
        /// Returns a value found under a specified key.
        /// <para>If the key does not exist, it calls the specified loader call-back function </para>
        /// <para>The loader function should be defined as <c>T LoaderFunc&lt;T&gt;()</c></para>
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        T Get<T>(string Key, Func<T> LoaderFunc);
        /// <summary>
        /// Returns a value found under a specified key.
        /// <para>If the key does not exist, it calls the specified loader call-back function </para>
        /// <para>The loader function should be defined as <c>(int, T) LoaderFunc&lt;T&gt;().</c></para>
        /// <para>The loader function must return a tuple where the first value is the eviction timeout and the second is the result object.</para>
        /// <para>NOTE: Key is case sensitive.</para>
        /// </summary>
        T Get<T>(string Key, Func<(int, T)> LoaderFunc);

        // ● properties
        /// <summary>
        /// The default eviction timeout of an entry from the cache, in minutes. Defaults to 0 which means "use the timeouts of the internal implementation".
        /// </summary>
        int DefaultEvictionTimeoutMinutes { get; set; }
    }

}
