namespace CovidWin
{
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// Interaction logic for CovidWindowControl.
    /// </summary>
    public partial class CovidWindowControl : UserControl
    {
        public CovidWindowControl()
        {
            InitializeComponent();
        }

        public void ShowErrorMessage(string errorMessage)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, errorMessage, this.ToString()),
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Exception Occurred", this.ToString())
            );
        }

        /// <summary>
        /// Handles click on each sites
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void OnSiteClick(object sender, RoutedEventArgs e)
        {
            //object { System.Windows.Controls.Stackpanel }
            //var query = sender.InheritanceParent.CacheModeChangedHandler.Target.Content.Name + ' ' + sender.InheritanceParent.CacheModeChangedHandler.Target.Content.Address;
            //var uri = "https://www.bing.com/maps?q=" + Regex.Replace(query, @"\s+", "+");
            //var name = e.OriginalSource.Inlines.FirstInline.Text;
            VsShellUtilities.OpenSystemBrowser("https://www.bing.com/maps");
        }
    }
}