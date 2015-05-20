using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CreditApproval_Web.Controllers
{
    public class CreditApprovalController : Controller
    {
        // GET: CreditApproval
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            //Initilize String List that will hold data that will posted from form
            List<String> formData = new List<string>();

            //Populate String List with data from form
            foreach (string _formData in formCollection)
                formData.Add(formCollection[_formData]);

            //Open HttpClient
            using (var client = new HttpClient())
            {
                //Populate data structure that will be posted to Azure ML Service
                ScoreData scoreData = new ScoreData()
                {
                    FeatureVector = new Dictionary<string, string>() 
                    {
                        { "Status of checking account", formData[0] },
                        { "Duration in months", formData[1] },
                        { "Credit history", formData[2] },
                        { "Purpose", formData[3] },
                        { "Credit amount", formData[4] },
                        { "Savings account/bond", formData[5] },
                        { "Present employment since", formData[6] },
                        { "Installment rate in percentage of disposable income", formData[7] },
                        { "Personal status and sex", formData[8] },
                        { "Other debtors", formData[9] },
                        { "Present residence since", formData[10] },
                        { "Property", formData[11] },
                        { "Age in years", formData[12] },
                        { "Other installment plans", formData[13] },
                        { "Housing", formData[14] },
                        { "Number of existing credits", formData[15] },
                        { "Job", formData[16] },
                        { "Number of people providing maintenance for", formData[17] },
                        { "Telephone", formData[18] },
                        { "Foreign worker", formData[19] },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                //Encapsulate request and make it ready for posting
                ScoreRequest scoreRequest = new ScoreRequest()
                {
                    Id = "score00001",
                    Instance = scoreData
                };

                // Replace this with the API key for the web service
                const string apiKey = "6C4v+O+N7HsaR/fFnzzI9U8yAlTrbvKPbAFvUg624+wmc0ayQRk4gY74egfHzLFQD5sIgB05nOxgwbBRLQgEVg==";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                //Set the Web Service address in Azure ML
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/557ef824663045bbaffe6a0b37abe781/services/ba11b13608d04063a7fbda494dca5ef3/score");

                //Send the request as JSON to web service and get the response
                HttpResponseMessage response = client.PostAsJsonAsync("", scoreRequest).Result;                

                //If response is success
                if (response.IsSuccessStatusCode)
                {
                    //Get unformatted result set from Azure ML
                    string result = response.Content.ReadAsStringAsync().Result;

                    string[] resultArray = result.Split(',');
                    
                    //Get the result data from, 1 =>  Low Credit Risk / 2 => Hight Credit Risk
                    ViewData["result"] = resultArray[20].Replace('"', ' ').Trim() == "1" ? "Kredi Riski Düşük" : "Kredi Riski Yüksek";
                }
                else
                {
                    ViewData["result"] = "İşlem Başarısız, hata kodu: " + response.StatusCode;
                }
            }

            return View();
        }
    }

    public class ScoreData
    {
        public Dictionary<string, string> FeatureVector { get; set; }
        public Dictionary<string, string> GlobalParameters { get; set; }
    }

    public class ScoreRequest
    {
        public string Id { get; set; }
        public ScoreData Instance { get; set; }
    }
}