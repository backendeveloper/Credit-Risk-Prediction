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
    public class HomeController : Controller
    {
        // GET: CreditApproval
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index( Models.CreditApplication application)
        {
            //Open HttpClient
            using (var client = new HttpClient())
            {
                //Populate data structure that will be posted to Azure ML Service
                ScoreData scoreData = new ScoreData()
                {
                    FeatureVector = new Dictionary<string, string>() 
                    {
                        { "Checking account", application.CheckingAccount },
                        { "Duration in months", application.DurationInMonths },
                        { "Credit history", application.CreditHistory },
                        { "Purpose", application.Purpose },
                        { "Credit amount", application.CreditAmount },
                        { "Savings account/bond", application.SavingsAccountBonds },
                        { "Present employment since", application.PresentEmploymentSince },
                        { "Installment rate in percentage of disposable income", application.InstallmentRate },
                        { "Personal status and sex", application.PersonalStatusAndSex },
                        { "Other debtors", application.OtherDebtorsGuarantors },
                        { "Present residence since", application.PresentResidenceSince },
                        { "Property", application.Property },
                        { "Age in years", application.AgeInYears },
                        { "Other installment plans", application.OtherInstallmentPlans },
                        { "Housing", application.Housing },
                        { "Number of existing credits", application.NumberOfExistingCredits },
                        { "Job", application.Job },
                        { "Number of people providing maintenance for", application.NumberOfPeopleBeingLiableFor },
                        { "Telephone", application.Telephone },
                        { "Foreign worker", application.ForeignWorker },
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
                const string apiKey = "FvTvOPjO+4PHQnO4sXgdMlvLQfkVvsSX8T5c0QLnRHdZZ1Um2inymPNoiRV/oNFQ+uu64Si4vr+2PhFbT4WXzg==";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                //Set the Web Service address in Azure ML
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/f0be5175783044cbba05e008f3f58135/services/b494338b3ac24356ae69d864200288bb/score");

                //Send the request as JSON to web service and get the response
                HttpResponseMessage response = client.PostAsJsonAsync("", scoreRequest).Result;

                //If response is success
                if (response.IsSuccessStatusCode)
                {
                    //Get unformatted result set from Azure ML
                    string result = response.Content.ReadAsStringAsync().Result;

                    string[] resultArray = result.Split(',');

                    //Get the result data from ML and set to model, 1/true =>  Low Credit Risk / 2/false => Hight Credit Risk
                    application.Result = resultArray[20].Replace('"', ' ').Trim() == "1" ? "Kredi Vermeye Uygun" : "Kredi İçin Riskli";

                    ViewData["CreditResult"] = application.Result;

                     ViewData["ResultText"] = "Kredi İsteğiniz Başarılı Bir Şekilde İşleme Konuldu. İşlem Sonucu: " ;
                }
                else
                {
                    ViewData["ResultText"] = "İşlem Başarısız, hata kodu: " + response.StatusCode;
                }
            }

            return View(application);
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