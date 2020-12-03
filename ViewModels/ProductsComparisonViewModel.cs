using CSE.BL;
using CSE.BL.ScannedData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ProductsComparisonViewModel : BaseViewModel
    {
        private readonly ProductsComparer _productsComparer;
        private ObservableCollection<ScannedItem> productsComparisonList;
        public ObservableCollection<ScannedItem> ProductsComparisonList
        {
            get { return productsComparisonList; }
            set
            {
                productsComparisonList = value;
                OnPropertyChanged(nameof(ProductsComparisonList));
            }
        }


        public ProductsComparisonViewModel(ScannedListManager productsToCompare)
        {
            if (productsToCompare.ScannedItems != null)
            {
                _productsComparer = new ProductsComparer();
                var list = new List<ScannedItem>
                    (productsToCompare.ScannedItems.Where(product => product.Discount == 0));
                
                foreach(ScannedItem product in list)    // set displayable price to price per quantity if the product is                                                       
                {                                       // priced per quantity
                    if (product.PricePerQuantity > 0)
                    {
                        product.Price = product.PricePerQuantity;
                    }
                }

                GetComparisonList(list);
            }
        }

        private async Task GetComparisonList(List<ScannedItem> list)
        {
            await _productsComparer.SearchForProductsWithBetterPrices(list);
            ProductsComparisonList = new ObservableCollection<ScannedItem>(list);
        }
    }
}
