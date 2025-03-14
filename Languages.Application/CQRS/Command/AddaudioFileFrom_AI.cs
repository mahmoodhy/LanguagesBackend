using MediatR;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net;
using Application.CQRS.Query;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net.Http.Json;

namespace Application.CQRS.Command
{
    public class AddaudioFileFrom_AI : IRequest<Unit>
    {
        public dictionaryRoot Word { get; set; }

        internal class AddaudioFileFrom_AIHandler : IRequestHandler<AddaudioFileFrom_AI, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public AddaudioFileFrom_AIHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(AddaudioFileFrom_AI command, CancellationToken cancellationToken)
            {
                try
                {
                    var mp3Data = await GetaudioFileFrom_UnrealSpeech(command.Word.word);
                    if (mp3Data == Array.Empty<byte>())
                        mp3Data = await GetaudioFileFrom_Playht(command.Word.word);
                    if (mp3Data == Array.Empty<byte>())
                        return Unit.Value;

                    var savePath = Path.Combine("audioFiles/", command.Word.word + "-playht.mp3");
                    // Save the byte array to a file
                    await File.WriteAllBytesAsync(savePath, mp3Data);
                    command.Word.audioFile = savePath.Substring(savePath.IndexOf('/'));
                    _unitOfWork.ApiDictionaryRoot.Update(command.Word);
                    _unitOfWork.Complete();

                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }

            }
            private async Task<byte[]> GetaudioFileFrom_Playht(string word)
            {
                var client = new HttpClient();
                var url = "https://api.play.ht/api/v2/tts/stream/";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("X-USER-ID", "LlJVpGvJuKO5u0dNmOkxhbqxIQY2");
                request.Headers.Add("AUTHORIZATION", "d360e210dd6b42aab87aff73a779db0c");
                request.Headers.Add("accept", " audio/mpeg");
                var content = JsonContent.Create(new
                {
                    text = word,
                    voice_engine = "Play3.0",
                    voice = "s3://voice-cloning-zero-shot/d9ff78ba-d016-47f6-b0ef-dd630f59414e/female-cs/manifest.json",
                    output_format = "mp3"
                });
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                request.Content = content;

                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                    return Array.Empty<byte>();
                var mp3Data = await response.Content.ReadAsByteArrayAsync();
                return mp3Data;
            }
            private async Task<byte[]> GetaudioFileFrom_UnrealSpeech(string word)
            {
                var voicelist = new List<string> { "Will", "Scarlett", "Amy", "Dan", "Liv" };
                Random random = new Random();
                int index = random.Next(voicelist.Count);
                string randomvoice = voicelist[index];
                var client = new HttpClient();
                var url = "https://api.v7.unrealspeech.com/stream";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", "Bearer lNLdhrpTsdpJny76vRIOBF4EgdHUReBFiYW4pXvwH1WJuIWeODttwh");
                var content = new StringContent("{\r\n    " +
                    "\"Text\":  \" " + word + " \",\r\n   " +
                    " \"VoiceId\": \"" + randomvoice + "\", \r\n    " +
                    "\"Bitrate\": \"192k\", \r\n    " +
                    "\"Speed\": \"0\", \r\n    " +
                    "\"Pitch\": \"1\", \r\n    " +
                    "\"Codec\": \"libmp3lame\"\r\n  }",
                    null,
                    "application/json"
                    );

                request.Content = content;

                var response = await client.SendAsync(request);



                if (!response.IsSuccessStatusCode)
                    return Array.Empty<byte>();

                var mp3Data = await response.Content.ReadAsByteArrayAsync();
                return mp3Data;
            }
        }
    }
}
