using Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.Query;
using Application.CQRS.Command;
using System.IO;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using Microsoft.AspNetCore.Cors;
using Application.FrameWork;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Permissions;


namespace Languages.Controllers
{
    [ApiResultFilterAttribute]
    [Route("api/[controller]/[Action]")]
    [ApiController]
   [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string _username;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<HomeController> logger, 
            IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.User.Identity.Name;
            

            //_username = "me";
        }

        [HttpPost]
        public async Task<IActionResult> StartNewDay(int wordcount, bool? Force)
        {
            var todaywords = await _mediator.Send(new GetTodayWordsRemainingIds() { UserName = _username });
            if (todaywords.Count > 0)
                return Ok(todaywords.Count);
            var isTodayFinished = await _mediator.Send(new IsTodayFinished() { UserName = _username });
            if (isTodayFinished && !(Force ?? false))
                return Ok(0);
            var newdaywors = await _mediator.Send(new GetAllWordsOfToday() { WordsCount = wordcount, UserName = _username });
            await _mediator.Send(new UpdateTodayWordsBoxNumber() { Words = newdaywors, UserName = _username });
            return Ok(newdaywors.Count);
        }
        [HttpPost]
        public async Task<IActionResult> GetTodayWordsRemainingIds()
        {

            var wordCount = await _mediator.Send(new GetTodayWordsRemainingIds() { UserName = _username });
            return Ok(wordCount);
        }
        [HttpPost]
        public async Task<IActionResult> IsTodayFinished()
        {
            var isTodayFinished = await _mediator.Send(new IsTodayFinished() { UserName = _username });
            return Ok(isTodayFinished);
        }

        [HttpPost("{wordId}")]
        public async Task<IActionResult> WordById(int wordId)
        {
            try
            {
                var userboxWord = await _mediator.Send(new GetUserBoxViewWordById() { WordId = wordId });
                var dictionaryWord = await _mediator.Send(new GetWordFromDictionaryById() { QuestionId = userboxWord.BoxId });
                if (dictionaryWord is null)
                {
                    dictionaryWord = await _mediator.Send(new GetWordFromApiDictionaryByWord() { Word = userboxWord.EnglishWord });
                    dictionaryWord.BoxId = userboxWord.BoxId;
                    if (dictionaryWord.GTAnswer is null)
                        dictionaryWord.GTAnswer = await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = userboxWord.EnglishWord });
                    if (dictionaryWord.word is null)
                        dictionaryWord.word = userboxWord.EnglishWord;
                    if (dictionaryWord.BoxId == 0)
                        dictionaryWord.BoxId = userboxWord.BoxId;

                }
                await _mediator.Send(new AddWordToDictionary() { Word = dictionaryWord });

