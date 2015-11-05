using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using OsmSharp.Android.UI.Data.SQLite;
using OsmSharp.Android.UI.Renderer.Images;
using OsmSharp.Collections.Tags;
using OsmSharp.Math.Geo;
using OsmSharp.Routing;
using OsmSharp.Routing.CH;
using OsmSharp.Routing.Navigation;
using OsmSharp.Routing.Osm.Interpreter;
using OsmSharp.UI;
using OsmSharp.UI.Animations.Navigation;
using OsmSharp.UI.Map;
using OsmSharp.UI.Map.Layers;
using OsmSharp.UI.Renderer.Scene;
using OsmSharp.Android.UI;
using System.Collections.Generic;
using OsmSharp.UI.Map.Styles;
using System.Reflection;
using System.IO;

namespace DataCollectionApp.Activitys
{
    /// <summary>
    /// The main activity.
    /// </summary>
    // [Activity(ConfigurationChanges = global::Android.Content.PM.ConfigChanges.Orientation | global::Android.Content.PM.ConfigChanges.ScreenLayout)]
    [Activity]
    public class MainActivity : Activity
    {
        /// <summary>
        /// Holds the mapview.
        /// </summary>
        private MapView _mapView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // hide title bar.
            this.RequestWindowFeature(global::Android.Views.WindowFeatures.NoTitle);

            // initialize map.
            var map = new Map();

            // add a preprocessed vector data file.
            //var sceneStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Android.Demo.default.map");
            //map.AddLayer(new LayerScene(Scene2D.Deserialize(sceneStream, true)));

            //OsmSharp.Osm.Data.Memory.MemoryDataSource dataSource;
            //using (var sceneStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Android.Demo.map1.osm"))
            //{
            //    dataSource = OsmSharp.Osm.Data.Memory.MemoryDataSource.CreateFromXmlStream(sceneStream);
            //}
            string path = Assembly.GetExecutingAssembly().GetName().Name;
            StreamReader streamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(path + ".EmbeddedResources.default.mapcss"));
            string test = streamReader.ReadToEnd();
            StyleInterpreter styleInterpreter = new OsmSharp.UI.Map.Styles.MapCSS.MapCSSInterpreter(test);
            map.AddLayerOsm(OsmSharp.Osm.Data.Memory.MemoryDataSource.CreateFromXmlStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(path + ".EmbeddedResources.map.osm")), styleInterpreter);

            //var r2d = new CanvasRenderer2D();
            //var renderer = new OsmSharp.UI.Map.MapRenderer<Canvas>(r2d);
            //var view = OsmSharp.UI.Renderer.View2D.CreateFromBounds(dataSource.BoundingBox.TopLeft.Latitude, dataSource.BoundingBox.TopLeft.Longitude,
            //                                             dataSource.BoundingBox.BottomRight.Latitude, dataSource.BoundingBox.BottomRight.Longitude);
            //map.ViewChanged(16, new OsmSharp.Math.Geo.GeoCoordinate(view.Center), null, view);

            // define the mapview.
            var mapViewSurface = new MapViewSurface(this);
            mapViewSurface.MapScaleFactor = 2;
            _mapView = new MapView(this, mapViewSurface);
            _mapView.Map = map;
            _mapView.MapMaxZoomLevel = 17; // limit min/max zoom, the vector data in this sample covers only a small area.
            _mapView.MapMinZoomLevel = 12;
            _mapView.MapTilt = 0;
            //_mapView.MapCenter = new GeoCoordinate(51.26361, 4.78620);
            _mapView.MapCenter = new GeoCoordinate(30.538079, 114.4159972);
            _mapView.MapZoom = 16;
            _mapView.MapAllowTilt = false;

            // set the map view as the default content view.
            SetContentView(_mapView);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // dispose of all resources.
            // the mapview is completely destroyed in this sample, read about the Android Activity Lifecycle here:
            // http://docs.xamarin.com/guides/android/application_fundamentals/activity_lifecycle/
            _mapView.Map.Close();

            _mapView.Close();
            _mapView.Dispose();
            _mapView = null;
        }
    }
}

