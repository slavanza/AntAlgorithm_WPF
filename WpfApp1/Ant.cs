using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    class Ant
    {
        // Variables
        readonly Dictionary<Point, Dictionary<Point, int>> graph;
        public Point Current { get; private set; }
        public Point Destination { get; private set; }
        List<Point> visited;
        Queue<Point> way;
        int numPoints;
        int lenWay;
        // Functions
        public Ant(Dictionary<Point, Dictionary<Point, int>> g, Point cur, Point dest)
        {
            numPoints = 0;
            lenWay = 0;

            visited = new List<Point>();
            visited.Add(cur);
            way = new Queue<Point>();
            way.Enqueue(cur);
            graph = g;

            Current = cur;
            Destination = dest;
        }

        public void SetDestination(Point p)
        {
            Destination = p;
        }
        public void SetCurrent(Point p)
        {
            Current = p;
            visited.Add(p);
        }

        public bool Move(Point p)
        {
            lenWay += graph[Current][p];
            numPoints++;
            visited.Add(p);
            way.Enqueue(p);
            if (!visited.Contains(p))
                return true;
            else
                return false;
        }

        public bool MoveToNearest()
        {
            var ways = graph[Current];
            Point p = new Point(-1, -1);
            int min = int.MaxValue;
            foreach (var v in ways)
            {
                if (v.Value < min && !visited.Contains(v.Key))
                {
                    min = v.Value;
                    p = v.Key;
                }
            }
            return Move(p);
        }
    }
}
