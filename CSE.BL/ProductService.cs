using CSE.BL.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSE.BL
{
    public class ProductService
    {
        private readonly string productRoute = "https://localhost:44317/product";

        public ProductService() { }

        public async Task<List<ShoppingItemData>> GetProductData()
        {
            HttpResponseMessage response = await ServiceClient.httpClient.GetAsync(productRoute);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ServiceResponse<List<ShoppingItemData>>>(data).Data;
            }
            else return null;
        }

        public async Task<ShoppingItemData> GetProductDataByName(string name)
        {
            HttpResponseMessage response = await ServiceClient.httpClient.GetAsync(productRoute + '/' + name);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ServiceResponse<ShoppingItemData>>(data).Data;
            }
            else return null;
        }
    }
}
