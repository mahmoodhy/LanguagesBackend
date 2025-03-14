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

namespace Application.CQRS.Query
{
    public class GetaudioFileFrom_UnrealSpeech : IRequest<string>
    {
        public string Word { get; set; }

        internal class GetaudioFileFrom_UnrealSpeechHandler : IRequestHandler<GetaudioFileFrom_UnrealSpeech, string>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetaudioFileFrom_UnrealSpeechHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<string> Handle(GetaudioFileFrom_UnrealSpeech query, CancellationToken cancellationToken)
            {
                try
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
                        "\"Text\":  \" "+query.Word + " \",\r\n   " +
                        " \"VoiceId\": \""+ randomvoice + "\", \r\n    " +
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
                        return string.Empty;
                    var mp3file = await response.Content.ReadAsByteArrayAsync();
                    var base64Audio = Convert.ToBase64String(mp3file);
                    return $"data:audio/mp3;base64,{base64Audio}";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }
}
