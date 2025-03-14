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
using System.Net.Http.Json;

namespace Application.CQRS.Query
{
    public class GetWordFromGoogleTranslateByWord : IRequest<string>
    {
        public string Word { get; set; }

        internal class GetWordFromGoogleTranslateByWordHandler : IRequestHandler<GetWordFromGoogleTranslateByWord, string>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetWordFromGoogleTranslateByWordHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<string> Handle(GetWordFromGoogleTranslateByWord query, CancellationToken cancellationToken)
            {

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.one-api.ir/translate/v1/google/");
                request.Headers.Add("one-api-token", "737633:647c7299736c4");
                var content = new StringContent("{\r\n    \"source\" : \"en\",\r\n\"target\" : \"fa\",\r\n\"text\" : \" "+query.Word+" \"\r\n}", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                var _wordfromgoogle = JsonConvert.DeserializeObject<GoogleTranslate>(result);


                return _wordfromgoogle.result??"";
            }
        }
    }
}
