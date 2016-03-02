using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.IO;
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
using MapSurfer.IO;
using MapSurfer.IO.FileTypes;
using MapSurfer.Geometries;
using MapSurfer.Reflection;
using MapSurfer.Utilities;
using MapSurfer.Windows.Forms;
using MapSurfer.Rendering;
using Microsoft.Win32;

namespace MapSurfer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MapViewer m_mapViewer;
        private Renderer m_renderer;
        public MainWindow()
        {
            InitializeComponent();
            CoreUtility.Initialize();
            CoreUtility.CheckAndFixCulture();

            m_renderer = (MapSurfer.Rendering.Renderer)MapSurfer.Rendering.RendererManager.CreateDefaultInstance();

            m_mapViewer = new MapViewer();
            //m_mapViewer.Dock = DockStyle.Fill;
            m_mapViewer.RedrawOnAttach = true;
            m_mapViewer.ActiveTool = MapTool.Zoom;
            m_mapViewer.RedrawOnResizing = true;
            //this.Controls.Add(m_mapViewer);
            this.AddChild(m_mapViewer);

            // Initialize file types
            FileTypeManager<Map> ftmMap = FileTypeManagerCache.GetFileTypeManager<Map>();
            ftmMap.AddSearchPath(typeof(Map).Assembly.GetLocation());
            ftmMap.AddSearchPath(MSNEnvironment.GetFolderPath(MSNSpecialFolder.StylingFormats));
            ftmMap.InitializeFileTypes();

            // Load map project
            string fileName = System.IO.Path.Combine(MSNUtility.GetMSNInstallPath(), @"Samples\Projects\Bremen.msnpx");
            if (File.Exists(fileName))
            {
                LoadProject(fileName);

                Envelope env = new Envelope(978779.133679862, 6990983.0938755, 990289.718456316, 6997278.67914107); //map.Extent
                m_mapViewer.ZoomToEnvelope(env);
            }
        }
        private void LoadProject(string fileName)
        {
            Map map = Map.FromFile(fileName);
            map.Initialize(System.IO.Path.GetDirectoryName(fileName), true);
            m_mapViewer.Map = map;
            m_mapViewer.ZoomToFullExtent();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuOpenProject_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            FileTypeCollection<Map> ftc = FileTypeManagerCache.GetFileTypeCollection<Map>(FileTypeFlags.ReadSupport);
            ofd.Filter = ftc.GetFileTypesFilter(false);

            if (ofd.ShowDialog() == true)
            {
                LoadProject(ofd.FileName);
            }
        }

        private void mnuZoomToExtent_Click(object sender, EventArgs e)
        {
            m_mapViewer.ZoomToFullExtent();
            m_mapViewer.Redraw();
        }
    }
}
