﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageCommon
{
    public class QueueUtility
    {
        //NOTE - queue names are lowercase
        private string _queueIncPlays = "plays";
        private string _queueIncSkips = "skips";

        private CloudStorageAccount _storageAccount;

        public QueueUtility(string accountName, string accountKey)
        {
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountName, accountKey);
            _storageAccount = CloudStorageAccount.Parse(UserConnectionString);
        }

        public bool UpdatePlays(string songName)
        {
            return UpdateTable(songName, true, false);
        }

        public bool UpdateSkips(string songName)
        {
            return UpdateTable(songName, false, true);
        }

        private bool UpdateTable(string songName, bool updatePlays, bool updateSkips)
        {
            var queueName = string.Empty;
            if (updatePlays)
            {
                queueName = _queueIncPlays;
            }
            else
            {
                if (updateSkips)
                {
                    queueName = _queueIncSkips;
                }
            }
            if (string.IsNullOrEmpty(queueName))
            {
                return false;
            }
            //CloudQueueClient queueClient = _storageAccount.CreateCloudQueueClient();
            //var audioQueue = queueClient.GetQueueReference(queueName);
            //audioQueue.CreateIfNotExists();

            var audioQueue = getQueue(queueName);

            var message = StringUtility.GetBytes(songName);
            audioQueue.AddMessage(new CloudQueueMessage(message));

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