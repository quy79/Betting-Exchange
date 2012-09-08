using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using BetEx247.Core.Common.Utils;
using BetEx247.Core;

namespace BetEx247.Plugin.DownloadFeed
{
    public partial class DownloadXMLFeed
    {
        IDownloadFeed idownload;
        public DownloadXMLFeed(string parserSource)
        {
            switch (parserSource)
            {
                case Constant.SourceXML.BETCLICK:
                    idownload = new BetclickFeed();
                    break;
                case Constant.SourceXML.PINNACLESPORTS:
                    idownload = new PinnaclesportsFeed();
                    break;
                case Constant.SourceXML.TITANBET:
                    idownload = new TitanbetFeed();
                    break;
                default:
                    idownload = new PinnaclesportsFeed();
                    break;
            }                            
        }

        public void DownloadXML()
        {
            idownload.DownloadXML();
        }
    }
}
