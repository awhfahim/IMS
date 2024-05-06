using IMS.Application;
using IMS.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.UnitOfWorks;

public class ApplicationUnitOfWork(IApplicationDbContext dbContext)
    : UnitOfWork((DbContext)dbContext), IApplicationUnitOfWork;