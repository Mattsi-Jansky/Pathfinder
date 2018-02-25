using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace Jansk.Pathfinding.Tests
{
    public class PathfindingTests
    {
        private Map map;

        private Func<Tile, Tile, int> heuristic = delegate (Tile from, Tile to)
        {
            return Math.Abs(from.x - to.x) + Math.Abs(from.y + to.y) + Math.Abs(from.z + to.z);
        };

        private Func<Tile, int> indexMap;

        private Func<Tile, Tile[]> neighbours;

        [SetUp]
        public void Setup()
        {
            neighbours = delegate (Tile tile)
            {
                var neighbours = new List<Tile>();
                if (tile.x - 1 >= 0 && !map.tiles[tile.x - 1, tile.y, tile.z].isBlocking)
                    neighbours.Add(map.tiles[tile.x - 1, tile.y, tile.z]);
                if (tile.x + 1 < map.sizeX && !map.tiles[tile.x + 1, tile.y, tile.z].isBlocking)
                    neighbours.Add(map.tiles[tile.x + 1, tile.y, tile.z]);
                if (tile.y + 1 < map.sizeY && !map.tiles[tile.x, tile.y + 1, tile.z].isBlocking)
                    neighbours.Add(map.tiles[tile.x, tile.y + 1, tile.z]);
                if (tile.y - 1 >= 0 && !map.tiles[tile.x, tile.y - 1, tile.z].isBlocking)
                    neighbours.Add(map.tiles[tile.x, tile.y - 1, tile.z]);
                if (tile.z - 1 >= 0 && map.tiles[tile.x, tile.y, tile.z - 1].isStairs)
                    neighbours.Add(map.tiles[tile.x, tile.y, tile.z - 1]);
                if (tile.z + 1 < map.sizeZ && map.tiles[tile.x, tile.y, tile.z + 1].isStairs)
                    neighbours.Add(map.tiles[tile.x, tile.y, tile.z + 1]);

                return neighbours.ToArray();
            };
            indexMap = tile => (tile.x * map.sizeX * map.sizeY) + (tile.y * map.sizeY) + tile.z;
        }

        private class Map
        {
            public int sizeX;
            public int sizeY;
            public int sizeZ;
            public Tile[,,] tiles;

            public Map(int sizeX, int sizeY, int sizeZ)
            {
                this.sizeX = sizeX;
                this.sizeY = sizeY;
                this.sizeZ = sizeZ;
                tiles = new Tile[sizeX,sizeY,sizeZ];

                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        for (int z = 0; z < sizeZ; z++)
                        {
                            tiles[x, y, z] = new Tile(x,y,z);
                        }
                    }
                }
            }
        }

        private class Tile
        {
            public int x;
            public int y;
            public int z;
            public bool isBlocking;
            public bool isStairs;

            public Tile(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                isBlocking = false;
                isStairs = false;
            }

            public override bool Equals(object obj)
            {
                var t = (Tile) obj;
                return t.x == x && t.y == y && t.z == z;
            }
        }

        [Test]
        public void TwoDimensionalTest()
        {
            map = new Map(4,4,1);
            var pathFinder = new PathFinder<Tile>(heuristic, (4 * 4 * 4) + (4 * 4) + 2);
            map.tiles[0, 1,0].isBlocking = true;
            map.tiles[2, 0,0].isBlocking = true;


            Tile[] path = pathFinder.Path(map.tiles[0, 0, 0], map.tiles[3, 3, 0], indexMap, neighbours);

            Assert.AreEqual(6, path.Length);
        }

        [Test]
        public void ThreeDimensionalTest()
        {
            map = new Map(4, 4, 2);
            var pathFinder = new PathFinder<Tile>(heuristic, (4*4*4)+(4*4)+2);
            map.tiles[3, 3, 0].isStairs = true;
            map.tiles[3, 3, 1].isStairs = true;

            Tile[] path = pathFinder.Path(map.tiles[0, 0, 0], map.tiles[0, 0, 1], indexMap, neighbours);

            Assert.AreEqual(13, path.Length);
        }

        [Test]
        public void WhenImpossibleShouldReturnEmptyPath()
        {
            map = new Map(4, 4, 1);
            var pathFinder = new PathFinder<Tile>(heuristic, (4 * 4 * 4) + (4 * 4) + 1);
            map.tiles[0, 1, 0].isBlocking = true;
            map.tiles[1, 0, 0].isBlocking = true;


            Tile[] path = pathFinder.Path(map.tiles[0, 0, 0], map.tiles[3, 3, 0], indexMap, neighbours);

            Assert.AreEqual(0, path.Length);
        }

        [Test]
        public void TwoDimensionalLargeTest()
        {
            map = new Map(20, 20, 1);
            var pathFinder = new PathFinder<Tile>(heuristic, (20 * 20 * 20) + (20 * 20) + 1);

            Tile[] path = pathFinder.Path(map.tiles[0, 0, 0], map.tiles[10, 19, 0], indexMap, neighbours);

            Assert.AreEqual(29, path.Length);
        }

        [Test]
        public void ProfilerTest()
        {
            for (int i = 0; i < 4; i++)
            {
                map = new Map(20, 20, 1);
                var pathFinder = new PathFinder<Tile>(heuristic, (20 * 20 * 20) + (20 * 20) + 1);

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                for (int ii = 0; ii < 500; ii++)
                {
                    pathFinder.Path(map.tiles[0, 0, 0], map.tiles[10, 19, 0], indexMap, neighbours);
                }

                stopWatch.Stop();
                System.Console.WriteLine(stopWatch.ElapsedMilliseconds);
            }
        }
    }
}
