using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageCommon
{
    public interface ICloudTableUtility
    {
        List<AudioEntity> ReadAll();
        void UpdateAudioData(bool isPlayed, bool isSkipped, string songTitle);
        bool AddAudioData(string songTitle, string artist, string fileName);
    }
}