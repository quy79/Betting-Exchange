using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Common.Utils;
using BetEx247.Core;

namespace BetEx247.Plugin.DownloadFeed
{
    public class BetclickFeed:IDownloadFeed
    {
        public void DownloadXML()
        {
            try
            {
                string downloadTime = DateTime.Now.Ticks.ToString();
                CommonHelper.DownloadXML(Constant.SourceXML.BETCLICKURL, Constant.SourceXML.BETCLICK, null,downloadTime);
            }
            catch { }
        }
    }
}
