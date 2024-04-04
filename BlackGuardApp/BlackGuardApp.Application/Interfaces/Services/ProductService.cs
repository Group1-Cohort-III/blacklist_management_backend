using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Common.Utilities;
using BlackGuardApp.Domain;


namespace BlackGuardApp.Application.Interfaces.Services

{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
       // private readonly IMapper _mapper;
        //private readonly ILogger<ProductService> _logger;


        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }


        public ApiResponse<GetProductsDto> GetAllProducts(int PerPage, int Page)
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
                return new ApiResponse<GetProductsDto>(true, "products retrieved.", 200, getProductsDto);
            }
            catch (Exception ex)
            {


                _logger.LogError(ex, "Error occurred while getting all productss.");
                return new ApiResponse<GetProductsDto>(false, "Error occurred while getting all products.", 500, null, new List<string>() { ex.Message });
            }
        }


        public ApiResponse<ProductResponseDto> GetItemById(string id)
        {
            try
            {
                var item = _unitOfWork.ProductRepository.GetItemById(id);
                if (item == null)
                {
                    return new ApiResponse<ProductResponseDto>(false, "product not found", 404, null, new List<string>());
                }
                var itemDto = _mapper.Map<ProductResponseDto>(item);
                return new ApiResponse<ProductResponseDto>(true, "product found", 200, itemDto, new List<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all items.");
                return new ApiResponse<ProductResponseDto>(false, "Error occurred while getting all products.", 500, null, new List<string> { ex.Message });
            }
        }


        public ApiResponse<ProductResponseDto> AddItem(ProductRequestDto productRequestDto)
        {
            try
            {
                var items = _mapper.Map<Products>(productRequestDto);
                _unitOfWork.ProductsRepository.AddProduct(product);
                _unitOfWork.SaveChanges();


                var responseDto = _mapper.Map<ProductResponseDto>(items);
                return new ApiResponse<ProductResponseDto>(true, $"Successfully added an item", 201, responseDto, new List<string>());


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a product");
                var errorList = new List<string>();
                return new ApiResponse<ProductResponseDto>(true, "Error occurred while adding a product", 500, null, errorList);
            }


        }


        public ApiResponse<ProductResponseDto> UpdateItem(string id, ProductRequestDto itemRequestDto)
        {
            try
            {
                var existingItem = _unitOfWork.ProductsRepository.GetItemById(id);
                if (existingItem == null)
                    return new ApiResponse<ProductResponseDto>(false, 400, $"product not found.");


                var item = _mapper.Map(itemRequestDto, existingItem);
                _unitOfWork.ProductsRepository.Update(existingItem);
                _unitOfWork.SaveChanges();


                var responseDto = _mapper.Map<ProductResponseDto>(item);
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
        public ApiResponse<ProductResponseDto> DeleteItem(string id)
        {
            try
            {
                var existingProduct = _unitOfWork.ProductsRepository.GetItemById(id);
                if (existingProduct == null)
                {
                    return new ApiResponse<ProductResponseDto>(false, 404, $"product not found.");


                }
                _unitOfWork.ProductsRepository.Delete(existingProduct);
                _unitOfWork.SaveChanges();
                return new ApiResponse<ProductResponseDto>(true, 200, $"productdeleted successfully .");
            }
            catch (Exception)
            {


                return new ApiResponse<ProductResponseDto>(false, 500, $"An error occured during this process.");
            }
        }
    }
}
