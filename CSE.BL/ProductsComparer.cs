using CSE.BL.Database;
using CSE.BL.Database.Models;
using CSE.BL.ScannedData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CSE.BL
{
    public class ProductsComparer
    {
        ShoppingItemRepository _repository;

        public ProductsComparer()
        {
            _repository = new ShoppingItemRepository(new MysqlShoppingItemGateway());
        }

        public void ChangeIncorrectNames(List<ScannedItem> scannedList)
        {
            List<ShoppingItemData> dataList = _repository.GetAll();
            const float distanceToleration = 0.6f;   // min distance between strings toleration percentage
                                                     // meaning: if 0.6 of strings names matches, they're considered the same
            int minDistance;

            /*
            var temp = new Dictionary<ShopTypes, decimal>();
            temp.Add(ShopTypes.NORFA, 0.89m);
            _repository.Insert(new ShoppingItemData("BANANAI", ShoppingList.UnitTypes.piece, temp));
            _repository.SaveChanges();
            */

            foreach (var scannedProduct in scannedList)
            {
                minDistance = (int)(scannedProduct.Name.Length * distanceToleration);
                foreach (var dataProduct in dataList)
                {
                    if (NamesMatch(minDistance, scannedProduct.Name, dataProduct.Name))
                    {
                        scannedProduct.Name = dataProduct.Name;
                    }
                }
            }
        }

        private bool NamesMatch(int minDistance, string name1, string name2)
        {
            name1 = name1.ToUpper();
            name2 = name2.ToUpper();
            return (name1.Length - minDistance >= name1.DistanceTo(name2));
        }

        public void SearchForProductsWithBetterPrices(List<ScannedItem> products)
        {

            // 1. find the product in the database

            // 2. compare the prices

            // 3. if there is a better priced product, set it in the property BetterPricedItem(only PriceString and Shop properties will be needed)

            var dataList = _repository.GetAll();

            foreach (var scannedProduct in products)
            {
                foreach (var dataProduct in dataList)
                {
                    if (scannedProduct.Name == dataProduct.Name)
                    {
                        foreach(KeyValuePair<ShopTypes, decimal> shopPrice in dataProduct.ShopPrices)
                        {
                            
                            if (shopPrice.Key != scannedProduct.Shop && shopPrice.Value < scannedProduct.Price)
                            {
                                scannedProduct.BetterPricedItem = new ScannedItem
                                {
                                    Price = shopPrice.Value,
                                    Shop = shopPrice.Key
                                };
                            }
                        }
                    }
                }
            }

        }
    }
}
