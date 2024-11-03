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

    public class EditYourExampleMeaningofWord : IRequest<Unit>
    {
        public string NewYourExample { get; set; }
        public UserBox Word { get; set; }
       

        internal class EditYourExampleMeaningofWordHandler : IRequestHandler<EditYourExampleMeaningofWord, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public EditYourExampleMeaningofWordHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(EditYourExampleMeaningofWord command, CancellationToken cancellationToken)
            {
                command.Word.YourExample = command.NewYourExample;

                _unitOfWork.userBox.Update(command.Word);
                _unitOfWork.Complete();

                return Unit.Value;
            }
        }
    }
}
