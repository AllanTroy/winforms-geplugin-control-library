
---


**The Google Earth API has been deprecated as of December 12th, 2014.
The API, and hence this library, will only continue to work until December 12th, 2015**


---


Hi,

I hope this project will be usefull to anyone wishing to integrate the Google Earth Plugin in to their own applications - either by using the controls directly or any of the code and ideas behind them.

The library uses the [Google Earth Plugin](http://www.google.com/earth/explore/products/plugin.html) along with the [dynamic language runtime](http://msdn.microsoft.com/en-us/library/dd233052.aspx) (DLR) to help integrate the [Google Earth Javascript API](http://code.google.com/apis/earth/documentation/reference/index.html) into managed code.

The library makes extensive use of the [C# type dynamic](http://msdn.microsoft.com/en-us/library/dd264741.aspx) when accessing objects in the Google Earth API. Essentially this means that you can use the Earth Api in managed code [just as if you where using JavaScript](http://code.google.com/p/winforms-geplugin-control-library/wiki/ExampleForm?ts=1328647132&updated=ExampleForm). This dynamic late binding also means  that this control library works regardless of the version of the Google Earth Plugin that is installed on a users machine.

As this library uses the Google Earth Plugin API you should read the [Google Maps/Google Earth APIs Terms of Service](http://code.google.com/apis/maps/terms.html) before downloading or using any of the code on this site.

If you are interested in a more basic example of embedding the plug-in into an application, please see the examples for both windows and mac in the [Google Earth API Demo Gallery](http://earth-api-samples.googlecode.com/svn/trunk/demos/desktop-embedded/index.html)

If you find a bug in this control library please submit it using the [Issue Tracker Form](http://code.google.com/p/winforms-geplugin-control-library/issues/entry?template=Defect%20report%20from%20user)

If you would like some particular functionality to be supported by the controls please use the [Feature Request Form](http://code.google.com/p/winforms-geplugin-control-library/issues/entry?template=Enhancement%20report%20from%20user)

Thanks,

Fraser

![https://sites.google.com/site/earthapi2/gepluginctrls.jpg](https://sites.google.com/site/earthapi2/gepluginctrls.jpg)

_Test application showing a loaded network link of live earth quake data from the USGS_

## Main Controls ##

  * [GEWebBrowser](GEWebBrowser.md) - A custom [WebBrowser](http://msdn.microsoft.com/en-us/library/system.windows.forms.webbrowser.aspx) control to hold the plug-in

  * [KmlTreeView](KmlTreeView.md) - A custom [TreeView](http://msdn.microsoft.com/en-us/library/system.windows.forms.treeview.aspx) control for displaying kml/kmz that integrates with GEWebBrowser

  * [GEToolStrip](GEToolStrip.md) - Useful controls on a custom [ToolStrip](http://msdn.microsoft.com/en-us/library/system.windows.forms.toolstrip.aspx) control that integrates with GEWebBrowser

  * [GEStatusStrip](GEStatusStrip.md) - A custom [StatusStrip](http://msdn.microsoft.com/en-us/library/system.windows.forms.statusstrip(VS.85).aspx) control that displays useful api/plugin/browser version information and a loading bar

## Changes ##

  * Added a wrapper for the KmlViewerOptions class.
  * Geocoding now uses the Google Maps API v3 rather than the depreciated version 2
  * The KmlTreeView is now virtual (only loads the data that is actually displayed to the user)
  * ~~Context menus for KmlTreeViewNodes (reload/remove)~~
  * KmlTreeView now supports tri-state checkboxes
  * NetworkLinks can be expanded in the KmlTreeView - more to follow!
  * Added support for the trees layer and the street view functionality
  * Added 'wrapper' classes for [GEOptions](GEOptions.md) and [GENavigationControl](GENavigationControl.md)
  * Added enums for most of the plugin type (AltitudeMode, MapType, etc)
  * Added support for toggling the historical imagery (5.2.1.1329)
  * Controls are fully independent of the plug-in version.
  * Moved from early to late binding of the type library
  * Updated project solution file to Visual Studio 2010

[See the full list of changes](http://code.google.com/p/winforms-geplugin-control-library/source/list)