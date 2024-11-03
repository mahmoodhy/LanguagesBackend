using Core.Entities;
using MediatR;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Command
{

    public class UpdateTodayWordsBoxNumber : IRequest<Unit>
    {
        public List<UserBox> Words { get; set; }
        public string UserName { get; set; }

        internal class UpdateTodayWordsBoxNumberHandler : IRequestHandler<UpdateTodayWordsBoxNumber, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateTodayWordsBoxNumberHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(UpdateTodayWordsBoxNumber command, CancellationToken cancellationToken)
            {
                command.Words.ForEach(item =>
                              {
                                  item.working = 1;
                                  if (item.BoxDay < 0)
                                      item.BoxDay = 0;
                              });

                _unitOfWork.userBox.UpdateRange(command.Words);
                _unitOfWork.Complete();

                return Unit.Value;
            }
        }
    }
}
