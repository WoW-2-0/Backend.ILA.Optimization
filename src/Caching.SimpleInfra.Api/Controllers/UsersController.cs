using Caching.SimpleInfra.Application.Common.Identity.Services;
using Caching.SimpleInfra.Domain.Common.Query;
using Caching.SimpleInfra.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LocalIdentity.SimpleInfra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> Get([FromQuery] FilterPagination paginationOptions, CancellationToken cancellationToken = default)
    {
        var specification = new QuerySpecification<User>(paginationOptions.PageSize, paginationOptions.PageToken);
        specification.FilteringOptions.Add(user => user.FirstName.Length > 4);
        specification.FilteringOptions.Add(user => user.LastName.Length > 5);

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