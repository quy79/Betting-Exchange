using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Web;


using System.Data.SqlClient;

using System.Collections;
using BetEx247.Plugin.DataManager;
using BetEx247.Core;
using BetEx247.Plugin.DownloadFeed;


using BetEx247.Plugin.DataManager.XMLObjects.Sport;

using BetEx247.Plugin.DataManager.XMLObjects.SoccerMatch;

using BetEx247.Plugin.DataManager.XMLObjects.SoccerCountry;

using BetEx247.Plugin.DataManager.XMLObjects.SoccerLeague;

using BetEx247.Plugin.DataManager.XMLObjects.SportCountry;

using BetEx247.Data.Model;
using BetEx247.Services;

namespace BetEx247.Services
{
    public partial class Service1 : ServiceBase
    {
        BetclickFeed xmlBetClickDownloadManager = new BetclickFeed();
        GoalServeFeed xmlGoalServeDownloadManager = new GoalServeFeed();

        XMLParserManager mgr = new XMLParserManager();
        DBManager db = new DBManager();

        public Service1()
        {
            InitializeComponent();
        }


        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            // Sleep here for Debugging purpose
           Thread.Sleep(20 * 1000);
            db.initConnection();
            //  while ( true)
            {
                Debug.WriteLine("OnStart: start refresh data");

                Thread t = new Thread(mainThread);
                t.Start();

              // SportsDataRenderManager renderMgr = new SportsDataRenderManager();
                //List<Bet247xSport> list = renderMgr.refreshData();
                Debug.WriteLine("OnStart:end onstart");
            }
        }

        protected void mainThread()
        {
            // Create a thread for Updating Master tables

            try
            {
                Thread initMasterTableThread = new Thread(checkInitUpdateMasterTablesThread);
                initMasterTableThread.Start();
                Thread DownloadXMLFeedThread = new Thread(checkDownloadXMLFeedsThread);
                DownloadXMLFeedThread.Start();
                Thread UpdateOddsThread = new Thread(checkUpdateOddsThread);
                UpdateOddsThread.Start();
                Thread UpdateDataStatusThread = new Thread(checkResheshDataThread);
                UpdateDataStatusThread.Start();
                Thread SettleThread = new Thread(DoSettleThread);
                SettleThread.Start();
                
                
                while (true)
                {
                    Thread.Sleep(60 * 1000);// 1 minute
                    if (!initMasterTableThread.IsAlive)
                    {
                        initMasterTableThread = new Thread(checkInitUpdateMasterTablesThread);
                        initMasterTableThread.Start();
                    }
                    if (!DownloadXMLFeedThread.IsAlive)
                    {
                        DownloadXMLFeedThread = new Thread(checkDownloadXMLFeedsThread);
                        DownloadXMLFeedThread.Start();
                    }
                    if (!UpdateOddsThread.IsAlive)
                    {
                        UpdateOddsThread = new Thread(checkUpdateOddsThread);
                        UpdateOddsThread.Start();
                    }
                    if (!UpdateDataStatusThread.IsAlive)
                    {
                        UpdateDataStatusThread = new Thread(checkResheshDataThread);
                        UpdateDataStatusThread.Start();
                    }
                    if (!SettleThread.IsAlive)
                    {
                        SettleThread = new Thread(DoSettleThread);
                        SettleThread.Start();
                    }

                }
            }
            catch { }

        }



        protected override void OnStop()
        {

        }

