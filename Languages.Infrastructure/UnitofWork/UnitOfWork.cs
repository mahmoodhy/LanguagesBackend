using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Repository;
using Core.Interfaces;
using Infrastructure.DataAccess;
using Infrastructure.Repository;


namespace Infrastructure.UnitofWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LeitnerBoxDbcontext _context;
        public UnitOfWork(LeitnerBoxDbcontext context)
        {
            _context = context;
            BoxData = new BoxDataRepository(_context);
            ApiDictionaryRoot = new ApiDictionaryRepository(_context);
            userBox= new UserBoxRepository(_context);
            userBoxView = new UserBoxViewRepository(_context);
            
        }
        public IBoxDataRepository BoxData { get; private set; }

        public IApiDictionaryRepository ApiDictionaryRoot { get; private set; }
        public IUserBoxRepository userBox { get; private set; }
        public IUserBoxViewRepository userBoxView { get; }



        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
