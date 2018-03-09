using Priority_Queue;

namespace Jansk.Pathfinding
{
    public class Node<T> : FastPriorityQueueNode
    {
        public T Position;
        public int Previous;
        public int Cost;
        public int Heuristic;
        public int Index;

        public Node(T position)
        {
            Position = position;
        }
    }
}
