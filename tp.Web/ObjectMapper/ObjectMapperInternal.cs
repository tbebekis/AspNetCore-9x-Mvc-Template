namespace tp.Web
{
 
    /// <summary>
    /// Object mapper. Wrapper for the AutoMaper.
    /// </summary>
    static internal class ObjectMapperInternal
    {
        class MapItem
        {
            public MapItem(Type Source, Type Dest, bool TwoWay)
            {
                this.Source = Source;
                this.Dest = Dest;
                this.TwoWay = TwoWay;
            }

            public Type Source { get; }
            public Type Dest { get; }
            public bool TwoWay { get; }
        }

        static List<MapItem> MapList = new List<MapItem>();

        static MapperConfiguration Configuration;
        static Mapper Mapper;

        // ● construction  
        /// <summary>
        /// Static constructor
        /// </summary>
        static ObjectMapperInternal()
        {
        }

        // ● public 
        /// <summary>
        /// Creates the mappings based on the internal map list. 
        /// <para>NOTE: Throws an exception if the <see cref="Configure"/>() method is already called. </para>
        /// </summary>
        static public void Configure()
        {
            if (IsReady)
                Sys.Throw($"{nameof(ObjectMapper)} is already configured.");

            Configuration = new MapperConfiguration(cfg => {

                foreach (var Item in MapList)
                {
                    cfg.CreateMap(Item.Source, Item.Dest);
                    if (Item.TwoWay)
                        cfg.CreateMap(Item.Dest, Item.Source);
                }

                MapList.Clear();
            });


            Mapper = new Mapper(Configuration);
        }

        /// <summary>
        /// Adds a map between two types, from a source type to a destination type. A flag controls whether the mapping is a two-way one.
        /// <para>NOTE: Throws an exception if the <see cref="Configure"/>() method is already called. </para>
        /// </summary>
        static public void Add(Type Source, Type Dest, bool TwoWay = false)
        {
            if (IsReady)
                Sys.Throw($"Can not add map configuration. {nameof(ObjectMapper)} is already configured.");

            MapList.Add(new MapItem(Source, Dest, TwoWay));
        }
 
        /// <summary>
        /// Creates and returns a destination object, based on a specified type argument, and maps a specified source object to destination object.
        /// </summary>
        static public TDestination Map<TDestination>(this object Source) where TDestination : class
        {
            if (!IsReady)
                Sys.Throw($"Can not map objects. {nameof(ObjectMapper)} is not configured.");

            if (Source == null)
                throw new ArgumentNullException(nameof(Source));

            return Mapper.Map<TDestination>(Source);
        }
        /// <summary>
        /// Maps a source to a destination object.
        /// </summary>
        static public TDestination MapTo<TSource, TDestination>(this TSource Source, TDestination Dest) where TSource : class where TDestination : class
        {
            if (!IsReady)
                Sys.Throw($"Can not map objects. {nameof(ObjectMapper)} is not configured.");

            if (Source == null)
                throw new ArgumentNullException(nameof(Source));

            if (Dest == null)
                throw new ArgumentNullException(nameof(Dest));

            return Mapper.Map(Source, Dest);
        }

        // ● properties 
        /// <summary>
        /// True when the <see cref="Map()" /> and <see cref="MapTo()" /> can be called.
        /// <para>NOTE: Calling <see cref="Add()"/> when the <see cref="IsReady"/> is true throws an exception . </para>
        /// </summary>
        static public bool IsReady { get { return Mapper != null; } }

    }
}
