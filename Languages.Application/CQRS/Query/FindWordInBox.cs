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
    public class FindWordInBox : IRequest<BoxData?>
    {
        public string Word { get; set; }

        internal class FindWordInBoxHandler : IRequestHandler<FindWordInBox, BoxData?>
        {
            private readonly IUnitOfWork _unitOfWork;

            public FindWordInBoxHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<BoxData?> Handle(FindWordInBox query, CancellationToken cancellationToken)
            {
                var word = await _unitOfWork.BoxData.FindWordAsync(query.Word);
                return word;
            }
        }
    }
}
