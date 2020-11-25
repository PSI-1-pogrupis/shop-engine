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
            List<Receipt> receipts = await _context.Receipts.Include(a => a.ReceiptProducts).ThenInclude(x => x.Product).Include(a => a.Shop).Where(a => a.UserId == userId).ToListAsync();
            serviceResponse.Data = receipts.Select(p => _mapper.Map<GetReceiptDto>(p)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetReceiptDto>> Insert(AddReceiptDto receipt, int userId)
        {
            ServiceResponse<GetReceiptDto> serviceResponse = new ServiceResponse<GetReceiptDto>();

            string shopName = receipt.Products.First().Shop;

            decimal totalPrice = 0m;

            foreach (var i in receipt.Products)
                totalPrice += i.Price;

            Receipt newReceipt = new Receipt()
            {
                UserId = userId,
                Shop = _context.Shops.FirstOrDefault(x => x.ShopName == shopName),
                Date = DateTime.UtcNow,
                Total = totalPrice
            };

            await _context.Receipts.AddAsync(newReceipt);
            await _context.SaveChangesAsync();

            foreach (var product in receipt.Products)
            {
                var productDb = await _context.Products.FirstOrDefaultAsync(x => x.ProductName == product.Name);
                await _context.ReceiptProducts.AddAsync(new ReceiptProduct()
                {
                    ReceiptId = newReceipt.ReceiptId,
                    ProductId = productDb.ProductId,
                    Discount = product.Discount,
                    PricePerQuantity = product.PricePerQuantity,
                    Product = productDb,
                    Price = product.Price
                });
            }
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetReceiptDto>(newReceipt);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetReceiptDto>> Remove(int receiptId, int userId)
        {
            ServiceResponse<GetReceiptDto> serviceResponse = new ServiceResponse<GetReceiptDto>();

            Receipt receipt = await _context.Receipts.Include(a => a.ReceiptProducts).ThenInclude(x => x.Product).Include(a => a.Shop).Where(a => a.ReceiptId == receiptId && a.UserId == userId).FirstOrDefaultAsync();
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
