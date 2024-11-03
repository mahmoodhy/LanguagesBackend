using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Repository;

namespace Core.Interfaces
{
       public interface IUnitOfWork : IDisposable
    {
        IBoxDataRepository BoxData { get; }
        IApiDictionaryRepository ApiDictionaryRoot { get; }
        IUserBoxRepository userBox { get; }
        IUserBoxViewRepository userBoxView { get; }
        //newline
        int Complete();
    }
}

