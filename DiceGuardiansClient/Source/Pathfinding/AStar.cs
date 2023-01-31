using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DiceGuardiansClient.Source.Pathfinding;

public static class AStar {
    private static int[,] _grid;
    private static Vector2 _start;
    private static Vector2 _end;
    private static bool[,] _visited;

    public static List<Vector2> FindPath(int[,] grid, Vector2 start, Vector2 end) {
        _grid = grid;
        _start = start;
        _end = end;

        return FindPath();
    }

    private static List<Vector2> FindPath() {
        Node s = new Node(_start, 0, GetManhattanDistance(_start, _end), null);
        
        _visited = new bool[_grid.GetLength(0), _grid.GetLength(1)];
        PriorityQueue<Node, int> priorityQueue = new PriorityQueue<Node, int>();
        
        _visited[(int)_start.X, (int)_start.X] = true;
        priorityQueue.Enqueue(s, s.fCost);

        while (priorityQueue.Count > 0) {
            Node currentNode = priorityQueue.Dequeue();

            if (currentNode.position == _end) {
                return RetracePath(currentNode);
            }

            List<Vector2> neighbours = GetNeighbours(currentNode.position);
            foreach (Vector2 neighbour in neighbours) {
                if (_visited[(int)neighbour.X, (int)neighbour.Y]) {
                    continue;
                }
                
                int gCost = currentNode.gCost + 1;
                int hCost = GetManhattanDistance(neighbour, _end);

                Node newNode = new Node(neighbour, gCost, hCost, currentNode);
                priorityQueue.Enqueue(newNode, newNode.fCost);
                _visited[(int)neighbour.X, (int)neighbour.Y] = true;
            }
        }

        return null;
    }

    private static int GetManhattanDistance(Vector2 a, Vector2 b) {
        int xDistance = Math.Abs((int)a.X - (int)b.X);
        int yDistance = Math.Abs((int)a.Y - (int)b.Y);

        return xDistance + yDistance;
    }
    
    private static List<Vector2> GetNeighbours(Vector2 position) {
        List<Vector2> neighbours = new List<Vector2>();
        int x = (int) position.X;
        int y = (int) position.Y;

        if (x + 1 < _grid.GetLength(0) && _grid[x + 1, y] == 0) {
            neighbours.Add(new Vector2(x + 1, y));
        }
        if (x - 1 >= 0 && _grid[x - 1, y] == 0) {
            neighbours.Add(new Vector2(x - 1, y));
        }
        if (y + 1 < _grid.GetLength(1) && _grid[x, y + 1] == 0) {
            neighbours.Add(new Vector2(x, y + 1));
        }
        if (y - 1 >= 0 && _grid[x, y - 1] == 0) {
            neighbours.Add(new Vector2(x, y - 1));
        }

        return neighbours;
    }

    private static List<Vector2> RetracePath(Node node) {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = node;

        while (currentNode != null) {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}