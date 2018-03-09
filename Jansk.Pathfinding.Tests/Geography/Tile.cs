using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jansk.Pathfinding.Tests.Geography
{
    public class Tile
    {
        public int x;
        public int y;
        public int z;
        public bool IsBlocking;
        public bool IsStairs;

        public Tile(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            IsBlocking = false;
            IsStairs = false;
        }

        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            var t = (Tile)obj;
            return t.x == x && t.y == y && t.z == z;
        }
    }
}
