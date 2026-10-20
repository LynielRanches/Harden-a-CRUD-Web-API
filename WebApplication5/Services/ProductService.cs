using ProductsApi.Exceptions;
using ProductsApi.Models;

namespace ProductsApi.Services;

public class ProductService
{
    private readonly List<Product> _products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Keyboard",
            Sku = "KB001",
            Price = 1000,
            SalePrice = 900
        },
        new Product
        {
            Id = 2,
            Name = "Mouse",
            Sku = "MS001",
            Price = 500,
            SalePrice = 450
        }
    };

    private int _nextId = 3;

    public List<Product> GetAll()
    {
        return _products.ToList();
    }

    public Product GetById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id)
            ?? throw new NotFoundException(
                $"Product with ID {id} was not found.");
    }

    public Product Create(CreateProductRequest request)
    {
        EnsureSkuIsUnique(request.Sku);

        var product = new Product
        {
            Id = _nextId++,
            Name = request.Name,
            Sku = request.Sku,
            Price = request.Price,
            SalePrice = request.SalePrice
        };

        _products.Add(product);

        return product;
    }

    public Product Update(int id, UpdateProductRequest request)
    {
        var product = GetById(id);

        EnsureSkuIsUnique(request.Sku, id);

        product.Name = request.Name;
        product.Sku = request.Sku;
        product.Price = request.Price;
        product.SalePrice = request.SalePrice;

        return product;
    }

    public void Delete(int id)
    {
        var product = GetById(id);
        _products.Remove(product);
    }

    private void EnsureSkuIsUnique(string sku, int? excludeId = null)
    {
        if (_products.Any(p =>
            p.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase)
            && p.Id != excludeId))
        {
            throw new ConflictException(
                $"A product with SKU '{sku}' already exists.");
        }
    }
}