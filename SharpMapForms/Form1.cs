using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpMapForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Create the layer
            SharpMap.Layers.VectorLayer vlay = new SharpMap.Layers.VectorLayer("States");
            //Assign the data source
            vlay.DataSource = new SharpMap.Data.Providers.ShapeFile("C:\\Users\\dtaylor\\Downloads\\utah-latest.shp\\roads.shp", true);

            //Create the style for Land
            SharpMap.Styles.VectorStyle landStyle = new SharpMap.Styles.VectorStyle();
            landStyle.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(232, 232, 232));

            //Create the style for Water
            SharpMap.Styles.VectorStyle waterStyle = new SharpMap.Styles.VectorStyle();
            waterStyle.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(198, 198, 255));

            //Create the theme items
            Dictionary<string, SharpMap.Styles.IStyle> styles = new Dictionary<string, SharpMap.Styles.IStyle>();
            styles.Add("land", landStyle);
            styles.Add("water", waterStyle);

            //Assign the theme
            vlay.Theme = new SharpMap.Rendering.Thematics.UniqueValuesTheme<string>("class", styles, landStyle);

            //Add layer to map
            mapBox1.Map.Layers.Add(vlay);
            mapBox1.Map.ZoomToExtents();
            mapBox1.Refresh();
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }
    }
}
