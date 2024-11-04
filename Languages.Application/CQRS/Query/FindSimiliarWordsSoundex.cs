using MediatR;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net;
using System.Text.RegularExpressions;

namespace Application.CQRS.Query
{
    public class FindSimiliarWordsSoundex : IRequest<List<SimiliarWords>>
    {
        public string Word { get; set; }

        internal class FindSimiliarWordsSoundexHandler : IRequestHandler<FindSimiliarWordsSoundex, List<SimiliarWords>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILeitnerBoxDbcontext _dbContext;
            private readonly IMediator _mediator;


            public FindSimiliarWordsSoundexHandler(IUnitOfWork unitOfWork, ILeitnerBoxDbcontext dbContext, IMediator mediator)
            {
                _unitOfWork = unitOfWork;
                _dbContext = dbContext;
                _mediator = mediator;

            }
            public async Task<List<SimiliarWords>> Handle(FindSimiliarWordsSoundex query, CancellationToken cancellationToken)
            {
               
                
                var similiarwords = new List<SimiliarWords>();
                if (query.Word.Length > 3)
                {
                    similiarwords = await _dbContext.similiarWords.FromSqlRaw(
                         $"SELECT" +
                         $" 0 Boxid,EnglishWord word,PersianWords officialTranslate " +
                         $"FROM dbo.fulldic WHERE '{query.Word}' LIKE '%'+EnglishWord+'%' AND LEN(EnglishWord)>3").ToListAsync();
                }

                
                    var othersimiliarwords=await _dbContext.similiarWords.FromSqlRaw(
                           $"SELECT" +
                           $" 0 Boxid,EnglishWord word,PersianWords officialTranslate " +
                           $"FROM dbo.fulldic WHERE SOUNDEX(EnglishWord) = SOUNDEX('{query.Word}')").ToListAsync();
                
                similiarwords.AddRange(othersimiliarwords);
                
                similiarwords = similiarwords
                               .Where(s => s.word.Length >= 3 && !Regex.IsMatch(s.word, @"[^\w\s]") && !char.IsUpper(s.word[0])).ToList();

                if (similiarwords.Count() < 3)
                    foreach (var similiarword in similiarwords)
                        if (string.IsNullOrWhiteSpace(similiarword.officialTranslate))
                            similiarword.officialTranslate = await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = similiarword.word });
               

                return similiarwords;
            }
        }
    }
}
