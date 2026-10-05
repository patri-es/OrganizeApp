using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Application.Product;
using OrganizeApp.Application.Product.DTOs;


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
        return Ok(products);
    }

    // Get Product by Id 
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id) 
    { 
        var product = await _productUseCases.GetProductById(id);


        if (product == null)
            return NotFound();

        return Ok(product);
    }
   
    // Add new product
    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProductDTO product)
    {
        await _productUseCases.CreateProduct(product);
        return Ok(product);
        //return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // Delete Product
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id) 
    {
        await _productUseCases.DeleteProduct(id);
        return Ok("Product successfully deleted.");
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO product)
    {
        await _productUseCases.UpdateProduct(id, product);
        return Ok(product);
    }

    #endregion

    #region category, stock & status

    // Change availability of product
    [HttpPost("{id:int}/activate")]
    public async Task<ActionResult> ActivateProduct(int id)
    {
        await _productUseCases.ActivateProduct(id);
        return Ok("Product successfully activated.");
    }

    [HttpPost("{id:int}/deactivate")]
    public async Task<ActionResult> DeactivateProduct(int id)
    {
        await _productUseCases.DeactivateProduct(id);
        return Ok("Product successfully deactivated.");
    }

    // Change Stock of product
    [HttpPost("{id:int}/stock/increase")]
    public async Task<ActionResult> IncreaseStock(int id, [FromBody] ChangeStockDTO dto)
    {
        await _productUseCases.IncreaseProductStock(id, dto.Quantity);
        return Ok("Product stock successfully increased.");
    }

    [HttpPost("{id:int}/stock/decrease")]
    public async Task<ActionResult> DecreaseStock(int id, [FromBody] ChangeStockDTO dto)
    {
        await _productUseCases.DecreaseProductStock(id, dto.Quantity);
        return Ok("Product stock successfully decreased.");
    }

    [HttpPost("{productId:int}/categories/{categoryId:int}")]
    public async Task<ActionResult> AsignProductCategory(int productId, int categoryId)
    {
        await _productUseCases.AssignProductCategory(productId, categoryId);
        return Ok("Category successfully assigned to product.");
    }

    [HttpDelete("{productId:int}/categories/{categoryId:int}")]
    public async Task<ActionResult> RemoveProductCategory(int productId, int categoryId)
    {
        await _productUseCases.RemoveProductCategory(productId, categoryId);
        return Ok("Category successfully removed to product.");
    }

    #endregion

}
