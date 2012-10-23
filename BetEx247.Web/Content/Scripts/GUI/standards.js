// JavaScript Document



var QSUtil = {
    getHashParams : function() {
        var hash = document.location.hash;
        if(hash.indexOf("#")==0) {
            hash = hash.substring(1);
        }
        var params = hash.split("&");
        var paramsHash = $H();
        for(var i = 0; i<params.length; i++) {
            var values = params[i].split("=");
            if(values.length==2) {
                paramsHash.set(values[0],values[1]);
            }
        }
        return paramsHash;
    }


}

function QS(){
    this.qs = {};
    var s = location.search.replace( /^\?|#.*$/g, '' );
    if( s ) {
        var qsParts = s.split('&');
        var i, nv;
        for (i = 0; i < qsParts.length; i++) {
            nv = qsParts[i].split('=');
            this.qs[nv[0]] = nv[1];
        }
    }
}


QS.prototype.add = function( name, value ) {
    if( arguments.length == 1 && arguments[0].constructor == Object ) {
        this.addMany( arguments[0] );
        return;
    }
    this.qs[name] = value;
}

QS.prototype.addMany = function( newValues ) {
    for( nv in newValues ) {
        this.qs[nv] = newValues[nv];
    }
}

QS.prototype.remove = function( name ) {
    if( arguments.length == 1 && arguments[0].constructor == Array ) {
        this.removeMany( arguments[0] );
        return;
    }
    delete this.qs[name];
}

QS.prototype.removeMany = function( deleteNames ) {
    var i;
    for( i = 0; i < deleteNames.length; i++ ) {
        delete this.qs[deleteNames[i]];
    }
}

QS.prototype.getQueryString = function() {
    var nv, q = [];
    for( nv in this.qs ) {
        q[q.length] = nv+'='+this.qs[nv];
    }
    return q.join( '&' );
}

QS.prototype.toString = QS.prototype.getQueryString;




function framerefresh(fr) {
    fr.location = fr.location;
}
function printerwin() {
    if (window.name!='print')
        window.open(document.location, 'print', 'toolbar,scrollbars,resizable,height=400,width=700,top=100,left=50');
}
function helpwin(pageid, section) {
    if (window.name!='help')
        window.open('/pagecontent/pagecontent.jsp?pageid='+pageid+'#'+section, 'help', 'toolbar,scrollbars,resizable,height=400,width=700,top=100,left=50');
}
function jumpto(url) {
    location = url;
}
function wnd(url) {
    var ewnd = null;
    end = window.open(url, "wnd", "scrollbars,resizable,height=400,width=500,top=100,left="+((screen.availWidth-300)/2));
}
function wndWH(url,w,h) {
    var ewnd = null;
    end = window.open(url, "wnd", "scrollbars,resizable,height="+h+",width="+w+",top=100,left="+((screen.availWidth-300)/2));
}
function depositwnd(url) {
    var ewnd = null;
    end = window.open(url, "wnd", "toolbar,scrollbars,resizable,height=400,width=600,top=100,left="+((screen.availWidth-300)/2));
}

function paysafedepositwnd(url) {
    var ewnd = null;
    end = window.open(url, "wnd", "scrollbars,resizable,height=600,width=700,top=50,left="+((screen.availWidth-300)/2));
}
function cteams(team1, team2, cat) {
    window.open('/statistics/compareteams.jsp?team1='+team1+'&team2='+team2+'&cat='+cat, 'info', 'scrollbars,resizable,height=400,width=700,top=100,left=50');
}
function windowopen_denied() {
    var newWind = null;
    if(newWind && !newWind.closed) {
        newWind.close();
    }
    newWind = window.open('/denied.html', 'popupwin', 'scrollbars,resizable,height=500,width=420,top=100,left=50');
}

// NEW V3 FUNCTIONS
/**
 * Tell slip to reload with new url
 */
function addToSlip(strUrl)
{
    parent.frames['left'].slip.location.href=strUrl;
}

/**
 * Update saldo from flash or applet in content frame.
 * If used in popup, call self.opener.updateSaldo(strAmount);
 */
function updateSaldo(strAmount) {
    if ($('saldo')) {
        $('saldo').innerHTML = strAmount;
        $('saldo').appear({
            duration: 0.7,
            from: 0.1,
            to:1.0
        });
    }
}

/**
 * Call this function on load on all content pages
 * catName = category that should be marked as active in the top menu
 * pageName = menu option in the sub menu that should be marked active
 */
var checkMenu;
function displayMenu(catName,pageName) 
{
    if (window.top.frames['menu'] != null && window.top.frames['menu'].menuGenerated)
    {
        window.top.frames['menu'].updateMenu(catName,pageName);
    }
    else
    {
        // Menu is not rendered yet.
        setTimeout("displayMenu('"+catName+"','"+pageName+"'),500");
    }
}

/**
 * Change left content frame
 */
function changeLeftTo(strUrl)
{
    window.top.frames['left'].location.href = strUrl;
}
/**
 * Show more / less betting types
 */
function moreLessTypes(strMore, strLess)
{
    oTable 	= document.getElementById('tableMoreTypes');
    oSpan 	= document.getElementById('moreLessTypes');
    if (oTable.style.display == 'none')
    {
        // Show Table
        oTable.style.display = 'block';
        oSpan.innerHTML = strLess;
    }
    else
    {
        // Hide Table
        oTable.style.display = 'none';
        oSpan.innerHTML = strMore;
    }
}
var UserUtils = {
		getLocaleDir : function() {
			var localeDir = "";
			if(typeof(UserValues)!="undefined" && UserValues && UserValues.localeDir) {
				localeDir = UserValues.localeDir;
			}
			return localeDir;
		}
};
/**
 * Show / Hide group in odds list
 */
function showHideOdds(obj) {
    // obj = clicked image
    oTable = document.getElementById('list_'+obj.id);
    if (oTable.style.display == "block")
    {
        //  Hide Table
        oTable.style.display = 'none';
        obj.src = '/images/v3Sport/rightCol_arrow_right.gif'
    }
    else
    {
        //  Show Table
        oTable.style.display = 'block';
        obj.src = '/images/v3Sport/rightCol_arrow_down.gif';
    }
}

/**
 * Show hide groups
 *
 *  Thomas Sonander (thomas@sonander.se) 20050401 - Added small usage documentation
 *	
 *	Example usage:
 *	
 *	<a href="javascript:showHideGroup('mycategory');" class="txt1" onfocus="this.blur();"><img name="catHeader" id="myPicId" src="/images/v3Poker/arrow0_right.gif" alt="" width="8" height="9" align="top" />Open category</a>
 *	<div style="margin-top: 0px; margin-bottom: 15px; display: none;" id="list_mycategory">
 *		Content to be displayed here..
 *	</div>
 *	
 **/
function showHideGroup(id) {
    // obj = clicked image
    oTable = document.getElementById('list_'+id);
    obj = document.getElementById(id);
	
    if (oTable.style.display == "block")
    {
        //  Hide Table
        oTable.style.display = 'none';
        obj.src = '/images/v3Sport/rightCol_arrow_right.gif'
    }
    else
    {
        //  Show Table
        oTable.style.display = 'block';
        obj.src = '/images/v3Sport/rightCol_arrow_down.gif';
    }
}

/**
 * Show hide payments type on the account page
 */
function showPaymentType(img)
{
    // First Hide all payments
    hideAllPayments();
	
    // Change this image
    img.src = '/images/v3Account/arrow0_down.gif'
	
    // Display content
    var intImgId = img.id;
    intImgId = intImgId.substring(intImgId.length-1,intImgId.length);
	
    document.getElementById('content_paymentType'+intImgId).style.display = 'block';
	
}
function hideAllPayments()
{
    var types = document.getElementsByName('img_paymentArrow');
    for (var i=0; i<types.length; i++)
    {
        // Change image link
        document.getElementById('img_paymentArrow'+i).src = '/images/v3Account/arrow0_right.gif';
        // Hide child
        document.getElementById('content_paymentType'+i).style.display = 'none';
    }
    return;
}
/* --------------------------------- */

/**
 * Show hide groups in left frame,
 * This function need to reload the slip to
 * guarantee that design is correct
 */
function showHideOddsLeft(obj) {
    // obj = clicked image
    oTable = document.getElementById('list_'+obj.id);
    if (oTable.style.display == "block")
    {
        //  Hide Table
        oTable.style.display = 'none';
        obj.src = '/images/v3Sport/rightCol_arrow_right.gif'
    }
    else
    {
        //  Show Table
        oTable.style.display = 'block';
        obj.src = '/images/v3Sport/rightCol_arrow_down.gif';
    }
    if (window.top.frames['left'] != null &&
        window.top.frames['left'].frames['slip'] != null) {
        slip.document.location.reload();
    }

}

/**
 * This functions is called from the slip iframe.
 * It doesn't call the setSlipHeight function untill
 * left frame is rendered.
 */
function tellParentToChange()
{
    if (parent.contentLoaded)
    {
        parent.setSlipHeight();
    }
    else
    {
        setTimeout("tellParentToChange()",500);
    }
}

/**
 * This is used to change url on the document
 */
function gotoURL(strURL, strTarget)
{
    strTarget = (strTarget == 'undefined')?null:strTarget;
    if (strTarget==null)
    {
        location.href = strURL;
    }
    else if (strTarget=='blank')
    {
        top.location.href = strURL;
    }
}

/**
 * Switch tab on active category list
 */
function switchOddsTab(strActive)
{
    // Hide all tabs
    document.getElementById('rightCol_favourite').style.display = 'none';
    document.getElementById('rightCol_odds').style.display = 'none';
	
    // Show selected
    document.getElementById('rightCol_'+strActive).style.display = 'block';
}


/**
 * Update clock on live matches
 */
var startTime = new Array(); 
var strPlayMsg;
function initClock(strClockName,intStartTime,strPlay)
{
    arrId = startTime.length;
    startTime[arrId] = new Date();
    startTime[arrId].setMilliseconds(intStartTime);
    strPlayMsg = strPlay;
    updateClock(strClockName,arrId);
} 
function updateClock(strClockName,arrId)
{
    clk = document.getElementById(strClockName);
    dateNow = new Date();
    timeDiff = startTime[arrId].getTime() - dateNow.getTime();
    timeDiffMin = (timeDiff - (timeDiff % 60000)) / 60000;
	
    timeDiff = timeDiff - (timeDiffMin * 60000);
    timeDiffSec = (timeDiff - (timeDiff % 1000)) / 1000;
	
    if (timeDiffMin == 0 && timeDiffSec == 0)
    {
        clk.innerHTML = strPlayMsg;
        clk.style.color = '#FF0000';
    }
    else
    {
        if (timeDiffMin < 60)
        {
            clk.innerHTML = addZero(timeDiffMin)+':'+addZero(timeDiffSec);
        }
        setTimeout("updateClock('"+strClockName+"','"+arrId+"')",1000);
    }
}
function addZero(intAmount)
{
    return (parseInt(intAmount)<10)?'0'+intAmount:intAmount;
}
/**
 * General function to open pop up
 * mypage = url, string
 * w = width, int
 * h = height, int
 * scrolls = scrollbars, yes/no
 * resizable = yes/no
 * Lpos,Tpos = Position on screen, int
 * UPDATED 20050516
 * - Added popup name option
 * - Added center function
 */
openPopup = function(mypage,w,h,scrolls,resizable,Lpos,Tpos,strName) {
    var popName = strName;
    if (popName=="")
    {
        popName = "popUp";
    }
	
    //check which popup to call
    var strUrlLower = mypage;
    strUrlLower = strUrlLower.toLowerCase();

	
    if (strUrlLower.indexOf('why')>=0 || popName=="why")
    {
        popName = "why";
        w = 450;
    }
    else if (strUrlLower.indexOf('how')>=0 || popName=="how")
    {
        popName = "how";
        w = 580;
    }
    else if (strUrlLower.indexOf('faq')>=0 || popName=="faq")
    {
        popName = "faq";
        w = 580;
    }
    else if (strUrlLower.indexOf('rules')>=0 || popName=="rules")
    {
        popName = "rules";
        w = 580;
    }
    else if (strUrlLower.indexOf('stats')>=0 || popName=="stats")
    {
        popName = "stats";
        w = 620;
        h = 550;
    }
	
    var win = null;
    var newL = (screen.width)?(screen.width-w)/2:Lpos;
    var newT = (screen.height)?(screen.height-h)/2:Tpos;

    settings = 'height='+h+',width='+w+',top='+newT+',left='+newL+',scrollbars='+scrolls+',resizable='+resizable+'';
	
    win = window.open(mypage,popName,settings)
    win.focus();
}

/**
 * Jump location
 */
function jumpLocation(url) {
    top.location = url;
}
/**
 * Hide and show news.
 * Used in home,sport,poker lobbies
 */
function showNews(newsId)
{
    document.getElementById('newsElm'+newsId).style.display = 'block';
    document.getElementById('newsLink'+newsId).style.display = 'none';
}
function hideNews(newsId)
{
    document.getElementById('newsElm'+newsId).style.display = 'none';
    document.getElementById('newsLink'+newsId).style.display = 'block';
}

function popupLink(mylink, windowname, properties)
{
    if (! window.focus) {
        return true;
        U
    }
    var href;
    if (typeof(mylink) == 'string') {
        href=mylink;
        return href;
    } else {
        href=mylink.href;
        window.open(href, windowname, properties);
        return false;
    }
}
function popupLink(mylink, windowname) {
    return popupLink(mylink,windowname,'width=840,height=640,scrollbars=yes');
}

function openFrameUrl(url) {
    if(parent && parent.frames["bets"]) {
        document.location = url;
    } else {
        document.location = "/frameset.jsp?location="+escape(url);
    }
}

function consoleDebug(info){
    if(typeof(console)!="undefined") {
        try {
            console.debug(info);
        } catch (e) { }
    }
}

function setRequestParameter(url, param, value){
    var regexp = new RegExp("(\\?|\\&)" + param + "\\=([^\\&]*)(\\&|$)");
    if (regexp.test(url))
        return (url.toString().replace(regexp, function(a, b, c, d)
        {
            return (b + param + "=" + value + d);
        }));
    else{ 
        var sign = "&";
        if(url.indexOf('?') == -1){
            sign = "?";
        }
        return url + sign + param + "=" + value;
    }
}

function _MM_preloadImages() { 
    var d=document;
    if(d.images){
        if(!d.MM_p) d.MM_p=new Array();
        var i,j=d.MM_p.length,a=_MM_preloadImages.arguments[0];
        for(i=0; i<a.length; i++)
            if (a[i].indexOf("#")!=0){
                d.MM_p[j]=new Image;
                d.MM_p[j++].src=a[i];
            }
    }
}

/*
 * LanguageSelector
 * Common functionality for switching language
 * author: PAP
 * 
 */
var LanguageSelector = {
	
    MIN_BROWSER_HEIGHT : 635,
		
    changeLanguage : function(ld) {
        var hash = window.location.hash;
        var url = "";
        if(hash && hash.length!=""){
            url = window.location.href.replace(hash,"");
        } else {
            url = window.location.href;
        }

        if(url.indexOf("#")!=-1) {
            url = url.replace("#","");
            hash = "#";
        }
        url = url.replace(/\/[a-z]{2}(\/|$)/,'/'+ld+'/');
        url+=hash;
        window.location.href = url;
    },
    mouseOver : function() {
        document.getElementById('languageSelection').style.display = "block";
    },
    mouseOut : function() {
        var browserHeight = document.viewport.getDimensions().height;
        if(browserHeight > this.MIN_BROWSER_HEIGHT ){
            this.hideSelector();
        }
    },
    mouseClick : function() {
        this.hideSelector();
    },
    hideSelector : function() {
        var langselector = document.getElementById('languageSelection');
        if (langselector && langselector != null && langselector != 'undefined') {
            document.getElementById('languageSelection').style.display = "none";
        }
    },
    toggleSelector : function() {
        // this is the way the standards work
        var style2 = document.getElementById('languageSelection').style;
        style2.display = style2.display? "":"block";
    }
}




/**
 * Function to retrieve parameters from the URL.
 */
function get_param(param) {
    var search = window.location.search.substring(1);
    if(search.indexOf('&') > -1) {
        var params = search.split('&');
        for(var i = 0; i < params.length; i++) {
            var key_value = params[i].split('=');
            if(key_value[0] == param) return key_value[1];
        }
    } else {
        var params = search.split('=');
        if(params[0] == param) return params[1];
    }
    return null;
}

/**
 * This Function is to click on the link using javascript.
 */
function clickLink(link) {
    var cancelled = false;

    if (document.createEvent) {
        var event = document.createEvent("MouseEvents");
        event.initMouseEvent("click", true, true, window,
            0, 0, 0, 0, 0,
            false, false, false, false,
            0, null);
        cancelled = !link.dispatchEvent(event);
    }
    else if (link.fireEvent) {
        cancelled = !link.fireEvent("onclick");
    }

    if (!cancelled) {
        window.location = link.href;
    }
}
/**
 * Function to obtain the original URL of a link.
 */
function returnOriginalLink(aString) {

    var returnValue = "";
    var strLenght = aString.length;
    strLenght = strLenght - 24;
    returnValue = aString.substring(0,strLenght);
    return returnValue;

}

/**
 *Method to destroy the cache of the IE, in order not to get
 *the flash game, even after logout.
 */
function generateChacheDestroyer(linkId) {
//    alert("Gen");
    var timestamp = Number(new Date());
    var subStr = linkId.href;
    if ( (-1 != subStr.search("timeStamp"))) {
        linkId.href = returnOriginalLink(subStr) + "&timeStamp="  + timestamp;
    } else {
        linkId.href = linkId.href + "&timeStamp="  + timestamp;
    }
}


function getInternetExplorerVersion() {
	var rv = -1; 
	if (navigator.appName == 'Microsoft Internet Explorer'){
	   var ua = navigator.userAgent;
	   var re  = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
	   if (re.exec(ua) != null)
	      rv = parseFloat( RegExp.$1 );
	}
	return rv;
}

