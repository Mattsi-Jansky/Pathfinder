using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jansk.Pathfinding.Tests.Geography.Maps
{
    public class Map2D
    {
        private readonly int _sizeX;
        private readonly int _sizeY;
        public readonly Tile[,] Tiles;

        public Func<Tile, Tile[]> Neighbours()
        {
            return delegate (Tile tile)
            {
                var neighbours = new List<Tile>();
                if (tile.x - 1 >= 0 && !Tiles[tile.x - 1, tile.y].IsBlocking)
                    neighbours.Add(Tiles[tile.x - 1, tile.y]);
                if (tile.x + 1 < _sizeX && !Tiles[tile.x + 1, tile.y].IsBlocking)
                    neighbours.Add(Tiles[tile.x + 1, tile.y]);
                if (tile.y + 1 < _sizeY && !Tiles[tile.x, tile.y + 1].IsBlocking)
                    neighbours.Add(Tiles[tile.x, tile.y + 1]);
                if (tile.y - 1 >= 0 && !Tiles[tile.x, tile.y - 1].IsBlocking)
                    neighbours.Add(Tiles[tile.x, tile.y - 1]);

                return neighbours.ToArray();
            };
        }

        public Func<Tile, int> IndexMap()
        {
            return tile => (tile.x * _sizeX) + tile.y;
        }

        public Action<Node<Tile>[]> Debug(Tile goal)
        {
            return (graph) =>
            {
                for (int x = 0; x < _sizeX - 1; x++)
                {
                    var line = new StringBuilder();
                    for (int y = 0; y < _sizeY - 1; y++)
                    {
                        int index = IndexMap()(new Tile(x, y));
                        var node = graph[index]; ;
                        if(node == null) line.Append("----");
                        else if (node.Position.Equals(goal)) line.Append("[GL]");
                        else line.Append($"[{(node.Cost + node.Heuristic):00}]");
                    }
                    Console.WriteLine(line);
                }
                Console.WriteLine();
            };
        }

        public Map2D(int sizeX, int sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
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
