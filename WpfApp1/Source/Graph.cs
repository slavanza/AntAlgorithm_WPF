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
        int radius = 10;

        public int Radius { get { return radius; } }

        Point? selected;
        public Point? Selected { get {return selected;} }

        Dictionary<Point, Dictionary<Point, int>> g;

        public Dictionary<Point, Dictionary<Point, int>> G { get { return g; } }

        public Graph()
        {
            g = new Dictionary<Point, Dictionary<Point, int>>();
        }

        static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(p1.X - p2.X), 2) + Math.Pow(Math.Abs(p1.Y - p2.Y), 2));
        }

        bool Select(double x, double y)
        {
            Point p = new Point(x, y);
            bool r = g.ContainsKey(p);

            if (r)
                return true;
            else
            {
                foreach (var v in g)
                {
                    if (Distance(v.Key, p) <= radius*2)
                    {
                        selected = v.Key;
                        return true;
                    }
                }
            }
            return false;
        }
        public Point? Select(Point p)
        {
            return Select(p.X, p.Y)?new Nullable<Point>(p):null;
        }
        bool Add(double x, double y)
        {
            if (!g.ContainsKey(new Point(x, y)))
                g.Add(new Point(x, y), new Dictionary<Point, int>());
            else
                return false;
            return true;
        }
        public bool Add(Point p)
        {
            bool res = Add(p.X, p.Y);
            Select(p);
            return res;
        }
        void Remove()
        {
            if (selected != null)
            {
                foreach (var v in g)
                {
                    if (v.Value.ContainsKey(selected.Value))
                        v.Value.Remove(selected.Value);
                }
                g.Remove(selected.Value);
            }
        }
        void Remove(double x, double y)
        {
            Select(x, y);
            Remove();
        }
        public void Remove(Point p)
        {
            Remove(p.X, p.Y);
        }
        bool Connect(int i, double x, double y)
        {
            if (selected == null)
                return false;
            if (selected.Value.X == x && selected.Value.Y == y)
                return false;

            Point p = new Point(x, y);
            if (g[selected.Value].ContainsKey(p) && g[p].ContainsKey(selected.Value))
                return false;
            g[selected.Value].Add(p, i);
            g[p].Add(selected.Value, i);

            return true;
        }
        public bool Connect(int i, Point p)
        {
            return Connect(i, p.X, p.Y);
        }
        void Disconnect(double x, double y)
        {
            if(selected != null)
            {
                g[selected.Value].Remove(new Point(x, y));
            }
        }
        public void Disconnect(Point p)
        {
            Disconnect(p.X, p.Y);
        }
    }
}
