using System;
using System.Linq;
using Jansk.Pathfinding.Tests.Geography;
using Jansk.Pathfinding.Tests.Geography.Maps;
using NUnit.Framework;

namespace Jansk.Pathfinding.Tests.Tests
{
    public class HeuristicTests : BasePathfindingTest
    {
        [Test]
        public void GivenNoHeuristic_ShouldBuildBigGraph()
        {
            var map2D = new Map2D(14,14);
            var pathFinder = new PathFinder<Tile>(delegate {return 0;}, 14 * 14, map2D.IndexMap(), map2D.Neighbours());
            var goalPosition = map2D.Tiles[11, 12];

            var graph = pathFinder.BuildGraph(map2D.Tiles[0, 2], goalPosition);

            Assert.AreEqual(192, graph.Count(x => x != null));
        }
        
        [Test]
        public void GivenValidHeuristic_ShouldBuildSmallerGraph()
        {
            var map2D = new Map2D(14,14);
            var pathFinder = new PathFinder<Tile>(_heuristic, 14 * 14, map2D.IndexMap(), map2D.Neighbours());
            var goalPosition = map2D.Tiles[11, 12];
            
            var graph = pathFinder.BuildGraph(map2D.Tiles[0, 2], goalPosition);
            var test = graph.Where(x => x != null).Select(x => x.Heuristic).ToList();

            Assert.AreEqual(155, graph.Count(x => x != null));
        }
    }
}