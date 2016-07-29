using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageCommon
{
    public interface ICloudQueueUtility
    {
        bool AddMessage(string queueName, string message);
        CloudQueue getQueue(string queueName);
    }
}