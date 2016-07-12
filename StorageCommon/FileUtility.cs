﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageCommon
{
    public class FileUtility
    {
        private string _folderName = "music";

        private CloudStorageAccount _storageAccount;

        public FileUtility(string accountName, string accountKey)
        {
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountName, accountKey);
            _storageAccount = CloudStorageAccount.Parse(UserConnectionString);
        }

        public List<Audio> DownloadAllFiles()
        {
            CloudFileClient fileClient = _storageAccount.CreateCloudFileClient();
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