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
        private AudioService _audioService;

        public AudioController()
        {
            ICloudFileUtility audioStorage = new FileUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");
            ICloudTableUtility tableUtility = new TableUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");
            ICloudQueueUtility queueUtility = new QueueUtility("zz1zz", "RDYGKOyoiv2nZMD8qXrIHY+gcLE2I5c3vnaPQBuubRNEt7+V/8/iTTwPgu3mQWiyfrpKSrIF2m6FgzaX4jB5Ow==");

            _audioService = new AudioService(audioStorage, queueUtility, tableUtility);
        }

        // GET: api/audio
        [HttpGet]
        public IEnumerable<Audio> Get()
        {
            List<Audio> audios = new List<Audio>();
            audios = _audioService.GetAll();

            return audios;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
                var artist = HttpContext.Current.Request.Form["artist"];
                var title = HttpContext.Current.Request.Form["title"];

                foreach (var stream in filesReadToProvider.Contents)
                {
                    if (stream.Headers.ContentType != null)
                    {
                        if (stream.Headers.GetValues("Content-Type").FirstOrDefault() == "audio/mp3")
                        {
                            var fileBytes = await stream.ReadAsByteArrayAsync();
                            var fileName = stream.Headers.ContentDisposition.FileName.ToString();
                            _audioService.AddAudio(fileBytes, fileName, artist, title);
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
            _audioService.IncPlays(songName);
        }

        [HttpPut]
        public void IncSkips([FromBody]string songName)
        {
            _audioService.IncSkips(songName);
        }
    }
}