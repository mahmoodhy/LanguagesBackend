using System.IO;
using AutoMapper;
using Identity;
using Languages.Controllers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationUser _user;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MoviesController(IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<HomeController> logger,
            IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

           // _user = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name).Result;

        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided or file is empty.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, file.FileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FilePath = filePath });
        }


        [HttpPost("extract-sentences")]
        public async Task<IActionResult> ExtractSentences(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided or file is empty.");
            }

            // Ensure the file is an SRT file
            if (Path.GetExtension(file.FileName)?.ToLower() != ".srt")
            {
                return BadRequest("Only .srt files are supported.");
            }

            var sentences = new List<string>();
            string subtitleText = string.Empty;
            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                   

                    while (!stream.EndOfStream)
                    {
                        var line = await stream.ReadLineAsync();

                        // Ignore sequence numbers and timestamps
                        if (string.IsNullOrWhiteSpace(line) ||
                            line.Contains("-->") ||
                            int.TryParse(line.Trim(), out _))
                        {
                            continue;
                        }

                        // Collect subtitle lines
                        subtitleText += line + " ";
                    }

                    // Clean up and split sentences
                    //foreach (var sentence in subtitleText.Split(new[] { '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries))
                    //{
                    //    sentences.Add(sentence.Trim());
                    //}
                }

                return Ok(subtitleText);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
