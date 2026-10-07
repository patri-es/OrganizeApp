using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Application.Product;
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Application.Product.Interfaces;


namespace OrganizeApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Products")]
public class ProductsController(IProductUseCases productUseCases) : ControllerBase
{
    // DI interface UseCases
    private readonly IProductUseCases _productUseCases = productUseCases;

    #region endPoints
    /// <summary>
    /// Get product list filter by Category, status and stock
    /// </summary>
    // GET /api/products
    [HttpGet]
    public async Task<ActionResult<List<ProductDTO>>> GetProducts([FromQuery] FiltersProductDTO? filters)
    {
        var products = await _productUseCases.GetProducts(filters);
        return Ok(products);
    }

    // Get Product by Id 
    // GET /api/products/id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id) 
    { 
        var product = await _productUseCases.GetProductById(id);


        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // Add new product
    // POST /api/products
    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProductDTO product)
    {
        await _productUseCases.CreateProduct(product);
        return Ok(product);
        //return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // Delete Product
    // DELETE /api/products/id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id) 
    {
        await _productUseCases.DeleteProduct(id);
        return Ok("Product successfully deleted.");
    }

    // Update Product
    // PATCH /api/products/id
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO product)
    {
        await _productUseCases.UpdateProduct(id, product);
        return Ok(product);
    }

    #endregion

    #region category, stock & status

    // Change availability of product
    // POST /api/products/id/activate
    [HttpPost("{id:int}/activate")]
    public async Task<ActionResult> ActivateProduct(int id)
    {
        await _productUseCases.ActivateProduct(id);
        return Ok("Product successfully activated.");
    }

    // POST /api/products/id/deactivate
    [HttpPost("{id:int}/deactivate")]
    public async Task<ActionResult> DeactivateProduct(int id)
    {
        await _productUseCases.DeactivateProduct(id);
        return Ok("Product successfully deactivated.");
    }

    // Change Stock of product
    // POST /api/products/id/stock/increase
    [HttpPost("{id:int}/stock/increase")]
    public async Task<ActionResult> IncreaseStock(int id, [FromBody] ChangeStockDTO dto)
    {
        await _productUseCases.IncreaseProductStock(id, dto.Quantity);
        return Ok("Product stock successfully increased.");
    }

    // POST /api/products/id/stock/decrease
    [HttpPost("{id:int}/stock/decrease")]
    public async Task<ActionResult> DecreaseStock(int id, [FromBody] ChangeStockDTO dto)
    {
        await _productUseCases.DecreaseProductStock(id, dto.Quantity);
        return Ok("Product stock successfully decreased.");
    }

    // Change categories of product
    // POST /api/products/id/categories/id
    [HttpPost("{productId:int}/categories/{categoryId:int}")]
    public async Task<ActionResult> AsignProductCategory(int productId, int categoryId)
    {
        await _productUseCases.AssignProductCategory(productId, categoryId);
        return Ok("Category successfully assigned to product.");
    }

    // DELETE /api/products/id/categories/id
    [HttpDelete("{productId:int}/categories/{categoryId:int}")]
    public async Task<ActionResult> RemoveProductCategory(int productId, int categoryId)
    {
        await _productUseCases.RemoveProductCategory(productId, categoryId);
        return Ok("Category successfully removed to product.");
    }

    #endregion

}
