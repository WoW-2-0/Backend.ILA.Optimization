using Caching.SimpleInfra.Application.Common.Extensions;
using Caching.SimpleInfra.Application.Common.Identity.Services;
using Caching.SimpleInfra.Domain.Common.Query;
using Caching.SimpleInfra.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> GetById([FromQuery] FilterPagination paginationOptions, CancellationToken cancellationToken = default)
    {
        var specification = new QuerySpecification<User>(paginationOptions.PageSize, paginationOptions.PageToken);

        var result = await userService.GetAsync(specification, true, cancellationToken);
        return result.Any() ? Ok(result) : NotFound();
    }

    [HttpGet("{userId:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid userId)
    {
        var result = await userService.GetByIdAsync(userId);
        return result is not null ? Ok(result) : NotFound();
    }
}