using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using ChatterSolidworks.Controls;
using System;
using System.Runtime.InteropServices;
using Marshal = System.Runtime.InteropServices.Marshal;

namespace ChatterSolidworks
{
    /// <summary>
    /// SolidWorks Add-in implementation for Chatter.
    /// This class is the main entry point for the add-in.
    /// </summary>
    [ComVisible(true)]
    [Guid("A8E7D4F2-3B1C-4E5A-9F8D-6C2B1A0E9D3F")]
    [ProgId("ChatterSolidworks.SwAddin")]
    public class SwAddin : ISwAddin
    {
        #region Private Fields

        private ISldWorks? _swApp;
        private int _addinCookie;

        // TaskPane fields
        private ITaskpaneView? _taskPaneView;
        private RedditTaskPaneControl? _taskPaneControl;

        #endregion

        #region SolidWorks Registration

        [ComRegisterFunction]
        public static void RegisterFunction(Type t)
        {
            try
            {
                var keyPath = @"SOFTWARE\SolidWorks\AddIns\{" + t.GUID.ToString() + "}";

                using var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath);
                key?.SetValue(null, 0); // 0 = Not loaded at startup, 1 = Loaded at startup
                key?.SetValue("Title", "Chatter Reddit");
                key?.SetValue("Description", "Reddit browser for SolidWorks");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering add-in: {ex.Message}");
            }
        }

        [ComUnregisterFunction]
        public static void UnregisterFunction(Type t)
        {
            try
            {
                var keyPath = @"SOFTWARE\SolidWorks\AddIns\{" + t.GUID.ToString() + "}";
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unregistering add-in: {ex.Message}");
            }
        }

        #endregion

        #region ISwAddin Implementation

        /// <summary>
        /// Called when SolidWorks loads the add-in.
        /// </summary>
        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            _swApp = (ISldWorks)ThisSW;
            _addinCookie = Cookie;

            // Setup callbacks
            _swApp.SetAddinCallbackInfo2(0, this, _addinCookie);

            // Create TaskPane with Reddit browser
            CreateTaskPane();

            return true;
        }

        /// <summary>
        /// Called when SolidWorks unloads the add-in.
        /// </summary>
        public bool DisconnectFromSW()
        {
            RemoveTaskPane();

            // Explicit COM release for reliable cleanup
            if (_swApp != null)
            {
                Marshal.ReleaseComObject(_swApp);
                _swApp = null;
            }

            return true;
        }

        #endregion

        #region TaskPane Management

        private void CreateTaskPane()
        {
            if (_swApp == null) return;

            try
            {
                // Create the TaskPane view (appears in the right sidebar)
                // First parameter is icon path (empty string for no icon)
                // Second parameter is the tooltip/title
                _taskPaneView = _swApp.CreateTaskpaneView2(
                    string.Empty,
                    "Reddit");

                if (_taskPaneView != null)
                {
                    // Add the UserControl to the TaskPane using its ProgId
                    // SolidWorks will create an instance of the control
                    _taskPaneControl = _taskPaneView.AddControl(
                        "ChatterSolidworks.RedditTaskPaneControl",
                        string.Empty) as RedditTaskPaneControl;
                }
            }
            catch (Exception ex)
            {
                _swApp?.SendMsgToUser2(
                    $"Failed to create Reddit TaskPane: {ex.Message}",
                    (int)swMessageBoxIcon_e.swMbWarning,
                    (int)swMessageBoxBtn_e.swMbOk);
            }
        }

        private void RemoveTaskPane()
        {
            try
            {
                _taskPaneControl?.Dispose();
                _taskPaneControl = null;

                if (_taskPaneView != null)
                {
                    _taskPaneView.DeleteView();
                    Marshal.ReleaseComObject(_taskPaneView);
                    _taskPaneView = null;
                }
            }
            catch
            {
                // Ignore cleanup errors during shutdown
                _taskPaneControl = null;
                _taskPaneView = null;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the SolidWorks application instance.
        /// </summary>
        public ISldWorks? SwApp => _swApp;

        /// <summary>
        /// Gets the TaskPane control instance.
        /// </summary>
        public RedditTaskPaneControl? TaskPaneControl => _taskPaneControl;

        #endregion
    }
}