                if (dictionaryWord.audioFile is not null)
                {
                    var file = Path.Combine("audioFiles/", dictionaryWord.audioFile.Split('/').Last().Replace("%20", ""));
                    if (!System.IO.File.Exists(file))
                        await _mediator.Send(new AddaudioFile() { Word = dictionaryWord });
                    if (!System.IO.File.Exists(file))
                        await _mediator.Send(new AddaudioFileFrom_Playht() { Word = dictionaryWord });
                }
                else
                {
                   await _mediator.Send(new AddaudioFileFrom_Playht() { Word = dictionaryWord });
                }
                if (dictionaryWord.GTAnswer is null)
                {
                    dictionaryWord.GTAnswer = await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = userboxWord.EnglishWord });
                    await _mediator.Send(new AddWordToDictionary() { Word = dictionaryWord });
                }
                var dictionaryWorddto = _mapper.Map<dictionaryRootDto>(dictionaryWord);
                dictionaryWorddto.YourTranslate = userboxWord.YourAnswer;
                dictionaryWorddto.YourExample = userboxWord.YourExample;
                dictionaryWorddto.DayNo = userboxWord.BoxDay;
                dictionaryWorddto.Farsi = userboxWord.PersianWords;
                dictionaryWorddto.userBoxid = userboxWord.Id;
                //var similiarWords = await _mediator.Send(new GetSimiliarWordsinDataBase() { Word = userboxWord.EnglishWord });
                //dictionaryWorddto.similiarWords = similiarWords;
                return Ok(dictionaryWorddto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "->  " + ex.InnerException);
            }
        }
        [HttpPost]
        public async Task<IActionResult> NextRandomWord()
        {
            var randomWord = await _mediator.Send(new GetOnerandomWordforToday() { UserName = _username });
            return Ok(randomWord);
        }
        [HttpPost]
        public async Task<IActionResult> CorrectAnswer(int wordId)
        {
            await _mediator.Send(new SetCorrectAnswer() { WordId = wordId, UserName = _username });
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> WrongAnswer(int wordId)
        {
            await _mediator.Send(new SetWrongAnswer() { WordId = wordId, UserName = _username });
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> ThisWordIsLearned(int wordId)
        {
            await _mediator.Send(new SetThisWordIsLearned() { WordId = wordId, UserName = _username });
            return Ok();
        }
        [HttpPost]
        [SwaggerOperation(Summary = "Find a searched word", Description = "Returns details about the searched word.")]
        [SwaggerResponse(200, "Success", typeof(SearchedWord))]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IActionResult> FindSearchedWord(string word)
        {
            try
            {
                var resultword = new SearchedWord();
                var userboxviewWord = await _mediator.Send(new FindWordInUserBox() { Word = word, UserName = _username });
                if (userboxviewWord != null)
                {
                    if (string.IsNullOrWhiteSpace(userboxviewWord.PersianWords))
                        userboxviewWord.PersianWords = await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = userboxviewWord.EnglishWord });
                    
                    resultword = new SearchedWord()
                    {
                        word = userboxviewWord.EnglishWord,
                        Boxday = userboxviewWord.BoxDay,
                        officialTranslate = userboxviewWord.PersianWords ?? await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = userboxviewWord.EnglishWord }),
                        Boxid = userboxviewWord.BoxId
                    };
                    return Ok(resultword);
                }
                var boxWord = await _mediator.Send(new FindWordInBox() { Word = word });
                if (boxWord != null)
                {
                    if (string.IsNullOrWhiteSpace(boxWord.PersianWords))
                        boxWord.PersianWords = await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = boxWord.EnglishWord });

                    resultword = new SearchedWord()
                    {
                        word = boxWord.EnglishWord,
                        officialTranslate = boxWord.PersianWords ?? await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = boxWord.EnglishWord }),
                        Boxid = boxWord.id
                    };
                    return Ok(resultword);
                }

                resultword = await _mediator.Send(new FindSearchedWord() { Word = word });


                return Ok(resultword);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "->  " + ex.InnerException);
            }
        }
        [HttpPost("{word}")]
        public async Task<IActionResult> GetsimiliarwordsinDataBase(string word)
        {
            var similiarWords = await _mediator.Send(new GetSimiliarWordsinDataBase() { Word = word });
            return Ok(similiarWords);
        }
        [HttpPost("{word}")]
        public async Task<IActionResult> GetTranslation(string word)
        {
            try
            {
                var translate = await _mediator.Send(new GetWordFromGoogleTranslateByWord() { Word = word });
                return Ok(translate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "->  " + ex.InnerException);
            }
        }
        [HttpPost("{newMeaning},{wordId}")]
        public async Task<IActionResult> EditMainMeaningofWordin_UserBox(string newMeaning,int wordId)
        {
            try
            {
                var word=await _mediator.Send(new GetUserBoxWordById() { WordId = wordId });
                if (word == null)
                    throw new Exception("لغت پیدا نشد");
                if(word.userName!= _httpContextAccessor.HttpContext.User.Identity.Name)
                    throw new Exception("خطای دسترسی");

                await _mediator.Send(new EditMainMeaningofWord() { Word = word, NewWord= newMeaning });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "->  " + ex.InnerException);
            }
        }
        /// <summary>
        /// not complete
        /// </summary>
        /// <param name="newMeaning"></param>
        /// <param name="wordId"></param>
        /// <returns></returns>
        [HttpPost("{newMeaning},{wordId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> EditMainMeaningofWordin_Box(string newMeaning, int wordId)
        {
            try
            {
                //var word = await _mediator.Send(new GetBoxWordById() { WordId = wordId });
                //if (word == null)
                //    throw new Exception("لغت پیدا نشد");
                //if (word.userName != _httpContextAccessor.HttpContext.User.Identity.Name)
                //    throw new Exception("خطای دسترسی");

                //await _mediator.Send(new EditMainMeaningofWord() { Word = word });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "->  " + ex.InnerException);
            }
        }
        [HttpPost("{newTranslate},{wordId}")]
        public async Task<IActionResult> EditYourTranslateMeaningofWord(string newTranslate, int wordId)
        {
            try
            {
                var word = await _mediator.Send(new GetUserBoxWordById() { WordId = wordId });
                if (word == null)
                    throw new Exception("لغت پیدا نشد");
                if (word.userName != _httpContextAccessor.HttpContext.User.Identity.Name)
                    throw new Exception("خطای دسترسی");

                await _mediator.Send(new EditYourTranslateMeaningofWord() { Word = word, NewYourTranslate= newTranslate });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "->  " + ex.InnerException);
            }
        }
        [HttpPost("{newExample},{wordId}")]
        public async Task<IActionResult> EditYourExampleMeaningofWord(string newExample, int wordId)
        {
            try
            {
                var word = await _mediator.Send(new GetUserBoxWordById() { WordId = wordId });
                if (word == null)
                    throw new Exception("لغت پیدا نشد");
                if (word.userName != _httpContextAccessor.HttpContext.User.Identity.Name)
                    throw new Exception("خطای دسترسی");

                await _mediator.Send(new EditYourExampleMeaningofWord() { Word = word, NewYourExample= newExample });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "->  " + ex.InnerException);
            }
        }
        /// <summary>
        /// not complete
        /// </summary>
        /// <param name="newMeaning"></param>
        /// <param name="wordId"></param>
        /// <returns></returns>
        [HttpPost("{newMeaning},{wordId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditGTMeaningofWord(string newMeaning, int wordId)
        {
            try
            {
                var word = await _mediator.Send(new GetUserBoxWordById() { WordId = wordId });
                if (word == null)
                    throw new Exception("لغت پیدا نشد");
                if (word.userName != _httpContextAccessor.HttpContext.User.Identity.Name)
                    throw new Exception("خطای دسترسی");

                await _mediator.Send(new EditGTMeaningofWord() { Word = word,  });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "->  " + ex.InnerException);
            }
        }
        [HttpPost]        
        public async Task<IActionResult> GetaudioFileFrom_Playht(string word)
        {
            var file = await _mediator.Send(new GetaudioFileFrom_Playht() { Word = word });
            return Ok(file);
        }
        [HttpPost("{boxid}")]
        public async Task<IActionResult> AddWordtoUserBox(int boxid)
        {
            var file = await _mediator.Send(new AddWordtoUserBox() { WordId = boxid, userName= _username });
            return Ok(file);
        }
        [HttpPost]
        public async Task<IActionResult> GetUserBoxWordsStatistics()
        {
            var file = await _mediator.Send(new GetUserBoxWordsStatistics() {  userName = _username });
            return Ok(file);
        }
    }
}
