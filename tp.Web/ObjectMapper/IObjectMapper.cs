namespace tp.Web
{
    /// <summary>
    /// Object mapper.
    /// </summary>
    public interface IObjectMapper
    {
        // ● methods
        /// <summary>
        /// Adds a map between two types, from a source type to a destination type. 
        /// <para>A flag controls whether the mapping is a two-way one.</para>
        /// <para>NOTE: Throws an exception if the <see cref="IsReady"/> is true. </para>
        /// </summary>
        void Add(Type Source, Type Dest, bool TwoWay = false);
 
        /// <summary>
        /// Creates and returns a destination object, based on a specified type argument, and maps a specified source object to destination object.
        /// </summary>
        TDestination Map<TDestination>(object Source) where TDestination : class;
        /// <summary>
        /// Maps a source to a destination object.
        /// </summary>
        TDestination MapTo<TSource, TDestination>(TSource Source, TDestination Dest) where TSource : class where TDestination : class;

        // ● properties 
        /// <summary>
        /// True when the <see cref="Map()" /> and <see cref="MapTo()" /> can be called.
        /// <para>NOTE: Calling <see cref="Add()"/> when the <see cref="IsReady"/> is true throws an exception . </para>
        /// </summary>
        bool IsReady { get; }
    }
}
