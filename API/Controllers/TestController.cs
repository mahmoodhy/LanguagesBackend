using Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.Query;
using Application.CQRS.Command;

using AutoMapper;
using System.Net.Mail;

using Application.FrameWork;
using System.Net;
using System.ComponentModel;



namespace Languages.Controllers
{
    [ApiResultFilterAttribute]
    [Route("api/[controller]/[Action]")]
    [ApiController]
  // [Authorize]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationUser _user;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TestController(IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<HomeController> logger, 
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
        
        [HttpPost]
        public async Task<IActionResult> SendEmail()
        {
            try
            {
                // Create a new MailMessage object
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("leitnerboxmailserver@gmail.com"); // Your Gmail address
                mail.To.Add("baninshakery@gmail.com"); // Recipient's email address
                mail.Subject = "Test Email from C#"; // Subject of the email
                mail.Body = "Hello \r\n This is a test email sent from a C# application."; // Body of the email
                mail.IsBodyHtml = false; // Set to true if sending HTML content

                // Create a new SmtpClient object
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587); // Use port 587 for TLS
                smtpClient.Credentials = new NetworkCredential("leitnerboxmailserver@gmail.com", "rxei npnd hnpo ifiz"); // Your credentials
                smtpClient.EnableSsl = true; // Enable SSL

                // Send the email
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}");
            }

            return Ok("Email sent successfully!");
        }
        
    }
}
