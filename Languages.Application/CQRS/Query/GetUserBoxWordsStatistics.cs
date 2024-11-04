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
    public class GetUserBoxWordsStatistics : IRequest<List<UserBoxStatistic>>
    {
        public string userName { get; set; }

        internal class GetUserBoxWordsStatisticsHandler : IRequestHandler<GetUserBoxWordsStatistics, List<UserBoxStatistic>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetUserBoxWordsStatisticsHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<List<UserBoxStatistic>> Handle(GetUserBoxWordsStatistics query, CancellationToken cancellationToken)
            {
                var word = await _unitOfWork.userboxstatistics.GetWordLearnedStatisticsAsync(query.userName);
                return word;
            }
        }
    }
}
