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
    public class GetUserBoxViewWordById : IRequest<UserBoxView>
    {
        public int WordId { get; set; }

        internal class GetUserBoxViewWordByIdHandler : IRequestHandler<GetUserBoxViewWordById, UserBoxView>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetUserBoxViewWordByIdHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<UserBoxView> Handle(GetUserBoxViewWordById query, CancellationToken cancellationToken)
            {
                var word = await _unitOfWork.userBoxView.GetById(query.WordId);
                return word;
            }
        }
    }
}
