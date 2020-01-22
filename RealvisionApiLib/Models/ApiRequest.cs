using System;
using System.Collections.Generic;
using System.Text;

namespace REALvisionApiLib
{
    // ************************************************************************************* //
    //  This is the Object we pass to every API call using the Initialize Request function.
    //  Each API Function has it's own ApiRequest instance that has different values.
    //  There is a different constructor for different kinds of requests.
    // ************************************************************************************* //

    public class ApiRequest
    {

    }
    public class NoConfigApiRequest : ApiRequest
    {
        public WsFile File { get; set; }
        public String SupportType { get; set; }
        public String PrinterModel { get; set; }
        public String ConfigPresetName { get; set; }

        public NoConfigApiRequest(WsFile file, string supportType, string printerModel, string configPresetName)
        {
            File = file;
            SupportType = supportType;
            PrinterModel = printerModel;
            ConfigPresetName = configPresetName;
        }
    }
    public class ConfigApiRequest : ApiRequest
    {
        public WsFile File { get; set; }
        public WsFile ConfigFile { get; set; }

        public ConfigApiRequest(WsFile file, WsFile configFile)
        {
            File = file;
            ConfigFile = configFile;
        }
    }
    public class TaskApiRequest : ApiRequest
    {
        public String TaskId { get; set; }

        public TaskApiRequest(string taskId)
        {
            TaskId = taskId;
        }
    }
}
