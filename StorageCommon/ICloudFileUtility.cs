using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageCommon
{
    public interface ICloudFileUtility
    {
        List<Audio> DownloadAllFiles();
        bool UploadFile(byte[] fileBytes, string filename);
    }
}