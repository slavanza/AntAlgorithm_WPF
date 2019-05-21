using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum state { Vertex, Edge, Way, Distance, Begin, End };

        Graph graph;

        public MainWindow()
        {
            graph = new Graph();

            InitializeComponent();
        }

        state CheckState()
        {
            state res = state.Vertex;
            if (Edge.IsChecked.Value)
                res = state.Edge;
            else
            if (SearchWay.IsChecked.Value)
                res = state.Way;
            else
            if (SearchDistance.IsChecked.Value)
                res = state.Distance;
            else
            if (Begin.IsChecked.Value)
                res = state.Begin;
            else
            if (End.IsChecked.Value)
                res = state.End;
            return res;
        }

        void OnMouseDown(object sender, MouseButtonEventArgs args)
        {

            state s = CheckState();

            Point p = args.GetPosition(sender as IInputElement);
            

            if (args.LeftButton == MouseButtonState.Pressed)
            {
                switch (s)
                {
                    case state.Vertex:
                        if (graph.Nearest(p) == null)
                        {
                            graph.Add(p);
                        }
                        else
                            graph.Select(p);
                        break;
                    case state.Edge:
                        graph.Select(p);
                        if (graph.LastSelected != null && graph.Selected != null)
                        {
                            if (!graph.AreConnected(graph.LastSelected.Value, graph.Selected.Value))
                            {
                                CostDialog dialog = new CostDialog();
                                if (dialog.ShowDialog() == true)
                                {
                                    int r = dialog.Cost;
                                    graph.Connect(r, graph.LastSelected.Value, graph.Selected.Value);
                                    Redraw(sender, args);
                                }
                            }
                        }
                        break;
                    case state.Way:
                        graph.SearchWay(new Point(), new Point()); // Implement
                        break;
                    case state.Distance:
                        graph.SearchDistance(new Point(), new Point()); // Implement
                        break;
                    case state.Begin:
                        graph.Select(p);
                        graph.SetBegin(p);
                        break;
                    case state.End:
                        graph.Select(p);
                        graph.SetEnd(p);
                        break;
                }
            }
            else if (args.RightButton == MouseButtonState.Pressed)
            {
                switch (s)
                {
                    case state.Vertex:
                        graph.Select(p);
                        if (graph.Selected != null)
                        {
                            graph.Remove(p);
                        }
                        break;
                    case state.Edge:
                        graph.Select(p);
                        if (graph.LastSelected != null && graph.Selected != null)
                        {
                            graph.Disconnect(graph.LastSelected.Value, graph.Selected.Value);
                        }
                        break;
                    case state.Begin:
                        graph.ResetBegin();
                        break;
                    case state.End:
                        graph.ResetEnd();
                        break;
                }
            }
        }


        void Redraw(object sender, MouseButtonEventArgs e)
        {
            var list = graph.G;
            DrawingField.Children.Clear();

            var lineSet = new Dictionary<Point, List<Point>>();

            //Lines
            foreach (var v1 in list)
            {
                foreach (var v2 in v1.Value)
                {
                    if (!(lineSet.ContainsKey(v1.Key) && lineSet[v1.Key].Contains(v2.Key)) && !(lineSet.ContainsKey(v2.Key) && lineSet[v2.Key].Contains(v1.Key)))
                    {
                        if(graph.Way.Contains(v1.Key) && graph.Way.Contains(v2.Key))
                        {
                            int n = graph.Way.FindIndex(x => { return x == v1.Key; });
                            if(graph.Way.ElementAt(n-1) == v2.Key || graph.Way.ElementAt(n+1) == v2.Key)
                                DrawingField.Children.Add(new Line()
                                {
                                    X1 = v1.Key.X,
                                    X2 = v2.Key.X,
                                    Y1 = v1.Key.Y,
                                    Y2 = v2.Key.Y,
                                    StrokeStartLineCap = PenLineCap.Round,
                                    StrokeEndLineCap = PenLineCap.Round,
                                    StrokeThickness = 1,
                                    Stroke = Brushes.Green
                                });
                        }
                        else
                        DrawingField.Children.Add(new Line()
                        {
                            X1 = v1.Key.X,
                            X2 = v2.Key.X,
                            Y1 = v1.Key.Y,
                            Y2 = v2.Key.Y,
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round,
                            StrokeThickness = 1,
                            Stroke = Brushes.Black
                        });

                        DrawingField.Children.Add(new Label()
                        {
                            Content = v2.Value.ToString(),
                            Margin = new Thickness((v1.Key.X + v2.Key.X) / 2 + 10, (v1.Key.Y + v2.Key.Y) / 2 + 10, 0, 0),
                            Foreground = Brushes.DarkOrange
                            
                        });

                        if (lineSet.ContainsKey(v1.Key))
                            lineSet[v1.Key].Add(v2.Key);
                        else
                        {
                            lineSet.Add(v1.Key, new List<Point>());
                            lineSet[v1.Key].Add(v2.Key);
                        }
                    }

                }
            }
            // Circles
            foreach (var v in list)
            {
                bool isSelected = false, isBegin = false, isEnd = false;
                if (v.Key == graph.Selected) // red bounds
                    isSelected = true;
                if (v.Key == graph.Begin) // green filling
                    isBegin = true;
                if (v.Key == graph.End) // blue filling
                    isEnd = true;

                Brush border = Brushes.Black, fill = null;

                if (isSelected)
                    border = Brushes.Red;
                if (isBegin)
                    fill = Brushes.Lime;
                else if (isEnd)
                    fill = Brushes.Cyan;
                if (isBegin && isEnd)
                    fill = Brushes.Yellow;

                DrawingField.Children.Add(new Ellipse()
                {
                    Width = graph.Radius * 2,
                    Height = graph.Radius * 2,
                    Margin = new Thickness(v.Key.X - graph.Radius, v.Key.Y - graph.Radius, 0, 0),
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeThickness = 1,
                    Stroke = border,
                    Fill = fill
                });
            }

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            graph.Clear();
            DrawingField.Children.Clear();
        }
    }
}
