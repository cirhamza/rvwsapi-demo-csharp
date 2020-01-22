using System;
using System.Collections.Generic;
using System.Text;

namespace REALvisionApiLib.Models
{
    public class GeneralConfigs
    {
        public double CheckProgressInterval { get; set; }
        public string AssetsFolder { get; set; }
        public string DownloadsFolder { get; set; }

        public GeneralConfigs(string rootPath)
        {
            CheckProgressInterval = 2000;
            AssetsFolder = rootPath + @"/Assets/";
            DownloadsFolder = rootPath + @"/Downloads";
        }
    }
}
