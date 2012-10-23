using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Plugin.XMLParser;
using BetEx247.Core.XMLObjects.Sport.Interface;
using BetEx247.Core;
using BetEx247.Core.Infrastructure;
using BetEx247.Core.Caching;

namespace BetEx247.Data.DAL
{
    public class GuiService : IGuiService
    {
        private static GuiService _instance;
        private XMLParserObjectManager obj;
        private string key = "betex247";

        /// <summary>
        /// Gets a cache manager
        /// </summary>
        public ICacheManager CacheManager
        {
            get
            {
                return new MemoryCacheManager();
            }
        }

        public GuiService()
        {
            obj = new XMLParserObjectManager();
            obj.Parse();
            CacheManager.Set(key, GetAllSport(), 10);
        }

        public static GuiService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GuiService();
                }
                return _instance;
            }
        }

        /// <summary>
        /// get all sport in system
        /// </summary>
        /// <returns>list sport</returns>
        public List<ISport> GetAllSport()
        {
            if (CacheManager.IsSet(key))
                return CacheManager.Get<List<ISport>>(key);
            return obj.Sports.Where(w => w.Leagues != null).ToList<ISport>();
        }

        /// <summary>
        /// Get Sport
        /// </summary>
        /// <param name="type">Sport type</param>
        /// <returns>Sport</returns>
        public ISport GetSport(Constant.SportType type)
        {
            return obj.Sport(type);
        }
    }
}
