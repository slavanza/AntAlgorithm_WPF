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
        Ant(Dictionary<Point, Dictionary<Point, int>> g, Point c, Point d)
        {
            numPoints = 0;
            lenWay = 0;

            visited = new List<Point>();
            visited.Add(c);
            way = new Queue<Point>();
            way.Enqueue(c);
            graph = g;

            Current = c;
            Destination = d;
        }
        Ant(Dictionary<Point, Dictionary<Point, int>> g) : this(g, g.First().Key, g.Last().Key)
        {
        }

        public void Configure(Point cur, Point dest)
        {
            Current = cur;
            Destination = dest;
        }

        void Move(Point p)
        {
            lenWay += graph[Current][p];
            numPoints++;
            visited.Add(p);
            way.Enqueue(p);
        }
    }
}
