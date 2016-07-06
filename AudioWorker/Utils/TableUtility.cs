using AudioWorker.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioWorker.Utils
{
    public class TableUtility
    {
        private string _tableName = "MusicMetrics";

        public CloudStorageAccount storageAccount;

        public TableUtility(string accountName, string accountKey)
        {
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountName, accountKey);
            storageAccount = CloudStorageAccount.Parse(UserConnectionString);
        }

        //public List<AudioEntity> ReadAll()
        //{
        //    var audios = new List<AudioEntity>();

        //    CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

        //    CloudTable table = tableClient.GetTableReference(_tableName);
        //    TableQuery<AudioEntity> query = new TableQuery<AudioEntity>();

        //    foreach (AudioEntity audio in table.ExecuteQuery(query))
        //    {
        //        audios.Add(audio);
        //    }
        //    return audios;
        //}

        public void UpdateAudioData(bool isPlayed, bool isSkipped, string songTitle)
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
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
    }
}