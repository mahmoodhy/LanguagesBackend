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

namespace Application.CQRS.Query
{
    public class GetWordmeaningsFromLLmaAiApi : IRequest<List<Content>>
    {
        public int BoxId { get; set; }
        internal class GetWordmeaningsFromLLmaAiApiHandler : IRequestHandler<GetWordmeaningsFromLLmaAiApi, List<Content>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetWordmeaningsFromLLmaAiApiHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<List<Content>> Handle(GetWordmeaningsFromLLmaAiApi query, CancellationToken cancellationToken)
            {
                var word = await _unitOfWork.BoxData.GetById(query.BoxId);

                var client = new HttpClient();
                var url = "https://api.llama-api.com/chat/completions";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("AUTHORIZATION", "Bearer LA-264bb735dc8449be884e2130ff9f6f5d4f1beac2ef694c45ae62525f1c8d5ddb");
                var stringcontent = "{\"model\":\"llama3-8b\",\r\n    \"messages\": [\r\n            {\"role\": \"system\", \"content\": \"give me the all meaning of given word in persian language and tell me the example of using this word in english language for each meaning and Respond with a JSON array of objects, exactly in this format: Persian:the Persian meaning of the word,Meaning:the meaning of the word in English,Example:an example of the word in English. and no more words. i want this response for my program model and any extra words will be corrupt this\"},\r\n{\"role\": \"user\", \"content\": \"" + word.EnglishWord + "\"}\r\n    ],\r\n    \r\n    \"return_json\": true,\r\n   \"max_tokens\" :4096\r\n}";

                var content = new StringContent(stringcontent, null, "application/json");

                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var aioutput = await response.Content.ReadAsStringAsync();
                var aioutputModel = JsonConvert.DeserializeObject<LlmaMeaningWord>(aioutput);
                var result = JsonConvert.DeserializeObject<List<Content>>(aioutputModel.choices[0].message.content);
                return result;
            }
        }
    }
}
