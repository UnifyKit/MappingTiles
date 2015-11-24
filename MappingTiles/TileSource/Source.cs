
namespace MappingTiles
{
    public abstract class Source
    {
        protected Source()
        {
        }

        protected Source(string id)
        {
            Id = id;
        }

        public string Id
        {
            get;
            protected set;
        }
    }
}
