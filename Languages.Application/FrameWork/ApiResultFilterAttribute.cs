using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BadRequestResult = Microsoft.AspNetCore.Mvc.BadRequestResult;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;
using OkResult = Microsoft.AspNetCore.Mvc.OkResult;

namespace Application.FrameWork
{
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult)
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, okObjectResult.Value);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is OkResult okResult)
            {
                var apiResult = new ApiResult(true, ApiResultStatusCode.Success);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is BadRequestResult obj)
            {
                var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is BadRequestObjectResult badRequestObjectResult)
            {
                string fmessage = string.Empty;
                if (badRequestObjectResult.Value.GetType().GetProperty("Errors") is not null)
                {
                    System.Reflection.PropertyInfo pi = badRequestObjectResult.Value.GetType().GetProperty("Errors");
                    Dictionary<string, string[]> errors = (Dictionary<string, string[]>)(pi.GetValue(badRequestObjectResult.Value, null));

                    var message = badRequestObjectResult.ToString();
                    var errorMessage = errors.SelectMany(p => p.Value).Distinct();
                    message = string.Join(" \n ", errorMessage);
                    fmessage = message;
                }
                else
                {

                    var message = badRequestObjectResult.Value.ToString();
                    if (badRequestObjectResult.Value is SerializableError errors1)
                    {
                        var errorMessages = errors1.SelectMany(p => (string[])p.Value).Distinct();
                        message = string.Join(" | ", errorMessages);
                    }
                    fmessage = message;
                }
                var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest, fmessage);

                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is ContentResult contentResult)
            {
                var apiResult = new ApiResult(true, ApiResultStatusCode.Success, contentResult.Content);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is NotFoundResult notFoundResult)
            {
                var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is NotFoundObjectResult notFoundObjectResult)
            {
                var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is ObjectResult objectResult && objectResult.StatusCode == null
                && !(objectResult.Value is ApiResult))
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult.Value);
                context.Result = new JsonResult(apiResult);
            }else if (context.Result is ForbidResult)
            {
                var apiResult = new ApiResult(false, ApiResultStatusCode.UnAllowed);
                context.Result = new JsonResult(apiResult);
            }

            base.OnResultExecuting(context);
        }
    }
}