        protected void checkInitUpdateMasterTablesThread()
        {
            // Check in the xml file if need update new country or sport ...

            mgr.InitMasterTable();
            Thread.Sleep(6 * 60 * 60 * 1000); // Refresh each 6 hrs
        }
        protected void checkDownloadXMLFeedsThread()
        {
            // checkInitUpdateMasterTablesThread();
            Thread DownloadXMLFeedThread;
            Thread DownloadOtherSportFeedThread;
            Constant.PlaceFolder.GOALSERVE_FOLDER = "GOALSERVE\\SCHEDULES";
            Constant.PlaceFolder.BETCLICK_FOLDER = "BETCLICK";
            Constant.PlaceFolder.PINNACLESPORTS_FOLDER = "GOALSERVE\\SCHEDULES\\OTHERS";
            Bet247xSoccerCountry _bet247xSoccerCountry;
            Bet247xSportCountry _bet247xSportCountry;
            while (true)
            {
                if (mgr.updatedMastertable)
                {
                  

                     _bet247xSoccerCountry = (Bet247xSoccerCountry)mgr.masterTableManager.sports[0].Bet247xSoccerCountries[0];
                    DownloadXMLFeedThread = new Thread(DownloadBetclickFeedThread);
                    DownloadXMLFeedThread.Start(_bet247xSoccerCountry);
                   
                    for (int i = 0; i < mgr.masterTableManager.sports[0].Bet247xSoccerCountries.Count; i++)
                    {
                         _bet247xSoccerCountry = (Bet247xSoccerCountry)mgr.masterTableManager.sports[0].Bet247xSoccerCountries[i];

                       // xmlGoalServeDownloadManager.DownloadXML(_bet247xSoccerCountry.Goalserve_OddsFeed, _bet247xSoccerCountry.Country);
                        DownloadXMLFeedThread = new Thread(DownloadGoalFeedThread);
                        DownloadXMLFeedThread.Start(_bet247xSoccerCountry);
                       
                    }
                    for (int k = 1; k < 9; k++)
                    {
                        for (int i = 0; i < mgr.masterTableManager.sports[k].Bet247xSportCountries.Count; i++)
                        {
                            _bet247xSportCountry = (Bet247xSportCountry)mgr.masterTableManager.sports[k].Bet247xSportCountries[i];

                            // xmlGoalServeDownloadManager.DownloadXML(_bet247xSoccerCountry.Goalserve_OddsFeed, _bet247xSoccerCountry.Country);
                            DownloadOtherSportFeedThread = new Thread(DownloadOtherSportFeed);
                            DownloadOtherSportFeedThread.Start(_bet247xSportCountry);

                        }
                    }

                    Thread.Sleep(5 * 60 * 1000);
                   
                }
            }

            //checkUpdateOddsThread();
        }
        protected void DownloadOtherSportFeed(object country)
        {
            Bet247xSportCountry _bet247xSportCountry = (Bet247xSportCountry)country;
            xmlGoalServeDownloadManager.DownloadOtherSportXML(_bet247xSportCountry.Goalserve_OddsFeed, _bet247xSportCountry.Country, _bet247xSportCountry.SportID);
        }
        protected void DownloadGoalFeedThread(object country)
        {
            Bet247xSoccerCountry _bet247xSoccerCountry = (Bet247xSoccerCountry)country;
            xmlGoalServeDownloadManager.DownloadXML(_bet247xSoccerCountry.Goalserve_OddsFeed, _bet247xSoccerCountry.Country);
        }
        protected void DownloadBetclickFeedThread(object country)
        {
            Bet247xSoccerCountry _bet247xSoccerCountry = (Bet247xSoccerCountry)country;
           // xmlGoalServeDownloadManager.DownloadXML(_bet247xSoccerCountry.Goalserve_OddsFeed, _bet247xSoccerCountry.Country);
            String urlBetClick = urlBetClick = _bet247xSoccerCountry.Betclick_OddsFeed;
            xmlBetClickDownloadManager.DownloadXML(urlBetClick);
        }
        protected void checkUpdateOddsThread()
        {
            if (mgr.updatedMastertable)
            while (true)
            {
                DateTime currentTime = DateTime.Now;
                for (int i = 0; i < mgr.masterTableManager.sports[0].Bet247xSoccerCountries.Count; i++)
                {
                    Bet247xSoccerCountry _bet247xSoccerCountry = (Bet247xSoccerCountry)mgr.masterTableManager.sports[0].Bet247xSoccerCountries[i];


                    mgr.SoccerGoalServeParser(ref _bet247xSoccerCountry);
                    // urlBetClick = _bet247xSoccerCountry.Betclick_OddsFeed;
                   // DateTime lastestTime = DateTime.Now;
                    // if (currentTime.Minute - lastestTime.Minute)
                    // {
                }

                for (int k = 1; k < mgr.masterTableManager.sports.Count;k++ )
                {
                    for (int i = 0; i < mgr.masterTableManager.sports[k].Bet247xSportCountries.Count; i++)
                    {
                        Bet247xSportCountry _bet247xSoccerCountry = (Bet247xSportCountry)mgr.masterTableManager.sports[k].Bet247xSportCountries[i];


                        mgr.SoccerGoalServeOtherSportParser(ref _bet247xSoccerCountry);
                        // urlBetClick = _bet247xSoccerCountry.Betclick_OddsFeed;
                        // DateTime lastestTime = DateTime.Now;
                        // if (currentTime.Minute - lastestTime.Minute)
                        // {
                    }
                }

            }
        }

        protected void checkResheshDataThread()
        {
            SportsDataRenderManager Mgr = new SportsDataRenderManager();
            while (true)
            {
                
                Mgr.CollectInfoToSerialize();

                Thread.Sleep(60 * 1000);
            }

        }
        protected void DoSettleThread()
        {
             BetSettlementSvc _betSettleSvr = new BetSettlementSvc();
            while (true)
            {
                 
                try
                {
                   _betSettleSvr.doSetle4Type1();
                    _betSettleSvr.doSetle4Type2();
                }catch(Exception ee){
                }

                Thread.Sleep(5*60 * 1000); // 5 minutes
            }
        }

    }
}
