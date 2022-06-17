using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Json;
using Microsoft.AspNetCore.Http;
using Google.Apis.Logging;
using RestSharp.Authenticators;
using Google.Cloud.Storage.V1;
using Google.Cloud.SecretManager.V1;
using Google.Protobuf;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Diagnostics.AspNetCore3;

namespace DataAccess
{
    public class RestAPI
    {
       public ILogger<RestAPI> _logger;
        public string AccessSecretVersion()
        {
            string projectId = "Project";
            string secretId = "secretkey_ouathAPI";
            string secretVersionId = "1";
            // Create the client.
            SecretManagerServiceClient client = SecretManagerServiceClient.Create();

            // Build the resource name.
            SecretVersionName secretVersionName = new SecretVersionName(projectId, secretId, secretVersionId);

            // Call the API.
            AccessSecretVersionResponse result = client.AccessSecretVersion(secretVersionName);

            // Convert the payload to a string. Payloads are bytes by default.
            String payload = result.Payload.Data.ToStringUtf8();
            return payload;
        }

        public void API(string attachment, [FromServices] IExceptionLogger exceptionLogger)
        {
            try
            {
                _logger.LogInformation("Accessing the restAPI");
                _logger.LogInformation("Getting the API Key from secret manager");
                var client = new RestClient("https://getoutpdf.com/api/convert/image-to-pdf");
                RestRequest request = new RestRequest();
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("api_key", AccessSecretVersion());
                request.AddParameter("image", attachment);
                request.Method = Method.Post;
                Task<RestResponse> t = client.ExecuteAsync(request);
                t.Wait();
                var convertedJPG = t.Result.Content;
                Byte[] bytes = File.ReadAllBytes(convertedJPG);
                String image = Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                exceptionLogger.Log(e);
            }
        }

    }
}




    
    

        



