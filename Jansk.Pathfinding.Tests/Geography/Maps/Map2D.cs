using System;
using System.Collections.Generic;

namespace Jansk.Pathfinding.Tests.Geography.Maps
{
    public class Map2D
    {
        public int SizeX;
        public int SizeY;
        public Tile[,] Tiles;

        public Func<Tile, Tile[]> Neighbours()
        {
            return delegate (Tile tile)
            {
                var neighbours = new List<Tile>();
                if (tile.x - 1 >= 0 && !Tiles[tile.x - 1, tile.y].IsBlocking)
                    neighbours.Add(Tiles[tile.x - 1, tile.y]);
                if (tile.x + 1 < SizeX && !Tiles[tile.x + 1, tile.y].IsBlocking)
                    neighbours.Add(Tiles[tile.x + 1, tile.y]);
                if (tile.y + 1 < SizeY && !Tiles[tile.x, tile.y + 1].IsBlocking)
                    neighbours.Add(Tiles[tile.x, tile.y + 1]);
                if (tile.y - 1 >= 0 && !Tiles[tile.x, tile.y - 1].IsBlocking)
                    neighbours.Add(Tiles[tile.x, tile.y - 1]);

                return neighbours.ToArray();
            };
        }

        public Func<Tile, int> IndexMap()
        {
            return tile => (tile.x * SizeX) + tile.y;
        }

        public Map2D(int sizeX, int sizeY)
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            Tiles = new Tile[sizeX, sizeY];

            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                        Tiles[x, y] = new Tile(x, y);
                }
            }
        }
    }
}
