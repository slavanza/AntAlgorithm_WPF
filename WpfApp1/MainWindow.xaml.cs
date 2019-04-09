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
        enum state { Vertex, Edge, Way, Distance };

        Point? selected1, selected2;

        Graph graph;

        public MainWindow()
        {
            graph = new Graph();

            InitializeComponent();

            Label.Visibility = Visibility.Hidden;
            Cost.Visibility = Visibility.Hidden;
        }

        state CheckState()
        {
            state res = state.Vertex;
            if (Vertex.IsChecked.Value)
                res = state.Vertex;
            if (Edge.IsChecked.Value)
                res = state.Edge;
            if (SearchWay.IsChecked.Value)
                res = state.Way;
            if (SearchDistance.IsChecked.Value)
                res = state.Distance;
            return res;
        }

        void OnMouseDown(object sender, MouseButtonEventArgs args)
        {

            state s = CheckState();

            Point p = args.GetPosition((IInputElement)sender);

            selected1 = graph.Selected;
            selected2 = graph.Select(p);

            if (args.LeftButton == MouseButtonState.Pressed)
            {

                switch (s)
                {
                    case state.Vertex:
                        if (selected2 == null)
                        {
                            graph.Add(p);
                        }
                        break;
                    case state.Edge:
                        if (selected1 != null && selected2 != null)
                        {
                            int r;
                            bool b = int.TryParse(Cost.Text, out r);
                            if (!b)
                                r = 1;
                            graph.Connect(r, selected1.Value);
                        }
                        break;
                    case state.Way:
                        break;
                    case state.Distance:
                        break;
                }
            }
            else if (args.RightButton == MouseButtonState.Pressed)
            {
                switch (s)
                {
                    case state.Vertex:
                        if (selected2 == null)
                        {
                            graph.Remove(p);
                        }
                        break;
                    case state.Edge:
                        if (selected1 != null && selected2 != null)
                        {
                            graph.Disconnect(selected1.Value);
                        }
                        break;
                    case state.Way:
                        break;
                    case state.Distance:
                        break;
                }
            }
        }
        void Redraw(object sender, MouseButtonEventArgs e)
        {
            var list = graph.G;
            DrawingField.Children.Clear();

            var lineSet = new Dictionary<Point, List<Point>>();

            foreach (var v1 in list)
            {
                foreach (var v2 in v1.Value)
                {
                    if (!(lineSet.ContainsKey(v1.Key) && lineSet[v1.Key].Contains(v2.Key)) && !(lineSet.ContainsKey(v2.Key) && lineSet[v2.Key].Contains(v1.Key)))
                    {
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
            foreach (var v in list)
            {
                if (v.Key == graph.Selected)
                    DrawingField.Children.Add(new Ellipse()
                    {
                        Width = graph.Radius * 2,
                        Height = graph.Radius * 2,
                        Margin = new Thickness(v.Key.X - graph.Radius, v.Key.Y - graph.Radius, 0, 0),
                        StrokeStartLineCap = PenLineCap.Round,
                        StrokeEndLineCap = PenLineCap.Round,
                        StrokeThickness = 1,
                        Stroke = Brushes.Red
                    });
                else
                    DrawingField.Children.Add(new Ellipse()
                    {
                        Width = graph.Radius * 2,
                        Height = graph.Radius * 2,
                        Margin = new Thickness(v.Key.X - graph.Radius, v.Key.Y - graph.Radius, 0, 0),
                        StrokeStartLineCap = PenLineCap.Round,
                        StrokeEndLineCap = PenLineCap.Round,
                        StrokeThickness = 1,
                        Stroke = Brushes.Black
                    });
            }

        }
        void Chose(object sender, RoutedEventArgs args)
        {
            var s = (RadioButton)sender;
            if (s == Edge)
                ShowText();
            else
                HideText();
        }
        void HideText()
        {
            if (Label != null)
                Label.Visibility = Visibility.Hidden;
            if (Cost != null)
                Cost.Visibility = Visibility.Hidden;
        }
        void ShowText()
        {
            if (Label != null)
                Label.Visibility = Visibility.Visible;
            if (Cost != null)
                Cost.Visibility = Visibility.Visible;
        }
    }
}
