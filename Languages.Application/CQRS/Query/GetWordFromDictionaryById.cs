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
    public class GetWordFromDictionaryById : IRequest<dictionaryRoot>
    {
        public int QuestionId { get; set; }

        internal class GetWordFromDictionaryByIdHandler : IRequestHandler<GetWordFromDictionaryById, dictionaryRoot>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetWordFromDictionaryByIdHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<dictionaryRoot> Handle(GetWordFromDictionaryById query, CancellationToken cancellationToken)
            {

                var dicword = await _unitOfWork.ApiDictionaryRoot.GetAllByQuestionId(query.QuestionId);
                return dicword;
            }
        }
    }
}
