using AutoMapper;
using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces;
using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Common.Utilities;
using BlackGuardApp.Domain;
using BlackGuardApp.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BlackGuardApp.Application.ServicesImplementation

{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;


        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<ApiResponse<GetProductsDto>> GetAllProducts(int PerPage, int Page)
        {
            try
            {
                var products = _unitOfWork.ProductRepository.GetAllProductsAsync();
                var productsDtos = _mapper.Map<List<ProductResponseDto>>(products);
                var pagedProductDtos = Pagination<ProductResponseDto>.GetPager(
                    productsDtos,
                    PerPage,
                    Page,
                    product => product.Id,
                    product => product.Name
                    );
                var getProductsDto = new GetProductsDto
                {
                    Product = pagedProductDtos.Result.Data.ToList(),
                    PerPage = pagedProductDtos.Result.PerPage,
                    CurrentPage = pagedProductDtos.Result.CurrentPage,
                    TotalPageCount = pagedProductDtos.Result.TotalPageCount,
                    TotalCount = pagedProductDtos.Result.TotalCount
                };
                return new ApiResponse<GetProductsDto>(true, "products retrieved.", getProductsDto, new List<string>() { });
            }
            catch (Exception ex)
            {


                _logger.LogError(ex, "Error occurred while getting all products.");
                return new ApiResponse<GetProductsDto>(false, "Error occurred while getting all products.", 500, null, new List<string>() { ex.Message });
            }
        }


        public async Task<ApiResponse<ProductResponseDto>> GetProductById(string id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    return new ApiResponse<ProductResponseDto>(false, "product not found", 404, null, new List<string>());
                }
                var productDto = _mapper.Map<ProductResponseDto>(product);
                return new ApiResponse<ProductResponseDto>(true, "product found", 200, productDto, new List<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all items.");
                return new ApiResponse<ProductResponseDto>(false, "Error occurred while getting all products.", 500, null, new List<string> { ex.Message });
            }
        }


        public async Task<ApiResponse<ProductResponseDto>> AddProduct(ProductRequestDto productRequestDto)
        {
            try
            {
                var products = _mapper.Map<Product>(productRequestDto);
                await _unitOfWork.ProductRepository.AddProductAsync(products);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<ProductResponseDto>(products);
                return new ApiResponse<ProductResponseDto>(true, $"Successfully added a product", 201, responseDto, new List<string>());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a product");
                var errorList = new List<string>();
                return new ApiResponse<ProductResponseDto>(true, "Error occurred while adding a product", 500, null, errorList);
            }

        }
        public async Task<ApiResponse<ProductResponseDto>> UpdateProduct(string id, ProductRequestDto productRequestDto)
        {
            try
            {
                var existingProduct = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
                if (existingProduct == null)
                    return new ApiResponse<ProductResponseDto>(false, 400, $"product not found.");


                var product = _mapper.Map(productRequestDto, existingProduct);
                await _unitOfWork.ProductRepository.UpdateProductAsync(existingProduct);
                await _unitOfWork.SaveChangesAsync();


                var responseDto = _mapper.Map<ProductResponseDto>(product);
                return new ApiResponse<ProductResponseDto>(true, $"Successfully updated a product", 200, responseDto, new List<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the product");
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return new ApiResponse<ProductResponseDto>(true, "Error occurred while updating the product", 500, null, errorList);
            }
        }
        public async Task<ApiResponse<ProductResponseDto>> DeleteProduct(string id)
        {
            try
            {
                var existingProduct = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return new ApiResponse<ProductResponseDto>(false, 404, $"product not found.");


                }
                await _unitOfWork.ProductRepository.DeleteProductAsync(existingProduct);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<ProductResponseDto>(true, 200, $"productdeleted successfully .");
            }
            catch (Exception)
            {


                return new ApiResponse<ProductResponseDto>(false, 500, $"An error occured during this process.");
            }
        }


    }




}
