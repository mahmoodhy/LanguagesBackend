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

    public class SetWordLearnedPermanently : IRequest<Unit>
    {
        public int WordId { get; set; }
        public string UserName { get; set; }

        internal class SetWordLearnedPermanentlyHandler : IRequestHandler<SetWordLearnedPermanently, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public SetWordLearnedPermanentlyHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(SetWordLearnedPermanently command, CancellationToken cancellationToken)
            {
                var item = await _unitOfWork.userBox.GetById(command.WordId);

                
               
                if (item.userName != command.UserName)
                    throw new Exception("خطا");
               
                item.LearnDate = DateTime.Now;

                item.working = 0;
                 
                    item.BoxDay =512;
                item.Priority = false;

                _unitOfWork.userBox.Update(item);
                _unitOfWork.Complete();

                return Unit.Value;
            }
        }
    }
}
