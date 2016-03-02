using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Telerik.Windows.Controls.Map;
using mapbox.vector.tile;

namespace RadControls
{
    public class OSMCustomSource : TiledMapSource
    {
        private const string TileMapnikUrlFormat = @"http://{prefix}.tile.openstreetmap.org/{zoom}/{x}/{y}.png";
        private const int TileSize = 256;

        private string[] tilePathPrefixes = new string[] { "a", "b", "c" };

        /// <summary>
        /// Initializes a new instance of the OSMCustomSource class.
        /// </summary>
        public OSMCustomSource()
            : base(1, 18, TileSize, TileSize)
        {
        }

        /// <summary>
        /// Initialize provider.
        /// </summary>
        public override void Initialize()
        {
            // Raise provider intialized event.
            this.RaiseInitializeCompleted();
        }

        /// <summary>
        /// Gets the image URI.
        /// </summary>
        /// <param name="tileLevel">Tile level.</param>
        /// <param name="tilePositionX">Tile X.</param>
        /// <param name="tilePositionY">Tile Y.</param>
        /// <returns>URI of image.</returns>
        protected override Uri GetTile(int tileLevel, int tilePositionX, int tilePositionY)
        {
            int zoomLevel = this.ConvertTileToZoomLevel(tileLevel);
            const string mapboxfile = @"D:\utah.mbtiles";

            // act
            var pbfStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(mapboxfile);
            var layerInfos = VectorTileParser.Parse(pbfStream, tilePositionX, tilePositionY, zoomLevel, false);


            string url = TileMapnikUrlFormat;
            string prefix = this.GetTilePrefix(tilePositionX, tilePositionY);

            url = ProtocolHelper.SetScheme(url);
            url = url.Replace("{prefix}", prefix);
            url = url.Replace("{zoom}", zoomLevel.ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{x}", tilePositionX.ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{y}", tilePositionY.ToString(CultureInfo.InvariantCulture));

            return new Uri(url);
        }

        private string GetTilePrefix(int tilePositionX, int tilePositionY)
        {
            int index = tilePositionY % 3;
            if ((tilePositionX & 1) == 1)
            {
                index = 2 - index;
            }

            return this.tilePathPrefixes[index];
        }
    }
}
