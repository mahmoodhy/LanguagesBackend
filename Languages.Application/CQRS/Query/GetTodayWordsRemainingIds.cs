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
    public class GetTodayWordsRemainingIds : IRequest<List<int>>
    {
        public string UserName { get; set; }

        internal class GetTodayWordsRemainingIdsHandler : IRequestHandler<GetTodayWordsRemainingIds, List<int>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetTodayWordsRemainingIdsHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<List<int>> Handle(GetTodayWordsRemainingIds query, CancellationToken cancellationToken)
            {    
                var words = await _unitOfWork.userBox.GetTodayWordsRemainingIdsAsync(query.UserName);
                return words;
            }
        }
    }
}
