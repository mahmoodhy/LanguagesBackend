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
    public class AddtoAIMeanings : IRequest<Unit>
    {
        public List<Aimeaning> Contents { get; set; }

        internal class AddtoAIMeaningsHandler : IRequestHandler<AddtoAIMeanings, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public AddtoAIMeaningsHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(AddtoAIMeanings command, CancellationToken cancellationToken)
            {
                try
                {
                    _unitOfWork.aimeanings.AddRange(command.Contents);
                    _unitOfWork.Complete();
                    return Unit.Value;

                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
        }
    }
}
