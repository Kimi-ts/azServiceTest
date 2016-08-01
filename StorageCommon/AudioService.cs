using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageCommon
{
    public class AudioService
    {
        private ICloudFileUtility _fileUtility;
        private ICloudQueueUtility _queueUtility;
        private ICloudTableUtility _tableUtility;
        public AudioService(ICloudFileUtility fileUtility, ICloudQueueUtility queueUtility, ICloudTableUtility tableUtility)
        {
            _fileUtility = fileUtility;
            _queueUtility = queueUtility;
            _tableUtility = tableUtility;
        }

        public List<Audio> GetAll()
        {
            List<Audio> audios = _fileUtility.DownloadAllFiles();
            List<AudioEntity> audioEntities = _tableUtility.ReadAll();
            foreach (var song in audios)
            {
                int plays = audioEntities.Where(c => c.Title == song.Name).Select(p => p.Plays).FirstOrDefault();
                int skips = audioEntities.Where(c => c.Title == song.Name).Select(p => p.Skips).FirstOrDefault();
                string artist = audioEntities.Where(c => c.Title == song.Name).Select(p => p.Artist).FirstOrDefault();

                song.Artist = artist;
                song.Plays = plays;
                song.Skips = skips;
            }

            return audios;
        }

        public bool AddAudio(byte[] fileContent, string filename)
        {
            return _fileUtility.UploadFile(fileContent, filename);
        }

        public void IncPlays(string songName)
        {
            _queueUtility.AddMessage("plays", songName);
        }

        public void IncSkips(string songName)
        {
            _queueUtility.AddMessage("skips", songName);
        }

        public void UpdateAudioMetric(string metricname)
        {
            var songName = _queueUtility.GetMessage(metricname);
            if (!string.IsNullOrEmpty(songName))
            {
                if (metricname == "plays")
                {
                    _tableUtility.UpdateAudioData(true, false, songName);
                }
                else
                {
                    if (metricname == "skips")
                    {
                        _tableUtility.UpdateAudioData(false, true, songName);
                    }
                }
            }
        }
    }
}