using System;

namespace MappingTiles
{
    public struct TileRange : IEquatable<TileRange>
    {
        public TileRange(int startColumn, int startRow)
            : this(startColumn, startRow, 1, 1)
        { }

        public TileRange(int startColumn, int startRow, int numberOfColumns, int numberOfRows)
        {
            StartColumn = startColumn;
            StartRow = startRow;
            NumberOfColumns = numberOfColumns;
            NumberOfRows = numberOfRows;
        }

        public int StartColumn
        {
            get;
            private set;
        }

        public int StartRow
        {
            get;
            private set;
        }

        public int NumberOfColumns
        {
            get;
            private set;
        }

        public int NumberOfRows
        {
            get;
            private set;
        }

        public override bool Equals(object other)
        {
            if (!(other is TileRange))
            {
                return false;
            }

            return Equals((TileRange)other);
        }

        public bool Equals(TileRange other)
        {
            return StartColumn == other.StartColumn && NumberOfColumns == other.NumberOfColumns &&
              StartRow == other.StartRow && NumberOfRows == other.NumberOfRows;
        }

        public override int GetHashCode()
        {
            return StartColumn ^ NumberOfColumns ^ StartRow ^ NumberOfRows;
        }

        public static bool operator ==(TileRange tileRange1, TileRange tileRange2)
        {
            return Equals(tileRange1, tileRange2);
        }

        public static bool operator !=(TileRange tileRange1, TileRange tileRange2)
        {
            return !Equals(tileRange1, tileRange2);
        }
    }
}
