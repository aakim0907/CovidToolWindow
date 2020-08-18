namespace CovidWin
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Controls;
    using Microsoft.Internal.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Shell.Interop;
    using RestSharp;
    using Newtonsoft.Json;
    //using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("80e52590-c666-46ba-8b8d-850286de4147")]
    public class CovidWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CovidWindow"/> class.
        /// </summary>
        public CovidWindow() : base(null)
        {
            this.Caption = "See how COVID is going on";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new CovidWindowControl();
        }

        public override bool SearchEnabled
        {
            get { return true; }
        }

        public override IVsSearchTask CreateSearch(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback)
        {
            if (pSearchQuery == null || pSearchCallback == null)
                return null;
            return new CovidWindowTask(dwCookie, pSearchQuery, pSearchCallback, this);
        }

        public override void ClearSearch()
        {
            CovidWindowControl control = (CovidWindowControl)this.Content;
            control.SearchResultsTextBox.Text = control.SearchContent;
        }

        public override void ProvideSearchSettings(IVsUIDataSource pSearchSettings)
        {
            // for search to happen on enter 
            // SST_INSTANT for real-time
            Utilities.SetValue(pSearchSettings,
                SearchSettingsDataSource.SearchStartTypeProperty.Name,
                (uint)VSSEARCHSTARTTYPE.SST_ONDEMAND);
            // show progress bar
            Utilities.SetValue(pSearchSettings,
                SearchSettingsDataSource.SearchProgressTypeProperty.Name,
                 (uint)VSSEARCHPROGRESSTYPE.SPT_DETERMINATE);
        }

        internal class CovidWindowTask : VsSearchTask
        {
            private CovidWindow m_toolWindow;

            public CovidWindowTask(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback, CovidWindow toolwindow)
                : base(dwCookie, pSearchQuery, pSearchCallback)
            {
                m_toolWindow = toolwindow;
            }

            protected override void OnStartSearch()
            {
                // Use the original content of the text box as the target of the search.
                //var separator = new string[] { Environment.NewLine };
                CovidWindowControl control = (CovidWindowControl)m_toolWindow.Content;

                //string[] contentArr = control.SearchContent.Split(separator, StringSplitOptions.None);
                //string[] contentArr = { "1 go", "2 good", "3 Go", "4 Good", "5 Goodbye", "6 goodbye" };

                // Set variables that are used in the finally block.
                //StringBuilder sb = new StringBuilder("");
                //uint resultCount = 0;
                this.ErrorCode = VSConstants.S_OK;

                ObservableCollection<SiteItem> Sites = new ObservableCollection<SiteItem>();


                try
                {
                    string searchString = this.SearchQuery.SearchString;

                    var client = new RestClient($"https://covid-19-testing.github.io/locations/{searchString}/complete.json");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    var testingSites = JsonConvert.DeserializeObject<List<CovidTestingSite>>(response.Content);
                    var sortedSites = testingSites.OrderByDescending(site => site.updated).ToList();

                    foreach (CovidTestingSite site in sortedSites)
                    {
                        var number = "";
                        var address = "";
                        if (site.phones != null)
                        {
                            number = site.phones[0].number;
                        }
                        if (site.physical_address != null)
                        {
                            var current = site.physical_address[0];
                            address = $"{current.address_1}, {current.city} ({current.postal_code})";
                        }
                        Sites.Add(new SiteItem(site.name, site.description, site.updated, address, number));
                    }


                    // Determine the results.
                    uint progress = 0;
                    //foreach (string line in contentArr)
                    //{
                    //    if (line.ToLower().Contains(searchString.ToLower()))
                    //    {
                    //        sb.AppendLine(line);
                    //        resultCount++;
                    //    }

                    //SearchCallback.ReportProgress(this, progress++, (uint)contentArr.GetLength(0));

                    //    // Uncomment the following line to demonstrate the progress bar.
                    //    //System.Threading.Thread.Sleep(100);
                    //}
                }
                catch (Exception e)
                {
                    this.ErrorCode = VSConstants.E_FAIL;
                }
                finally
                {
                    //ThreadHelper.Generic.Invoke(() =>
                    //{ ((TextBox)((CovidWindowControl)m_toolWindow.Content).SearchResultsTextBox).Text = sb.ToString(); });                    

                    //this.SearchResults = resultCount;

                    ThreadHelper.Generic.Invoke(() =>
                    { ((CovidWindowControl)m_toolWindow.Content).SearchResultsList = Sites; 
                        ((CovidWindowControl)m_toolWindow.Content).DataContext = Sites; });
                }

                // Call the implementation of this method in the base class.
                // This sets the task status to complete and reports task completion.
                base.OnStartSearch();
            }

            protected override void OnStopSearch()
            {
                this.SearchResults = 0;
            }
        }
    }
}
