using Google.Cloud.SecretManager.V1;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class SecretManager
    {
        public Secret GetSecret(string projectId = "vernal-layout-340609", string secretId = "secretkey_ouathlogin")
        {
            // Create the client.
            SecretManagerServiceClient client = SecretManagerServiceClient.Create();

            // Build the resource name.
            SecretName secretName = new SecretName(projectId, secretId);

            // Call the API.
            Secret secret = client.GetSecret(secretName);
            return secret;
        }

}
}
