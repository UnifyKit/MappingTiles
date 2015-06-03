
namespace MappingTiles
{
    public interface ITileCache<T>
    {
        /// <summary>
        /// Adds a tile that corresponds to the index
        /// </summary>
        /// <param name="tileInfo">The information of the tile to add. If the tile already exists no exepection is thrown.</param>
        /// <param name="tile">The tile data</param>
        void Add(TileInfo tileInfo, T tile);

        /// <summary>
        /// Removes the tile that corresponds with the index passed as argument. When the tile is not found no exception is thrown.
        /// </summary>
        /// <param name="tileInfo">The information of the tile to be removed.</param>
        void Remove(TileInfo tileInfo);

        /// <summary>
        /// Tries to find a tile that corresponds with the index. Returns null if not found.
        /// </summary>
        /// <param name="index">The information of the tile to find</param>
        /// <returns>The tile data that corresponds with the id or null.</returns>
        T Get(TileInfo tileInfo);
    }
}
