using IMS.Domain;
using IMS.Domain.Repositories;

namespace IMS.Application;

public interface IApplicationUnitOfWork : IUnitOfWork
{
    IProductRepository Products { get; }
}

