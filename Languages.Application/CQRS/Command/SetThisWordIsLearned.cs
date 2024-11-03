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

    public class SetThisWordIsLearned : IRequest<Unit>
    {
        public int WordId { get; set; }
        public string UserName { get; set; }

        internal class SetThisWordIsLearnedHandler : IRequestHandler<SetThisWordIsLearned, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public SetThisWordIsLearnedHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(SetThisWordIsLearned command, CancellationToken cancellationToken)
            {
                var item =await _unitOfWork.userBox.GetById(command.WordId);

                item.LearnDate = DateTime.Now;

           
                    item.BoxDay = 500;
                    item.working-= 0;
                    item.Priority = false;

                    _unitOfWork.userBox.Update(item);
                _unitOfWork.Complete();

                return Unit.Value;
            }
        }
    }
}
