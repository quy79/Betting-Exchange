using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Common.Utils;
using BetEx247.Core;

namespace BetEx247.Plugin.DownloadFeed
{
    public class BetclickFeed
    {
        public void DownloadXML(String url)
        {
            bool downloaded = false;
            while (!downloaded){
            try
            {
                string downloadTime = "betclick";// DateTime.Now.Ticks.ToString();
                CommonHelper.DownloadXML(url, Constant.SourceXML.BETCLICK, null,downloadTime);
                downloaded = true;
            }
            catch (Exception e){ 
            
            }
        }

        }
    }
}
