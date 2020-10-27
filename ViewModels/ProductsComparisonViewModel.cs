using CSE.BL.ScannedData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class ProductsComparisonViewModel : BaseViewModel
    {
        public ObservableCollection<ScannedItem> ProductsComparisonList { get; set; }

        public ObservableCollection<ScannedItem> ProductsWithBetterPrices { get; set; }

        public ProductsComparisonViewModel(ScannedListManager productsToCompare)
        {
            if (productsToCompare.ScannedItems != null)
            {
                ProductsComparisonList = new ObservableCollection<ScannedItem>
                    (productsToCompare.ScannedItems.Where(product => product.Discount == 0));
                SearchForProductsWithBetterPrices(ProductsComparisonList);
            }
        }

        public void SearchForProductsWithBetterPrices(ObservableCollection<ScannedItem> products)
        {
            // TODO: implement this

            // 1. find the product in the database

            // 2. compare the prices

            // 3. if there is a better priced product, set it in the property BetterPricedItem(only PriceString and Shop properties will be needed)

 
        }
    }
}
