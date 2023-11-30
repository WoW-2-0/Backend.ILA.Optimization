using Caching.SimpleInfra.Domain.Common.Query;
using Caching.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Api.Configurations;

var userQuerySpecification = new QuerySpecification<User>(100, 1);

userQuerySpecification.AddPredicate(user => user.FirstName.Contains("John"));
userQuerySpecification.AddPredicate(user => user.LastName.Contains("Doe"));
userQuerySpecification.AddOrderBy(user => user.FirstName);

var userQuerySpecificationB = new QuerySpecification<User>(100, 1);

userQuerySpecificationB.AddPredicate(user => user.FirstName.Contains("John"));
userQuerySpecificationB.AddPredicate(user => user.LastName.Contains("Doe"));
userQuerySpecificationB.AddOrderBy(user => user.FirstName);

var testA = userQuerySpecification.CacheKey;
var testB = userQuerySpecificationB.CacheKey;

var builder = WebApplication.CreateBuilder(args);
await builder.ConfigureAsync();

var app = builder.Build();

await app.ConfigureAsync();
await app.RunAsync();