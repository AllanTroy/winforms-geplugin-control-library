 // <copyright file="External.cs" company="FC">
// Copyright (c) 2008 Fraser Chapman
// </copyright>
// <author>Fraser Chapman</author>
// <email>fraser.chapman@gmail.com</email>
// <date>2008-12-22</date>
// <summary>This file is part of FC.GEPluginCtrls
// FC.GEPluginCtrls is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
// </summary>
namespace FC.GEPluginCtrls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Microsoft.CSharp.RuntimeBinder;

    /// <summary>
    /// This COM Visible class contains all the public methods to be called from JavaScript.
    /// The various events are used by the <see cref="GEWebBrowser"/> when dealing with the plug-in
    /// </summary>
    [ComVisible(true)]
    public class External
    {
        #region Private fields

        /// <summary>
        /// Cache of kml event objects
        /// </summary>
        private static readonly Dictionary<string, AutoResetEvent> AutoResetDictionary =
            new Dictionary<string, AutoResetEvent>();

        /// <summary>
        /// Stores fetched Kml Objects
        /// </summary>
        private static readonly Dictionary<string, object> KmlObjectDictionary =
            new Dictionary<string, object>();

        #endregion

        #region Public events

        /// <summary>
        /// Raised when the plugin is ready
        /// </summary>
        public event EventHandler<GEEventArgs> PluginReady;

        /// <summary>
        /// Raised when there is a kml event
        /// </summary>
        public event EventHandler<GEEventArgs> KmlEvent;

        /// <summary>
        /// Raised when a KML/KMZ file has loaded
        /// </summary>
        public event EventHandler<GEEventArgs> KmlLoaded;

        /// <summary>
        /// Raised when there is a script error in the document 
        /// </summary>
        public event EventHandler<GEEventArgs> ScriptError;

        /// <summary>
        /// Raised when there is a GEPlugin event (frameend, balloonclose) 
        /// </summary>
        public event EventHandler<GEEventArgs> PluginEvent;

        /// <summary>
        /// Raised when there is a viewchangebegin, viewchange or viewchangeend event 
        /// </summary>
        public event EventHandler<GEEventArgs> ViewEvent;

        #endregion

        #region Internal properites

        /// <summary>
        /// Gets the store of fetched IKmlObjects.
        /// Used in the process of synchronously loading network links
        /// </summary>
        internal static Dictionary<string, object> KmlDictionary
        {
            get { return KmlObjectDictionary; }
        }

        internal static Dictionary<string, AutoResetEvent>  ResetDictionary
        {
            get
            {
                return AutoResetDictionary;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Allows JavaScript to send debug messages
        /// </summary>
        /// <param name="category">the category of the message</param>
        /// <param name="message">the debug message</param>
        public void DebugMessage(string category, string message)
        {
            Debug.WriteLine(message, category);
        }

        /// <summary>
        /// Called from JavaScript when the plugin is ready
        /// </summary>
        /// <param name="ge">the plugin instance</param>
        public void Ready(dynamic ge)
        {
            if (!GEHelpers.IsGE(ge))
            {
                throw new ArgumentException("ge is not of the type GEPlugin");
            }

            this.OnPluginReady(this, new GEEventArgs("Ready", "None", ge));
        }

        /// <summary>
        /// Called from JavaScript when there is an error
        /// </summary>
        /// <param name="type">the error message</param>
        /// <param name="message">the error type</param>
        public void SendError(string type, string message)
        {
            this.OnScriptError(
                this,
                new GEEventArgs(type, message));
        }

        /// <summary>
        /// Called from JavaScript when there is a kml event
        /// </summary>
        /// <param name="kmlEvent">the kml event</param>
        /// <param name="action">the event id</param>
        public void KmlEventCallback(object kmlEvent, string action)
        {
            dynamic eventObject = kmlEvent;

            try
            {
                this.OnKmlEvent(
                    this,
                    new GEEventArgs(eventObject.getType(), action, eventObject));
            }
            catch (RuntimeBinderException rbex)
            {
                Debug.WriteLine("KmlEventCallBack: " + rbex.Message, "External");
            }
        }

        /// <summary>
        /// Called from JavaScript when there is a GEPlugin event
        /// </summary>
        /// <param name="sender">The plugin object</param>
        /// <param name="action">The event action</param>
        public void PluginEventCallback(object sender, string action)
        {
            dynamic pluginEvent = sender;

            try
            {
                this.OnPluginEvent(
                    this,
                    new GEEventArgs(pluginEvent.getType(), action, pluginEvent));
            }
            catch (RuntimeBinderException rbex)
            {
                Debug.WriteLine("PluginEventCallBack: " + rbex.Message, "External");
            }
        }

        /// <summary>
        /// Called from JavaScript when there is a View event
        /// </summary>
        /// <param name="sender">The plug-in object</param>
        /// <param name="action">The event action</param>
        public void ViewEventCallback(object sender, string action)
        {
            dynamic viewEvent = sender;

            try
            {
                this.OnViewEvent(
                    this,
                    new GEEventArgs(viewEvent.getType(), action, viewEvent));
            }
            catch (RuntimeBinderException rbex)
            {
                Debug.WriteLine("ViewEventCallBack: " + rbex.Message, "External");
            }
        }

        /// <summary>
        /// Called from JavaScript when Kml has been fetched
        /// </summary>
        /// <param name="KmlObject">The fetched Kml object</param>
        public void FetchKmlCallback(object KmlObject)
        {
            this.OnFetchKml(new GEEventArgs(KmlObject));
        }

        /// <summary>
        /// Called from JavaScript when Kml has been synchronously fetched
        /// </summary>
        /// <param name="KmlObject">The fetched Kml object</param>
        /// <param name="url">The url that the object was fetched from</param>
        public void FetchKmlSynchronousCallback(object KmlObject, string url)
        {
            this.OnFetchKmlSynchronous(new GEEventArgs("FetchKmlSynchronous", url, KmlObject));
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Protected method for raising the PluginReady event
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPluginReady(object sender, GEEventArgs e)
        {
            EventHandler<GEEventArgs> handler = this.PluginReady;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Protected method for raising the KmlEvent event
        /// </summary>
        /// <param name="kmlEvent">The kmlEvent object</param>
        /// <param name="e">The Event arguments</param>
        protected virtual void OnKmlEvent(dynamic kmlEvent, GEEventArgs e)
        {
            EventHandler<GEEventArgs> handler = this.KmlEvent;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Protected method for raising the KmlLoaded event
        /// </summary>
        /// <param name="e">The Event arguments</param>
        protected virtual void OnFetchKml(GEEventArgs e)
        {
            EventHandler<GEEventArgs> handler = this.KmlLoaded;
            dynamic kmlObject = e.ApiObject;

            if (handler != null)
            {
                handler(this, new GEEventArgs(kmlObject));
            }
        }

        /// <summary>
        /// Protected method for capturing fetched IKmlObjects
        /// </summary>
        /// <param name="e">The Event arguments</param>
        protected virtual void OnFetchKmlSynchronous(GEEventArgs e)
        {
            lock (KmlDictionary)
            {
                KmlDictionary[e.Data] = e.ApiObject;
                ResetDictionary[e.Data].Set();
            }
        }

        /// <summary>
        /// Protected method for raising the ScriptError event
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnScriptError(object sender, GEEventArgs e)
        {
            if (this.ScriptError != null)
            {
                this.ScriptError(this, e);
            }
        }

        /// <summary>
        /// Protected method for raising the PluginEvent event
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPluginEvent(object sender, GEEventArgs e)
        {
            if (this.PluginEvent != null)
            {
                this.PluginEvent(this, e);
            }
        }

        /// <summary>
        /// Protected method for raising the ViewEvent event
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnViewEvent(object sender, GEEventArgs e)
        {
            if (this.ViewEvent != null)
            {
                this.ViewEvent(this, e);
            }
        }

        #endregion
    }
}