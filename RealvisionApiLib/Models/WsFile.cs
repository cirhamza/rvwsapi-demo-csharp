using System;
using System.Collections.Generic;
using System.Text;

namespace REALvisionApiLib
{
    public class WsFile
    {
        public String FileName { get; set; }
        public string WsConfigs { get; set; }

        public WsFile()
        {
        }

        public WsFile(string fileName, string wsConfigs)
        {
            FileName = fileName;
            WsConfigs = wsConfigs;
        }
    }
}
