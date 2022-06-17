using Common;
using DataAccess;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Paula_Cardona_SWD6._3A.Controllers
{
    public class UsersController : Controller
    {
        private readonly FireStoreDataAccess fireStore;
        private readonly PubSubAccess pubsub;

        public UsersController(FireStoreDataAccess _firestore, PubSubAccess _pubsub)
        {
            pubsub = _pubsub;
            fireStore = _firestore;
        }


        [HttpGet]
        [Authorize]
        public IActionResult Send()
        {
            return View();
        }


        [HttpGet]
        [Authorize]
       public IActionResult SendDOCToPDF()
        {
           return View();
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            Common.User myUser = await fireStore.GetUser(User.Claims.ElementAt(4).Value);
            if (myUser == null)
                return View(new Common.User() { Email = User.Claims.ElementAt(4).Value });
            else return View(myUser);
        }

        [Authorize]
        public async Task<IActionResult> Register(User user)
        {
            user.Email = User.Claims.ElementAt(4).Value;
            await fireStore.AddUser(user);

            return RedirectToAction("Index");
        }


        [Authorize] [HttpPost]
        public async Task<IActionResult> Send(UserData msg, IFormFile attachment)
        {
            string bucketName = "programmingforthecloudpa";
            msg.Id = Guid.NewGuid().ToString();

            if (attachment != null)
            {
                //1. upload file on bucket
                var storage = StorageClient.Create();
                var bucket = storage.GetBucket(bucketName);

                //Uniform Bucket setup
                bucket.IamConfiguration.UniformBucketLevelAccess.Enabled = true;

                using (Stream myStream = attachment.OpenReadStream())
                {
                    storage.UploadObject(bucketName, msg.Id + Path.GetExtension(attachment.FileName), null, myStream);
                }

                //2. set msg.AttachmentUri to the uri to download it
                msg.AttachmentUri = $"https://storage.googleapis.com/{bucketName}/{ msg.Id + Path.GetExtension(attachment.FileName)}";
            }
            await fireStore.AddMessage(User.Claims.ElementAt(4).Value, msg);

            await pubsub.Publish(msg);


            return RedirectToAction("List");
        }

        [Authorize]
        [HttpPost]
        public IActionResult SendDOCToPDF(string attach)
        {
            RestAPI rest = new RestAPI();
            //rest.API(attach);

            return RedirectToAction("List");
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var messages = await fireStore.ListMessages(User.Claims.ElementAt(4).Value);
            return View(messages);
        }

        [Authorize]
        public async Task<IActionResult> ListCredit()
        {
            var credits = await fireStore.ListCredits(User.Claims.ElementAt(4).Value);
            return View(credits);
        }



    }
}
