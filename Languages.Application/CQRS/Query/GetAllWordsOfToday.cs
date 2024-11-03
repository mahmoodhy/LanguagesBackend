using MediatR;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Application.CQRS.Query
{
    public class GetAllWordsOfToday : IRequest<List<UserBox>>
    {
        public int WordsCount { get; set; }
        public string UserName { get; set; }

        internal class GetAllWordsOfTodayHandler : IRequestHandler<GetAllWordsOfToday, List<UserBox>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllWordsOfTodayHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<List<UserBox>> Handle(GetAllWordsOfToday query, CancellationToken cancellationToken)
            {
                var wordsStartedBefor = await _unitOfWork.userBox.TodayIsStartedBeforeList(query.UserName);
                if (wordsStartedBefor.Count > 0)
                    return wordsStartedBefor;

                var IsTodayFinished =await _unitOfWork.userBox.IsTodayFinishedAsync(query.UserName);
                
                if (IsTodayFinished)
                    return new List<UserBox>();


                var words = await _unitOfWork.userBox.GetWordsForTodayAsync(query.WordsCount, query.UserName);

                return words;
            }
        }
    }
}
