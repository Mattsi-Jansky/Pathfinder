using System;
using System.Diagnostics;
using Jansk.Pathfinding.Tests.Geography;
using Jansk.Pathfinding.Tests.Geography.Maps;
using NUnit.Framework;

namespace Jansk.Pathfinding.Tests.Tests
{
    public class PathfindingTests : BasePathfindingTest
    {
        private Map3D _map3D;
        private Map2D _map2D;

        [Test]
        public void TwoDimensionalTest()
        {
            _map2D = new Map2D(4,4);
            var pathFinder = new PathFinder<Tile>(_heuristic, 4 * 4, _map2D.IndexMap(), _map2D.Neighbours());
            _map2D.Tiles[0, 1].IsBlocking = true;
            _map2D.Tiles[2, 0].IsBlocking = true;

            var path = pathFinder.Path(_map2D.Tiles[0, 0], _map2D.Tiles[3, 3]);

            Assert.AreEqual(6, path.Length);
        }

        [Test]
        public void ThreeDimensionalTest()
        {
            _map3D = new Map3D(4, 4, 2);
            var pathFinder = new PathFinder<Tile>(_heuristic, 4*4*2, _map3D.IndexMap(), _map3D.Neighbours());
            _map3D.Tiles[3, 3, 0].IsStairs = true;
            _map3D.Tiles[3, 3, 1].IsStairs = true;

            var path = pathFinder.Path(_map3D.Tiles[0, 0, 0], _map3D.Tiles[0, 0, 1]);

            Assert.AreEqual(13, path.Length);
        }

        [Test]
        public void WhenImpossibleShouldReturnEmptyPath()
        {
            _map3D = new Map3D(4, 4, 1);
            var pathFinder = new PathFinder<Tile>(_heuristic, 4 * 4, _map3D.IndexMap(), _map3D.Neighbours());
            _map3D.Tiles[0, 1, 0].IsBlocking = true;
            _map3D.Tiles[1, 0, 0].IsBlocking = true;


            var path = pathFinder.Path(_map3D.Tiles[0, 0, 0], _map3D.Tiles[3, 3, 0]);

            Assert.AreEqual(0, path.Length);
        }

        [Test]
        public void TwoDimensionalLargeTest()
        {
            _map2D = new Map2D(20, 20);
            var pathFinder = new PathFinder<Tile>(_heuristic, 20 * 20, _map2D.IndexMap(), _map2D.Neighbours());

            var path = pathFinder.Path(_map2D.Tiles[0, 0], _map2D.Tiles[19, 19]);

            Assert.AreEqual(38, path.Length);
        }

        [Test]
        public void ProfilerTest()
        {
            for (var i = 0; i < 4; i++)
            {
                _map3D = new Map3D(20, 20, 1);
                var pathFinder = new PathFinder<Tile>(_heuristic, 20 * 20, _map3D.IndexMap(), _map3D.Neighbours());

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                for (var ii = 0; ii < 500; ii++)
                {
                    pathFinder.Path(_map3D.Tiles[0, 0, 0], _map3D.Tiles[10, 19, 0]);
                }

                stopWatch.Stop();
                Console.WriteLine(stopWatch.ElapsedMilliseconds);
            }
        }
    }
}
