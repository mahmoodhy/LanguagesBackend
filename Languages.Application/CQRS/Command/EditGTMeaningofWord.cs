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

    public class EditGTMeaningofWord : IRequest<Unit>
    {
        public string NewWord { get; set; }
        public UserBox Word { get; set; }
       

        internal class EditGTMeaningofWordHandler : IRequestHandler<EditGTMeaningofWord, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public EditGTMeaningofWordHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(EditGTMeaningofWord command, CancellationToken cancellationToken)
            {
                //command.Word.Answer = command.NewWord;

                _unitOfWork.userBox.Update(command.Word);
                _unitOfWork.Complete();

                return Unit.Value;
            }
        }
    }
}
