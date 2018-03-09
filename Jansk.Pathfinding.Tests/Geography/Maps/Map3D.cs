using System;
using System.Collections.Generic;

namespace Jansk.Pathfinding.Tests.Geography.Maps
{
    public class Map3D
    {
        public int SizeX;
        public int SizeY;
        public int SizeZ;
        public Tile[,,] Tiles;

        public Func<Tile, Tile[]> Neighbours()
        {
            return delegate(Tile tile)
            {
                var neighbours = new List<Tile>();
                if (tile.x - 1 >= 0 && !Tiles[tile.x - 1, tile.y, tile.z].IsBlocking)
                    neighbours.Add(Tiles[tile.x - 1, tile.y, tile.z]);
                if (tile.x + 1 < SizeX && !Tiles[tile.x + 1, tile.y, tile.z].IsBlocking)
                    neighbours.Add(Tiles[tile.x + 1, tile.y, tile.z]);
                if (tile.y + 1 < SizeY && !Tiles[tile.x, tile.y + 1, tile.z].IsBlocking)
                    neighbours.Add(Tiles[tile.x, tile.y + 1, tile.z]);
                if (tile.y - 1 >= 0 && !Tiles[tile.x, tile.y - 1, tile.z].IsBlocking)
                    neighbours.Add(Tiles[tile.x, tile.y - 1, tile.z]);
                if (tile.z - 1 >= 0 && Tiles[tile.x, tile.y, tile.z - 1].IsStairs)
                    neighbours.Add(Tiles[tile.x, tile.y, tile.z - 1]);
                if (tile.z + 1 < SizeZ && Tiles[tile.x, tile.y, tile.z + 1].IsStairs)
                    neighbours.Add(Tiles[tile.x, tile.y, tile.z + 1]);

                return neighbours.ToArray();
            };
        }

        public Func<Tile, int> IndexMap()
        {
            return tile => (tile.x * SizeX * SizeY) + (tile.y * SizeY) + tile.z;
        }

        public Map3D(int sizeX, int sizeY, int sizeZ)
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            this.SizeZ = sizeZ;
            Tiles = new Tile[sizeX, sizeY, sizeZ];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    for (int z = 0; z < sizeZ; z++)
                    {
                        Tiles[x, y, z] = new Tile(x, y, z);
                    }
                }
            }
        }
    }
}
