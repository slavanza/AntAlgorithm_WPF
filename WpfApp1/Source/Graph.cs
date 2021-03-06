﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace WpfApp1
{
    class Graph
    {
        
        #region [Variables(Referenses)]
        public List<Point> Way { get; private set; }

        public int Radius { get; } = 10;

        Point selected, lastSelected;

        Point begin, end; // begin = anthill, end = food

        public Point? LastSelected { get { return lastSelected == new Point(-1, -1) ? null : new Point?(lastSelected); } private set { lastSelected = value == null ? new Point(-1, -1) : value.Value; } }
        public Point? Selected { get { return selected == new Point(-1, -1) ? null : new Point?(selected); } private set { selected = value == null ? new Point(-1, -1) : value.Value; } }

        public Point? Begin { get { return begin == new Point(-1, -1) ? null : new Point?(begin); } }
        public Point? End { get { return end == new Point(-1, -1) ? null : new Point?(end); } }

        public Dictionary<Point, Dictionary<Point, double>> G { get; }
        #endregion
        #region [Functions]
        public Graph()
        {
            lastSelected = new Point(-1, -1);
            selected = new Point(-1, -1);
            Way = new List<Point>();
            G = new Dictionary<Point, Dictionary<Point, double>>();
        }

        static public double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
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
                G.Add(p, new Dictionary<Point, double>());
            }
            else
                return false;
            return true;
        }
        public void Remove(Point p)
        {
            var d = Nearest(p);
            if (d == null)
                return;
            else
                p = d.Value;
            foreach (var v in G)
            {
                if (v.Value.ContainsKey(p))
                    v.Value.Remove(p);
            }
            G.Remove(p);
        }
        public bool Connect(double i, Point p1, Point p2)
        {
            if (!G.ContainsKey(p1) && !G.ContainsKey(p2))
                return false;
            if (p1 == p2)
                return false;

            if (!G[p1].ContainsKey(p2))
                G[p1].Add(p2, i);
            if (!G[p2].ContainsKey(p1))
                G[p2].Add(p1, i);

            return true;
        }

        public bool AreConnected(Point p1, Point p2)
        {
            if (!G.ContainsKey(p1) && !G.ContainsKey(p2))
                return false;
            if (p1 == p2)
                return true;
            if (G[p1].ContainsKey(p2) && G[p2].ContainsKey(p1))
                return true;
            return false;
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

        public bool SetBegin(Point p)
        {
            bool result = false;

            Point? point = Nearest(p);

            if(point != null)
            {
                result = true;
                begin = point.Value;
            }

            return result;
        }

        public bool SetEnd(Point p)
        {
            bool result = false;

            Point? point = Nearest(p);

            if (point != null)
            {
                result = true;
                end = point.Value;
            }

            return result;
        }

        public void ResetBegin()
        {
            begin = new Point(-1, -1);
        }

        public void ResetEnd()
        {
            end = new Point(-1, -1);
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

        public List<Point> AStar(Point p1, Point p2) // p1 = начало, p2 = конец
        {
            var closed = new List<Point>();

            var open = new List<Point>();

            var from = new Dictionary<Point, Point>();

            open.Add(p1);
            while (open.Count > 0)
            {
                open = open.OrderBy(_p => Distance(p2, _p)).ToList();
                var p = open.First();
                if (p == p2)
                {
                    Way = MakeWay(from, p2);
                    return Way;
                }
                open.Remove(p);
                closed.Add(p);
                foreach (var v in G[p])
                {
                    if (!closed.Contains(v.Key))
                    {
                        open.Add(v.Key);
                        from[v.Key] = p;
                    }
                }
            }

            

            return new List<Point>();
        }

        private List<Point> MakeWay(Dictionary<Point, Point> from, Point p)
        {
            var way = new List<Point>();
            Point cur = p;
            while(from.ContainsKey(cur))
            {
                way.Add(cur);
                cur = from[cur];
            }
            way.Add(cur);
            return way;
        }

        internal void Clear()
        {
            G.Clear();
            Way.Clear();
        }
        #endregion
    }
}
