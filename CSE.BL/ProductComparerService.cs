using CSE.BL.ScannedData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSE.BL
{
    public class ProductComparerService
    {
        private readonly string comparerRoute = "https://localhost:44317/ocr/compare";

        public ProductComparerService() { }

        public async Task<List<ScannedItem>> GetBetterList(List<ScannedItem> list)
        {
            try
            {
                var scannedList = list.Select(x => new { x.Name, Shop = x.Shop.ToString(), x.Price, x.Discount, x.PricePerQuantity }).ToArray();
                var json = JsonConvert.SerializeObject(scannedList);

                HttpResponseMessage response = await ServiceClient.httpClient.PostAsync(comparerRoute, new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var serviceResponse = JsonConvert.DeserializeObject<List<ScannedItem>>(data);

                    return serviceResponse;
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
