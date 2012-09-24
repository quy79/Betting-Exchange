using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Sport.Interface;
using BetEx247.Core;

namespace BetEx247.Core.XMLObjects
{
    /// <summary>
    /// This classs is a container to store all data object of sports and their details.
    /// </summary>
    class XMLObjectsManager
    {
        private DateTime fdTime;
        private List<ISport> sports;
        private Constant.SourceFeedType sourceFeed ;
        /// <summary>
        /// Init without paramater
        /// </summary>
        XMLObjectsManager() { }
        /// <summary>
        /// Init with only Time of XML feed
        /// </summary>
        /// <param name="fdTime"></param>
        XMLObjectsManager(DateTime fdTime) {
            FDTime = fdTime;
        }
        /// <summary>
        /// Init with Time and Sports
        /// </summary>
        /// <param name="fdTime"></param>
        /// <param name="sports"></param>
        XMLObjectsManager(DateTime fdTime, List<ISport> sports) {
            FDTime = fdTime;
            Sports = sports;
        }
        /// <summary>
        /// Init with Time, Sport and Which Feed of xml
        /// </summary>
        /// <param name="fdTime"></param>
        /// <param name="sports"></param>
        /// <param name="sourceFeed"></param>
        XMLObjectsManager(DateTime fdTime, List<ISport> sports, Constant.SourceFeedType sourceFeed) {
            FDTime = fdTime;
            Sports = sports;
            SourceFeed = sourceFeed;
        }

        /// <summary>
        /// Time of the XML feed
        /// </summary>
        public DateTime FDTime
        {
            set { fdTime = value; }
            get { return fdTime; }
        }
        /// <summary>
        /// All sports
        /// </summary>
        public List<ISport> Sports
        {
            set { sports = value; }
            get { return sports; }
        }
        /// <summary>
        /// Which XML feed Url where we get data
        /// </summary>
        public Constant.SourceFeedType SourceFeed
        {
            set { sourceFeed = value; }
            get { return sourceFeed; }
        }


           
    }
}
