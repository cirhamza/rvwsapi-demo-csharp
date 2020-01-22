using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace REALvisionApiLib
{
    public class ApiResponse
    {
        public String ApiVersion { get; set; }
        public String CoreVersion { get; set; }
        public String RealvisionHtmlVersion { get; set; }
    }

    public class ApiErrorResponse : ApiResponse
    {
        public double StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public Error Error { get; set; }
    }
    public class Error
    {
        public String ErrorCode { get; set; }
        public String ErrorMessage { get; set; }
        public JObject Details { get; set; }
    }

    public class ActivationStatusResponse : ApiResponse
    {
        public ActivationStatus Result { get; set; }
    }
    public class ActivationStatus
    {
        public string Activated { get; set; }
    }
    public class FileIdResponse : ApiResponse
    {
        public FileToSlice Result { get; set; }

    }

    public class TaskIdResponse : ApiResponse
    {
        public SlicingTask Result { get; set; }

    }

    public class ProgressResponse : ApiResponse
    {
        public SlicingProgress Result { get; set; }

    }
    public class PrintingInformationResponse : ApiResponse
    {
        public PrintingInformation Result { get; set; }
    }

    public class FileToSlice
    {
        public String FileId { get; set; }

        public FileToSlice(String fileId)
        {
            this.FileId = fileId;
        }
    }
    public class SlicingTask
    {
        public String TaskId { get; set; }

        public SlicingTask(string taskId)
        {
            TaskId = taskId;
        }
    }

    public class SlicingProgress
    {
        public double Progress { get; set; }

        public SlicingProgress(double progress)
        {
            Progress = progress;
        }
    }

    public class PrintingInformation 
    {
        public String Time { get; set; }
        public String Length { get; set; }
        public String Weight { get; set; }

        public PrintingInformation(string time, string length, string weight)
        {
            Time = time;
            Length = length;
            Weight = weight;
        }
    }
}
