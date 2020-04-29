using Priority_Queue;

namespace Jansk.Pathfinding
{
    public class Node<T> : FastPriorityQueueNode
    {
        public readonly T Position;
        public readonly int Previous;
        public readonly int Cost;
        public readonly int Heuristic;
        public readonly int Index;

        public Node(T position, int index, int cost = 0, int heuristic = 0, int previous = 0)
        {
            Position = position;
            Index = index;
            Heuristic = heuristic;
            Cost = cost;
            Previous = previous;
        }
    }
}
