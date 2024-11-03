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
    public class GetaudioFileFrom_Playht : IRequest<string>
    {
        public string Word { get; set; }

        internal class GetaudioFileFrom_PlayhtHandler : IRequestHandler<GetaudioFileFrom_Playht, string>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetaudioFileFrom_PlayhtHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<string> Handle(GetaudioFileFrom_Playht query, CancellationToken cancellationToken)
            {
                try
                {
                    var client = new HttpClient();
                    var url = "https://api.play.ht/api/v2/tts/stream/";
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Headers.Add("X-USER-ID", "LlJVpGvJuKO5u0dNmOkxhbqxIQY2");
                    request.Headers.Add("AUTHORIZATION", "caeb47b3745a423ca151d0b2d2224a10");
                    request.Headers.Add("accept", " audio/mpeg");
                    var content = JsonContent.Create(new
                    {
                        text = query.Word,
                        voice_engine = "Play3.0",
                        voice = "s3://voice-cloning-zero-shot/d9ff78ba-d016-47f6-b0ef-dd630f59414e/female-cs/manifest.json",
                        output_format = "mp3"
                    });
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    request.Content = content;

                    var response = await client.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                        throw new Exception("خطا");
                    
                    var mp3file= await response.Content.ReadAsByteArrayAsync();
                    var base64Audio = Convert.ToBase64String(mp3file);
                    return $"data:audio/mp3;base64,{base64Audio}";
                }
                catch (Exception ex) {  
                    throw new Exception(ex.Message, ex); 
                }
            }
        }
    }
}
