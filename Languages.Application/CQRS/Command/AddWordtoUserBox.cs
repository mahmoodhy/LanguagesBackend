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
using Application.CQRS.Query;
using Microsoft.IdentityModel.Tokens;

namespace Application.CQRS.Command
{
    public class AddWordtoUserBox : IRequest<Unit>
    {
        public int WordId { get; set; }
        public string userName { get; set; }

        internal class AddWordtoUserBoxHandler : IRequestHandler<AddWordtoUserBox, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public AddWordtoUserBoxHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(AddWordtoUserBox command, CancellationToken cancellationToken)
            {
                try
                {
                    var newUserBox = new UserBox() { BoxDay = 0, BoxId = command.WordId, userName = command.userName };
                    _unitOfWork.userBox.Add(newUserBox);
                    _unitOfWork.Complete();


                    return Unit.Value;
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
        }
    }
}
