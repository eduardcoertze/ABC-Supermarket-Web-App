using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace _20112255.CLDV6212.BlobHandler
{
    public class BlobManager
    {
            private CloudBlobContainer blobContainer;
            public BlobManager(string ContainerName)
            {
                //check if the container name is null or empty
                if (string.IsNullOrEmpty(ContainerName))
                {
                    throw new ArgumentNullException("ContainerName", "Container name can't b empty");
                }
                try
                {
                    //get azure storage connection string
                    string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=20112255;AccountKey=1aPx8/DddWm4+gqjChMKQgaeaISnPaXs9v+G2npyQxS+1DkvaIRYU0w7DEz9IOSJ2D6IQH1YUYurD9k/Ye3/yQ==;EndpointSuffix=core.windows.net";
                    CloudStorageAccount storageAccount =
                   CloudStorageAccount.Parse(ConnectionString);
                    CloudBlobClient cloudBlobClient =
                   storageAccount.CreateCloudBlobClient();
                    blobContainer =
                   cloudBlobClient.GetContainerReference(ContainerName);
                    
                    if (blobContainer.CreateIfNotExists())
                    {
                        blobContainer.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                       );
                    }
                }
                catch (Exception ExceptionObj)
                {
                    throw ExceptionObj;
                }
            }
            //upload Blob 'Product Image'
            public string UploadFile(HttpPostedFileBase FileToUpload)
            {
                string AbsoluteUri;
                //check if posted file base is null or not
                if (FileToUpload == null || FileToUpload.ContentLength == 0)
                    return null;
                try
                {
                    string FileName = Path.GetFileName(FileToUpload.FileName);
                    //create a block blob
                    CloudBlockBlob blockBlob;
                    blockBlob = blobContainer.GetBlockBlobReference(FileName);
                    //set objects content type
                    blockBlob.Properties.ContentType = FileToUpload.ContentType;
                    //upload the blob
                    blockBlob.UploadFromStream(FileToUpload.InputStream);
                    //get the Uri
                    AbsoluteUri = blockBlob.Uri.AbsoluteUri;
                }
                catch (Exception ExceptionObj)
                {
                    throw ExceptionObj;
                }
                return AbsoluteUri;
            }
            //Delete Blob
            public bool DeleteBlob(string AbsoluteUri)
            {
                try
                {
                    Uri uriObj = new Uri(AbsoluteUri);
                    string BlobName = Path.GetFileName(uriObj.LocalPath);
                    //get blob reference
                    CloudBlockBlob blockblob =
                   blobContainer.GetBlockBlobReference(BlobName);
                    //delete blob from container
                    blockblob.Delete();
                    return true;
                }
                catch (Exception ExceptionObj)
                {
                    throw ExceptionObj;
                }
            }
            //Retrieve Blobs
            public List<string> BlobList()
            {
                List<string> _blobList = new List<string>();
                foreach (IListBlobItem item in blobContainer.ListBlobs())
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob _blobpage = (CloudBlockBlob)item;
                        _blobList.Add(_blobpage.Uri.AbsoluteUri.ToString());
                    }
                }
                return _blobList;
            }
        }
    }