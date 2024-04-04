using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackGuardApp.Domain;

namespace BlackGuardApp.Application.Interfaces
{
    public class IProductService
    {
        ApiResponse<GetItemsDto> GetAllItems(int PerPage, int Page);
        ApiResponse<ItemResponseDto> GetItemById(string id);
        ApiResponse<ItemResponseDto> AddItem(ItemRequestDto itemDto);
        ApiResponse<ItemResponseDto> UpdateItem(string id, ItemRequestDto itemDto);
        ApiResponse<ItemResponseDto> DeleteItem(string id);
    }
}
