namespace Jansk.Pathfinding.Tests.Geography
{
    public struct Tile
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
            this.z = 0;
            IsBlocking = false;
            IsStairs = false;
        }

        public override bool Equals(object obj)
        {
            var t = (Tile)obj;
            return t.x == x && t.y == y && t.z == z;
        }

        public Tile Translate(int x, int y)
        {
            return new Tile(this.x + x, this.y + y);
        }
    }
}
