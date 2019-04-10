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
        public List<Point> Way { get; private set; }

        public int Radius { get; } = 10;
        public Point? Selected { get; private set; }

        public Dictionary<Point, Dictionary<Point, int>> G { get; }

        public Graph()
        {
            G = new Dictionary<Point, Dictionary<Point, int>>();
        }

        static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(p1.X - p2.X), 2) + Math.Pow(Math.Abs(p1.Y - p2.Y), 2));
        }

        bool Select(double x, double y)
        {
            Point p = new Point(x, y);
            bool r = G.ContainsKey(p);

            if (r)
            {
                Selected = p;
                return true;
            }
            else
            {
                foreach (var v in G)
                {
                    if (Distance(v.Key, p) <= Radius * 2)
                    {
                        Selected = v.Key;
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
            if (!G.ContainsKey(new Point(x, y)))
                G.Add(new Point(x, y), new Dictionary<Point, int>());
            else
                return false;
            return true;
        }
        public bool Add(Point p)
        {
            return Add(p.X, p.Y);
        }
        void Remove()
        {
            if (Selected != null)
            {
                foreach (var v in G)
                {
                    if (v.Value.ContainsKey(Selected.Value))
                        v.Value.Remove(Selected.Value);
                }
                G.Remove(Selected.Value);
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
            if (Selected == null)
                return false;
            if (Selected.Value.X == x && Selected.Value.Y == y)
                return false;

            Point p = new Point(x, y);
            if (G[Selected.Value].ContainsKey(p) || G[p].ContainsKey(Selected.Value))
                return false;
            G[Selected.Value].Add(p, i);
            G[p].Add(Selected.Value, i);

            return true;
        }
        public bool Connect(int i, Point p)
        {
            return Connect(i, p.X, p.Y);
        }
        void Disconnect(double x, double y)
        {
            if(Selected != null)
            {
                G[Selected.Value].Remove(new Point(x, y));
            }
        }
        public void Disconnect(Point p)
        {
            Disconnect(p.X, p.Y);
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
    }
}
