using System.Web;
using System.Web.Optimization;

namespace AudioWeb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/AudioApp")
                .Include("~/bower_components/angular/angular.min.js")
                .Include("~/bower_components/jquery/dist/jquery.min.js")
                .Include("~/bower_components/angular-audio/app/angular.audio.js")
                .Include("~/Scripts/audioApp.js")
                .IncludeDirectory("~/Scripts/Controllers", "*.js")
                .IncludeDirectory("~/Scripts/Factories", "*.js"));

            bundles.Add(new StyleBundle("~/Content/SiteStyles")
                .Include("~/Content/Site.css"));
        }
    }
}