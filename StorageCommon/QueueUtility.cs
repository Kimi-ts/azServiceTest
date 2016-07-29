using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageCommon
{
    public class QueueUtility: ICloudQueueUtility
    {
        //NOTE - queue names are lowercase

        private CloudStorageAccount _storageAccount;

        public QueueUtility(string accountName, string accountKey)
        {
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountName, accountKey);
            _storageAccount = CloudStorageAccount.Parse(UserConnectionString);
        }

        public bool AddMessage(string queueName, string message)
        {
            if (string.IsNullOrEmpty(queueName))
            {
                return false;
            }

            var audioQueue = getQueue(queueName);

            var messageText = StringUtility.GetBytes(message);
            audioQueue.AddMessage(new CloudQueueMessage(messageText));

            return true;
        }

        public CloudQueue getQueue(string queueName)
        {
            CloudQueueClient queueClient = _storageAccount.CreateCloudQueueClient();
            var audioQueue = queueClient.GetQueueReference(queueName);
            audioQueue.CreateIfNotExists();
            return audioQueue;
        }
    }
}