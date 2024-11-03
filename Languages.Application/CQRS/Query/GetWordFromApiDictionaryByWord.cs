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
    public class GetWordFromApiDictionaryByWord : IRequest<dictionaryRoot>
    {
        public string Word { get; set; }

        internal class GetWordFromApiDictionaryByWordHandler : IRequestHandler<GetWordFromApiDictionaryByWord, dictionaryRoot>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetWordFromApiDictionaryByWordHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<dictionaryRoot> Handle(GetWordFromApiDictionaryByWord query, CancellationToken cancellationToken)
            {

                dynamic _word;
                List<dictionaryRoot> dword = new List<dictionaryRoot>();

                var client = new HttpClient();
                var url = "https://api.dictionaryapi.dev/api/v2/entries/en/";
                var request = new HttpRequestMessage(HttpMethod.Get, url + query.Word);
                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                    return new dictionaryRoot();
                _word = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                dword = _word.ToObject<List<dictionaryRoot>>();

                return dword[0];
            }
        }
    }
}
