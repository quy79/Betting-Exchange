using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.Common.Utils
{
    public class Pager
    {
        #region Member Variables
        private const int TOTAL_PAGE_APPEAR = 4;

        protected int m_visibleInitLeftPages = 5;
        protected int m_visibleLeftOne = 1;
        protected int m_visibleRightOne = 1;
        protected int m_visibleLeftPages = 2;
        protected int m_visibleRitePages = 2;
       
        protected string m_multiPageSymbols = ""; 
        protected string m_prevButton = "<<";
        protected string m_nextButton = ">>";
        protected string m_firstButton = "<< Previous";
        protected string m_lastButton = "Last >>";
        protected string m_cssClass = "";
        protected string m_urlLink = "";

        private Constant.eLANGUAGE m_Language = Constant.eLANGUAGE.EN;
        private int m_totalSize;
        private int m_pageSize;
        private int m_pageNum;        
        protected string sNumFormat = "#,###";
        
        private string sHref = "[HREF_PAGE_NUM]";
        private string sOnclick = "[CLICK_FUNCTION]";
        #endregion

        #region Contructors
        public Pager(int pvTotalSize, int pvPageSize, int pvPageNum, string pvBaseUrl)
        {
            m_totalSize = pvTotalSize;
            m_pageSize = pvPageSize;
            m_pageNum = pvPageNum;
            m_urlLink = pvBaseUrl;

            if (m_totalSize < 0) m_totalSize = 0;
            //if (m_pageSize <= 0) m_pageSize = 15;
            if (m_pageNum <= 0) m_pageNum = 1;
            if (m_urlLink.IndexOf("?") < 4) m_urlLink += "?";
            if (m_urlLink.EndsWith("?")) m_urlLink += "0=1";
        }
        public Pager(int type, int pvTotalSize, int pvPageSize, int pvPageNum, string pvBaseUrl)
        {
            m_totalSize = pvTotalSize;
            m_pageSize = pvPageSize;
            m_pageNum = pvPageNum;
            m_urlLink = pvBaseUrl;

            if (m_totalSize < 0) m_totalSize = 0;
            if (m_pageSize <= 0) m_pageSize = 15;
            if (m_pageNum <= 0) m_pageNum = 1;
            if (m_urlLink.IndexOf("?") < 4) m_urlLink += "?t=" + type;
            if (m_urlLink.EndsWith("?")) m_urlLink += "0=1&t=" + type;
        }
        #endregion

        #region Properties
        public int TotalSize { get { return m_totalSize; } }
        public int PageSize { get { return m_pageSize; } }
        public int PageNum { get { return m_pageNum; } }
        public int TotalPages
        {
            get
            {
                if (TotalSize <= PageSize) return 1;
                int pages = (TotalSize - (TotalSize % PageSize)) / PageSize;
                return (TotalSize % PageSize > 0 ? pages + 1 : pages);
            }
        }
        public int StartIndex
        {
            get
            {
                if (TotalSize > 0) return (PageNum - 1) * PageSize + 1;
                return 1;
            }
        }
        public int EndIndex
        {
            get
            {
                if (TotalSize > 0) return (StartIndex + m_pageSize - 1 > TotalSize ? TotalSize : StartIndex + m_pageSize - 1);
                return 0;
            }
        }    
        
        public string CssClass
        {
            get { return m_cssClass; }
            set { m_cssClass = value; }
        }  
        
        public Constant.eLANGUAGE Language
        {
            get { return m_Language; }
            set { m_Language = value; }
        }       
        
        public string TotalSizeString { get { return m_totalSize.ToString(sNumFormat); } }
        public string StartIndexString { get { return StartIndex.ToString(sNumFormat); } }
        public string EndIndexString { get { return EndIndex.ToString(sNumFormat); } }
        #endregion

        #region Public Methods
        public string selected(int pageSize)
        {
            if (pageSize == m_pageSize)
                return " selected=\"selected\"";
            return "";
        }
        public virtual string DisplayResultInfo()
        {
            StringBuilder sb = new StringBuilder();
            string sResult = "results";
            string s_Total = TotalSize.ToString(sNumFormat);// TotalSize > 1000 ? "1000+" : TotalSize.ToString(sNumFormat);

            if (TotalSize > 0)
            {                     
                if (TotalSize == 1) sResult = "result";
                sb.AppendFormat("<span class=\"{0}pagerinfo\" >", CssClass);
                if (Language == Constant.eLANGUAGE.VI)
                    sb.AppendFormat("Hiển thị {0} đến {1} của {2} kết quả", StartIndex, EndIndex, s_Total);
                else
                {
                    if (TotalSize > 1000 && EndIndex == TotalSize)
                    {
                        sb.AppendFormat("<b>Last Page</b> of&nbsp;<b>{0}</b> " + sResult, s_Total);
                    }
                    else
                    {
                        if (EndIndex == 0)
                            sb.AppendFormat("<b>Displaying &nbsp; {0}</b> - <b>{1}</b> of&nbsp;<b>{2}</b> " + sResult, StartIndex.ToString(sNumFormat), TotalSize.ToString(sNumFormat), s_Total);
                        else
                            sb.AppendFormat("<b>Displaying &nbsp; {0}</b> - <b>{1}</b> of&nbsp;<b>{2}</b> " + sResult, StartIndex.ToString(sNumFormat), EndIndex.ToString(sNumFormat), s_Total);
                    }
                }
                sb.Append("</span>");
                #region add script
                //string script = "<script type=\"text/javascript\">";
                //script += "function chagePageSize(t){";
                //script += "var link = window.location.pathname;";
                //script += "if(link.indexOf('aspx')<0 && link[link.length-1] != '/')link += '/';";
                //script += "link+='?';";
                //script += " var queryString = window.location.search.substring(1);";
                //script += " var params = queryString.split('&');";
                //script += " var pagingInfo = 'PageSize='+$(t).val()+'&PageNum=1';";
                //script += " var query = '';";
                //script += " for (var i = 0; i < params.length; i++) {";
                //script += " if (params[i].split('=')[0] != 'PageSize' && params[i].split('=')[0] != 'PageNum' && params[i] != '0=1'){";
                //script += "  if(query !='')";
                //script += "     query+='&';";
                //script += "  query+= params[i];";
                //script += " }";
                //script += " }";
                //script += " if(query != '')";
                //script += "     query += '&'+pagingInfo+'&0=1';";
                //script += " else query += '0=1&'+pagingInfo;";
                //script += " window.location.href = link + query;";
                //script += " }";
                //script += "</script>";
                //sb.Append(script);
                #endregion
                string paging = "<span style='margin-left:10px;'>| Show </span><select id='cbNumOfPage' onchange='chagePageSize(this)'><option value='10'" + selected(10) + ">10</option><option value='25'" + selected(25) + ">25</option><option value='50'" + selected(50) + ">50</option><option value='100'" + selected(100) + ">100</option><option value='200'" + selected(200) + ">200</option>";
                if (m_urlLink.Contains("ReportResult.aspx"))
                    paging += "<option value='0'" + selected(0) + ">All</option>";
                paging += "</select><span> Records</span>";
                sb.Append(paging);
            }
            return sb.ToString();
        }

        public virtual string DisplayResultInfoNoneScript(string ScriptFuntion)
        {
            StringBuilder sb = new StringBuilder();
            string sResult = "results";
            string s_Total = TotalSize.ToString(sNumFormat);// TotalSize > 1000 ? "1000+" : TotalSize.ToString(sNumFormat);
            if (ScriptFuntion == "") ScriptFuntion = "ChagePageSize(this)";
            if (TotalSize > 0)
            {
                if (TotalSize == 1) sResult = "result";
                sb.AppendFormat("<span class=\"{0}pagerinfo\" >", CssClass);
                if (Language == Constant.eLANGUAGE.VI)
                    sb.AppendFormat("Hiển thị {0} đến {1} của {2} kết quả", StartIndex, EndIndex, s_Total);
                else
                {
                    if (TotalSize > 1000 && EndIndex == TotalSize)
                    {
                        sb.AppendFormat("<b>Last Page</b> of&nbsp;<b>{0}</b> " + sResult, s_Total);
                    }
                    else
                    {
                        if (EndIndex == 0)
                            sb.AppendFormat("<b>Displaying &nbsp; {0}</b> - <b>{1}</b> of&nbsp;<b>{2}</b> " + sResult, StartIndex.ToString(sNumFormat), TotalSize.ToString(sNumFormat), s_Total);
                        else
                            sb.AppendFormat("<b>Displaying &nbsp; {0}</b> - <b>{1}</b> of&nbsp;<b>{2}</b> " + sResult, StartIndex.ToString(sNumFormat), EndIndex.ToString(sNumFormat), s_Total);
                    }
                }
                sb.Append("</span>");
                string paging = "<span style='margin-left:10px;'>| Show </span><select id='cbNumOfPage' onchange='" + ScriptFuntion + "'><option value='10'" + selected(10) + ">10</option><option value='25'" + selected(25) + ">25</option><option value='50'" + selected(50) + ">50</option><option value='100'" + selected(100) + ">100</option><option value='200'" + selected(200) + ">200</option>";
                if (m_urlLink.Contains("ReportResult.aspx"))
                    paging += "<option value='0'" + selected(0) + ">All</option>";
                paging += "</select><span> Records</span>";
                sb.Append(paging);
            }
            return sb.ToString();
        }

        public virtual string DisplayResultInfo(string ResultFormat)
        {
            StringBuilder sb = new StringBuilder();
            string sResult = "results";
            string s_Total = TotalSize > 1000 ? "1000+" : TotalSize.ToString(sNumFormat);

            if (TotalSize > 0)
            {
                if (TotalSize == 1) sResult = "result";
                sb.AppendFormat("<span class=\"{0}pagerinfo\" >", CssClass);
                if (Language == Constant.eLANGUAGE.VI)
                    sb.AppendFormat("Hiển thị {0} đến {1} của {2} kết quả", StartIndex, EndIndex, s_Total);
                else
                {
                    if (TotalSize > 1000 && EndIndex == TotalSize)
                    {
                        sb.AppendFormat("<b>Last Page</b> of&nbsp;<b>{0}</b> " + sResult, s_Total);
                    }
                    else
                    {
                        sb.AppendFormat(ResultFormat, StartIndex.ToString(sNumFormat), EndIndex.ToString(sNumFormat), s_Total);
                    }
                }
                sb.Append("</span>");
            }
            return sb.ToString();
        }

        public virtual string DisplayPaging()
        {                         
            StringBuilder sb = new StringBuilder();
            if (TotalSize > PageSize)
            {
                //sb.AppendFormat("<ul class=\"{0}\">", CssClass);
                sb.Append(GetPagingItem(0));

                int left = PageNum - (TOTAL_PAGE_APPEAR / 2 + 1);
                int right = PageNum + (TOTAL_PAGE_APPEAR / 2);

                if (left < 1 || right < TOTAL_PAGE_APPEAR + 1 || TOTAL_PAGE_APPEAR > TotalPages)
                {
                    left = 1;
                    right = TOTAL_PAGE_APPEAR + 1;
                }
                else if (right > TotalPages)
                {
                    left = TotalPages - (TOTAL_PAGE_APPEAR - 1);
                }

                for (int i = left; i < right && i <= TotalPages; i++)
                    sb.AppendFormat("{0}", GetPagingItem(i));

                sb.Append(GetPagingItem(-1));
                //sb.Append("</ul>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Show link javascript for load paging ajax
        /// Change http://... to javascript:LoadPage(PageNum)
        /// Ex : href="javascript:LoadPage(1)" 
        /// </summary>
        /// <returns>javascript:LoadPage(PageNum)</returns>
        public virtual string DisplayPagingJs()
        {
            string sPaging = "";

            //remove all query string
            m_urlLink = "";
            sPaging = DisplayPaging();

            for (int i = TotalPages; i > 0; i--)
            {
                sPaging = sPaging.Replace("&PageSize=" + PageSize + "&PageNum=" + i, "javascript:LoadPage(" + i + ")");
            }

            return sPaging;
        }

        /// <summary>
        /// Show link javascript for load paging ajax
        /// Change http://... to javascript:JsFunctionName(PageNum)
        /// Ex : href="javascript:JsFunctionName(1)" 
        /// </summary>
        /// <param name="functionJsName">Ex:LoadAjaxPage</param>
        /// <returns></returns>
        public virtual string DisplayPagingJs(string JsFunctionName)
        {
            string sPaging = "";

            //remove all query string
            m_urlLink = "";
            sPaging = DisplayPaging();

            for (int i = TotalPages; i > 0; i--)
            {
                sPaging = sPaging.Replace("&PageSize=" + PageSize + "&PageNum=" + i, "javascript:" + JsFunctionName + "(" + i + ")");
            }

            return sPaging;
        }
                            
        public virtual string DisplayPagingJsWithParams(string JsFunctionName, params string[] AdditionalParams)
        {
            string sPaging = "";

            //remove all query string
            m_urlLink = "";
            sPaging = DisplayPaging();
            for (int i = TotalPages; i > 0; i--)
            {
                sPaging = sPaging.Replace("&PageSize=" + PageSize + "&PageNum=" + i, "javascript:" + JsFunctionName + "(" + i + (AdditionalParams.Length > 0 ? "," + string.Join(",", AdditionalParams) : "") + ")");
            }

            return sPaging;
        }

        public virtual string DisplayPagingJs(string JsFunctionName, params string[] pars)
        {
            string sPaging = "";
            string param = "";
            for (int i = 0; i < pars.Length; i++)
            {
                //param += (pars[i].IsNumber() ? pars[i] : "'" + pars[i] + "'") + ",";
                param += pars[i] + ",";
            }

            //remove all query string
            m_urlLink = "";
            sPaging = DisplayPaging();

            for (int i = TotalPages; i > 0; i--)
            {
                sPaging = sPaging.Replace("&PageSize=" + PageSize + "&PageNum=" + i, "javascript:" + JsFunctionName + "(" + param + i + ")");
            }

            return sPaging;
        }

        public virtual string DisplayPagingJs(string JsFunctionName, string url)
        {
            string sPaging = "";

            //remove all query string
            m_urlLink = "";
            sPaging = DisplayPaging();

            for (int i = TotalPages; i > 0; i--)
            {
                sPaging = sPaging.Replace("&PageSize=" + PageSize + "&PageNum=" + i, "javascript:" + JsFunctionName + "('" + url + "&PageSize=" + PageSize + "&PageNum=" + i + "')");
            }

            return sPaging;
        }                    
        #endregion

        #region Protected Methods
        protected virtual string GetPagingItem(int pagenum)
        {
            string html = String.Empty;               
            if (pagenum == 0 && PageNum > 1) //PREV button
                html = String.Format("<a href=\"javascript:void(0)\" onclick=\"load_pg(24780,200,{0})\">{1}</a>", PageNum - 1, m_prevButton);
            else if ((pagenum == -1 || pagenum > TotalPages) && PageNum < TotalPages) //NEXT button
                html = String.Format("<a href=\"javascript:void(0)\" onclick=\"load_pg(24780,200,{0})\">{1}</a>", PageNum + 1, m_nextButton);
            else if (pagenum >= 1 && pagenum <= TotalPages)
            {
                html = (pagenum == PageNum ? "<span class=\"paging\">{0}</span>" : "<a href=\"javascript:void(0)\" onclick=\"load_pg(24780,200,{0})\">{0}</a>");
                html = String.Format( html, pagenum);
            }              

            return html;
        }

        //protected virtual string GetLink(int pagenum)
        //{
        //    return String.Format("{0}&PageSize={1}&PageNum={2}", m_urlLink.Trim("&".ToCharArray()), PageSize, pagenum);
        //}
        #endregion
    }
}
