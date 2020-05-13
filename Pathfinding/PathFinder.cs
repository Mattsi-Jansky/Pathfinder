using System;
using System.Collections.Generic;
using Priority_Queue;

namespace Jansk.Pathfinding
{
    public class PathFinder<T>
    {
        private FastPriorityQueue<Node<T>> _frontier;
        private Node<T>[] _graph;
        private readonly Func<T, T, float> _heuristic;
        private readonly Func<T, IEnumerable<T>> _neighbours;
        private readonly Func<T, int> _indexMap;
        private readonly int _maxNumberOfNodes;
        private readonly Action<Node<T>[]> _debug;

        public PathFinder(Func<T, T, float> heuristic, int maxNumberOfNodes, Func<T, int> indexMap, Func<T, IEnumerable<T>> neighbours, Action<Node<T>[]> debug = null)
        {
            _heuristic = heuristic;
            _maxNumberOfNodes = maxNumberOfNodes;
            _indexMap = indexMap;
            _neighbours = neighbours;
            _debug = debug;
        }

        public T[] Path(T startPosition, T goalPosition)
        {
            var goalNode = BuildGraphAndReturnGoalNode(startPosition, goalPosition);

            return goalNode != null ? GeneratePathFromGraph(startPosition, goalNode) : new T[0];
        }

        public Node<T>[] BuildGraph(T startPosition, T goalPosition)
        {
            BuildGraphAndReturnGoalNode(startPosition, goalPosition);
            return _graph;
        }

        private Node<T> BuildGraphAndReturnGoalNode(T startPosition, T goalPosition)
        {
            _frontier = new FastPriorityQueue<Node<T>>(150);
            _graph = new Node<T>[_maxNumberOfNodes];
            var heuristic = new Func<T,float>(position => _heuristic(position, goalPosition));

            var initial = new Node<T>(startPosition, _indexMap(startPosition));
            _frontier.Enqueue(initial, 0);
            _graph[initial.Index] = initial;

            while (_frontier.Count > 0)
            {
                var current = _frontier.Dequeue();

                if (current.Position.Equals((goalPosition)))
                {
                    return current;
                }

                AddNeighbours(current, heuristic);

                _debug?.Invoke(_graph);
            }

            return null;
        }
        
        private void AddNeighbours(Node<T> node, Func<T,float> heuristic)
        {
            foreach (var neighbour in _neighbours(node.Position))
            {
                var newCost = node.Cost + 1;
                var index = _indexMap(neighbour);
                if (index >= 0 && index < _maxNumberOfNodes)
                {
                    var existingNeighbour = _graph[index];

                    if (existingNeighbour == null || newCost < existingNeighbour.Cost)
                    {
                        var next = new Node<T>(neighbour, index, newCost, heuristic(neighbour), node.Index);
                        _graph[next.Index] = next;
                        _frontier.Enqueue(next, next.Cost + next.Heuristic);
                    }
                }
            }
        }
        
        private T[] GeneratePathFromGraph(T startPosition, Node<T> goalNode)
        {
            var path = new List<T>();

            var node = goalNode;
            while (!node.Position.Equals(startPosition))
            {
                path.Add(node.Position);
                node = _graph[node.Previous];
            }

            path.Reverse();
            return path.ToArray();
        }
    }
}
