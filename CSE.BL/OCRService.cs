using CSE.BL.ScannedData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSE.BL
{
    public class OCRService
    {
        private readonly string ocrRoute = "https://localhost:44317/ocr/read";

        public OCRService() { }

        public async Task<List<ScannedItem>> GetScannedItems(string path)
        {
            try
            {
                MultipartFormDataContent content = new MultipartFormDataContent();

                var file = File.ReadAllBytes(path);

                content.Add(new ByteArrayContent(file), "file", path);

                HttpResponseMessage response = await ServiceClient.httpClient.PostAsync(ocrRoute, content);

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
