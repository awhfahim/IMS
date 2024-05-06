namespace IMS.Application.Contracts.DTOs;

public record ProductDto(Guid CategoryId, string Name, uint BuyPrice, uint SellPrice, string Brand, string Size);