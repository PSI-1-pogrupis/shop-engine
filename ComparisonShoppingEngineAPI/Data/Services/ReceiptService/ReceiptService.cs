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
            var receiptDto = new List<GetReceiptDto>();
            foreach (var receiptItem in receipts)
            {
                var newReceipt = new GetReceiptDto { Id = receiptItem.ReceiptId, Shop = receiptItem.Shop.ShopName, Date = receiptItem.Date, Total = receiptItem.Total, Products = new List<GetReceiptProductDto>() };
                foreach (var product in receiptItem.ReceiptProducts)
                {
                    newReceipt.Products.Add(new GetReceiptProductDto { Name = product.Product.ProductName, Discount = product.Discount, Price = product.Price, PricePerQuantity = product.PricePerQuantity });
                }
                receiptDto.Add(newReceipt);
            }
            serviceResponse.Data = receiptDto;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetReceiptDto>>> Insert(AddReceiptDto receipt, int userId)
        {
            ServiceResponse<List<GetReceiptDto>> serviceResponse = new ServiceResponse<List<GetReceiptDto>>();

            Receipt newReceipt = new Receipt()
            {
                UserId = userId,
                Shop = _context.Shops.FirstOrDefault(x => x.ShopName == receipt.Shop),
                Date = DateTime.UtcNow,
                Total = receipt.Total
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
            var insertedList = _context.Receipts.Include(x => x.ReceiptProducts).Include(x => x.Shop).Where(x => x.ReceiptId == newReceipt.ReceiptId && x.UserId == userId).Select(p => _mapper.Map<Receipt>(p)).ToList();
            var receiptDto = new List<GetReceiptDto>();
            foreach (var receiptItem in insertedList)
            {
                var currentReceipt = new GetReceiptDto { Id = receiptItem.ReceiptId, Shop = receiptItem.Shop.ShopName, Date = receiptItem.Date, Total = receiptItem.Total, Products = new List<GetReceiptProductDto>() };
                foreach (var product in receiptItem.ReceiptProducts)
                {
                    currentReceipt.Products.Add(new GetReceiptProductDto { Name = product.Product.ProductName, Discount = product.Discount, Price = product.Price, PricePerQuantity = product.PricePerQuantity });
                }
                receiptDto.Add(currentReceipt);
            }
            serviceResponse.Data = receiptDto;
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

            var deletedReceipt = new GetReceiptDto { Id = receipt.ReceiptId, Shop = receipt.Shop.ShopName, Date = receipt.Date, Total = receipt.Total, Products = new List<GetReceiptProductDto>() };
            foreach (var product in receipt.ReceiptProducts)
            {
                deletedReceipt.Products.Add(new GetReceiptProductDto { Name = product.Product.ProductName, Discount = product.Discount, Price = product.Price, PricePerQuantity = product.PricePerQuantity });
            }

            serviceResponse.Data = deletedReceipt;
            serviceResponse.Message = "Receipt has been deleted!";
            return serviceResponse;
        }
    }
}
