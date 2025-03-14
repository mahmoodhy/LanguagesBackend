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
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Newtonsoft.Json;
using System.ComponentModel;


namespace Languages.Controllers
{
    [ApiResultFilterAttribute]
    [Route("api/[controller]/[Action]")]
    [ApiController]
   [Authorize]
    public class AIController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationUser _user;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AIController(IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<HomeController> logger, 
            IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            
            _user = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name).Result;

          
        }



        [HttpPost("{boxId}")]
        public async Task<IActionResult> GetAIMeanings(int boxId)
        {
            try
            {
                var aiMeanings = await _mediator.Send(new GetAIMeaningsByBoxId() {  BoxId = boxId });
                if (aiMeanings.Count > 0)
                {
                    var aiMeaningsresult = _mapper.Map<List<Content>>(aiMeanings);
                    return Ok(aiMeaningsresult);
                }
                var apiresult = await _mediator.Send(new GetWordmeaningsFromLLmaAiApi() {  BoxId = boxId });
                var contents=_mapper.Map<List<Aimeaning>>(apiresult);
                foreach (var content in contents)
                    content.BoxId = boxId;
                await _mediator.Send(new AddtoAIMeanings() { Contents = contents });
                return Ok(apiresult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "->  " + ex.InnerException);
            }
        }

        [HttpPost("{word}")]
        public async Task<IActionResult> Get_AI_LlmaWordMeanings(int boxId)
        {
            try
            {
              var apiresult= await _mediator.Send(new GetWordmeaningsFromLLmaAiApi() {  BoxId= boxId });
               // var response = JsonConvert.DeserializeObject<List<Content>>(apiresult.choices[0].message.content);
                return Ok(apiresult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "->  " + ex.InnerException);
            }
        }
        
    }
}
