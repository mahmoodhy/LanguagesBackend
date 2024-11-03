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
    public class GetOnerandomWordforToday : IRequest<UserBox>
    {
        public int WordsCount { get; set; }
        public string UserName { get; set; }

        internal class GetOnerandomWordforTodayHandler : IRequestHandler<GetOnerandomWordforToday, UserBox>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetOnerandomWordforTodayHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<UserBox> Handle(GetOnerandomWordforToday query, CancellationToken cancellationToken)
            {    
                var words = await _unitOfWork.userBox.GetOneRandomWordforTodayAsync(query.UserName);
                return words;
            }
        }
    }
}
