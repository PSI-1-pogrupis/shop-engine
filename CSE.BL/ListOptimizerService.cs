using CSE.BL.ShoppingList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSE.BL
{
    public class ListOptimizerService
    {
        private readonly string optimizeRoute = "https://localhost:44317/optimize";

        public ListOptimizerService() { }

        public async Task<ShoppingListManager> OptimizeList(ShoppingListManager list, List<ShopTypes> shopList, bool onlyReplaceUnspecified)
        {

            var items = list.ShoppingList.Select(x => new { x.Name, x.Amount, Shop = x.Shop.ToString() });
            var shops = shopList.Select(x => x.ToString());
            var content = new { shoppingList = items, allowedShops = shops, onlyReplaceUnspecified };
            var json = JsonConvert.SerializeObject(content);

            HttpResponseMessage response = await ServiceClient.httpClient.PostAsync(optimizeRoute, new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<List<ShoppingItem>>>(data);

                return new ShoppingListManager(serviceResponse.Data);
            }
            else return null;
        }
    }
}
