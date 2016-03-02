using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls.Map;

namespace RadControls
{
    public class OSMCustomProvider : TiledProvider, ICloneable
    {
        private ISpatialReference spatialReference = new MercatorProjection();

        /// <summary>
        /// Initializes a new instance of the OSMCustomProvider class.
        /// </summary>
        public OSMCustomProvider()
            : base()
        {
            var mapnik = new OSMCustomSource();
            this.MapSources.Add(mapnik.UniqueId, mapnik);
        }

        /// <summary>
        /// Returns the SpatialReference for the map provider.
        /// </summary>
        public override ISpatialReference SpatialReference
        {
            get
            {
                return spatialReference;
            }
        }

        public object Clone()
        {
            OSMCustomProvider clone = new OSMCustomProvider();
            this.InheritCurrentSource(clone);
            this.InheritParameters(clone);

            return clone;
        }
    }
}
