
using Newtonsoft.Json;
using Store.MVC.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Store.MVC.WebServiceAccess.Base
{
    public abstract class WebApiCallsBase : ClaimsPrincipal
    {
        protected readonly string ServiceAddress;
        protected readonly string CartBaseUri;
        protected readonly string CategoryBaseUri;
        protected readonly string CustomerBaseUri;
        protected readonly string ProductBaseUri;
        protected readonly string OrdersBaseUri;

 
    protected WebApiCallsBase(IWebServiceLocator settings)
        {
            ServiceAddress = settings.ServiceAddress;
            CartBaseUri = $"{ServiceAddress}api/ShoppingCart/";
            CategoryBaseUri = $"{ServiceAddress}api/category/";
            CustomerBaseUri = $"{ServiceAddress}api/customer/";
            ProductBaseUri = $"{ServiceAddress}api/product/";
            OrdersBaseUri = $"{ServiceAddress}api/orders/";
        }

        internal async Task<string> GetResponseAsync(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    SetTokenAccsess(client);
                    var response = await client.GetAsync(uri);
                    if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("You need to login");
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"The call to {uri} failed. Status code: {response.StatusCode}");
                    }
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                // Do something intelligent here 
                Console.WriteLine(ex);
                throw;
            }
        }

       
        protected async Task<string> PostRequestAsync(string uri, string json)
        {
            using (var client = new HttpClient())
            {
                SetTokenAccsess(client);
                var task = client.PostAsync(uri, CreateStringContent(json));
                return await ProcessResponse(uri, task);
            }
        }
        protected async Task<string> PutRequestAsync(string uri, string json)
        {
            using (var client = new HttpClient())
            {
                SetTokenAccsess(client);
                Task<HttpResponseMessage> task = client.PutAsync(uri, CreateStringContent(json));
                return await ProcessResponse(uri, task);
            }
        }

        protected async Task DeleteRequestAsync(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SetTokenAccsess(client);
                    Task<HttpResponseMessage> deleteAsync = client.DeleteAsync(uri);
                    var response = await deleteAsync;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(response.StatusCode.ToString());
                    }
                }
            }


            catch (Exception ex)
            {
                //Do something intelligent here
                Console.WriteLine(ex);
                throw;
            }
        }
        internal async Task<T> GetItemAsync<T>(string uri) where T : class, new()
        {
            try
            {
                var json = await GetResponseAsync(uri);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                //Do something intelligent here
                Console.WriteLine(ex);
                throw;
            }
        }
        internal async Task<IList<T>> GetItemListAsync<T>(string uri) where T : class, new()
        {
            try
            {
                return JsonConvert.DeserializeObject<IList<T>>(await GetResponseAsync(uri));
            }
            catch (Exception ex)
            {
                //Do something intelligent here
                Console.WriteLine(ex);
                throw;
            }
        }

        protected static async Task<string> ProcessResponse(
            string uri, Task<HttpResponseMessage> task)
        {
            try
            {
                var response = await task;
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("You need to login");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {

                    throw new WebException($"The Call to {uri} failed.  Status code: {response.StatusCode}", WebExceptionStatus.ProtocolError);
                    // throw new WebException("Unable to Found", );

                }
                else if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"The Call to {uri} failed.  Status code: {response.StatusCode}");
                }
               

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {             
                //Do something intelligent here
                Console.WriteLine(ex);
                throw;
                               
            }
        }
        protected StringContent CreateStringContent(string json)
        {
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        
        protected HttpClient SetTokenAccsess(HttpClient client)
        {
            IPrincipal threadPrincipal = Thread.CurrentPrincipal;
            if (threadPrincipal != null && threadPrincipal.Identity.AuthenticationType == "token")
            {
                var x = threadPrincipal.Identity;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", x.Name);
            }

                return client;
        }
    }
    
}
