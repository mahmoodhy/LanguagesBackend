using MediatR;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Application.CQRS.Query
{
    public class GetSimiliarWordsinDataBase : IRequest<List<SimiliarWords>>
    {
        public string Word { get; set; }


        internal class GetSimiliarWordsinDataBaseHandler : IRequestHandler<GetSimiliarWordsinDataBase, List<SimiliarWords>>
        {
            private readonly IMediator _mediator;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILeitnerBoxDbcontext _dbContext;

            public GetSimiliarWordsinDataBaseHandler(IUnitOfWork unitOfWork, ILeitnerBoxDbcontext dbContext, IMediator mediator)
            {
                _unitOfWork = unitOfWork;
                _dbContext = dbContext;
                _mediator = mediator;
            }
            public async Task<List<SimiliarWords>> Handle(GetSimiliarWordsinDataBase query, CancellationToken cancellationToken)
            {
                //var similiarwords = await _dbContext.similiarWords.FromSqlRaw(
                //    $"SELECT Top(5) " +
                //    $" id Boxid,EnglishWord word,PersianWords officialTranslate " +
                //    $"FROM LeitnerBox.Box WHERE SOUNDEX(EnglishWord) = SOUNDEX('{query.Word}')").ToListAsync();

                var similiarwords = await _dbContext.similiarWords.FromSqlRaw(
                   
                    $"EXEC dbo.FindSimiliarWords @CompareWord = N'{query.Word}'").ToListAsync();

                foreach (var similiarword in similiarwords)
                {
                    var youruserbox = await _unitOfWork.userBoxView.GetWordByBoxIdAsync(similiarword.Boxid);
                    if (youruserbox is not null)
                    {
                        if (string.IsNullOrWhiteSpace(youruserbox.PersianWords))
                        {
                            youruserbox.PersianWords = await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = similiarword.word });
                            similiarword.officialTranslate = youruserbox.PersianWords;
                            var box =await _unitOfWork.BoxData.GetById(youruserbox.BoxId);
                            box.PersianWords= youruserbox.PersianWords;
                            _unitOfWork.BoxData.Update(box);
                            _unitOfWork.Complete();
                        }
                        else
                            similiarword.officialTranslate = youruserbox.PersianWords;
                    }
                    else
                    {
                        var box = await _unitOfWork.BoxData.GetById(similiarword.Boxid);
                        if (box is not null)
                        {
                            if (string.IsNullOrWhiteSpace(box.PersianWords))
                            {
                                box.PersianWords = await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = similiarword.word });
                                similiarword.officialTranslate = box.PersianWords;
                                _unitOfWork.BoxData.Update(box);
                                _unitOfWork.Complete();
                            }
                            else
                                similiarword.officialTranslate = box.PersianWords;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(similiarword.officialTranslate))
                        similiarword.officialTranslate = await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = similiarword.word });

                }


                var result = similiarwords
                                .Where(s => s.word != query.Word).ToList();
                return result;


            }
        }
    }
}
