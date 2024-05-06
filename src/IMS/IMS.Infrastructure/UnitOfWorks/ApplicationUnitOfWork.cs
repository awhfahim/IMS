using IMS.Application;
using IMS.Domain.Repositories;
using IMS.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.UnitOfWorks;

public class ApplicationUnitOfWork(IApplicationDbContext dbContext, IProductRepository productRepository)
    : UnitOfWork(dbContext as DbContext), IApplicationUnitOfWork
{
    public IProductRepository Products { get; } = productRepository;
}