using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatterSolidworks.Controls
{
    /// <summary>
    /// WinForms UserControl that hosts WebView2 for displaying Reddit in SolidWorks TaskPane.
    /// Must be COM-visible for SolidWorks TaskPane integration via AddControl().
    /// </summary>
    [ComVisible(true)]
    [Guid("C1D2E3F4-5A6B-7C8D-9E0F-1A2B3C4D5E6F")]
    [ProgId("ChatterSolidworks.RedditTaskPaneControl")]
    public class RedditTaskPaneControl : UserControl
    {
        private WebView2? _webView;
        private Task? _initTask;
        private bool _isInitialized;
        private bool _isDisposed;

        /// <summary>
        /// The default URL to navigate to when the control loads.
        /// </summary>
        private const string DefaultUrl = "https://www.reddit.com/r/SolidWorks";

        public RedditTaskPaneControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "RedditTaskPaneControl";
            this.Size = new System.Drawing.Size(350, 600);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Initialize WebView2 when the control handle is created.
        /// This is safer than initializing in the constructor.
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Start async initialization - store task to track completion
            _initTask = InitializeWebViewAsync();
        }

        private async Task InitializeWebViewAsync()
        {
            // Don't initialize if already disposed
            if (_isDisposed) return;

            try
            {
                // Create WebView2 control
                _webView = new WebView2
                {
                    Dock = DockStyle.Fill
                };

                // Check again before modifying Controls collection
                if (_isDisposed)
                {
                    _webView.Dispose();
                    return;
                }

                this.Controls.Add(_webView);

                // Configure user data folder for persistence (cookies, login sessions, etc.)
                var userDataFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "ChatterSolidworks",
                    "WebView2");

                // Ensure the directory exists
                Directory.CreateDirectory(userDataFolder);

                // Create WebView2 environment with persistent user data
                var environment = await CoreWebView2Environment.CreateAsync(
                    userDataFolder: userDataFolder);

                // Check if disposed during async operation
                if (_isDisposed) return;

                // Initialize the CoreWebView2
                await _webView.EnsureCoreWebView2Async(environment);

                // Check if disposed during async operation
                if (_isDisposed) return;

                // Configure WebView2 settings
                ConfigureWebView();

                // Navigate to Reddit
                _webView.CoreWebView2.Navigate(DefaultUrl);

                _isInitialized = true;
            }
            catch (WebView2RuntimeNotFoundException)
            {
                // WebView2 runtime not installed
                if (!_isDisposed)
                {
                    ShowWebView2MissingMessage();
                }
            }
            catch (Exception ex)
            {
                // Other initialization error (ignore if disposed)
                if (!_isDisposed)
                {
                    ShowErrorMessage($"Failed to initialize browser: {ex.Message}");
                }
            }
        }

        private void ConfigureWebView()
        {
            if (_webView?.CoreWebView2 == null) return;

            // Enable standard web features
            _webView.CoreWebView2.Settings.IsScriptEnabled = true;
            _webView.CoreWebView2.Settings.IsWebMessageEnabled = true;
            _webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
            _webView.CoreWebView2.Settings.IsStatusBarEnabled = false;

            // Enable dev tools only in debug builds
#if DEBUG
            _webView.CoreWebView2.Settings.AreDevToolsEnabled = true;
#else
            _webView.CoreWebView2.Settings.AreDevToolsEnabled = false;
#endif
        }

        private void ShowWebView2MissingMessage()
        {
            var label = new Label
            {
                Text = "WebView2 Runtime is not installed.\n\n" +
                       "Please download and install it from:\n" +
                       "https://developer.microsoft.com/microsoft-edge/webview2/",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Padding = new Padding(20)
            };
            this.Controls.Add(label);
        }

        private void ShowErrorMessage(string message)
        {
            var label = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                ForeColor = System.Drawing.Color.Red,
                Padding = new Padding(20)
            };
            this.Controls.Add(label);
        }

        /// <summary>
        /// Navigate to a specific URL.
        /// </summary>
        public void Navigate(string url)
        {
            if (_webView?.CoreWebView2 != null && _isInitialized)
            {
                _webView.CoreWebView2.Navigate(url);
            }
        }

        /// <summary>
        /// Navigate to the r/SolidWorks subreddit.
        /// </summary>
        public void NavigateToSolidWorksSubreddit()
        {
            Navigate("https://www.reddit.com/r/SolidWorks");
        }

        protected override void Dispose(bool disposing)
        {
            _isDisposed = true;

            if (disposing)
            {
                _webView?.Dispose();
                _webView = null;
            }
            base.Dispose(disposing);
        }
    }
}
