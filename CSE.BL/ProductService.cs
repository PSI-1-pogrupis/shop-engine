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
        HttpClient _client;

        public ProductService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<ShoppingItemData>> GetProductData()
        {
            HttpResponseMessage response = await _client.GetAsync("https://localhost:44303/products");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<ShoppingItemData>>(data);
            }
            else return null;
        }
    }
}
