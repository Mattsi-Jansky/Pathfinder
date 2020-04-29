using System;
using System.Collections.Generic;
using Priority_Queue;

namespace Jansk.Pathfinding
{
    public class PathFinder<T>
    {
        private FastPriorityQueue<Node<T>> frontier;
        public Node<T>[] Graph;
        private Func<T, T, int> heuristic;
        private Func<T, IEnumerable<T>> neighbours;
        private Func<T, int> indexMap;
        private int maxNumberOfNodes;

        public PathFinder(Func<T, T, int> heuristic, int maxNumberOfNodes)
        {
            this.heuristic = heuristic;
            this.maxNumberOfNodes = maxNumberOfNodes;
        }

        public T[] Path(T startPosition, T goalPosition, Func<T, int> indexMap, Func<T, IEnumerable<T>> neighbours)
        {
            Node<T> goalNode = null;
            this.indexMap = indexMap;
            this.neighbours = neighbours;

            BuildGraph(startPosition, delegate(Node<T> current)
            {
                if (current.Position.Equals(goalPosition))
                {
                    goalNode = current;
                    return true;
                }
                return false;
            }, position => heuristic(position, goalPosition));

            return GeneratePathFromGraph(startPosition, goalNode);
        }

        public Node<T>[] BuildGraph(T startPosition, Func<Node<T>, bool> goalTest, Func<T, int> heuristic,
            Func<T, IEnumerable<T>> neighbours, Func<T, int> indexMap)
        {
            this.neighbours = neighbours;
            this.indexMap = indexMap;
            BuildGraph(startPosition, goalTest, heuristic);
            return Graph;
        }

        public void BuildGraph(T startPosition, Func<Node<T>, bool> goalTest, Func<T, int> heuristic)
        {
            frontier = new FastPriorityQueue<Node<T>>(150);
            Graph = new Node<T>[maxNumberOfNodes];

            var initial = new Node<T>(startPosition) {Index = indexMap(startPosition) };
            frontier.Enqueue(initial, 0);
            Graph[initial.Index] = initial;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (goalTest(current))
                {
                    break;
                }

                AddNeighbours(current, heuristic);
            }
        }

        private T[] GeneratePathFromGraph(T startPosition, Node<T> goalNode)
        {
            var path = new List<T>();

            if (goalNode != null)
            {
                var node = goalNode;
                while (true)
                {
                    if (node.Position.Equals(startPosition)) break;
                    path.Add(node.Position);
                    node = Graph[node.Previous];
                }
            }

            path.Reverse();
            return path.ToArray();
        }

        private void AddNeighbours(Node<T> node, Func<T,int> heuristic)
        {
            foreach (var neighbour in neighbours(node.Position))
            {
                var newCost = node.Cost + 1;
                var index = indexMap(neighbour);
                if (index >= 0 && index < maxNumberOfNodes)
                {
                    var existingNeighbour = Graph[index];

                    if (existingNeighbour == null || newCost < existingNeighbour.Cost)
                    {
                        var next = new Node<T>(neighbour) {Cost = newCost, Index = index};
                        Graph[next.Index] = next;
                        if (heuristic != null) next.Heuristic = heuristic(neighbour);
                        frontier.Enqueue(next, next.Cost + next.Heuristic);

                        next.Previous = node.Index;
                    }
                }
            }
        }
    }
}
