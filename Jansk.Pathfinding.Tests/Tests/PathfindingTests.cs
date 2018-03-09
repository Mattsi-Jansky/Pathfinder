using System;
using System.Collections.Generic;
using System.Diagnostics;
using Jansk.Pathfinding.Tests.Geography;
using Jansk.Pathfinding.Tests.Geography.Maps;
using NUnit.Framework;

namespace Jansk.Pathfinding.Tests.Tests
{
    public class PathfindingTests
    {
        private Map3D _map3D;
        private Map2D _map2D;

        private Func<Tile, Tile, int> heuristic = delegate (Tile from, Tile to)
        {
            return Math.Abs(from.x - to.x) + Math.Abs(from.y + to.y) + Math.Abs(from.z + to.z);
        };

        [Test]
        public void TwoDimensionalTest()
        {
            _map2D = new Map2D(4,4);
            var pathFinder = new PathFinder<Tile>(heuristic, (4 * 4 * 4) + (4 * 4) + 2);
            _map2D.Tiles[0, 1].IsBlocking = true;
            _map2D.Tiles[2, 0].IsBlocking = true;

            Tile[] path = pathFinder.Path(_map2D.Tiles[0, 0], _map2D.Tiles[3, 3], _map2D.IndexMap(), _map2D.Neighbours());

            Assert.AreEqual(6, path.Length);
        }

        [Test]
        public void ThreeDimensionalTest()
        {
            _map3D = new Map3D(4, 4, 2);
            var pathFinder = new PathFinder<Tile>(heuristic, (4*4*4)+(4*4)+2);
            _map3D.Tiles[3, 3, 0].IsStairs = true;
            _map3D.Tiles[3, 3, 1].IsStairs = true;

            Tile[] path = pathFinder.Path(_map3D.Tiles[0, 0, 0], _map3D.Tiles[0, 0, 1], _map3D.IndexMap(), _map3D.Neighbours());

            Assert.AreEqual(13, path.Length);
        }

        [Test]
        public void WhenImpossibleShouldReturnEmptyPath()
        {
            _map3D = new Map3D(4, 4, 1);
            var pathFinder = new PathFinder<Tile>(heuristic, (4 * 4 * 4) + (4 * 4) + 1);
            _map3D.Tiles[0, 1, 0].IsBlocking = true;
            _map3D.Tiles[1, 0, 0].IsBlocking = true;


            Tile[] path = pathFinder.Path(_map3D.Tiles[0, 0, 0], _map3D.Tiles[3, 3, 0], _map3D.IndexMap(), _map3D.Neighbours());

            Assert.AreEqual(0, path.Length);
        }

        [Test]
        public void TwoDimensionalLargeTest()
        {
            _map3D = new Map3D(20, 20, 1);
            var pathFinder = new PathFinder<Tile>(heuristic, (20 * 20 * 20) + (20 * 20) + 1);

            Tile[] path = pathFinder.Path(_map3D.Tiles[0, 0, 0], _map3D.Tiles[10, 19, 0], _map3D.IndexMap(), _map3D.Neighbours());

            Assert.AreEqual(29, path.Length);
        }

        [Test]
        public void ProfilerTest()
        {
            for (int i = 0; i < 4; i++)
            {
                _map3D = new Map3D(20, 20, 1);
                var pathFinder = new PathFinder<Tile>(heuristic, (20 * 20 * 20) + (20 * 20) + 1);

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                for (int ii = 0; ii < 500; ii++)
                {
                    pathFinder.Path(_map3D.Tiles[0, 0, 0], _map3D.Tiles[10, 19, 0], _map3D.IndexMap(), _map3D.Neighbours());
                }

                stopWatch.Stop();
                System.Console.WriteLine(stopWatch.ElapsedMilliseconds);
            }
        }
    }
}
