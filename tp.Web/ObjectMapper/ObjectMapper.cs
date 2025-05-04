namespace tp.Web
{
    /// <summary>
    /// Object mapper.
    /// </summary>
    internal class ObjectMapper : IObjectMapper
    {
        // ● methods
        /// <summary>
        /// Adds a map between two types, from a source type to a destination type. 
        /// <para>A flag controls whether the mapping is a two-way one.</para>
        /// <para>NOTE: Throws an exception if the <see cref="IsReady"/> is true. </para>
        /// </summary>
        public void Add(Type Source, Type Dest, bool TwoWay = false)
        {
            ObjectMapperInternal.Add(Source, Dest, TwoWay);
        }

        /// <summary>
        /// Creates and returns a destination object, based on a specified type argument, and maps a specified source object to destination object.
        /// </summary>
        public TDestination Map<TDestination>(object Source) where TDestination : class
        {
            return ObjectMapperInternal.Map<TDestination>(Source);
        }
        /// <summary>
        /// Maps a source to a destination object.
        /// </summary>
        public TDestination MapTo<TSource, TDestination>(TSource Source, TDestination Dest)  where TSource : class where TDestination : class
        {
            return ObjectMapperInternal.MapTo<TSource, TDestination>(Source, Dest);
        }

        // ● properties 
        /// <summary>
        /// True when the <see cref="Map()" /> and <see cref="MapTo()" /> can be called.
        /// <para>NOTE: Calling <see cref="Add()"/> when the <see cref="IsReady"/> is true throws an exception . </para>
        /// </summary>
        public bool IsReady => ObjectMapperInternal.IsReady;
    }
}
