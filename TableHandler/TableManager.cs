using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace _20112255.CLDV6212.TableHandler
{
    public class TableManager
    {

        private CloudTable table;
        //method to check if table name is null
        public TableManager(string _CloudTableName)
        {
            if (string.IsNullOrEmpty(_CloudTableName))
            {
                throw new ArgumentNullException("Table", "Table name cannot be empty");
            }
            try
            {
                //Get azure table storage connection string
                string ConnectionString =
               "DefaultEndpointsProtocol=https;AccountName=20112255;AccountKey=1aPx8/DddWm4+gqjChMKQgaeaISnPaXs9v+G2npyQxS+1DkvaIRYU0w7DEz9IOSJ2D6IQH1YUYurD9k/Ye3/yQ==;EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount =
CloudStorageAccount.Parse(ConnectionString);

                //create table if it does not exist
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                table = tableClient.GetTableReference(_CloudTableName);
                table.CreateIfNotExists();
            }
            catch (StorageException StorageExceptionObj)
            {
                throw StorageExceptionObj;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        // Retrieve Products (Get > List)
        public List<T> RetrieveEntity<T>(String Query = null) where T : TableEntity,
       new()
        {
            try
            {
                TableQuery<T> DataTableQuery = new TableQuery<T>();
                if (!string.IsNullOrEmpty(Query))
                {
                    DataTableQuery = new TableQuery<T>().Where(Query);
                }
                IEnumerable<T> IDataList = table.ExecuteQuery(DataTableQuery);
                List<T> DataList = new List<T>();
                foreach (var singleItem in IDataList)
                    DataList.Add(singleItem);
                return DataList;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        //Insert Products
        public void InsertEntity<T>(T entity, bool forInsert = true) where T :
       TableEntity, new()
        {
            try
            {
                if (forInsert)
                {
                    var InsertOperation = TableOperation.Insert(entity);
                    table.Execute(InsertOperation);
                }
                else
                {
                    var InsertOrReplaceOperation =
                   TableOperation.InsertOrReplace(entity);
                    table.Execute(InsertOrReplaceOperation);
                }
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        //Delete Products
        public bool DeleteEntity<T>(T entity) where T : TableEntity, new()
        {
            try
            {
                var DeleteOperation = TableOperation.Delete(entity);
                table.Execute(DeleteOperation);
                return true;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

    }
}