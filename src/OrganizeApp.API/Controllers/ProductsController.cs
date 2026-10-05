using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Application.Product;
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Domain.Entities;
//using OrganizeApp.Infraestructure.


namespace OrganizeApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Products")]
public class ProductsController(ProductUseCases productUseCases) : ControllerBase
{
    // 
    private readonly ProductUseCases _productUseCases = productUseCases;
    
    #region endPoints

    // Get product list filter by Category, status and stock
    [HttpGet]
    public async Task<ActionResult<List<ProductDTO>>> GetProducts([FromQuery] FiltersProductDTO? filters)
    {
        var products = await _productUseCases.GetProducts(filters);

        if (products == null)
            return NotFound();

        return Ok(products);
    }

    // Get Product by Id 
    [HttpGet]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id) 
    { 
        var product = await _productUseCases.GetProductById(id);


        if (product == null)
            return NotFound();

        return Ok(product);
    }
   
    // Add new product
    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromQuery] CreateProductDTO product)
    {
        await _productUseCases.CreateProduct(product);

        return Ok(product);
    }

    // Delete Product
    public async Task<ActionResult> DeleteProduct([FromQuery] int id) 
    {
        await _productUseCases.DeleteProduct(id);

        return Ok();
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateProduct(int id, ProductDTO product) 
    {
        var produtSaved = await _productUseCases.GetProductById(id);
        bool productChanged = false;

        if (produtSaved == null)
            return NotFound(produtSaved);

        if (produtSaved.Name != product.Name)
        {
            await _productUseCases.ChangeProductName(id, produtSaved.Name);
            productChanged = true;
        }
        if (produtSaved.Description != null && produtSaved.Description != product.Description)
        {
            await _productUseCases.ChangeProductDescription(id, produtSaved.Description);
            productChanged = true;
        }
        if (produtSaved.Price != product.Price)
        {
            await _productUseCases.ChangeProductPrice(id, produtSaved.Price);
            productChanged = true;
        }
        if (produtSaved.ImageUrl != null && produtSaved.ImageUrl != product.ImageUrl)
        {
            await _productUseCases.ChangeProductImage(id, produtSaved.ImageUrl);
            productChanged = true;
        }

        if (!productChanged)
            return BadRequest("The product could not be updated.");

        return Ok("Product successfully updated.");
    }

    #endregion

}
