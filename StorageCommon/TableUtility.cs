﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageCommon
{
    public class TableUtility: ICloudTableUtility
    {
        private string _tableName = "MusicMetrics";

        private CloudStorageAccount _storageAccount;

        public TableUtility(string accountName, string accountKey)
        {
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountName, accountKey);
            _storageAccount = CloudStorageAccount.Parse(UserConnectionString);
        }

        public List<AudioEntity> ReadAll()
        {
            var audios = new List<AudioEntity>();

            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(_tableName);
            TableQuery<AudioEntity> query = new TableQuery<AudioEntity>();

            foreach (AudioEntity audio in table.ExecuteQuery(query))
            {
                audios.Add(audio);
            }
            return audios;
        }



        public void UpdateAudioData(bool isPlayed, bool isSkipped, string songTitle)
        {
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(_tableName);
            TableOperation retrieveOperation = TableOperation.Retrieve<AudioEntity>(songTitle, songTitle);
            TableResult retrievedResult = table.Execute(retrieveOperation);

            AudioEntity updateEntity = (AudioEntity)retrievedResult.Result;

            if (updateEntity != null)
            {
                if (isPlayed)
                {
                    updateEntity.Plays++;
                }
                else
                {
                    if (isSkipped)
                    {
                        updateEntity.Skips++;
                    }
                    else
                    {
                        return;
                    }
                }
                TableOperation updateOperation = TableOperation.Replace(updateEntity);
                table.Execute(updateOperation);
            }
        }

        public bool AddAudioData(string songTitle, string artist, string fileName)
        {
            fileName = string.IsNullOrEmpty(fileName) ? "janneAhonen.mp3" : fileName.Replace("\"", "");
            AudioEntity newAudioData = new AudioEntity();
            newAudioData.Artist = artist;
            newAudioData.PartitionKey = fileName;
            newAudioData.RowKey = fileName;
            newAudioData.Skips = 0;
            newAudioData.Plays = 0;
            newAudioData.Title = songTitle;

            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(_tableName);

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(newAudioData);

            // Execute the insert operation.
            table.Execute(insertOperation);

            return false;
        }
    }
}