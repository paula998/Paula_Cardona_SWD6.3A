using Google.Cloud.Firestore;
using System;

namespace Common
{
    [FirestoreData]
    public class UserData
    {

    [FirestoreProperty, ServerTimestamp]
    public Google.Cloud.Firestore.Timestamp DataSent { get; set; }

     [FirestoreProperty]
     public string Id { get; set; }

     [FirestoreProperty]
     public string AttachmentUri { get; set; }

    [FirestoreProperty]
     public string Message { get; set; }

    [FirestoreProperty]
    public string NumberOfCredits { get; set; }
    }

}


  

