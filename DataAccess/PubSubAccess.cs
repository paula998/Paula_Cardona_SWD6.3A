using Common;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PubSubAccess
    {
        private string projectId { get; set; }
        public PubSubAccess(string _projectId)
        {
            projectId = _projectId;
        }
        public async Task<string> Publish(UserData userdat)
        {

            TopicName topic = new TopicName(projectId, "conversiontopic");

            PublisherClient client = PublisherClient.Create(topic);

            string mail_serialized = JsonConvert.SerializeObject(userdat);

            PubsubMessage message = new PubsubMessage
            {
                Data = ByteString.CopyFromUtf8(mail_serialized)

            };

            return await client.PublishAsync(message);

        }
    }
}
