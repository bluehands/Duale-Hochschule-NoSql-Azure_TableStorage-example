using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace TableStorageSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=dhtablestoragesample;AccountKey=62RsWXw5raAo6IuudBH3NBM34SwVHWzRPzaf+oTmC9V1VWExlxBXc1/Md7y/W/GkJYNs1zfXVVJxNaIqDZc+9g==;EndpointSuffix=core.windows.net";
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("customers");
            table.CreateIfNotExists();

            var amm = new Customer {RowKey = "amm",PartitionKey="Karlsruhe", Name = "Aydin Mir Mohammadi", Street = "Hebelstrasse 15", ZipCode = "76133", City = "Karlsruhe"};

            table.Execute(TableOperation.Insert(amm));
            amm.Street = "Hebelstraße 15";
            table.Execute(TableOperation.Replace(amm));

            var q = from c in table.CreateQuery<Customer>() where c.City.Equals("Karlsruhe", StringComparison.InvariantCultureIgnoreCase) select c;
            var customer = q.FirstOrDefault();
        }
    }

    public class Customer : TableEntity
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
    }
}
