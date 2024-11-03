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
    public class FindWordInUserBox : IRequest<UserBoxView?>
    {
        public string Word { get; set; }
        public string UserName { get; set; }

        internal class FindWordInUserBoxHandler : IRequestHandler<FindWordInUserBox, UserBoxView?>
        {
            private readonly IUnitOfWork _unitOfWork;

            public FindWordInUserBoxHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<UserBoxView?> Handle(FindWordInUserBox query, CancellationToken cancellationToken)
            {
                var word = await _unitOfWork.userBoxView.GetUserBoxViewAsync(query.Word,query.UserName);
                return word;
            }
        }
    }
}
