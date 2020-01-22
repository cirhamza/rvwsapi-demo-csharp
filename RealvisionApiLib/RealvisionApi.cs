using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using REALvisionApiLib.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace
    REALvisionApiLib
{
    public class RealvisionApi
    {

        public static HttpClient RealvisionClient { get; set; }
        public String CurrentFolder { get; set; }

        public ApiSettings ApiSettings { get; set; }

        //The Slicing Configs
        public String FileToSlice { get; set; }     //Filename with extension
        public String FileFolder { get; set; }      //The folder where the file is stored, if this class isn't given a filefolder it will automatically use the Assets folder which should be supplied
        public String SupportType { get; set; }
        public String PrinterModel { get; set; }
        public String ConfigPresetName { get; set; }

        public String DownloadsFolder { get; set; } //The folder where the downloaded files will be stored 
        public String AssetsFolder { get; set; }    //The folder where the files to slice are stored.

        private String saveFileTo { get; set; }

        public RealvisionApi(String currentFolder)
        {
            this.CurrentFolder = currentFolder;
            this.ApiSettings = this.getApiSettings(currentFolder);
        }

        // ************************************************************************************* //
        // ******************************** API FUNCTIONS ************************************** //
        // ************************************************************************************* //

        public String getActivationStatus()
        {
            ApiRequest ApiRequest = new ApiRequest();
            return MakeRequest(HttpMethod.Post, "GetActivationStatus", ApiRequest).Result;
        }
        public String ProvideFile()
        {
            String fileUrl = (string.IsNullOrEmpty(this.FileFolder) ? this.AssetsFolder : this.FileFolder) + this.FileToSlice;
            string wsConfigs = File.ReadAllText(fileUrl, Encoding.UTF8);
            WsFile file = new WsFile(this.FileToSlice, wsConfigs);
            ApiRequest ApiRequest = new NoConfigApiRequest(file, this.SupportType, this.PrinterModel, this.ConfigPresetName);

            return MakeRequest(HttpMethod.Post, "ProvideFile", ApiRequest).Result;
        }
        public String GetProgress(String TaskId)
        {
            ApiRequest ApiRequest = new TaskApiRequest(TaskId);

            String tempProgress = MakeRequest(HttpMethod.Post, "GetProgress", ApiRequest).Result;
            return tempProgress;
        }
        public String GetPrintingInformation(String TaskId)
        {
            string progress = checkProgress(TaskId);

            if (progress == "1")
            {
                ApiRequest ApiRequest = new TaskApiRequest(TaskId);
                return MakeRequest(HttpMethod.Post, "GetPrintingInformation", ApiRequest).Result;
            }
            else if (progress == "-1")
            {
                throw new Exception("Slicing file failed ... ");
            }
            else if (string.IsNullOrEmpty(progress))
            {
                throw new Exception("Progress is Empty ... ");
            }
            else
            {
                throw new Exception("Encoutered error while downloading file ... ");
            }

        }
        public void Downloadfile(String TaskId)
        {
            string progress = checkProgress(TaskId);

            if (progress == "1")
            {
                var result = MakeRequest(HttpMethod.Get, "DownloadFile?taskid=" + TaskId, new ApiRequest(), true).Result;
            }
            else if (progress == "-1")
            {
                throw new Exception("Slicing file failed ... ");
            }
            else if (string.IsNullOrEmpty(progress))
            {
                throw new Exception("Progress is Empty ... ");
            }
            else
            {
                throw new Exception("Encoutered error while downloading file ... ");
            }
        }

        // ************************************************************************************* //
        // ***************************** SUPPORT FUNCTIONS ************************************* //
        // ************************************************************************************* //
        public ApiSettings getApiSettings(String currentFolder)
        {
            JToken appSettings = JToken.Parse(File.ReadAllText(Path.Combine(currentFolder, "appsettings.json")));
            return ApiSettings = JsonConvert.DeserializeObject<ApiSettings>(JsonConvert.SerializeObject(appSettings["ApiSettings"]));
        }

        public string checkProgress(string TaskId){
            String progress = GetProgress(TaskId);

            while (progress != "1" && progress != "-1" && !string.IsNullOrEmpty(progress) && progress != "2")
            {
                progress = GetProgress(TaskId);
                Console.WriteLine(progress);
            }

            return progress;
        }
      
        private String readHttpResponse(HttpWebResponse response)
        {
            System.IO.Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);

            return responseReader.ReadToEnd();
        }

        private async void logResponse(HttpResponseMessage response, String serviceCall, bool isDownload)
        {

            Console.WriteLine();
            Console.WriteLine("*************************************************************************");
            Console.WriteLine("SERVICECALL                  :::: " + serviceCall);

            Console.WriteLine();
            Console.WriteLine("METHOD                       :::: " + response.RequestMessage.Method);
            Console.WriteLine("REQUEST_STATUS_CODE          :::: " + response.StatusCode);

            if (isDownload && HttpStatusCode.OK == response.StatusCode)
            {
                Console.WriteLine("--------------------");
                Console.WriteLine("RESPONSE                 :::: " + " Please check the following folder for the downloaded FCode file: ");
                Console.WriteLine(this.saveFileTo);
                Console.WriteLine("--------------------");

            }
            else
            {
                Console.WriteLine("RESPONSE                     :::: " + await response.Content.ReadAsStringAsync());
            }

            Console.WriteLine("*************************************************************************");
            Console.WriteLine();

        }

        private void SaveFile(String response, String fileName, String fileExtention)
        {
            DateTime foo = DateTime.UtcNow;
            long utc = ((DateTimeOffset)foo).ToUnixTimeSeconds();
            String timeStamp = utc.ToString();
            String fileFolderLink = (string.IsNullOrEmpty(this.DownloadsFolder) ? (string.IsNullOrEmpty(this.AssetsFolder) ? this.FileFolder : this.AssetsFolder) : this.DownloadsFolder);
            this.saveFileTo = fileFolderLink + fileName + "." + timeStamp + fileExtention;

            try
            {
                File.WriteAllText(this.saveFileTo, response);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR WHILE SAVING FILE TO FILESYSTEM", ex);
            }
        }

        // ************************************************************************************* //
        //This function is used by all the API Functions to call the API 
        // ************************************************************************************* //

        public async Task<String> MakeRequest(HttpMethod method, String serviceCall, ApiRequest ApiRequest, bool isDownload = false)
        {
            using (HttpClient client = new HttpClient())
            {
                String FinalResponse = "";

                string json = JsonConvert.SerializeObject(ApiRequest, Formatting.Indented);

                //Preparing request 
                HttpRequestMessage req = new HttpRequestMessage();
                req.RequestUri = new Uri(this.ApiSettings.ApiUrl + serviceCall);
                req.Method = method;
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
                //Authentication & Authorization
                req.Headers.Add("Ocp-Apim-Subscription-Key", this.ApiSettings.ApiKey);

                //Making the request
                var result = await client.SendAsync(req);

                //Reading the response as a string
                var response = await result.Content.ReadAsStringAsync();

                if (!result.IsSuccessStatusCode)
                {
                    logResponse(result, serviceCall, false);
                    throw new Exception("Request unsuccessful.");
                }

                //Serializing the response depeding on which endpoint was called
                if (!isDownload)
                {
                    if (serviceCall == "ProvideFile")
                    {
                        TaskIdResponse responseObject = JsonConvert.DeserializeObject<TaskIdResponse>(response);
                        FinalResponse = responseObject.Result.TaskId;
                    }
                    else if (serviceCall == "GetProgress")
                    {
                        ProgressResponse responseObject = JsonConvert.DeserializeObject<ProgressResponse>(response);
                        FinalResponse = "" + responseObject.Result.Progress ;
                    }
                    else if (serviceCall == "GetPrintingInformation")
                    {
                        PrintingInformationResponse responseObject = JsonConvert.DeserializeObject<PrintingInformationResponse>(response);
                        FinalResponse = JsonConvert.SerializeObject(responseObject.Result);
                    }
                    else
                    {
                        TaskIdResponse responseObject = JsonConvert.DeserializeObject<TaskIdResponse>(response);
                        FinalResponse = responseObject.Result.TaskId;
                    }

                }
                else
                {
                    string fullFilename = result.Content.Headers.ContentDisposition.FileName;
                    string extentionlessFilename = Path.GetFileNameWithoutExtension(fullFilename);
                    string extention = Path.GetExtension(fullFilename);

                    SaveFile(response,extentionlessFilename , extention);
                    FinalResponse = response;
                }

                //Logging the results of the request
                this.logResponse(result, serviceCall, isDownload);

                return FinalResponse;


            }
        }
    }
}
