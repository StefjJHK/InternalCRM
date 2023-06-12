using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using BIP.InternalCRM.Primitives.ErrorHandling;
using Microsoft.AspNetCore.Http;

namespace BIP.InternalCRM.Infrastructure;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ApiControllerBase : ControllerBase
{
    [FromServices]
    public required IMediator Mediator { get; init; }

    [FromServices]
    public required IMapper Mapper { get; init; }


    [NonAction]
    protected ObjectResult InternalServerError(Error error) => Problem(
        statusCode: StatusCodes.Status500InternalServerError,
        title: error);
}