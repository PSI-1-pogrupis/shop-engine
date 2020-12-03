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
            try
            {

                var items = list.ShoppingList.Select(x => new { x.Name, x.Amount, Shop = x.Shop.ToString() });
                var shops = shopList.Select(x => x.ToString());
                var content = new { shoppingList = items, allowedShops = shops, onlyReplaceUnspecified };
                var json = JsonConvert.SerializeObject(content);

                HttpResponseMessage response = await ServiceClient.httpClient.PostAsync(optimizeRoute, new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    //var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<List<ShoppingItem>>>(data);
                    var definition = new[] { new { Name = "", Unit = "", Amount = 0.0, Shop = "", PricePerUnit = 0m } };
                    var serviceResponse = JsonConvert.DeserializeAnonymousType(data, definition);

                    List<ShoppingItem> parsedList = new List<ShoppingItem>();

                    foreach (var parsed in serviceResponse)
                    {
                        ShopTypes shop = (ShopTypes)Enum.Parse(typeof(ShopTypes), parsed.Shop);
                        UnitTypes unit = (UnitTypes)Enum.Parse(typeof(UnitTypes), parsed.Unit);

                        parsedList.Add(new ShoppingItem(parsed.Name, shop, parsed.PricePerUnit, parsed.Amount, unit));
                    }

                    return new ShoppingListManager(parsedList);
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
