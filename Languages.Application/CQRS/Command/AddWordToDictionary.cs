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
    public class AddWordToDictionary : IRequest<Unit>
    {
        public dictionaryRoot Word { get; set; }

        internal class AddWordToDictionaryHandler : IRequestHandler<AddWordToDictionary, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public AddWordToDictionaryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

            }
            public async Task<Unit> Handle(AddWordToDictionary command, CancellationToken cancellationToken)
            {

                if (command.Word.audioFile is null && command.Word.phonetics is not null)
                {
                    foreach (var phonetic in command.Word.phonetics)
                        if (!string.IsNullOrWhiteSpace(phonetic.audio))
                        {
                            var name = Path.Combine("audioFiles/", phonetic.audio.Split('/').Last().Replace("%20", ""));

                            using (var webClient = new WebClient())
                            {
                                webClient.DownloadFile(phonetic.audio, name);
                                command.Word.audioFile = name.Substring(name.IndexOf('/'));
                            }

                        }
                }
                _unitOfWork.ApiDictionaryRoot.Update(command.Word);
                _unitOfWork.Complete();

                return Unit.Value;
            }
        }
    }
}
