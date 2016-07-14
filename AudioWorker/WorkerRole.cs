using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using StorageCommon;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AudioWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private readonly TableUtility _tableUtility = new TableUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");
        private readonly QueueUtility _queueUtility = new QueueUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");

        public override void Run()
        {
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
            ServicePointManager.DefaultConnectionLimit = 12;

            bool result = base.OnStart();

            return result;
        }

        public override void OnStop()
        {
            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            CloudQueueMessage playsMsg = null;
            CloudQueueMessage skipsMsg = null;
            var playsQueue = _queueUtility.getQueue("plays");
            var skipsQueue = _queueUtility.getQueue("skips");
            while (!cancellationToken.IsCancellationRequested)
            {
                playsMsg = playsQueue.GetMessage();
                if (playsMsg != null)
                {
                    var songName = StringUtility.GetString(playsMsg.AsBytes);
                    _tableUtility.UpdateAudioData(true, false, songName);
                    playsQueue.DeleteMessage(playsMsg);

                }
                skipsMsg = skipsQueue.GetMessage();
                if (skipsMsg != null)
                {
                    var songName = StringUtility.GetString(skipsMsg.AsBytes);
                    _tableUtility.UpdateAudioData(false, true, songName);
                    skipsQueue.DeleteMessage(skipsMsg);
                }
                await Task.Delay(2000);
            }
        }
    }
}
