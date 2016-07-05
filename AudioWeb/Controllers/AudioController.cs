using AudioWeb.Models;
using AudioWeb.Utils;
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

        public AudioController()
        {
            _audioStorage = new FileUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");
            _tableUtility = new TableUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Audio> Get()
        {
            List<Audio> audios = _audioStorage.DownloadAllFiles();
            List<AudioEntity> audioEntities = _tableUtility.ReadAll();
            foreach (var song in audios)
            {
                int plays = audioEntities.Where(c => c.Title == song.Name).Select(p => p.Plays).FirstOrDefault();
                int skips = audioEntities.Where(c => c.Title == song.Name).Select(p => p.Skips).FirstOrDefault();

                song.Plays = plays;
                song.Skips = skips;
            }

            //call this to inc plays
            //_tableUtility.UpdateAudioData(true, false, "09_royal_blood_ten_tonne_skeleton_myzuka.fm.mp3");
            return audios;
        }
    }
}