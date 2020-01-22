using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using REALvisionApiLib;
using REALvisionApiLib.Models;

namespace
    RealvisionClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            String currentFolder = Directory.GetCurrentDirectory(); //The folder in which Program.cs exists

            //Don't forget to add the REALvisionApiLib to this App's reference
            REALvisionApiLib.RealvisionApi realvisionInstance = new REALvisionApiLib.RealvisionApi(currentFolder);

            // ********************************************************************************//
            //  To call https://realvisiononline.azure-api.net/GetActivationStatus
            // ********************************************************************************//

            var activationStatus = realvisionInstance.getActivationStatus();

            // ********************************************************************************//
            //  To call https://realvisiononline.azure-api.net/ProvideFile
            // ********************************************************************************//


            ////Specify the name of the file you want to slice with it's extension.
            realvisionInstance.FileToSlice = "cubetest.rvwj";

            //Specify where the file is stored
            //If it's stored in the Assets folder, use the Assets folder property
            //If not use the FileFolder property and specify the link to the file folder

            realvisionInstance.AssetsFolder = currentFolder + @"\Assets\";
            //realvisionInstance.FileFolder = currentFolder + @"\Assets\";

            //Specify where you want the downloaded FCode file to be stored.
            //If you don't specify it, the downloaded file will be stored in the same folder as the file you provided to slice

            realvisionInstance.DownloadsFolder = currentFolder + @"\Downloads\";

            //Specify the slicing configs
            realvisionInstance.SupportType = "n";
            realvisionInstance.PrinterModel = "IdeaWerk-Speed";
            realvisionInstance.ConfigPresetName = "Recommended";

            String TaskId = realvisionInstance.ProvideFile();

            // ********************************************************************************//
            //  To call https://realvisiononline.azure-api.net/GetProgress
            // ********************************************************************************//

            String progress = realvisionInstance.GetProgress(TaskId);

            // ********************************************************************************//
            //  To call https://realvisiononline.azure-api.net/GetPrintingInformation
            // ********************************************************************************//
            
            String printingInfos = realvisionInstance.GetPrintingInformation(TaskId);

            // ********************************************************************************//
            //  To call https://realvisiononline.azure-api.net/DownloadFile
            // ********************************************************************************//

            //Note: DownloadFile will first check the progress of the slicing process before downloading the file
            //      which is why you'll notice in the Console that GetProgress is executed a few times before DownloadFile is executed
            realvisionInstance.Downloadfile(TaskId);


        }
    }
}
