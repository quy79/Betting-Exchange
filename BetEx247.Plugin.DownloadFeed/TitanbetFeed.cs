using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Common.Utils;
using BetEx247.Core;

namespace BetEx247.Plugin.DownloadFeed
{
    public class TitanbetFeed:IDownloadFeed
    {
        public void DownloadXML()
        {
            try
            {
                string downloadTime = DateTime.Now.Ticks.ToString();
               CommonHelper.DownloadXML(Constant.SourceXML.TITABETURL, Constant.SourceXML.TITANBET, null,downloadTime);
            }
            catch { }
        }
    }
}
