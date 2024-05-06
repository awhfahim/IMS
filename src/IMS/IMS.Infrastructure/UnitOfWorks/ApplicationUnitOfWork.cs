using IMS.Application;
using IMS.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.UnitOfWorks;

public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
{
    protected ApplicationUnitOfWork(IApplicationDbContext dbContext) : base((DbContext)dbContext)
    {
    }
}