

using System.Net;
using FluentValidation;
using GoodHamburguerApp.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguerApp.Api.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {

        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
           _logger.LogError(exception, "Ocorreu um erro não tratado: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path,
                Extensions = new Dictionary<string, object?>()
            };

           switch (exception)
            {
                // 1. Erros do FluentValidation (Input do usuário)
                case ValidationException validationException:
                    problemDetails.Title = "Erro de Validação";
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Detail = "Um ou mais campos da requisição estão inválidos.";
                    
                    var errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key, 
                            g => g.Select(x => x.ErrorMessage).ToArray()
                        );
                    
                    problemDetails.Extensions.Add("errors", errors);
                    break;

                // 2. Erros de Domínio 
                case DomainException domainException:
                    problemDetails.Title = "Regra de Negócio Violada";
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Detail = domainException.Message;
                    break;

                // 3. Erros Críticos 
                default:
                    problemDetails.Title = "Erro Interno no Servidor";
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Detail = "Ocorreu um erro inesperado em nosso sistema.";
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);


            return true;
        }
    }
}