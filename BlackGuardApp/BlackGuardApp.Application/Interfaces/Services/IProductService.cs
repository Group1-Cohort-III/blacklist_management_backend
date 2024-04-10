using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Domain;

namespace BlackGuardApp.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ApiResponse<GetProductsDto>> GetAllProducts(int PerPage, int Page);
        Task<ApiResponse<ProductResponseDto>> GetProductById(string id);
        Task<ApiResponse<ProductResponseDto>> AddProduct(ProductRequestDto productDto);
        Task<ApiResponse<ProductResponseDto>> UpdateProduct(string id, ProductRequestDto productDto);
        Task<ApiResponse<ProductResponseDto>> DeleteProduct(string id);
    }
}
