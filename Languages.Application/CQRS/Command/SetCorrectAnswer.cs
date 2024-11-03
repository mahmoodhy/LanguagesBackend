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

    public class SetCorrectAnswer : IRequest<Unit>
    {
        public int WordId { get; set; }
        public string UserName { get; set; }

        internal class SetCorrectAnswerHandler : IRequestHandler<SetCorrectAnswer, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public SetCorrectAnswerHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(SetCorrectAnswer command, CancellationToken cancellationToken)
            {
                var item = await _unitOfWork.userBox.GetById(command.WordId);

                item.LearnDate = DateTime.Now;
               
                if (item.working <= 0 && item.BoxDay>0)
                    throw new Exception("خطا");
               
                if (item.working > 0)
                    item.working -= 1;
                if (item.working == 0)
                    item.BoxDay = item.BoxDay <= 0 ? 1 : item.BoxDay * 2;
                item.Priority = false;

                _unitOfWork.userBox.Update(item);
                _unitOfWork.Complete();

                return Unit.Value;
            }
        }
    }
}
