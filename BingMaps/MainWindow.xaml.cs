using System.Windows;
using System.Windows.Media;
using Catfood.Shapefile;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Win32;
using System.Windows.Input;

namespace BingMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoadShapefile_Clicked(object sender, RoutedEventArgs e)
        {
            //ShapeLayer.Children.Clear();

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "Select an ESRI Shapefile";
            dialog.Filter = "ESRI Shapefile (*.shp) |*.shp;";

            bool? valid = dialog.ShowDialog();

            if (valid.HasValue && valid.Value)
            {
                Cursor cursor = this.Cursor;
                this.ForceCursor = true;
                this.Cursor = Cursors.Wait;
                //foreach (string file in dialog.FileNames)
                //{
                    using (Shapefile shapefile = new Shapefile(dialog.FileName))
                    {
                        //Set the map view for the data set
                        MyMap.SetView(RectangleDToLocationRect(shapefile.BoundingBox));
                        foreach (Shape s in shapefile)
                        {
                            RenderShapeOnLayer(s, ShapeLayer);
                        }
                    }
                //}

                this.Cursor = cursor;
            }
        }

        private void ClearMap_Clicked(object sender, RoutedEventArgs e)
        {
            ShapeLayer.Children.Clear();
        }

        #region Helper Methods

        private LocationRect RectangleDToLocationRect(RectangleD bBox)
        {
            return new LocationRect(bBox.Top, bBox.Left, bBox.Bottom, bBox.Right);
        }

        private void RenderShapeOnLayer(Shape shape, MapLayer layer)
        {
            switch (shape.Type)
            {
                case ShapeType.Point:
                    ShapePoint point = shape as ShapePoint;
                    layer.Children.Add(new Pushpin()
                    {
                        Location = new Location(point.Point.Y, point.Point.X)
                    });
                    break;
                case ShapeType.PolyLine:
                    ShapePolyLine polyline = shape as ShapePolyLine;
                    for (int i = 0; i < polyline.Parts.Count; i++)
                    {
                        layer.Children.Add(new MapPolyline()
                        {
                            Locations = PointDArrayToLocationCollection(polyline.Parts[i]),
                            Stroke = new SolidColorBrush(Color.FromArgb(150, 255, 0, 0))
                        });
                    }
                    break;
                case ShapeType.Polygon:
                    ShapePolygon polygon = shape as ShapePolygon;
                    if (polygon.Parts.Count > 0)
                    {
                        //Only render the exterior ring of polygons for now.
                        for (int i = 0; i < polygon.Parts.Count; i++)
                        {
                            //Note that the exterior rings in a ShapePolygon have a Clockwise order
                            if (!IsCCW(polygon.Parts[i]))
                            {
                                layer.Children.Add(new MapPolygon()
                                {
                                    Locations = PointDArrayToLocationCollection(polygon.Parts[i]),
                                    Fill = new SolidColorBrush(Color.FromArgb(150, 0, 0, 255)),
                                    Stroke = new SolidColorBrush(Color.FromArgb(150, 255, 0, 0))
                                });
                            }
                        }
                    }
                    break;
                case ShapeType.MultiPoint:
                    ShapeMultiPoint multiPoint = shape as ShapeMultiPoint;
                    for (int i = 0; i < multiPoint.Points.Length; i++)
                    {
                        layer.Children.Add(new Pushpin()
                        {
                            Location = new Location(multiPoint.Points[i].Y, multiPoint.Points[i].X)
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        private LocationCollection PointDArrayToLocationCollection(PointD[] points)
        {
            LocationCollection locations = new LocationCollection();
            int numPoints = points.Length;
            for (int i = 0; i < numPoints; i++)
            {
                locations.Add(new Location(points[i].Y, points[i].X));
            }
            return locations;
        }

        /// <summary>
        /// Determines if the coordinates in an array are in a counter clockwise order. 
        /// </summary>
        /// <returns>A boolean indicating if the coordinates are in a counter clockwise order</returns>
        public bool IsCCW(PointD[] points)
        {
            int count = points.Length;

            PointD coordinate = points[0];
            int index1 = 0;

            for (int i = 1; i < count; i++)
            {
                PointD coordinate2 = points[i];
                if (coordinate2.Y > coordinate.Y)
                {
                    coordinate = coordinate2;
                    index1 = i;
                }
            }

            int num4 = index1 - 1;

            if (num4 < 0)
            {
                num4 = count - 2;
            }

            int num5 = index1 + 1;

            if (num5 >= count)
            {
                num5 = 1;
            }

            PointD coordinate3 = points[num4];
            PointD coordinate4 = points[num5];

            double num6 = ((coordinate4.X - coordinate.X) * (coordinate3.Y - coordinate.Y)) -
                ((coordinate4.Y - coordinate.Y) * (coordinate3.X - coordinate.X));

            if (num6 == 0.0)
            {
                return (coordinate3.X > coordinate4.X);
            }

            return (num6 > 0.0);
        }

        #endregion
    }
}
