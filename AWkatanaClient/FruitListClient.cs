using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;

namespace AWkatanaClient
{
    public class FruitListClient
    {
        string _accessToken;
        Uri _baseRequestUri;

        public FruitListClient(Uri baseUri, string accessToken)
        {
            _accessToken = accessToken;
            _baseRequestUri = new Uri(baseUri, "api/Fruits/");
        }


        // Handy helper method to set the access token for each request:
        void SetClientAuthentication(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }


        public async Task<IEnumerable<Fruit>> GetFruitListAsync()
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await client.GetAsync(_baseRequestUri);
            }
            return await response.Content.ReadAsAsync<IEnumerable<Fruit>>();
        }
    }

    public class Fruit
    {
        // Add Key Attribute: Add Reference to the project: System.ComponentModel.DataAnnotations
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
