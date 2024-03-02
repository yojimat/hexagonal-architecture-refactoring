﻿using Hexagonal_Refactoring.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Repositories.Contexts;

public class EventContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<DbEvent> Events { get; init; } = null!;
    public DbSet<DbTicket> Tickets { get; init; } = null!;
    public DbSet<DbPartner> Partners { get; init; } = null!;
    public DbSet<DbCustomer> Customers { get; init; } = null!;
}