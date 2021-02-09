using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunctionApp1.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class CalculateDatesAndAmountsFunction
    {
        [FunctionName("CalculateDatesAndAmountsFunction")]
        public async Task Run([QueueTrigger("messagetomom", Connection = "")]MessageToMom myQueueItem, 
            [Queue("outputletter")] IAsyncCollector<FormLetter> letterCollector,
            ILogger log)
        {
            log.LogInformation($"{myQueueItem.Greeting} {myQueueItem.HowMuch} {myQueueItem.HowSoon}");
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            //TODO parse flattery list into comma separated string
            string _flattery = "";
            foreach(string s in myQueueItem.Flattery)
            {
                _flattery += $"{(_flattery != "" ? "," : "")} {s.Substring(0,1).ToUpper()}{(s.Length > 1 ? s.Substring(1).ToLower() : "")}";
            }
            //TODO populate Header with salutation comma separated string and "Mother"
            string _header = $"{myQueueItem.Greeting} {_flattery} Mother";

            //TODO calculate likelihood of receiving loan based on this decision tree
            // 100 percent likelihood (initial value) minus the probability expressed from the quotient of howmuch and the total maximum amount ($10000)
            double _likelihood = (double)(1 - (myQueueItem.HowMuch / 10000));

            //TODO calculate approximate actual date of loan receipt based on this decision tree
            // funds will be made available 10 business days after day of submission
            // business days are weekdays, there are no holidays that are applicable
            DateTime _date = CalcNextBizDay(myQueueItem.HowSoon == null ? myQueueItem.HowSoon.Value : DateTime.Now.AddDays(1), 10);

            //TODO use new values to populate letter values per the following:
            //Body:"Really need help: I need $5523.23 by December 12,2020"
            //ExpectedDate = calculated date
            //RequestedDate = howsoon
            //Heading=Greeting
            //Likelihood = calculated likelihood

            FormLetter _letter = new FormLetter()
            {
                Body = $"Really need help: I need ${myQueueItem.HowMuch.ToString()} by {(_date.ToLongDateString())}",
                ExpectedDate = _date,
                RequestedDate = myQueueItem.HowSoon.Value,
                Heading = _header,
                Likelihood = _likelihood                
            };

            await letterCollector.AddAsync(_letter);
        }

        public DateTime CalcNextBizDay(DateTime date, int offset = 1)
        {
            if (offset >= 1)
            {
                int x = 0;
                do
                {
                    date = date.AddDays(1);
                    if (isWeekEnd(date) == false) x++;
                } while (x < offset);
            }
            return date;
        }

        public bool isWeekEnd(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }


    }


    
}
