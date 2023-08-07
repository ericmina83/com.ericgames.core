using UnityEngine;
using System.Collections.Generic;

namespace EricGames.Runtime.Triangulations
{
    internal class Point
    {
        public List<Edge> edges = new List<Edge>();

        private Vector2 position;
        public Vector2 Position => position;

        public Point(Vector2 position)
        {
            this.position = position;
        }

        public Edge FindEdge(Point target)
        {
            if (target == this)
                return null;

            foreach (var edge in edges)
            {
                if (edge.IsPointOfEdge(target))
                    return edge;
            }

            return null;
        }
    }
}