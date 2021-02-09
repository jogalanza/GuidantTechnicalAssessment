using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionApp1.Messages;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Linq;

namespace FunctionApp1
{
    public class ApiFunction
    {
        [FunctionName("ApiFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("messagetomom")] IAsyncCollector<MessageToMom> letterCollector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            bool _success = false;
            string _message = "";

            //TODO model HttpRequest from fields of MessageToMom
            //Map new model values (from HttpRequest) to MessageToMom below

            MessageToMom msgToMom;

            if (req.Method == "POST")
            {
                string bodyStr = "";
                using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = reader.ReadToEnd();
                }

                msgToMom = JsonConvert.DeserializeObject<MessageToMom>(bodyStr);
            }
            else
            {
                var c = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(req.QueryString.ToString()));
                string _flattery = req.Query["flattery"];
                string _greeting = req.Query["greeting"];
                string _howMuch = req.Query["howmuch"];
                string _from = req.Query["from"];
                string _howSoon = req.Query["howsoon"];

                decimal.TryParse(_howMuch, out decimal n);

                msgToMom = new MessageToMom
                {
                    Flattery = _flattery == null ? null : _flattery.Split(',').ToList(),
                    Greeting = _greeting != null && _greeting != "" ? _greeting : "So Good To Hear From You",
                    HowMuch = n,
                    HowSoon = DateTime.UtcNow.AddDays(1),
                    From = _from
                };
            }

            if (msgToMom != null)
            {
                if (msgToMom.HowSoon == null) msgToMom.HowSoon = DateTime.UtcNow.AddDays(1);

                if (msgToMom.HowMuch == 0)
                {
                    _message = "HowMuch should be a non-zero value";
                }
                else if (msgToMom.From == null || msgToMom.From == "")
                {
                    _message = "From should not be empty";
                }
                else
                {
                    await letterCollector.AddAsync(msgToMom);

                    _success = true;
                    _message = $"Hello, {msgToMom.From}";
                }
            }
            else
            {
                _message = "Cannot process an empty payload";
            }


            return (ActionResult)new OkObjectResult(new { success = _success, message = _message });    // ($"Hello, Johnny");
        }
    }
}
