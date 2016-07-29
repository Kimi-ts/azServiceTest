using Models;
using StorageCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
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

        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            //string root = HttpContext.Current.Server.MapPath("~/App_Data");
           // var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                //// Read the form data.
                //await Request.Content.ReadAsMultipartAsync(provider);

                //// This illustrates how to get the file names.
                //foreach (MultipartFileData file in provider.FileData)
                //{
                //    //Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                //    //Trace.WriteLine("Server file path: " + file.LocalFileName);
                //    var c = file.Headers.ContentDisposition.FileName;
                //    var m = "Server file path: " + file.LocalFileName;
                //}

                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

                foreach (var stream in filesReadToProvider.Contents)
                {
                    if (stream.Headers.ContentType != null)
                    {
                        if (stream.Headers.GetValues("Content-Type").FirstOrDefault() == "audio/mp3")
                        {
                            var fileBytes = await stream.ReadAsByteArrayAsync();
                            var fileName = stream.Headers.ContentDisposition.FileName.ToString();
                            _audioStorage.UploadFile(fileBytes, fileName);
                        }
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
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