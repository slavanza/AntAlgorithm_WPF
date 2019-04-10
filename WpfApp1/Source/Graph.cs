using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace WpfApp1
{
    class Graph
    {
        // Variables (Referenses)
        public List<Point> Way { get; private set; }

        public int Radius { get; } = 10;

        Point selected, lastSelected;

        public Point? LastSelected { get { return lastSelected == new Point(-1, -1) ? null : new Point?(lastSelected); } private set { lastSelected = value == null ? new Point(-1, -1) : value.Value; } }
        public Point? Selected { get { return selected == new Point(-1, -1) ? null : new Point?(selected); } private set { selected = value == null ? new Point(-1, -1) : value.Value; } }

        public Dictionary<Point, Dictionary<Point, int>> G { get; }

        // Functions
        public Graph()
        {
            lastSelected = new Point(-1, -1);
            selected = new Point(-1, -1);
            Way = new List<Point>();
            G = new Dictionary<Point, Dictionary<Point, int>>();
        }

        static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(p1.X - p2.X), 2) + Math.Pow(Math.Abs(p1.Y - p2.Y), 2));
        }

        public Point? Nearest(Point p)
        {
            foreach (var v in G)
            {
                if (Distance(v.Key, p) <= Radius * 2)
                    return new Point?(v.Key);
            }
            return null;
        }

        public Point? Select(Point p)
        {
            lastSelected = new Point(selected.X, selected.Y);
            Point? n = Nearest(p);
            selected = n == null ? new Point(-1, -1) : n.Value;
            return selected;
        }
        public bool Add(Point p)
        {
            if (!G.ContainsKey(p))
            {
                G.Add(p, new Dictionary<Point, int>());
            }
            else
                return false;
            return true;
        }
        public void Remove(Point p)
        {
            foreach (var v in G)
            {
                if (v.Value.ContainsKey(p))
                    v.Value.Remove(p);
                if (v.Key == p)
                    G.Remove(p);
            }
        }
        public bool Connect(int i, Point p1, Point p2)
        {
            if (!G.ContainsKey(p1) && !G.ContainsKey(p2))
                return false;
            if (p1 == p2)
                return false;

            foreach (var v in G)
            {
                if (v.Key == p1 && !v.Value.ContainsKey(p2))
                    v.Value.Add(p2, i);
                if (v.Key == p2 && !v.Value.ContainsKey(p1))
                    v.Value.Add(p1, i);
            }

            return true;
        }
        public void Disconnect(Point p1, Point p2)
        {
            foreach (var v in G)
            {
                if (v.Key == p1)
                    v.Value.Remove(p2);
                if (v.Key == p2)
                    v.Value.Remove(p1);
            }
        }


        // TODO: Implement searching
        public List<Point> SearchWay(Point p1, Point p2)
        {
            return new List<Point>()
;
        }
        public List<Point> SearchDistance(Point p1, Point p2)
        {
            return new List<Point>();
        }

        internal void Clear()
        {
            G.Clear();
            Way.Clear();
        }
    }
}
