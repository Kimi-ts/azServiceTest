using System.Web;
using System.Web.Optimization;

namespace AudioWeb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/AudioApp")
            //    .IncludeDirectory("~/Scripts/Controllers", "*.js")
            //    .IncludeDirectory("~/Scripts/Factories", "*.js")
            //    .Include("~/Scripts/audioApp.js"));

            bundles.Add(new ScriptBundle("~/bundles/AudioApp")
                .Include("~/bower_components/angular/angular.min.js")
                .Include("~/bower_components/angular-audio/app/angular.audio.js")
                .IncludeDirectory("~/Scripts/Controllers", "*.js")
                .IncludeDirectory("~/Scripts/Factories", "*.js")
                .Include("~/Scripts/audioApp.js"));
        }
    }
}