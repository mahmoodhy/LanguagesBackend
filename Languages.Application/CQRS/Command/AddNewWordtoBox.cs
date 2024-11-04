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
    public class AddNewWordtoBox : IRequest<int>
    {
        public BoxData WordBox { get; set; }

        internal class AddNewWordtoBoxHandler : IRequestHandler<AddNewWordtoBox, int>
        {
            private readonly IUnitOfWork _unitOfWork;

            public AddNewWordtoBoxHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<int> Handle(AddNewWordtoBox command, CancellationToken cancellationToken)
            {
                try
                {
                    _unitOfWork.BoxData.Add(command.WordBox);
                    _unitOfWork.Complete();


                    return command.WordBox.id;
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
        }
    }
}
