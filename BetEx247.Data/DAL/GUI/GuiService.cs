using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Plugin.XMLParser;
using BetEx247.Core.XMLObjects.Sport.Interface;
using BetEx247.Core;

namespace BetEx247.Data.DAL
{
    public class GuiService:IGuiService
    {
        private static GuiService _instance;
        private XMLParserObjectManager obj;
        public GuiService() 
        {
            obj = new XMLParserObjectManager();
            obj.Parse();
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
            return obj.Sports;
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
