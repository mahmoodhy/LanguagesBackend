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
    public class GetAIMeaningsByBoxId : IRequest<List<Aimeaning>>
    {
        public int BoxId { get; set; }

        internal class GetAIMeaningsByBoxIdHandler : IRequestHandler<GetAIMeaningsByBoxId, List<Aimeaning>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAIMeaningsByBoxIdHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<List<Aimeaning>> Handle(GetAIMeaningsByBoxId query, CancellationToken cancellationToken)
            {

                var aimeanings = await _unitOfWork.aimeanings.GetAIMeaningsByBoxIdAsync(query.BoxId);
                return aimeanings;
               
            }
        }
    }
}
