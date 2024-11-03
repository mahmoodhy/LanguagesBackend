using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DataAccess;
using Core.Interfaces.Repository;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Repository
{
    public class UserBoxRepository : GenericRepository<UserBox>, IUserBoxRepository
    {

        public UserBoxRepository(LeitnerBoxDbcontext context) : base(context)
        {

        }
        public async Task<List<UserBox>> TodayIsStartedBeforeList(string username)
        {
            return await _context.userBox.Where(x => x.working > 0 && x.userName.Equals(username)).ToListAsync();
        }
        public async Task<bool> IsTodayFinishedAsync(string username)
        {
            DateTime now = DateTime.Now;
            var hasLearnDateInRange = await _context.userBox
                    .Where(x => x.LearnDate.AddDays(1) > now && x.userName.Equals(username))
                    .AnyAsync();

            var todayStartedList = await TodayIsStartedBeforeList(username);

            return hasLearnDateInRange && todayStartedList.Any();
        }
        public async Task<List<UserBox>> GetWordsFromExistBoxes(string username, int Wordcount)
        {
            var Allboxes = await _context.userBox.Where(x => x.BoxDay >= 0 && x.BoxDay < 500 && x.LearnDate.AddDays(x.BoxDay) <= DateTime.Now && x.userName.Equals(username)).ToListAsync();

            if (Wordcount > Allboxes.Count)
                Allboxes.AddRange(await _context.userBox.Where(x => x.userName == username && x.BoxDay < 0).Take(Wordcount - Allboxes.Count).ToListAsync());
            if (Wordcount * 2 < Allboxes.Count)
                Allboxes = Allboxes.Take(Wordcount * 2).ToList();
            return Allboxes;
        }
        public async Task<List<UserBox>> GetNewWordsFortoday(int Wordcount, string username)
        {
            var existwordsId = await _context.userBox.Where(x => x.userName == username && x.BoxDay >= 0).Select(s => s.BoxId).ToListAsync();

            var mostPriority = await _context.Box.Where(x => x.Priority == 2 && !existwordsId.Contains(x.id)).Take(Wordcount).ToListAsync();
            if (mostPriority.Count < Wordcount)
                mostPriority.AddRange(await _context.Box.Where(x => x.Priority == 1 && !existwordsId.Contains(x.id)).Take(Wordcount - mostPriority.Count).ToListAsync());
            if (mostPriority.Count < Wordcount)
                mostPriority.AddRange(await _context.Box.Where(x => !existwordsId.Contains(x.id)).Take(Wordcount - mostPriority.Count).ToListAsync());

            var priority = mostPriority.Select(word => new UserBox
            {
                userName = username,
                BoxDay = 0,
                LearnDate = DateTime.Now,

                BoxId = word.id,
                working = 1
            }).ToList();

            return priority;
        }

        public async Task<List<UserBox>> GetWordsForTodayAsync(int count, string username)
        {
            var words = await GetWordsFromExistBoxes(username, count);
            //if (count * 2 - words.Count > count)
            //    words.AddRange(await GetNewWordsFortoday(count, username));
            //else if (count * 2 - words.Count > 0)
            //    words.AddRange(await GetNewWordsFortoday(count * 2 - words.Count, username));
            return words;
        }
        public async Task<UserBox> GetOneRandomWordforTodayAsync(string username)
        {
            return await _context.userBox.Where(x => x.userName == username && x.working > 0).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
        }
        public async Task<List<int>> GetTodayWordsRemainingIdsAsync(string username)
        {
            return await _context.userBox.Where(x => x.userName == username && x.working > 0).Select(s => s.Id).ToListAsync();
        }
        public async Task<bool> IsTodayFinished(string username)
        {
            var lastworddate = await _context.userBox.Where(x => x.userName.Equals(username)).MaxAsync(m => m.LearnDate);
            var result = (DateTime.Now.Day - lastworddate.Day);
            return result < 1 ? true : false;
        }
        public async Task<UserBox?> GetWordByBoxIdAsync(int wordBoxId)
        {
            var word = await _context.userBox.Where(x => x.BoxId == wordBoxId).FirstOrDefaultAsync();
            return word;
        }
        public async Task<UserBox?> FindWordAsync(string word, string UserName)
        {
            var FindingwordinBox = await _context.Box.Where(x => x.EnglishWord == word).FirstOrDefaultAsync();
            if (FindingwordinBox == null)
                return null;
            var Findingword = await _context.userBox.Where(x => x.BoxId == FindingwordinBox.id && x.userName == UserName).FirstOrDefaultAsync();
            return Findingword;
        }
    }
}
