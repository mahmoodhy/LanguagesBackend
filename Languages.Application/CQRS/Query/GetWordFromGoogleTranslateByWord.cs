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
                        var url = "https://one-api.ir/translate/?token=737633:647c7299736c4&action=google&source=en&lang=fa&q=";
                        var request = new HttpRequestMessage(HttpMethod.Post, url + query.Word);
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        var _wordfromgoogle = JsonConvert.DeserializeObject<GoogleTranslate>(await response.Content.ReadAsStringAsync());
                        
                    
                    return _wordfromgoogle.result??"";
            }
        }
    }
}
