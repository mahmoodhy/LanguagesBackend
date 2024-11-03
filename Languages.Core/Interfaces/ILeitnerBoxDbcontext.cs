using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{ 
    public interface ILeitnerBoxDbcontext
    {
        DbSet<BoxData> Box { get; set; }
        DbSet<dictionaryRoot> ApiDictionaryRoot { get; set; }
        DbSet<SimiliarWords> similiarWords { get; set; }
        DbSet<SearchedWord> searchedWords { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
