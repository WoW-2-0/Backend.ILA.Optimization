﻿using Caching.SimpleInfra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Caching.SimpleInfra.Persistence.DataContexts;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }
}