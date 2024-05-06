using System.Net;
using FluentValidation.Results;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace IMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult SendResponse(HttpStatusCode code, string message, object data)
        {
            return ResponseMaker(code, data, message);
        }

        protected IActionResult SendResponse(HttpStatusCode code, string message)
        {
            return ResponseMaker(code, null, message);
        }

        protected IActionResult SendResponse(HttpStatusCode code, IEnumerable<ValidationFailure> data)
        {
            var errors = data.Select(error =>
            {
                var errorInfo = new Dictionary<string, object>
                {
                    { "propertyName", error.FormattedMessagePlaceholderValues["PropertyName"] },
                    { "errorMessage", error.ErrorMessage },
                    { "attemptedValue", error.AttemptedValue }
                };
                if (error.FormattedMessagePlaceholderValues.TryGetValue("CollectionIndex", out var index))
                {
                    errorInfo["collectionIndex"] = index;
                }
                return errorInfo;
            });

            return ResponseMaker(code, errors, null);
        }


        protected IActionResult SendResponse(IEnumerable<ValidationFailure> data)
        {
            return SendResponse(HttpStatusCode.BadRequest, data);
        }


        protected async Task<IActionResult> SendResponseAsync<TEntity, TResponse>(HttpStatusCode code, TEntity data)
        {
            return ResponseMaker(code, await data.BuildAdapter().AdaptToTypeAsync<TResponse>(), null);
        }

        protected async Task<IActionResult> SendResponseAsync<TEntity, TResponse>(HttpStatusCode code,
            IEnumerable<TEntity> data)
        {
            return ResponseMaker(
                code,
                await Task.WhenAll(data.Select(x => x.BuildAdapter().AdaptToTypeAsync<TResponse>())),
                null
            );
        }


        protected IActionResult SendResponse(HttpStatusCode code, object data)
        {
            return ResponseMaker(code, data, null);
        }

        protected IActionResult SendResponse(HttpStatusCode code)
        {
            return ResponseMaker(code, null, null);
        }

        protected IActionResult SendResponse(IError error)
        {
            var code = error.ErrorType switch
            {
                ErrorType.Failure => HttpStatusCode.InternalServerError,
                ErrorType.Unexpected => HttpStatusCode.InternalServerError,
                ErrorType.Validation => HttpStatusCode.BadRequest,
                ErrorType.Conflict => HttpStatusCode.Conflict,
                ErrorType.NotFound => HttpStatusCode.NotFound,
                ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
                ErrorType.Forbidden => HttpStatusCode.Forbidden,
                _ => HttpStatusCode.InternalServerError,
            };

            return ResponseMaker(code, null, error.Reason);
        }


        protected IActionResult SendResponse(ISuccess success)
        {
            var code = success.SuccessType switch
            {
                SuccessType.Ok => HttpStatusCode.OK,
                SuccessType.Created => HttpStatusCode.Created,
                SuccessType.Deleted => HttpStatusCode.NoContent,
                _ => HttpStatusCode.OK,
            };

            return ResponseMaker(code, null, success.Reason);
        }

        protected virtual IActionResult ResponseMaker(HttpStatusCode code, object? data, string? message)
        {
            if (code == HttpStatusCode.NoContent) return NoContent();

            var castedCode = (int)code;
            var isSuccess = castedCode is >= 200 and < 300;
            var res = new ApiResponse
            {
                Success = isSuccess,
                Message = message ?? ReasonPhrases.GetReasonPhrase(castedCode),
                Code = castedCode,
                Data = data
            };

            return StatusCode(castedCode, res);
        }
    }
    public interface IError
    {
        ErrorType ErrorType { get; }
        string? Reason { get; }
    }

    public interface ISuccess
    {
        SuccessType SuccessType { get; }
        string? Reason { get; }
    }


    public enum ErrorType : byte
    {
        Failure = 1,
        Unexpected,
        Validation,
        Conflict,
        NotFound,
        Unauthorized,
        Forbidden,
    }


    public enum SuccessType : byte
    {
        Ok = 1,
        Deleted,
        Created
    }


    public class ApiResponse
    {
        public required bool Success { get; set; }
        public required int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
