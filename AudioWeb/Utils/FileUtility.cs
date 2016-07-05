using AudioWeb.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioWeb.Utils
{
    public class FileUtility
    {
        private string _folderName = "music";

        public CloudStorageAccount storageAccount;

        public FileUtility(string accountName, string accountKey)
        {
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountName, accountKey);
            storageAccount = CloudStorageAccount.Parse(UserConnectionString);
        }

        public List<Audio> DownloadAllFiles()
        {
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            CloudFileShare fileShare = fileClient.GetShareReference(_folderName);

            List<IListFileItem> results = new List<IListFileItem>();
            FileContinuationToken token = null;
            do
            {
                FileResultSegment resultSegment = fileShare.GetRootDirectoryReference().ListFilesAndDirectoriesSegmented(token);
                results.AddRange(resultSegment.Results);
                token = resultSegment.ContinuationToken;
            }
            while (token != null);

            var audios = new List<Audio>();

            foreach (IListFileItem listItem in results)
            {
                var sasConstraints = new SharedAccessFilePolicy();
                sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
                sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(10);
                sasConstraints.Permissions = SharedAccessFilePermissions.Read;

                var sasFileToken = listItem.Share.GetSharedAccessSignature(sasConstraints);

                var audio = new Audio();
                audio.Name = ((CloudFile)listItem).Name;
                audio.Src = listItem.Uri.AbsoluteUri + sasFileToken;

                audios.Add(audio);
            }

            return audios;
        }
    }
}