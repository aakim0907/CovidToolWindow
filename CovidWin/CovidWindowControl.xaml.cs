namespace CovidWin
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.ObjectModel;
    using Microsoft.VisualStudio.Shell;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Interaction logic for CovidWindowControl.
    /// </summary>
    public partial class CovidWindowControl : UserControl
    {
        public TextBox SearchResultsTextBox { get; set; }
        public string SearchContent { get; set; }
        public ObservableCollection<SiteItem> SearchResultsList { get; set; }

        public CovidWindowControl()
        {
            InitializeComponent();

            //this.SearchResultsTextBox = resultsTextBox;
            //this.SearchContent = BuildContent();

            //this.SearchResultsTextBox.Text = this.SearchContent;

            this.DataContext = this.SearchResultsList;
            //this.SearchResultsList = resultsList;
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        //[SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        //[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void OnSiteClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(
            //    string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
            //    "CovidWindow");

            //var query = sender.InheritanceParent.CacheModeChangedHandler.Target.Content.Name + ' ' + sender.InheritanceParent.CacheModeChangedHandler.Target.Content.Address;
            // content or data context
            //var uri = "https://www.bing.com/maps?q=" + Regex.Replace(query, @"\s+", "+");
            //var name = e.OriginalSource.Inlines.FirstInline.Text;
            VsShellUtilities.OpenSystemBrowser("https://www.bing.com/maps");
        }
    }
}