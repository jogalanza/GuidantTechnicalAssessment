using System;
using System.Threading.Tasks;
using FunctionApp1.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionApp1
{
    public class LogFormLetterToStorage
    {
        [FunctionName("LogFormLetterToStorage")]
        public async Task Run([QueueTrigger("outputletter", Connection = "")]FormLetter myQueueItem,
            [Table("letters")] IAsyncCollector<LetterEntity> letterTableCollector,
            ILogger log)
        {
            try
            {
                //test
                string z = DateTime.Now.ToFileTime().ToString();
                log.LogInformation($"LogFormLetterToStorage.myQueueItem: {myQueueItem.Heading}, {myQueueItem.Likelihood}, {myQueueItem.Body}");

                //TODO map FormLetter message to LetterEntity type and save to table storage
                LetterEntity _letter = new LetterEntity()
                {
                    RowKey = DateTime.Now.ToFileTime().ToString(),
                    PartitionKey = "1",
                    Heading = myQueueItem.Heading,
                    Likelihood = myQueueItem.Likelihood,
                    Timestamp = DateTime.Now,
                    ExpectedDate = myQueueItem.ExpectedDate,
                    RequestedDate = myQueueItem.RequestedDate,
                    Body = myQueueItem.Body
                };

                await letterTableCollector.AddAsync(_letter);

                log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            }
            catch(Exception ex)
            {
                log.LogInformation($"***** ERROR *****: {ex.Message}");
            }
        }
    }

    public class LetterEntity : TableEntity {
        public string Heading { get; set; }
        public double Likelihood { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Body { get; set; }
    }
}
