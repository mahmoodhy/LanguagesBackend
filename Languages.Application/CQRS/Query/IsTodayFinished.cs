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
    public class IsTodayFinished : IRequest<bool>
    {
        public string UserName { get; set; }

        internal class IsTodayFinishedHandler : IRequestHandler<IsTodayFinished, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public IsTodayFinishedHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<bool> Handle(IsTodayFinished query, CancellationToken cancellationToken)
            {    
                var word = await _unitOfWork.userBox.IsTodayFinished(query.UserName);
                return word;
            }
        }
    }
}
