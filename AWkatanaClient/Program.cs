using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWkatanaClient
{
    class Program
    {
        private static string MyClientID = "123";
        private static string MySecretWord = "secret";
        private static string MyUserName = "arthur@startrek.com";
        private static string MyPassword = "enterprise";
        static void Main(string[] args)
        {

            Run_AndRefreshToken().Wait();
            GetProtectedResource_FruitsList().Wait();

            // Then Write Done...
            Console.WriteLine("");
            Console.WriteLine("Done! Press the Enter key to Exit...");
            Console.ReadLine();
            return;
        }

        static async Task Run_AndRefreshToken()
        {
            // Create an http client provider:
            string hostUriString = "http://localhost:8000";
            var provider = new ClientProvider(hostUriString);
            Dictionary<string, string> _tokenDictionary;
            string _refresh_token;

            try
            {
                // Pass in the credentials and retrieve a token dictionary:
                _tokenDictionary = await provider.GetTokenDictionary(MyUserName, MyPassword, MyClientID, MySecretWord);

                _refresh_token = _tokenDictionary["refresh_token"];

                // Write the contents of the dictionary:
                foreach (var kvp in _tokenDictionary)
                {
                    Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
                    Console.WriteLine("");
                }

                Console.WriteLine("Getting New access token by using the Refresh Token");

                Dictionary<string, string> _tokenDictionary2;
                _tokenDictionary2 = await provider.GetTokenDictionaryByRefreshToken(_refresh_token, MyClientID, MySecretWord);

                foreach (var kvp in _tokenDictionary)
                {
                    Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
                    Console.WriteLine("");
                }
            }
            catch (AggregateException ex)
            {
                // If it's an aggregate exception, an async error occurred:
                Console.WriteLine(ex.InnerExceptions[0].Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();
                return;
            }
            catch (Exception ex)
            {
                // Something else happened:
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();
                return;
            }


        }



        static async Task GetProtectedResource_FruitsList()
        {
            // Create an http client provider:
            string hostUriString = "http://localhost:8000";
            var provider = new ClientProvider(hostUriString);
            string _accessToken;
            Dictionary<string, string> _tokenDictionary;

            try
            {
                // Pass in the credentials and retrieve a token dictionary:
                _tokenDictionary = await provider.GetTokenDictionary(MyUserName, MyPassword, MyClientID, MySecretWord);
                _accessToken = _tokenDictionary["access_token"];

                // Write the contents of the dictionary:
                foreach (var kvp in _tokenDictionary)
                {
                    Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
                    Console.WriteLine("");
                }

                // Getting the protected resource
                var baseUri = new Uri(hostUriString);
                var fruitClient = new FruitListClient(baseUri, _accessToken);

                //Displaying results
                Console.WriteLine("Read all the fruits ...");
                var companies = await fruitClient.GetFruitListAsync();
                WriteList(companies);
      
            }
            catch (AggregateException ex)
            {
                // If it's an aggregate exception, an async error occurred:
                Console.WriteLine(ex.InnerExceptions[0].Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();
                return;
            }
            catch (Exception ex)
            {
                // Something else happened:
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();
                return;
            }
        }

        static void WriteList(IEnumerable<Fruit> fruitslist)
        {
            foreach (var fruit in fruitslist)
            {
                Console.WriteLine("Id: {0} Name: {1}", fruit.Id, fruit.Name);
            }
            Console.WriteLine("");
        }

    }
}
