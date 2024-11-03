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
    public class GetUserBoxWordById : IRequest<UserBox>
    {
        public int WordId { get; set; }

        internal class GetUserBoxWordByIdHandler : IRequestHandler<GetUserBoxWordById, UserBox>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetUserBoxWordByIdHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<UserBox> Handle(GetUserBoxWordById query, CancellationToken cancellationToken)
            {
                var word = await _unitOfWork.userBox.GetById(query.WordId);
                return word;
            }
        }
    }
}
