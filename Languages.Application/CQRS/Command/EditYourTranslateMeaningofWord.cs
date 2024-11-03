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

    public class EditYourTranslateMeaningofWord : IRequest<Unit>
    {
        public string NewYourTranslate { get; set; }
        public UserBox Word { get; set; }
       

        internal class EditYourTranslateMeaningofWordHandler : IRequestHandler<EditYourTranslateMeaningofWord, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public EditYourTranslateMeaningofWordHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(EditYourTranslateMeaningofWord command, CancellationToken cancellationToken)
            {
                command.Word.YourAnswer = command.NewYourTranslate;

                _unitOfWork.userBox.Update(command.Word);
                _unitOfWork.Complete();

                return Unit.Value;
            }
        }
    }
}
