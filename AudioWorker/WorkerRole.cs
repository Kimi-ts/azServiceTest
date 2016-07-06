using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
//using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using StorageCommon;

namespace AudioWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private TableUtility _tableUtility;

        public override void Run()
        {
            //Trace.TraceInformation("AudioWorker is running");
            //System.Diagnostics.Debug.WriteLine("AudioWorker is running Hello World");
            _tableUtility = new TableUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");
            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            //Trace.TraceInformation("AudioWorker has been started");
            System.Diagnostics.Debug.WriteLine("on start hello");

            return result;
        }

        public override void OnStop()
        {
            //Trace.TraceInformation("AudioWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            //Trace.TraceInformation("AudioWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                //Trace.TraceInformation("Working");


                System.Diagnostics.Debug.WriteLine("working hello world");
                await Task.Delay(1000);
            }
        }
    }
}