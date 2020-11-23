using AutoMapper;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.ReceiptService
{
    public partial class ReceiptService : IReceiptService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public ReceiptService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetReceiptDto>>> GetAll(int userId)
        {
            ServiceResponse<List<GetReceiptDto>> serviceResponse = new ServiceResponse<List<GetReceiptDto>>();
            List<Receipt> receipts = await _context.Receipts.Include(a => a.ReceiptProducts).Include(a => a.Shop).Where(a => a.UserId == userId).ToListAsync();
            serviceResponse.Data = receipts.Select(p => _mapper.Map<GetReceiptDto>(p)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetReceiptDto>>> Insert(AddReceiptDto receipt, int userId)
        {
            ServiceResponse<List<GetReceiptDto>> serviceResponse = new ServiceResponse<List<GetReceiptDto>>();

            Receipt newReceipt = new Receipt()
            {
                UserId = userId,
                ShopId = receipt.ShopId,
                Date = DateTime.UtcNow,
                Shop = _context.Shops.FirstOrDefault(x => x.ShopId == receipt.ShopId),
                Total = receipt.Total
            };

            await _context.Receipts.AddAsync(newReceipt);
            await _context.SaveChangesAsync();

            foreach (var product in receipt.Products)
            {
                await _context.ReceiptProducts.AddAsync(new ReceiptProduct()
                {
                    ReceiptId = newReceipt.ReceiptId,
                    ProductId = product.ProductId,
                    ProductPrice = product.Price,
                    Amount = product.Amount
                });
            }
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Receipts.Include(x => x.ReceiptProducts).Include(x => x.Shop).Where(x => x.ReceiptId == newReceipt.ReceiptId && x.UserId == userId).Select(p => _mapper.Map<GetReceiptDto>(p)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetReceiptDto>> Remove(int receiptId, int userId)
        {
            ServiceResponse<GetReceiptDto> serviceResponse = new ServiceResponse<GetReceiptDto>();

            Receipt receipt = await _context.Receipts.Include(a => a.ReceiptProducts).Include(a => a.Shop).Where(a => a.ReceiptId == receiptId && a.UserId == userId).FirstOrDefaultAsync();
            if(receipt == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Receipt has been already deleted!";
                return serviceResponse;
            }

            _context.Receipts.Remove(receipt);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetReceiptDto>(receipt);
            serviceResponse.Message = "Receipt has been deleted!";
            return serviceResponse;
        }
    }
}
