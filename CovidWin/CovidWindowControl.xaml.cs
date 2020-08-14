namespace CovidWin
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Text;

    /// <summary>
    /// Interaction logic for CovidWindowControl.
    /// </summary>
    public partial class CovidWindowControl : UserControl
    {
        public TextBox SearchResultsTextBox { get; set; }
        public string SearchContent { get; set; }

        public CovidWindowControl()
        {
            InitializeComponent();

            this.SearchResultsTextBox = resultsTextBox;
            this.SearchContent = BuildContent();

            this.SearchResultsTextBox.Text = this.SearchContent;
        }

        private string BuildContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("1 go");
            sb.AppendLine("2 good");
            sb.AppendLine("3 Go");
            sb.AppendLine("4 Good");
            sb.AppendLine("5 goodbye");
            sb.AppendLine("6 Goodbye");

            return sb.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CovidWindowControl"/> class.
        /// </summary>
        //public CovidWindowControl()
        //{
        //    this.InitializeComponent();
        //}

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        //[SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        //[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show(
        //        string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
        //        "CovidWindow");
        //}
    }
}