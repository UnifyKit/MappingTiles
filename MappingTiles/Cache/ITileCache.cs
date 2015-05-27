
namespace MappingTiles
{
    public interface ITileCache<T>
    {
        /// <summary>
        /// Adds a tile that corresponds to the index
        /// </summary>
        /// <param name="id">The id of the tile to add. If the tile already exists no exepection is thrown.</param>
        /// <param name="tile">The tile data</param>
        void Add(string id, T tile);

        /// <summary>
        /// Removes the tile that corresponds with the index passed as argument. When the tile is not found no exception is thrown.
        /// </summary>
        /// <param name="id">The index of the tile to be removed.</param>
        void Remove(string id);

        /// <summary>
        /// Tries to find a tile that corresponds with the index. Returns null if not found.
        /// </summary>
        /// <param name="index">The id of the tile to find</param>
        /// <returns>The tile data that corresponds with the id or null.</returns>
        T Get(string id);
    }
}
