namespace CovidWin
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using System.Collections.Generic;
    using Microsoft.Internal.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Shell.Interop;
    using RestSharp;
    using Newtonsoft.Json;
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
            this.Caption = "COVID19 Testing Sites";
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

        public override void ProvideSearchSettings(IVsUIDataSource pSearchSettings)
        {
            // Trigger search on enter. SST_INSTANT for real-time search.
            Utilities.SetValue(pSearchSettings, SearchSettingsDataSource.SearchStartTypeProperty.Name, (uint)VSSEARCHSTARTTYPE.SST_ONDEMAND);
        }

        internal class CovidWindowTask : VsSearchTask
        {
            private CovidWindow CovidToolWindow;
            private ObservableCollection<SiteItem> TestingSites;

            public CovidWindowTask(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback, CovidWindow toolwindow) : base(dwCookie, pSearchQuery, pSearchCallback)
            {
                CovidToolWindow = toolwindow;
            }

            protected override void OnStartSearch()
            {
                TestingSites = new ObservableCollection<SiteItem>();
                
                try
                {
                    string searchString = this.SearchQuery.SearchString;
                    var client = new RestClient($"https://covid-19-testing.github.io/locations/{searchString}/complete.json");
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);

                    var testingSites = JsonConvert.DeserializeObject<List<CovidTestingSite>>(response.Content);
                    var sortedSites = testingSites.OrderByDescending(site => site.updated).ToList();

                    foreach (CovidTestingSite site in sortedSites)
                    {
                        var number = site.phones != null ? site.phones[0].number : "";
                        var address = "";
                        if (site.physical_address != null)
                        {
                            var current = site.physical_address[0];
                            address = $"{current.address_1}, {current.city} {current.postal_code}";
                        }
                        TestingSites.Add(new SiteItem(site.name, site.description, site.updated, address, number));
                    }
                }
                catch (Exception e)
                {
                    ((CovidWindowControl)CovidToolWindow.Content).ShowErrorMessage(e.Message);
                }
                finally
                {
                    ThreadHelper.Generic.Invoke(() => { ((CovidWindowControl)CovidToolWindow.Content).DataContext = TestingSites; });
                }

                // Call the implementation of this method in the base class.
                // This sets the task status to complete and reports task completion.
                base.OnStartSearch();
            }
        }
    }
}
