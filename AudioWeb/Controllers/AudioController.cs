using Models;
using StorageCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AudioWeb.Controllers
{
    public class AudioController : ApiController
    {
        private FileUtility _audioStorage;
        private TableUtility _tableUtility;
        private QueueUtility _queueUtility;

        public AudioController()
        {
            _audioStorage = new FileUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");
            _tableUtility = new TableUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");
            _queueUtility = new QueueUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");
        }

        // GET: api/audio
        [HttpGet]
        public IEnumerable<Audio> Get()
        {
            List<Audio> audios = _audioStorage.DownloadAllFiles();
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

        // PUT: api/audio/IncPlays
        [HttpPut]
        public void IncPlays([FromBody]string songName)
        {
            _queueUtility.UpdatePlays(songName);
        }

        [HttpPut]
        public void IncSkips([FromBody]string songName)
        {
            _queueUtility.UpdateSkips(songName);
        }
    }
}