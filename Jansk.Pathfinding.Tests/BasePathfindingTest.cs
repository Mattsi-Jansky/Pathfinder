using System;
using Jansk.Pathfinding.Tests.Geography;

namespace Jansk.Pathfinding.Tests
{
    public class BasePathfindingTest
    {
        protected readonly Func<Tile, Tile, float> _heuristic = (from, to) =>
            (Math.Abs(from.x - to.x) + Math.Abs(from.y - to.y) + Math.Abs(from.z - to.z)) * 1.001f;
    }
}