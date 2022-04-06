using Common;
using DataAccess;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
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

        public UsersController(FireStoreDataAccess _firestore)
        {
            fireStore = _firestore;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Send()
        {
            return View();
        }

        [Authorize][HttpPost]
        public async Task<IActionResult> Send(UserData msg, IFormFile attachment)
        {
            string bucketName = "programmingforthecloudpa";
            msg.Id = Guid.NewGuid().ToString();

            if (attachment != null)
            {
                //1. upload file on bucket
                var storage = StorageClient.Create();
                using (Stream myStream = attachment.OpenReadStream())
                {
                    storage.UploadObject(bucketName, msg.Id + Path.GetExtension(attachment.FileName), null, myStream);
                }

                //2. set msg.AttachmentUri to the uri to download it
                msg.AttachmentUri = $"https://storage.googleapis.com/{bucketName}/{ msg.Id + Path.GetExtension(attachment.FileName)}";
            }
            await fireStore.AddMessage(User.Claims.ElementAt(4).Value, msg);
            return RedirectToAction("List");
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var messages = await fireStore.ListMessages(User.Claims.ElementAt(4).Value);
            return View(messages);
        }
    }
}
