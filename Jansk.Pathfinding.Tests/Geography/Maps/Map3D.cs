using System;
using System.Collections.Generic;

namespace Jansk.Pathfinding.Tests.Geography.Maps
{
    public class Map3D
    {
        private readonly int _sizeX;
        private readonly int _sizeY;
        private readonly int _sizeZ;
        public readonly Tile[,,] Tiles;

        public Func<Tile, Tile[]> Neighbours()
        {
            return delegate(Tile tile)
            {
                var neighbours = new List<Tile>();
                if (tile.x - 1 >= 0 && !Tiles[tile.x - 1, tile.y, tile.z].IsBlocking)
                    neighbours.Add(Tiles[tile.x - 1, tile.y, tile.z]);
                if (tile.x + 1 < _sizeX && !Tiles[tile.x + 1, tile.y, tile.z].IsBlocking)
                    neighbours.Add(Tiles[tile.x + 1, tile.y, tile.z]);
                if (tile.y + 1 < _sizeY && !Tiles[tile.x, tile.y + 1, tile.z].IsBlocking)
                    neighbours.Add(Tiles[tile.x, tile.y + 1, tile.z]);
                if (tile.y - 1 >= 0 && !Tiles[tile.x, tile.y - 1, tile.z].IsBlocking)
                    neighbours.Add(Tiles[tile.x, tile.y - 1, tile.z]);
                if (tile.z - 1 >= 0 && Tiles[tile.x, tile.y, tile.z - 1].IsStairs)
                    neighbours.Add(Tiles[tile.x, tile.y, tile.z - 1]);
                if (tile.z + 1 < _sizeZ && Tiles[tile.x, tile.y, tile.z + 1].IsStairs)
                    neighbours.Add(Tiles[tile.x, tile.y, tile.z + 1]);

                return neighbours.ToArray();
            };
        }

        public Func<Tile, int> IndexMap()
        {
            return tile => (tile.z * _sizeX * _sizeY) + (tile.y * _sizeY) + tile.x;
        }

        public Map3D(int sizeX, int sizeY, int sizeZ)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            _sizeZ = sizeZ;
            Tiles = new Tile[sizeX, sizeY, sizeZ];

            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    for (var z = 0; z < sizeZ; z++)
                    {
                        Tiles[x, y, z] = new Tile(x, y, z);
                    }
                }
            }
        }
    }
}
