var NewGenEntrantMethod = true;
var iEtDesc = 0;
var iEtIdent = 1;
var iEtSort = 2;
var iEtRF = 3;
var iEtStatus = 4;
var iEtIcon = 5;
var iEtHandicap = 6;
var iEtUPWin = 8;
var iEtUPLose = 9;
var iEtMWin = 10;
var iEtMLose = 11;
var iEtUMWin = 12;
var iEtUMLose = 13;
var iEtWinDisplay = 14;
var iEtLoseDisplay = 15;
var iEtProfitLossDisplay = 16;
var iEtIconError = 17;
var iEtSP = 10;
var iEtSCR = 11;
var iEtBO1 = 0;
var iEtBA1 = 1;
var iEtBO2 = 2;
var iEtBA2 = 3;
var iEtBO3 = 4;
var iEtBA3 = 5;
var iEtBO4 = 6;
var iEtBA4 = 7;
var iEtBO5 = 8;
var iEtBA5 = 9;
var iEtLO1 = 10;
var iEtLA1 = 11;
var iEtLO2 = 12;
var iEtLA2 = 13;
var iEtLO3 = 14;
var iEtLA3 = 15;
var iEtLO4 = 16;
var iEtLA4 = 17;
var iEtLO5 = 18;
var iEtLA5 = 19;
var iWarningOdds = 99;
var sOddsWarningMsg = "";
var sMarketID = "";
var sBetType = "";
var tpMarketFlag = "";
var sStatus = "";
var sUnit = "";
var sTeam1 = "";
var sTeam2 = "";
var iTimestamp = 0; var iDelay = 0; var iMarketCommission = 5; var sSymbol = ""; var numDec = 2;
var dStartDateTime = new Date(); var timeoutRefreshOdds = null; var timeoutRedirect = null; var timeoutMyBets = null;
var iRefreshOddsInterval = 10000; var iNoOfWinners = 1; var minBet = 1; var maxBet = 9999999; var timeoutResize = 300;
var htEntrants = {}; var htOdds = {}; var htRelatedMarkets = {}; var arrEntrantIDs; var arrEntrantIDsToIndex; var arrEntrantOrderDefault;
var arrEntrantOrderByName; var arrEntrantOrderByIdentifier; var arrEntrantOrderByOdds; var arrBestBetObj; var htLastBestBetOrder = {};
var bBetPanelUnPlacedShowProfit = true; var bBetPanelUnMatchedShowProfit = true; var bBetPanelMatchedShowProfit = true;
var bMemberRequestProcessing = false; var bMarketPage = true; var iShowStakeFillTool = 0; var bShowBetSettings = false;
var bShowPLSettings = false; var iPLMarketView = 0; var iPLMarketDepth = 0; var iPLMinWin = 0; var bPLShowTotalMatched = false;
var bPLShowBook = false; var dTotalMatched = -1; var MarketDepth_BestPriceOnly = 0; var MarketDepth_BestPriceAndStake = 1;
var MarketDepth_FullDepth = 2; var PerBet_Show = 1; var PerBet_Hide = 3; var DutchProfit_Show = 2; var DutchProfit_Hide = 4;
var DutchLiability_Show = 5; var DutchLiability_Hide = 7; var dProfit = ""; var dLiability = ""; var dBookProfit = "";
var dBookLiability = ""; var htMinWin = [0, 20, 50, 100, 200, 500]; var iBestOddPos = 0; var iBestStakePos = 1; var iBestEntrantIDPos = 2;
var iBestIdentifierPos = 3; var sMarketSnapshotChoice = ""; var sClipboardData = ""; var iNoOfOpenEntrants = 0; var htSortBy = {};
var iIdentifierText = 1;
var iIdentifierValue = 2;
var currentBrowser = new BrowserObj();
function initMarket(marketID, bSyncTree) {
    server_getMarketData(marketID, bSyncTree, false)
};
function server_getMarketData(marketID, bSyncTree, refresh) {
    if (sMarketID != marketID || refresh) {
        window.clearTimeout(timeoutRefreshOdds);
        window.clearTimeout(timeoutRedirect);
        frames["ifraOrdersRequest"].location = "Blank.html";
        frames["ifraMarketRequest"].location = "Blank.html";
        EndProcessing();
        frames["ifraMarketRequest"].location = "MarketDataJS" + sExt + "?marketID=" + marketID + "&SyncTree=" + bSyncTree + "&src=mnu" + (refresh ? "&InitLoad=false" : "")
    }
};

var bMarketIsChanged = false;

function server_receiveMarketData(htMarketData, htEnts, arrEntsOrderByName, arrRelatedMarkets, arrRelatedMarketGroups, initLoad, arrEntsOrderByIdentifier, mcSymbol, numDecPlaces) {
    try {
        var parentTabID = getQueryString("TabID");
        if (parentTabID == "") parentTabID = "0";
        if (parentTabID != "" && parent.setMarketTabInfo) {
            parent.setMarketTabInfo(parentTabID, htMarketData["event"], htMarketData["market"], htMarketData["marketID"])
        }
        document.getElementById("tblMarket").style.display = "";
        document.getElementById("tblMarketMessage").style.display = "none";
        htMarketData["marketID"];
        if (sMarketID.length == 0) {
            document.getElementById("selEntrantOrder").value = "Default";
            bMarketIsChanged = true
        }
        else if (sMarketID.toUpperCase() != htMarketData["marketID"].toUpperCase()) {
            arrEntrantOrderByOdds = new Array(); bMarketIsChanged = true; sClipboardData = ""; ui_closeMarketSnapshotSettings()
        } else {
            bMarketIsChanged = false
        }
        sMarketID = htMarketData["marketID"];
        sBetType = htMarketData["betType"];
        sUnit = htMarketData["unit"];
        sUnitSingle = htMarketData["unitSingle"];
        sTeam1 = htMarketData["team1"];
        sTeam2 = htMarketData["team2"];
        tpMarketFlag = htMarketData["tpMarketFlag"];
        iTimestamp = htMarketData["timestamp"];
        sStatus = htMarketData["status"];
        iDelay = htMarketData["delay"];
        sSymbol = mcSymbol;
        numDec = numDecPlaces;
        dStartDateTime = convertToGMT(htMarketData["yourStart"]);
        iNoOfWinners = htMarketData["winners"];
        minBet = htMarketData["minimumBet"];
        if (initLoad) { htOdds = {} } htRelatedMarkets = {};
        htEntrants = htEnts;
        arrEntrantOrderByName = arrEntsOrderByName;
        arrEntrantOrderByIdentifier = arrEntsOrderByIdentifier;
        iMarketCommission = htMarketData["marketRate"];
        client_renderMarketData(htMarketData, arrRelatedMarketGroups);
        window.clearTimeout(timeoutRefreshOdds);
        timeoutRefreshOdds = window.setTimeout("ui_doRefreshMarket()", iRefreshOddsInterval);
        if (initLoad) {
            document.getElementById("spMatchedAmount").innerHTML = "";
            document.getElementById("spMatchedAmountLiability").innerHTML = "";
            client_clearUnPlacedBets();
            client_renderBetType();
            if (top.addRecentMarket) top.addRecentMarket([sMarketID, htMarketData["market"], htMarketData["event"], htMarketData["parentEvent"]]);
            if (top.setRelatedMarkets) top.setRelatedMarkets(arrRelatedMarkets)
        } else { setProfitLossUI() }
    } catch (e) {
        top.showMessageBox("server_receiveMarketData:" + e.message, null, null)
    }
};

function client_renderBetType() {
    if (sBetType == "Odds") {
        document.getElementById("selEntrantOrder").style.display = "";
        document.getElementById("trOverallPLHeader").style.display = "none";
        document.getElementById("trOverallPLTable").style.display = "none";
        document.getElementById("trOverallPLTableNA").style.display = "none";
        document.getElementById("trOverallPLGap").style.display = "none";
        document.getElementById("trOverallPLGap2").style.display = "none";
        document.getElementById("trPLSettingsUnPlaced").style.display = ""
    } else {
        document.getElementById("selEntrantOrder").style.display = "none";
        document.getElementById("trPLSettingsUnPlaced").style.display = "none"
    }
}

function server_receiveOddsData(marketID, htNewOddsData, marketTS) {
    if (marketTS == "NotFound" || marketTS > iTimestamp) {
        EndProcessing();
        server_getMarketData(marketID.toLowerCase(), false, true);
    }
    var changes = false;
    var start = new Date();
    htOdds = htNewOddsData;
    changes = true;
    if (htNewOddsData != null) {
        if (changes) {
            if (NewGenEntrantMethod) {
                if (iPLMarketView == top.MarketView_SportsbookView) {
                    client_renderEntrantDataWithOddsForSportsbookView(marketID, false)
                } else {
                    client_renderEntrantDataWithOdds1(marketID, false)
                }
            } else {
                client_renderEntrantDataWithOdds(marketID)
            }
        }
    }
    if (!top.bLoggedIn) { EndProcessing() }
};

function server_receiveAmountMatched(amount, amountLiability) {
    if (amount == null) amount = 0;
    if (amountLiability == null) amountLiability = 0;
    amount = Math.floor(amount);
    dTotalMatched = amount;
    amountLiability = Math.floor(amountLiability);
    changeTotalMatched(bPLShowTotalMatched);
    document.getElementById("spMatchedAmount").innerHTML = sSymbol + formatDollars(amount);
    document.getElementById("spMatchedAmountLiability").innerHTML = sSymbol + formatDollars(amountLiability)
};

function server_receiveMarketUnavailable() {
    try {
        document.getElementById("tblMarket").style.display = "none";
        document.getElementById("tblMarketMessage").style.display = "";
        document.getElementById("dvStatusHolder").style.display = "none";
        var messageHTML = "";
        var bHasRelated = false;
        for (var marketID in htRelatedMarkets) {
            bHasRelated = true; break
        }
        if (bHasRelated) {
            var linksHTML = "";
            messageHTML = sMarketUnAvailableWithRelated;
            var linkTemplate = document.getElementById("taRelatedMarketTemplate").value;
            var nextMarketID = "";
            for (var marketID in htRelatedMarkets) {
                if (nextMarketID == "") nextMarketID = marketID;
                var re = new RegExp("@IM", "g");
                var singlelink = linkTemplate.replace(re, marketID.toLowerCase());
                re = new RegExp("@DM", "g");
                singlelink = singlelink.replace(re, htRelatedMarkets[marketID]);
                linksHTML += singlelink
            }
            var linksHolderHTML = document.getElementById("taRelatedMarketsTemplate").value;
            var re = new RegExp("@Links", "g");
            linksHolderHTML = linksHolderHTML.replace(re, linksHTML);
            messageHTML += linksHolderHTML;
            sMarketID = "";
            timeoutRedirect = window.setTimeout("initMarket('" + nextMarketID + "',false)", 10000)
        } else {
            messageHTML = sMarketUnAvailableWithNoRelated;
            timeoutRedirect = window.setTimeout("helper_redirectHome()", 5000)
        };
        document.getElementById("tdMarketMessage").innerHTML = messageHTML
    } catch (e) {
        top.showMessageBox("setMarketUnAvailable:" + e.message, null, null)
    }
};

function helper_redirectHome() { window.location = "Home" + sExt };
var linkTemplate = "";
function client_renderMarketData(htMarketData, arrRelatedMarketGroups) {
    if (linkTemplate == null) linkTemplate = "";
    document.getElementById("spEvent").innerHTML = htMarketData["event"];
    document.getElementById("spMarket").innerHTML = htMarketData["market"];
    document.getElementById("spLocalStart").innerHTML = formatDateTimeMins(parseDateTime(htMarketData["localStart"]));
    if (htMarketData["location"] == "") htMarketData["location"] = sVenue;
    document.getElementById("spLocation").innerHTML = htMarketData["location"];
    var dYourStart = parseDateTime(htMarketData["yourStart"]);
    document.getElementById("spYourStart").innerHTML = formatDateTimeMins(dYourStart);
    document.getElementById("spStatus").innerHTML = htStatuses[sStatus];
    document.getElementById("imgLiveNow").style.display = "none";
    document.getElementById("imgLive").style.display = "none";
    if (sStatus == "Live") {
        document.getElementById("imgLiveNow").style.display = "";
        document.getElementById("spTimeToStart").style.display = "none";
        document.getElementById("spStatus").style.display = ""
    } else {
        if (htMarketData["liveBetting"]) {
            document.getElementById("imgLive").style.display = ""
        }
        document.getElementById("spTimeToStart").style.display = "";
        document.getElementById("spStatus").style.display = "none"
    }
    document.getElementById("spWinners").innerHTML = iNoOfWinners;
    document.getElementById("spCountry").innerHTML = htMarketData["country"];
    var objMarketTeaser = document.getElementById("spMarketTeaser");
    var objForumID = document.getElementById("spLinkToForumID");
    if (objMarketTeaser != null) {
        if (htMarketData["MarketTeaser"] != null && htMarketData["MarketTeaser"] != "") {
            MTTemplate = htMarketData["MarketTeaser"];
            var re = new RegExp("<A ", "g");
            var sValue = MTTemplate.replace(re, "<A class='aInsightPreviewMoreLink' ");
            var re = new RegExp("<a ", "g");
            sValue = sValue.replace(re, "<a class='aInsightPreviewMoreLink' ");
            objMarketTeaser.innerHTML = sValue + "<br/><br/>"
        } else {
            objMarketTeaser.innerHTML = ""
        }
    }
    if (objForumID != null) {
        if (htMarketData["forum"] != null && htMarketData["forumUrl"] != null && htMarketData["forum"] != "" && htMarketData["forumUrl"] != "") {
            if (linkTemplate.length == 0) { linkTemplate = objForumID.innerHTML }
            var re = new RegExp("@ForumName", "g");
            var sValue = linkTemplate.replace(re, htMarketData["forum"]);
            var re = new RegExp("@ForumUrl", "g");
            var sValue = sValue.replace(re, sForumURL + htMarketData["forumUrl"]);
            objForumID.innerHTML = sValue
        } else {
            objForumID.innerHTML = ""
        }
    }
    if (objMarketTeaser.innerHTML == "" && objForumID.innerHTML == "") {
        document.getElementById("spMarketContent").innerHTML = ""
    }
    document.getElementById("spMaxCommissionRate").innerHTML = iMarketCommission + "%";
    document.getElementById("spCommissionRebate").innerHTML = htMarketData["marketRebate"] + "%";
    if (htMarketData["marketRebate"] == 0) {
        document.getElementById("trCommissionRebate").style.display = "none"
    } else {
        document.getElementById("trCommissionRebate").style.display = ""
    }
    if (top.bLoggedIn) {
        document.getElementById("spLoyaltyDiscount").innerHTML = top.iDiscount + "%";
        document.getElementById("trLoyaltyDiscount").style.display = ""
    } else {
        document.getElementById("trLoyaltyDiscount").style.display = "none"
    }
    if (iDelay > 0 && sStatus == "Live") {
        document.getElementById("spLiveBettingDelay").innerHTML = iDelay;
        document.getElementById("trLiveBettingDelay").style.display = ""
    } else {
        document.getElementById("trLiveBettingDelay").style.display = "none"
    }
    document.getElementById("spDetails").innerHTML = htMarketData["details"];
    document.getElementById("spMarketDirectLink").innerHTML = GetShortcutMarketLink(false);
    document.getElementById("spExplanation").innerHTML = htMarketData["explanation"];
    document.getElementById("trForum").style.display = "none";
    if (arrRelatedMarketGroups.length > 0) {
        document.getElementById("dvCouponLinksSection").style.display = "";
        var iMarketGroupIDPos = 0;
        var iMarketGroupDescriptionPos = 1;
        var iMarketGroupTypePos = 2;
        var sMarketGroupLinks = "";
        for (i = 0; i < arrRelatedMarketGroups.length; i++) {
            sMarketGroupLinks += "<a href='MarketGroup.ashx?IMG=" + arrRelatedMarketGroups[i][iMarketGroupIDPos] + "&MGT=" + arrRelatedMarketGroups[i][iMarketGroupTypePos] + "'>" + arrRelatedMarketGroups[i][iMarketGroupDescriptionPos] + "</a><br />"
        }
        sMarketGroupLinks += "<br />";
        document.getElementById("spCouponLinks").innerHTML = sMarketGroupLinks
    } else {
        document.getElementById("dvCouponLinksSection").style.display = "none"
    }
    document.getElementById("dvStatusHolder").style.display = "";
    document.getElementById("dvStatusClosed").style.display = "none";
    document.getElementById("dvStatusSuspended").style.display = "none";
    switch (sStatus) {
        case "Closed":
            document.getElementById("dvStatusClosed").style.display = "";
            break;
        case "Suspended":
            document.getElementById("dvStatusSuspended").style.display = "";
            break
    };
    client_renderTimeToStart()
};

function client_renderEntrantDataWithOdds1(marketID, bUpdateEntrantOrder) {
    var start;
    var start1;
    var bShowIdentifier = false;
    try {
        if (sMarketID.toLowerCase() != marketID.toLowerCase()) return;
        var orderByDefault = false;
        var openEntrantsHTML = "";
        var closedEntrantsHTML = "";
        var openTemplate = "";
        var voidTemplate = "";
        var blankTemplate = "";
        if (iPLMarketDepth == MarketDepth_BestPriceOnly || iPLMarketDepth == MarketDepth_BestPriceAndStake) {
            openTemplate = document.getElementById("taEntrantWithTwoSideOneOddOpenRowTemplate").value;
            voidTemplate = document.getElementById("taEntrantVoidRowTwoSideOneOddTemplate").value;
            blankTemplate = document.getElementById("taEntrantBlankRowTwoSideOneOddTemplate").value
        } else {
            openTemplate = document.getElementById("taEntrantWithTwoSideThreeOddsOpenRowTemplate").value;
            voidTemplate = document.getElementById("taEntrantVoidRowTwoSideThreeOddsTemplate").value;
            blankTemplate = document.getElementById("taEntrantBlankRowTemplate").value
        }
        var openEntrantHTML = new StringBuffer();
        var voidEntrantHTML = new StringBuffer();
        var blankEntrantHTML = blankTemplate;
        var index = 0;
        arrEntrantIDs = new Array();
        arrEntrantIDsToIndex = new Array();
        for (var entrantID in htEntrants) {
            arrEntrantIDs[index] = entrantID;
            arrEntrantIDsToIndex[entrantID] = index;
            index = index + 1
        }
        arrEntrantOrderDefault = new Array();
        for (i = 0; i < arrEntrantIDs.length; i++) { arrEntrantOrderDefault[i] = i }
        var arrEntrantOrder = arrEntrantOrderDefault;
        entrantOrder = document.getElementById("selEntrantOrder").value;
        switch (entrantOrder) {
            case "Selection":
                arrEntrantOrder = arrEntrantOrderByName;
                htLastBestBetOrder = {};
                break;
            case "Identifier":
                arrEntrantOrder = arrEntrantOrderByIdentifier;
                htLastBestBetOrder = {};
                break;
            case "Odds":
                if (bUpdateEntrantOrder || arrEntrantOrderByOdds.length == 0) {
                    arrBestBetObj = new Array();
                    for (i = 0; i < arrEntrantOrder.length; i++) {
                        var entrantID = arrEntrantIDs[arrEntrantOrder[i]];
                        if (htEntrants[entrantID] == null) break;
                        var identifier = htEntrants[entrantID][iEtIdent];
                        var BestBackBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Back, htMinWin[iPLMinWin], identifier);
                        arrBestBetObj[arrBestBetObj.length] = new BestBetObj(BestBackBet[iBestEntrantIDPos], BestBackBet[iBestStakePos], BestBackBet[iBestOddPos], BestBackBet[iBestIdentifierPos])
                    }
                    arrBestBetObj.sort(sortByOdds);
                    htLastBestBetOrder = {};
                    arrEntrantOrderByOdds = new Array();
                    for (var i = 0; i < arrBestBetObj.length; i++) {
                        arrEntrantOrderByOdds[i] = arrEntrantIDsToIndex[arrBestBetObj[i].entrantID];
                        htLastBestBetOrder[arrBestBetObj[i].entrantID] = arrBestBetObj[i].odds
                    }
                } else {
                    var bIsChanged = false;
                    for (i = 0; i < arrBestBetObj.length; i++) {
                        var entrantID = arrBestBetObj[i].entrantID;
                        var identifier = htEntrants[entrantID][iEtIdent];
                        var BestBackBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Back, htMinWin[iPLMinWin], identifier);
                        arrBestBetObj[i] = new BestBetObj(BestBackBet[iBestEntrantIDPos], BestBackBet[iBestStakePos], BestBackBet[iBestOddPos], BestBackBet[iBestIdentifierPos]);
                        if (htLastBestBetOrder[entrantID] != null) {
                            if (htLastBestBetOrder[entrantID] != arrBestBetObj[i].odds) bIsChanged = true || bIsChanged
                        } else {
                            bIsChanged = true || bIsChanged
                        }
                    }
                    if (bIsChanged) {
                        document.getElementById("selEntrantOrder").value = ""
                    }
                }
                arrEntrantOrder = arrEntrantOrderByOdds;
                break;
            case "":
                if (htLastBestBetOrder.length > 0) arrEntrantOrder = arrEntrantOrderByOdds;
                break;
            default:
                document.getElementById("selEntrantOrder").value = "Default";
                break
        }
        var allBacked = true;
        var allLayed = true;
        var bookLay = 0;
        var bookBack = 0;
        if (sBetType == "Handicap" || sBetType == "Asian" || sBetType == "Totals") {
            allBacked = false;
            allLayed = false
        }
        iNoOfOpenEntrants = 0;
        var VoidTemplateArray = voidTemplate.split('@');
        var OpenTemplateArray = openTemplate.split('@');
        var marketIDLowerCase = marketID.toLowerCase();
        var iTime = 0;
        if (iPLMarketDepth == MarketDepth_BestPriceOnly) OpenTemplateArray[32] = OpenTemplateArray[32].replace("Odds", "OddsOnly");
        for (i = 0; i < arrEntrantOrder.length; i++) {
            if (htEntrants[entrantID] == null) break;
            var entrantID = arrEntrantIDs[arrEntrantOrder[i]];
            var entrantHTML = null;
            var entrantStatus = htEntrants[entrantID][iEtStatus];
            var plDisplay = htEntrants[entrantID][iEtProfitLossDisplay] == null ? 0 : htEntrants[entrantID][iEtProfitLossDisplay];
            var winDisplay = htEntrants[entrantID][iEtWinDisplay] == null ? 0 : htEntrants[entrantID][iEtWinDisplay];
            var loseDisplay = htEntrants[entrantID][iEtLoseDisplay] == null ? 0 : htEntrants[entrantID][iEtLoseDisplay];
            var arrOpenTemplate = null;
            if (entrantStatus != "Open") {
                arrOpenTemplate = VoidTemplateArray;
                var entrantIDLowerCase = entrantID.toLowerCase();
                arrOpenTemplate[3] = entrantIDLowerCase;
                arrOpenTemplate[9] = entrantIDLowerCase;
                arrOpenTemplate[13] = entrantIDLowerCase;
                arrOpenTemplate[17] = entrantIDLowerCase;
                arrOpenTemplate[19] = entrantIDLowerCase;
                arrOpenTemplate[21] = entrantIDLowerCase;
                arrOpenTemplate[5] = marketIDLowerCase;
                arrOpenTemplate[11] = marketIDLowerCase
            } else {
                arrOpenTemplate = OpenTemplateArray;
                var entrantIDLowerCase = entrantID.toLowerCase();
                arrOpenTemplate[3] = entrantIDLowerCase;
                arrOpenTemplate[9] = entrantIDLowerCase;
                arrOpenTemplate[13] = entrantIDLowerCase;
                arrOpenTemplate[29] = entrantIDLowerCase;
                arrOpenTemplate[39] = entrantIDLowerCase;
                arrOpenTemplate[17] = entrantIDLowerCase;
                arrOpenTemplate[21] = entrantIDLowerCase;
                arrOpenTemplate[25] = entrantIDLowerCase;
                arrOpenTemplate[5] = marketIDLowerCase;
                arrOpenTemplate[11] = marketIDLowerCase;
                arrOpenTemplate[31] = entrantIDLowerCase;
                arrOpenTemplate[35] = entrantIDLowerCase;
                arrOpenTemplate[45] = entrantIDLowerCase;
                if (iPLMarketDepth == MarketDepth_FullDepth) {
                    arrOpenTemplate[49] = entrantIDLowerCase;
                    arrOpenTemplate[59] = entrantIDLowerCase;
                    arrOpenTemplate[69] = entrantIDLowerCase;
                    arrOpenTemplate[79] = entrantIDLowerCase;
                    arrOpenTemplate[41] = entrantIDLowerCase;
                    arrOpenTemplate[51] = entrantIDLowerCase;
                    arrOpenTemplate[55] = entrantIDLowerCase;
                    arrOpenTemplate[61] = entrantIDLowerCase;
                    arrOpenTemplate[65] = entrantIDLowerCase;
                    arrOpenTemplate[71] = entrantIDLowerCase;
                    arrOpenTemplate[75] = entrantIDLowerCase;
                    arrOpenTemplate[81] = entrantIDLowerCase;
                    arrOpenTemplate[85] = entrantIDLowerCase
                }
            }
            var ident = "";
            if (sBetType == "Odds") {
                ident = htEntrants[entrantID][iEtIdent]
            }
            if (ident.length > 0) {
                ident = "[" + ident + "] ";
                bShowIdentifier = bShowIdentifier || true
            }
            arrOpenTemplate[15] = htEntrants[entrantID][iEtDesc];
            arrOpenTemplate[7] = ident;
            if (htEntrants[entrantID][iEtIconError] == null) {
                var entrantIcon = htEntrants[entrantID][iEtIcon];
                var entrantIconHtml = "";
                if (entrantIcon.length > 0) {
                    entrantIconHtml = "<img width=\"29\" height=\"21\" src=\"" + sEntrantIconsURL + entrantIcon + "\" onerror=\"ui_EntrantHideIcon('" + entrantID + "',this)\"  />"
                }
                arrOpenTemplate[1] = entrantIconHtml
            } else {
                arrOpenTemplate[1] = ""
            }
            if (entrantStatus == "Open") {
                if (htOdds[entrantID] == null) {
                    if (iPLMarketDepth == MarketDepth_BestPriceOnly || iPLMarketDepth == MarketDepth_BestPriceAndStake) {
                        arrOpenTemplate[33] = "&nbsp;";
                        arrOpenTemplate[37] = "&nbsp;";
                        arrOpenTemplate[43] = "&nbsp;";
                        arrOpenTemplate[47] = "&nbsp;";
                    } else {
                        arrOpenTemplate[53] = "&nbsp;";
                        arrOpenTemplate[57] = "&nbsp;";
                        arrOpenTemplate[63] = "&nbsp;";
                        arrOpenTemplate[67] = "&nbsp;";
                        arrOpenTemplate[43] = "&nbsp;";
                        arrOpenTemplate[47] = "&nbsp;";
                        arrOpenTemplate[33] = "&nbsp;";
                        arrOpenTemplate[37] = "&nbsp;";
                        arrOpenTemplate[73] = "&nbsp;";
                        arrOpenTemplate[77] = "&nbsp;";
                        arrOpenTemplate[83] = "&nbsp;";
                        arrOpenTemplate[87] = "&nbsp;";
                    }
                } else {
                    var BO1 = ""; var BA1 = ""; var BO2 = ""; var BA2 = "";
                    var BO3 = ""; var BA3 = ""; var LO1 = ""; var LA1 = "";
                    var LO2 = ""; var LA2 = ""; var LO3 = ""; var LA3 = "";
                    if (iPLMarketDepth == MarketDepth_BestPriceOnly) {
                        var BestBackBet;
                        if (entrantOrder == "Odds") {
                            BO1 = iif(arrBestBetObj[i].odds > 0, arrBestBetObj[i].odds, "");
                            arrOpenTemplate[33] = iif(arrBestBetObj[i].odds > 0, ConvertOdds(arrBestBetObj[i].odds), "&nbsp;")
                        } else {
                            BestBackBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Back, htMinWin[iPLMinWin]);
                            BO1 = iif(BestBackBet[iBestOddPos] > 0, BestBackBet[iBestOddPos], "");
                            arrOpenTemplate[33] = iif(BestBackBet[iBestOddPos] > 0, ConvertOdds(BestBackBet[iBestOddPos]), "&nbsp;")
                        }
                        var BestLayBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Lay, htMinWin[iPLMinWin]);
                        LO1 = iif(BestLayBet[iBestOddPos] > 0, BestLayBet[iBestOddPos], "");
                        arrOpenTemplate[43] = iif(BestLayBet[iBestOddPos] > 0, ConvertOdds(BestLayBet[iBestOddPos]), "&nbsp;");
                        arrOpenTemplate[34] = "";
                        arrOpenTemplate[35] = "";
                        arrOpenTemplate[36] = "";
                        arrOpenTemplate[37] = "";
                        arrOpenTemplate[44] = "";
                        arrOpenTemplate[45] = "";
                        arrOpenTemplate[46] = "";
                        arrOpenTemplate[47] = "";
                    } else if (iPLMarketDepth == MarketDepth_BestPriceAndStake) {
                        var BestBackBet;
                        if (entrantOrder == "Odds") {
                            BA1 = iif(arrBestBetObj[i].stake > 0, arrBestBetObj[i].stake, "&nbsp;");
                            BO1 = iif(arrBestBetObj[i].odds > 0, arrBestBetObj[i].odds, "");
                            arrOpenTemplate[33] = iif(arrBestBetObj[i].odds > 0, ConvertOdds(arrBestBetObj[i].odds), "&nbsp;");
                            arrOpenTemplate[37] = iif(arrBestBetObj[i].stake > 0, sSymbol + formatDollars(arrBestBetObj[i].stake), "&nbsp;")
                        } else {
                            BestBackBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Back, htMinWin[iPLMinWin]);
                            BA1 = iif(BestBackBet[iBestStakePos] > 0, BestBackBet[iBestStakePos], "&nbsp;");
                            BO1 = iif(BestBackBet[iBestOddPos] > 0, BestBackBet[iBestOddPos], "");
                            arrOpenTemplate[33] = iif(BestBackBet[iBestOddPos] > 0, ConvertOdds(BestBackBet[iBestOddPos]), "&nbsp;");
                            arrOpenTemplate[37] = iif(BestBackBet[iBestStakePos] > 0, sSymbol + formatDollars(BestBackBet[iBestStakePos]), "&nbsp;")
                        }
                        var BestLayBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Lay, htMinWin[iPLMinWin]);
                        LA1 = iif(BestLayBet[iBestStakePos] > 0, BestLayBet[iBestStakePos], "&nbsp;");
                        LO1 = iif(BestLayBet[iBestOddPos] > 0, BestLayBet[iBestOddPos], "");
                        arrOpenTemplate[43] = iif(BestLayBet[iBestOddPos] > 0, ConvertOdds(BestLayBet[iBestOddPos]), "&nbsp;");
                        arrOpenTemplate[47] = iif(BestLayBet[iBestStakePos] > 0, sSymbol + formatDollars(BestLayBet[iBestStakePos]), "&nbsp;")
                    } else {
                        BO1 = (htOdds[entrantID][iEtBO1] == '') ? '' : parseFloat(htOdds[entrantID][iEtBO1]);
                        BA1 = (htOdds[entrantID][iEtBA1] == '') ? '' : parseFloat(htOdds[entrantID][iEtBA1]);
                        BO2 = (htOdds[entrantID][iEtBO2] == '') ? '' : parseFloat(htOdds[entrantID][iEtBO2]);
                        BA2 = (htOdds[entrantID][iEtBA2] == '') ? '' : parseFloat(htOdds[entrantID][iEtBA2]);
                        BO3 = (htOdds[entrantID][iEtBO3] == '') ? '' : parseFloat(htOdds[entrantID][iEtBO3]);
                        BA3 = (htOdds[entrantID][iEtBA3] == '') ? '' : parseFloat(htOdds[entrantID][iEtBA3]);
                        BO4 = (htOdds[entrantID][iEtBO4] == '') ? '' : parseFloat(htOdds[entrantID][iEtBO4]);
                        BA4 = (htOdds[entrantID][iEtBA4] == '') ? '' : parseFloat(htOdds[entrantID][iEtBA4]);
                        BO5 = (htOdds[entrantID][iEtBO5] == '') ? '' : parseFloat(htOdds[entrantID][iEtBO5]);
                        BA5 = (htOdds[entrantID][iEtBA5] == '') ? '' : parseFloat(htOdds[entrantID][iEtBA5]);
                        LO1 = (htOdds[entrantID][iEtLO1] == '') ? '' : parseFloat(htOdds[entrantID][iEtLO1]);
                        LA1 = (htOdds[entrantID][iEtLA1] == '') ? '' : parseFloat(htOdds[entrantID][iEtLA1]);
                        LO2 = (htOdds[entrantID][iEtLO2] == '') ? '' : parseFloat(htOdds[entrantID][iEtLO2]);
                        LA2 = (htOdds[entrantID][iEtLA2] == '') ? '' : parseFloat(htOdds[entrantID][iEtLA2]);
                        LO3 = (htOdds[entrantID][iEtLO3] == '') ? '' : parseFloat(htOdds[entrantID][iEtLO3]);
                        LA3 = (htOdds[entrantID][iEtLA3] == '') ? '' : parseFloat(htOdds[entrantID][iEtLA3]);
                        LO4 = (htOdds[entrantID][iEtLO4] == '') ? '' : parseFloat(htOdds[entrantID][iEtLO4]);
                        LA4 = (htOdds[entrantID][iEtLA4] == '') ? '' : parseFloat(htOdds[entrantID][iEtLA4]);
                        LO5 = (htOdds[entrantID][iEtLO5] == '') ? '' : parseFloat(htOdds[entrantID][iEtLO5]);
                        LA5 = (htOdds[entrantID][iEtLA5] == '') ? '' : parseFloat(htOdds[entrantID][iEtLA5]);
                        if (BO1 == "") {
                            arrOpenTemplate[53] = "&nbsp;"; arrOpenTemplate[57] = ""
                        } else {
                            arrOpenTemplate[53] = ConvertOdds(BO1);
                            arrOpenTemplate[57] = sSymbol + formatDollars(BA1)
                        }
                        if (BO2 == "") {
                            arrOpenTemplate[43] = "&nbsp;";
                            arrOpenTemplate[47] = ""
                        } else {
                            arrOpenTemplate[43] = ConvertOdds(BO2);
                            arrOpenTemplate[47] = sSymbol + formatDollars(BA2)
                        }
                        if (BO3 == "") {
                            arrOpenTemplate[33] = "&nbsp;";
                            arrOpenTemplate[37] = ""
                        } else {
                            arrOpenTemplate[33] = ConvertOdds(BO3);
                            arrOpenTemplate[37] = sSymbol + formatDollars(BA3)
                        }
                        if (LO1 == "") {
                            arrOpenTemplate[63] = "&nbsp;"; arrOpenTemplate[67] = ""
                        } else {
                            arrOpenTemplate[63] = ConvertOdds(LO1);
                            arrOpenTemplate[67] = sSymbol + formatDollars(LA1)
                        }
                        if (LO2 == "") {
                            arrOpenTemplate[73] = "&nbsp;";
                            arrOpenTemplate[77] = ""
                        } else {
                            arrOpenTemplate[73] = ConvertOdds(LO2);
                            arrOpenTemplate[77] = sSymbol + formatDollars(LA2)
                        }
                        if (LO3 == "") {
                            arrOpenTemplate[83] = "&nbsp;";
                            arrOpenTemplate[87] = ""
                        } else {
                            arrOpenTemplate[83] = ConvertOdds(LO3);
                            arrOpenTemplate[87] = sSymbol + formatDollars(LA3)
                        }
                    }
                    if (BO1 != "" && allBacked) {
                        bookBack = bookBack + 1 / BO1
                    } else { allBacked = false }
                    if (LO1 != "" && allLayed) { bookLay = bookLay + 1 / LO1 } else { allLayed = false }
                }
            }
            if (entrantStatus == "Open") {
                if (sBetType == "Odds" && top.bLoggedIn && !top.userGetSetting(top.cHideProfitLoss) && plDisplay != 0) {
                    arrOpenTemplate[19] = "";
                    if (iNoOfWinners == 1) {
                        if (plDisplay >= 0) {
                            arrOpenTemplate[23] = formatCurrency(plDisplay, sSymbol, numDec);
                            arrOpenTemplate[27] = ""
                        } else {
                            arrOpenTemplate[23] = "";
                            arrOpenTemplate[27] = formatCurrency(plDisplay, sSymbol, numDec)
                        }
                    } else {
                        if (winDisplay == 0 && loseDisplay == 0) {
                            arrOpenTemplate[23] = ""; arrOpenTemplate[27] = ""
                        } else {
                            arrOpenTemplate[23] = formatCurrency(winDisplay, sSymbol, numDec) + ", ";
                            arrOpenTemplate[27] = formatCurrency(loseDisplay, sSymbol, numDec)
                        }
                    }
                } else {
                    arrOpenTemplate[19] = "display:none"
                }
            }
            entrantHTML = arrOpenTemplate.join("");
            if (entrantStatus == "Open" && sBetType == "Odds" && top.bLoggedIn && !top.userGetSetting(top.cHideProfitLoss) && iNoOfWinners > 1) {
                if (winDisplay < 0) entrantHTML = entrantHTML.replace("PLWin", "PLTmpLose");
                if (loseDisplay >= 0) entrantHTML = entrantHTML.replace("PLLose", "PLWin");
                entrantHTML = entrantHTML.replace("PLTmpLose", "PLLose")
            }
            if (entrantStatus == "Open") {
                if (sBetType != "Odds" && i > 0 && i % 2 == 0) {
                    if (tpMarketFlag == "N") openEntrantHTML.append(blankEntrantHTML)
                }
                iNoOfOpenEntrants = iNoOfOpenEntrants + 1;
                openEntrantHTML.append(entrantHTML)
            } else {
                if (sBetType != "Odds" && i > 0 && i % 2 == 0) {
                    if (tpMarketFlag == "N")
                        voidEntrantHTML.append(blankEntrantHTML)
                }
                voidEntrantHTML.append(entrantHTML)
            }
        }
        document.getElementById("spSelections").innerHTML = iNoOfOpenEntrants;
        var entrantTable = document.getElementById("taEntrantTableTemplate1").value;
        ui_changeSortingDropDown("Identifier", bShowIdentifier);
        var arrEntrantTable = entrantTable.split('@');
        document.getElementById("dvEntrantsTable").innerHTML = arrEntrantTable[0] + openEntrantHTML.toString() + voidEntrantHTML.toString() + arrEntrantTable[2];
        if (allBacked) {
            bookBack = formatPercentage(bookBack * 100);
            document.getElementById("spBackBook").innerHTML = bookBack;
            if (bPLShowBook) {
                document.getElementById("spBackBook").style.display = ""
            } else {
                document.getElementById("spBackBook").style.display = "none"
            }
        } else {
            document.getElementById("spBackBook").innerHTML = ""
        }
        if (allLayed) {
            bookLay = formatPercentage(bookLay * 100);
            document.getElementById("spLayBook").innerHTML = bookLay;
            if (bPLShowBook) {
                document.getElementById("spLayBook").style.display = ""
            } else {
                document.getElementById("spLayBook").style.display = "none"
            }
        } else {
            document.getElementById("spLayBook").innerHTML = ""
        }
    } catch (e) {
        top.showMessageBox("client_renderEntrantDataWithOdds1:" + e.message, null, null)
    }
    client_renderWindowResize();
    window.setTimeout("client_renderWindowResize()", timeoutResize)
};
function client_renderEntrantDataWithOdds(marketID) {
    try {
        if (sMarketID.toLowerCase() != marketID.toLowerCase()) return;
        var orderByDefault = false;
        var openEntrantsHTML = "";
        var closedEntrantsHTML = "";
        var openTemplate = document.getElementById("taEntrantWithOddsOpenRowTemplate").value;
        var voidTemplate = document.getElementById("taEntrantVoidRowTemplate").value;
        var blankTemplate = document.getElementById("taEntrantBlankRowTemplate").value;
        var openEntrantHTML = "";
        var voidEntrantHTML = "";
        var blankEntrantHTML = blankTemplate;
        var index = 0;
        arrEntrantIDs = new Array();
        for (var entrantID in htEntrants) {
            arrEntrantIDs[index] = entrantID;
            index = index + 1
        };
        arrEntrantOrderDefault = new Array();
        for (i = 0; i < arrEntrantIDs.length; i++) {
            arrEntrantOrderDefault[i] = i
        }
        var arrEntrantOrder = arrEntrantOrderDefault;
        var entrantOrder = document.getElementById("selEntrantOrder").value;
        switch (entrantOrder) {
            case "Selection":
                arrEntrantOrder = arrEntrantOrderByName;
                break
        }
        var allBacked = true; var allLayed = true;
        var bookLay = 0; var bookBack = 0;
        if (sBetType == "Handicap" || sBetType == "Asian" || sBetType == "Totals") {
            allBacked = false; allLayed = false
        }
        iNoOfOpenEntrants = 0;
        for (i = 0; i < arrEntrantOrder.length; i++) {
            if (htEntrants[entrantID] == null) break;
            var entrantID = arrEntrantIDs[arrEntrantOrder[i]];
            var entrantHTML = openTemplate;
            var entrantStatus = htEntrants[entrantID][iEtStatus];
            var plDisplay = htEntrants[entrantID][iEtProfitLossDisplay] == null ? 0 : htEntrants[entrantID][iEtProfitLossDisplay];
            var winDisplay = htEntrants[entrantID][iEtWinDisplay] == null ? 0 : htEntrants[entrantID][iEtWinDisplay];
            var loseDisplay = htEntrants[entrantID][iEtLoseDisplay] == null ? 0 : htEntrants[entrantID][iEtLoseDisplay];
            if (entrantStatus != "Open") { entrantHTML = voidTemplate }
            var re = new RegExp("@IE", "g");
            entrantHTML = entrantHTML.replace(re, entrantID.toLowerCase());
            re = new RegExp("@IM", "g");
            entrantHTML = entrantHTML.replace(re, sMarketID.toLowerCase());
            re = new RegExp("@DE", "g");
            var ident = "";
            if (sBetType == "Odds") { ident = htEntrants[entrantID][iEtIdent] }
            if (ident.length > 0) { ident = "[" + ident + "] " }
            entrantHTML = entrantHTML.replace(re, htEntrants[entrantID][iEtDesc]);
            var re = new RegExp("@QI", "g");
            entrantHTML = entrantHTML.replace(re, ident);
            if (htEntrants[entrantID][iEtIconError] == null) {
                var entrantIcon = htEntrants[entrantID][iEtIcon];
                var entrantIconHtml = "";
                if (entrantIcon.length > 0) {
                    entrantIconHtml = "<img width=\"29\" height=\"21\" src=\"" + sEntrantIconsURL + entrantIcon + "\" onerror=\"ui_EntrantHideIcon('" + entrantID + "',this)\"  />"
                }
                re = new RegExp("@XIcon", "g");
                entrantHTML = entrantHTML.replace(re, entrantIconHtml)
            } else {
                re = new RegExp("@XIcon", "g");
                entrantHTML = entrantHTML.replace(re, "")
            }
            if (entrantStatus == "Open") {
                if (htOdds[entrantID] == null) {
                    re = new RegExp("@BO1", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@BA1", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@BO2", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@BA2", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@BO3", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@BA3", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@LO1", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@LA1", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@LO2", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@LA2", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@LO3", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;");
                    re = new RegExp("@LA3", "g");
                    entrantHTML = entrantHTML.replace(re, "&nbsp;")
                } else {
                    var BO1 = htOdds[entrantID][iEtBO1];
                    var BA1 = htOdds[entrantID][iEtBA1];
                    var BO2 = htOdds[entrantID][iEtBO2];
                    var BA2 = htOdds[entrantID][iEtBA2];
                    var BO3 = htOdds[entrantID][iEtBO3];
                    var BA3 = htOdds[entrantID][iEtBA3];
                    var LO1 = htOdds[entrantID][iEtLO1];
                    var LA1 = htOdds[entrantID][iEtLA1];
                    var LO2 = htOdds[entrantID][iEtLO2];
                    var LA2 = htOdds[entrantID][iEtLA2];
                    var LO3 = htOdds[entrantID][iEtLO3];
                    var LA3 = htOdds[entrantID][iEtLA3];
                    re = new RegExp("@BO1", "g");
                    var re2 = new RegExp("@BA1", "g");
                    if (BO1 == "") {
                        entrantHTML = entrantHTML.replace(re, "&nbsp;");
                        entrantHTML = entrantHTML.replace(re2, "")
                    } else {
                        entrantHTML = entrantHTML.replace(re, ConvertOdds(BO1));
                        entrantHTML = entrantHTML.replace(re2, sSymbol + formatDollars(BA1))
                    }
                    re = new RegExp("@BO2", "g");
                    re2 = new RegExp("@BA2", "g");
                    if (BO2 == "") {
                        entrantHTML = entrantHTML.replace(re, "&nbsp;");
                        entrantHTML = entrantHTML.replace(re2, "")
                    } else {
                        entrantHTML = entrantHTML.replace(re, ConvertOdds(BO2));
                        entrantHTML = entrantHTML.replace(re2, sSymbol + formatDollars(BA2))
                    }
                    re = new RegExp("@BO3", "g");
                    re2 = new RegExp("@BA3", "g");
                    if (BO3 == "") {
                        entrantHTML = entrantHTML.replace(re, "&nbsp;");
                        entrantHTML = entrantHTML.replace(re2, "")
                    } else {
                        entrantHTML = entrantHTML.replace(re, ConvertOdds(BO3));
                        entrantHTML = entrantHTML.replace(re2, sSymbol + formatDollars(BA3))
                    }
                    re = new RegExp("@LO1", "g");
                    re2 = new RegExp("@LA1", "g");
                    if (LO1 == "") {
                        entrantHTML = entrantHTML.replace(re, "&nbsp;");
                        entrantHTML = entrantHTML.replace(re2, "")
                    } else {
                        entrantHTML = entrantHTML.replace(re, ConvertOdds(LO1));
                        entrantHTML = entrantHTML.replace(re2, sSymbol + formatDollars(LA1))
                    }
                    re = new RegExp("@LO2", "g");
                    re2 = new RegExp("@LA2", "g");
                    if (LO2 == "") {
                        entrantHTML = entrantHTML.replace(re, "&nbsp;");
                        entrantHTML = entrantHTML.replace(re2, "")
                    } else {
                        entrantHTML = entrantHTML.replace(re, ConvertOdds(LO2));
                        entrantHTML = entrantHTML.replace(re2, sSymbol + formatDollars(LA2))
                    }
                    re = new RegExp("@LO3", "g");
                    re2 = new RegExp("@LA3", "g");
                    if (LO3 == "") {
                        entrantHTML = entrantHTML.replace(re, "&nbsp;");
                        entrantHTML = entrantHTML.replace(re2, "")
                    } else {
                        entrantHTML = entrantHTML.replace(re, ConvertOdds(LO3));
                        entrantHTML = entrantHTML.replace(re2, sSymbol + formatDollars(LA3))
                    }
                    if (BO1 != "" && allBacked) {
                        bookBack = bookBack + 1 / BO1
                    } else { allBacked = false }
                    if (LO1 != "" && allLayed) { bookLay = bookLay + 1 / LO1 } else { allLayed = false }
                }
            }
            if (entrantStatus == "Open") {
                if (sBetType == "Odds" && top.bLoggedIn && !top.userGetSetting(top.cHideProfitLoss) && plDisplay != 0) {
                    re = new RegExp("@PLDisplay", "g");
                    entrantHTML = entrantHTML.replace(re, "");
                    if (iNoOfWinners == 1) {
                        if (plDisplay >= 0) {
                            re = new RegExp("@PLWin", "g");
                            entrantHTML = entrantHTML.replace(re, formatCurrency(plDisplay, sSymbol, numDec));
                            re = new RegExp("@PLLose", "g");
                            entrantHTML = entrantHTML.replace(re, "")
                        } else {
                            re = new RegExp("@PLLose", "g");
                            entrantHTML = entrantHTML.replace(re, formatCurrency(plDisplay, sSymbol, numDec));
                            re = new RegExp("@PLWin", "g");
                            entrantHTML = entrantHTML.replace(re, "")
                        }
                    } else {
                        re = new RegExp("@PLWin", "g");
                        entrantHTML = entrantHTML.replace(re, formatCurrency(winDisplay, sSymbol, numDec) + ", ");
                        re = new RegExp("@PLLose", "g");
                        entrantHTML = entrantHTML.replace(re, formatCurrency(loseDisplay, sSymbol, numDec));
                        if (winDisplay < 0) {
                            re = new RegExp("PLWin", "g");
                            entrantHTML = entrantHTML.replace(re, "PLLose")
                        }
                        if (loseDisplay >= 0) {
                            re = new RegExp("PLLose", "g");
                            entrantHTML = entrantHTML.replace(re, "PLWin")
                        }
                    }
                } else {
                    re = new RegExp("@PLDisplay", "g");
                    entrantHTML = entrantHTML.replace(re, "display:none")
                }
            }
            if (entrantStatus == "Open") {
                if (sBetType != "Odds" && i > 0 && i % 2 == 0) {
                    if (tpMarketFlag == "N") openEntrantHTML += blankEntrantHTML
                }
                iNoOfOpenEntrants = iNoOfOpenEntrants + 1;
                openEntrantHTML = openEntrantHTML + entrantHTML
            } else {
                if (sBetType != "Odds" && i > 0 && i % 2 == 0) {
                    if (tpMarketFlag == "N") voidEntrantHTML += blankEntrantHTML
                }
                voidEntrantHTML = voidEntrantHTML + entrantHTML
            }
        }
        document.getElementById("spSelections").innerHTML = iNoOfOpenEntrants;
        var entrantTable = document.getElementById("taEntrantTableTemplate").value;
        var re = new RegExp("@Odds", "g");
        entrantTable = entrantTable.replace(re, openEntrantHTML + voidEntrantHTML);
        document.getElementById("dvEntrantsTable").innerHTML = entrantTable;
        if (allBacked) {
            bookBack = formatPercentage(bookBack * 100);
            document.getElementById("spBackBook").innerHTML = bookBack
        } else {
            document.getElementById("spBackBook").innerHTML = ""
        }
        if (iPLMarketView == top.MarketView_ExchangeView) {
            if (allLayed) {
                bookLay = formatPercentage(bookLay * 100);
                document.getElementById("spLayBook").innerHTML = bookLay
            } else {
                document.getElementById("spLayBook").innerHTML = ""
            }
        }
    } catch (e) {
        top.showMessageBox("client_renderEntrantDataWithOdds:" + e.message, null, null)
    }
    client_renderWindowResize();
    window.setTimeout("client_renderWindowResize()", timeoutResize)
};

function client_renderTimeToStart() {
    try {
        var gmtNow = new Date();
        gmtNow.setMinutes(gmtNow.getMinutes() + gmtNow.getTimezoneOffset());
        var gmtNow = gmtNow.getTime();
        var diffSecs = dStartDateTime.getTime() - gmtNow;
        diffSecs = diffSecs / 1000;
        if (diffSecs < 0) { diffSecs = 0 }
        var days = Math.floor(diffSecs / (60 * 60 * 24));
        diffSecs -= days * 60 * 60 * 24;
        var hours = Math.floor(diffSecs / (60 * 60));
        diffSecs -= hours * 60 * 60;
        var minutes = Math.floor(diffSecs / 60);
        diffSecs -= minutes * 60;
        var sDisplay = days + " " + sDays + " " + hours + " " + sHours + " " + minutes + " " + sMinutes + " " + Math.round(diffSecs) + " " + sSeconds;
        document.getElementById("spTimeToStart").innerHTML = sDisplay
    } catch (e) {
        top.showMessageBox("client_renderTimeToStart:" + e.message, null, null)
    }
};
function ui_doResize() { window.setTimeout("client_renderWindowResize()", 200) };
function client_renderWindowResize() {
    try {
        if (document.getElementById("tblMarket").style.display == "") {
            if (bMarketIsChanged) { ResetScrollBar() }
            var totalHeight = document.getElementById("tdMarketHeaderBar").offsetHeight + document.getElementById("tdMarketInfo").offsetHeight + document.getElementById("tdMarketInfoMore").offsetHeight + (document.getElementById("tdOddsHeader").offsetHeight * 2) + 5;
            var maxHeight = getWindowHeight() - totalHeight - 5 - 0;
            var oddsTableHeight = document.getElementById("tblEntrants").offsetHeight;
            var height = (oddsTableHeight > maxHeight ? maxHeight : oddsTableHeight + 1);
            if (oddsTableHeight > maxHeight) document.getElementById("dvEntrantsTable").style.height = maxHeight + "px";
            else document.getElementById("dvEntrantsTable").style.height = "100%";
            document.getElementById("tdBackAllHeader").className = "BackAllHeader";
            document.getElementById("tdBackAllFooter").className = "BackAllHeader";
            document.getElementById("tdEntrantHeader").className = "EntrantHeader";
            document.getElementById("tdEntrantFooter").className = "EntrantHeader";
            if (iPLMarketView == top.MarketView_SportsbookView) {
                document.getElementById("tdLayAllHeader").style.display = "none";
                document.getElementById("tdLayAllFooter").style.display = "none";
                document.getElementById("tdEntrantHeader").style.width = "28%";
                document.getElementById("tdEntrantFooter").style.width = "28%";
                document.getElementById("tdBackAllHeader").style.width = "36%";
                document.getElementById("tdBackAllFooter").style.width = "36%";
                document.getElementById("btnBackAll").style.display = "none";
                if (iPLMarketDepth == MarketDepth_BestPriceOnly || iPLMarketDepth == MarketDepth_BestPriceAndStake) {
                    document.getElementById("tdBackAllHeader").className = "BackAllHeaderInGrey";
                    document.getElementById("tdBackAllFooter").className = "BackAllHeaderInGrey";
                    document.getElementById("tdEntrantHeader").className = "EntrantHeaderInGrey";
                    document.getElementById("tdEntrantFooter").className = "EntrantHeaderInGrey";
                    document.getElementById("tdHeaderLeft").style.width = "70px";
                    document.getElementById("tdHeaderRight").style.width = "61px";
                    document.getElementById("tdHeaderLeft").style.display = "";
                    document.getElementById("tdHeaderRight").style.display = ""
                } else {
                    document.getElementById("tdBackAllHeader").style.width = "68%";
                    document.getElementById("tdBackAllFooter").style.width = "68%";
                    document.getElementById("tdHeaderLeft").style.display = "none";
                    document.getElementById("tdHeaderRight").style.display = "none"
                }
            } else {
                document.getElementById("tdHeaderLeft").style.display = "none";
                document.getElementById("tdHeaderRight").style.display = "none";
                document.getElementById("tdLayAllHeader").className = "LayAllHeader";
                document.getElementById("tdLayAllFooter").className = "LayAllHeader";
                document.getElementById("btnBackAll").style.display = "";
                document.getElementById("tdLayAllFooter").style.display = "";
                document.getElementById("tdLayAllHeader").style.display = ""
            }
            var sortingDisplayStr = "";
            if (iNoOfOpenEntrants <= 5 || sBetType == "Handicap" || sBetType == "Asian" || sBetType == "Totals") { sortingDisplayStr = "none" }
            document.getElementById("spSortBy").style.display = sortingDisplayStr;
            document.getElementById("selEntrantOrder").style.display = sortingDisplayStr;
            var adjustWidth = 0;
            if (oddsTableHeight > maxHeight) {
                if (iPLMarketView == top.MarketView_SportsbookView) {
                    document.getElementById("tdBackAllHeader").style.width = "36%";
                    document.getElementById("tdBackAllFooter").style.width = "36%"
                } else { adjustWidth = 16; }
            } else {
                if (iPLMarketView == top.MarketView_SportsbookView) {
                    document.getElementById("tdBackAllHeader").style.width = "36%";
                    document.getElementById("tdBackAllFooter").style.width = "36%"
                } else { }
            }
            if (iPLMarketView == top.MarketView_ExchangeView) {
                var back1Width = getCellWidthByName("dvEntrantsTable", "tdBack1");
                var back2Width = getCellWidthByName("dvEntrantsTable", "tdBack2");
                var back3Width = getCellWidthByName("dvEntrantsTable", "tdBack3");
                var back4Width = getCellWidthByName("dvEntrantsTable", "tdBack4");
                var lay1Width = getCellWidthByName("dvEntrantsTable", "tdLay1");
                var lay2Width = getCellWidthByName("dvEntrantsTable", "tdLay2");
                var lay3Width = getCellWidthByName("dvEntrantsTable", "tdLay3");
                var lay4Width = getCellWidthByName("dvEntrantsTable", "tdLay4");
                var totalBackWidth = back1Width + back2Width + back3Width + back4Width;
                var totalLayWidth = lay1Width + lay2Width + lay3Width + lay4Width + adjustWidth;
                var entrantWidth = getCellWidthByName("dvEntrantsTable", "tdEntrantOpen");
                document.getElementById("tdEntrantFooter").style.width = (entrantWidth - 10) + "px";
                document.getElementById("tdEntrantHeader").style.width = (entrantWidth - 10) + "px";
                document.getElementById("tdBackAllHeader").style.width = (totalBackWidth - 1) + "px";
                document.getElementById("tdBackAllFooter").style.width = (totalBackWidth - 1) + "px";
                document.getElementById("tdLayAllHeader").style.width = (totalLayWidth - 1) + "px";
                document.getElementById("tdLayAllFooter").style.width = (totalLayWidth - 1) + "px";
            }
        }
    } catch (e) { }
};
function client_synchroniseTree(sHierarchyID) { if (top.synchroniseTree) { top.synchroniseTree(sHierarchyID) } };
function helper_setHTML(id, HTML) {
    document.getElementById(id).innerHTML = HTML
};
var iBetEntrantID = 0; var iBetStake = 1; var iBetOdds = 2;
var iBetAmountMatched = 3; var iBetOddsMatched = 4; var iBetFreeBetID = 5;
var iBetEdited = 6; var iBetAmountBeforeEdit = 7; var iBetOddsBeforeEdit = 8;
var betState_UnPlaced = "UPB"; var betState_UnMatched = "UMB";
var betState_Matched = "MB"; var betState_UnPlacedFree = "UPFB";
var betState_UnMatchedFree = "UMFB"; var betSide_Back = "Back";
var betSide_Lay = "Lay"; var space = "&nbsp;";
var htPlaceBetsBack = {}; var htPlaceBetsLay = {}; var bPlaceBetsLayShowProfit = true;
var iNoOfPlacedBets = 0; var htMyBetsBackUnMatched = null; var htMyBetsLayUnMatched = null;
var htMyBetsBackMatched = null; var htMyBetsLayMatched = null;
var bUnMatchedBetsLayShowProfit = true; var bMatchedBetsLayShowProfit = true;
function server_getBetData() {
    if (BeginProcessing()) {
        frames["ifraOrdersRequest"].location = "OrdersGetJS" + sExt + "?marketID=" + sMarketID
    }
};
function server_addBets() {
    if (top.bLoggedIn) {
        if (BeginProcessing()) {
            var bHasBets = false; var orderCount = 0; var freeBetIDs = "";
            var entrantIDs = ""; var sides = ""; var amounts = ""; var odds = "";
            var orderType = sBetType;
            for (var freeBetID in htMyFreeBets) {
                if (htMyFreeBets[freeBetID][iFreeOdds].toString() != "") {
                    edited = htMyFreeBets[freeBetID][iFreeEdited];
                    if (edited) {
                        orderCount += 1;
                        freeBetIDs += ";" + freeBetID;
                        entrantIDs += ";" + htMyFreeBets[freeBetID][iFreeEntrant];
                        sides = sides + ";" + betSide_Back;
                        amounts = amounts + ";" + htMyFreeBets[freeBetID][iFreeOutstandingAmount].toString();
                        odds = odds + ";" + htMyFreeBets[freeBetID][iFreeOdds].toString()
                    }
                }
            }
            freeBetIDs = freeBetIDs.substr(1, freeBetIDs.length - 1);
            entrantIDs = entrantIDs.substr(1, entrantIDs.length - 1);
            sides = sides.substr(1, sides.length - 1);
            amounts = amounts.substr(1, amounts.length - 1);
            odds = odds.substr(1, odds.length - 1);
            var frmOrders = document.getElementById("frmOrders");
            frmOrders.reset();
            document.getElementById("txtOrderCount").value = orderCount;
            document.getElementById("txtFreeBetIDs").value = freeBetIDs;
            document.getElementById("txtEntrantIDs").value = entrantIDs;
            document.getElementById("txtSides").value = sides;
            document.getElementById("txtAmounts").value = amounts;
            document.getElementById("txtOdds").value = odds;
            document.getElementById("txtOrderType").value = orderType;
            if (orderCount > 0) bHasBets = true;
            orderCount = 0;
            entrantIDs = "";
            sides = "";
            amounts = "";
            odds = "";
            for (var entrantID in htPlaceBetsBack) {
                var singleStake = htPlaceBetsBack[entrantID][iBetStake].toString();
                var singleOdds = htPlaceBetsBack[entrantID][iBetOdds].toString();
                if (singleStake != "" && singleOdds != "") {
                    orderCount = orderCount + 1;
                    entrantIDs = entrantIDs + ";" + entrantID;
                    sides = sides + ";" + betSide_Back;
                    amounts = amounts + ";" + htPlaceBetsBack[entrantID][iBetStake].toString();
                    odds = odds + ";" + htPlaceBetsBack[entrantID][iBetOdds].toString()
                }
            }
            for (var entrantID in htPlaceBetsLay) {
                var singleStake = htPlaceBetsLay[entrantID][iBetStake].toString();
                var singleOdds = htPlaceBetsLay[entrantID][iBetOdds].toString();
                if (singleStake != "" && singleOdds != "") {
                    orderCount = orderCount + 1;
                    entrantIDs = entrantIDs + ";" + entrantID;
                    sides = sides + ";" + betSide_Lay;
                    amounts = amounts + ";" + htPlaceBetsLay[entrantID][iBetStake].toString();
                    odds = odds + ";" + htPlaceBetsLay[entrantID][iBetOdds].toString()
                }
            }
            entrantIDs = entrantIDs.substr(1, entrantIDs.length - 1);
            sides = sides.substr(1, sides.length - 1);
            amounts = amounts.substr(1, amounts.length - 1);
            odds = odds.substr(1, odds.length - 1);
            document.getElementById("txtMarketID").value = sMarketID;
            document.getElementById("txtOrderCount").value += "|" + orderCount;
            document.getElementById("txtEntrantIDs").value += "|" + entrantIDs;
            document.getElementById("txtSides").value += "|" + sides;
            document.getElementById("txtAmounts").value += "|" + amounts;
            document.getElementById("txtOdds").value += "|" + odds;
            document.getElementById("txtOrderType").value = orderType;
            if (orderCount > 0) bHasBets = true;
            if (!bHasBets) { EndProcessing(); return }
            frmOrders.action = "OrdersAddJS" + sExt + "?ts=" + iTimestamp + "&src=mkt";
            frmOrders.submit();
            var statusQueueDelayEnabled = top.readCookie("StatusQueueDelayEnabled");
            var bShowInPlayDelay = true;
            if (statusQueueDelayEnabled != null && statusQueueDelayEnabled == '1') bShowInPlayDelay = false;
            if (sStatus == "Live" && bShowInPlayDelay) { client_startInPlayDelay() }
        }
    } else {
        forceLogin()
    }
};
function server_amendBets() {
    if (top.bLoggedIn) {
        if (BeginProcessing()) {
            var orderCount = 0; var orderIDs = "";
            var amounts = ""; var odds = "";
            var amountsOriginal = ""; var amountsMatched = "";
            var freeBetIDs = ""; var entrantIDs = "";
            for (var orderID in htMyBetsBackUnMatched) {
                if (htMyBetsBackUnMatched[orderID][iBetEdited] == true) {
                    orderCount = orderCount + 1;
                    orderIDs = orderIDs + ";" + orderID;
                    var iFullAmount = htMyBetsBackUnMatched[orderID][iBetStake] * 1 + htMyBetsBackUnMatched[orderID][iBetAmountMatched] * 1;
                    var iFullAmountOriginal = htMyBetsBackUnMatched[orderID][iBetAmountBeforeEdit] * 1 + htMyBetsBackUnMatched[orderID][iBetAmountMatched] * 1;
                    amounts = amounts + ";" + iFullAmount.toString();
                    odds = odds + ";" + htMyBetsBackUnMatched[orderID][iBetOdds].toString();
                    amountsOriginal = amountsOriginal + ";" + iFullAmountOriginal.toString();
                    amountsMatched = amountsMatched + ";" + htMyBetsBackUnMatched[orderID][iBetAmountMatched].toString();
                    freeBetIDs += ";" + htMyBetsBackUnMatched[orderID][iBetFreeBetID].toString();
                    entrantIDs += ";" + htMyBetsBackUnMatched[orderID][iBetEntrantID].toString()
                }
            }
            for (var orderID in htMyBetsLayUnMatched) {
                if (htMyBetsLayUnMatched[orderID][iBetEdited] == true) {
                    orderCount = orderCount + 1;
                    orderIDs = orderIDs + ";" + orderID;
                    var iFullAmount = htMyBetsLayUnMatched[orderID][iBetStake] * 1 + htMyBetsLayUnMatched[orderID][iBetAmountMatched] * 1;
                    var iFullAmountOriginal = htMyBetsLayUnMatched[orderID][iBetAmountBeforeEdit] * 1 + htMyBetsLayUnMatched[orderID][iBetAmountMatched] * 1;
                    amounts = amounts + ";" + iFullAmount.toString();
                    odds = odds + ";" + htMyBetsLayUnMatched[orderID][iBetOdds].toString();
                    amountsOriginal = amountsOriginal + ";" + iFullAmountOriginal.toString();
                    amountsMatched = amountsMatched + ";" + htMyBetsLayUnMatched[orderID][iBetAmountMatched].toString();
                    freeBetIDs += ";" + htMyBetsLayUnMatched[orderID][iBetFreeBetID].toString();
                    entrantIDs += ";" + htMyBetsLayUnMatched[orderID][iBetEntrantID].toString()
                }
            }
            if (orderCount == 0) { EndProcessing(); return }
            orderIDs = orderIDs.substr(1, orderIDs.length - 1);
            freeBetIDs = freeBetIDs.substr(1, freeBetIDs.length - 1);
            amounts = amounts.substr(1, amounts.length - 1);
            odds = odds.substr(1, odds.length - 1);
            amountsOriginal = amountsOriginal.substr(1, amountsOriginal.length - 1);
            amountsMatched = amountsMatched.substr(1, amountsMatched.length - 1);
            entrantIDs = entrantIDs.substr(1, entrantIDs.length - 1);
            var frmOrders = document.getElementById("frmOrders");
            frmOrders.reset();
            document.getElementById("txtMarketID").value = sMarketID;
            document.getElementById("txtOrderCount").value = orderCount;
            document.getElementById("txtOrderIDs").value = orderIDs;
            document.getElementById("txtAmounts").value = amounts;
            document.getElementById("txtOdds").value = odds;
            document.getElementById("txtAmountsOriginal").value = amountsOriginal;
            document.getElementById("txtAmountsMatched").value = amountsMatched;
            document.getElementById("txtFreeBetIDs").value = freeBetIDs;
            document.getElementById("txtEntrantIDs").value = entrantIDs;
            frmOrders.action = "OrdersAmendJS" + sExt + "?ts=" + iTimestamp + "&src=mkt";
            frmOrders.submit();
            var statusQueueDelayEnabled = top.readCookie("StatusQueueDelayEnabled");
            var bShowInPlayDelay = true;
            if (statusQueueDelayEnabled != null && statusQueueDelayEnabled == '1') bShowInPlayDelay = false;
            if (sStatus == "Live" && bShowInPlayDelay) { client_startInPlayDelay(); }
        }
    } else { forceLogin() }
};
function server_cancelBet(side, orderID) {
    if (top.bLoggedIn) {
        if (BeginProcessing()) {
            var htUnMatchedOrders;
            if (side == betSide_Back) {
                htUnMatchedOrders = htMyBetsBackUnMatched
            } else {
                htUnMatchedOrders = htMyBetsLayUnMatched
            }
            var orderCount = 1;
            var amountOriginal = htUnMatchedOrders[orderID][iBetAmountBeforeEdit] * 1 + htUnMatchedOrders[orderID][iBetAmountMatched] * 1;
            var amountMatched = htUnMatchedOrders[orderID][iBetAmountMatched];
            var entrantID = htUnMatchedOrders[orderID][iBetEntrantID];
            var frmOrders = document.getElementById("frmOrders");
            frmOrders.reset();
            document.getElementById("txtMarketID").value = sMarketID;
            document.getElementById("txtMarketIDs").value = sMarketID;
            document.getElementById("txtEntrantIDs").value = entrantID;
            document.getElementById("txtOrderCount").value = orderCount;
            document.getElementById("txtOrderIDs").value = orderID;
            document.getElementById("txtAmountsOriginal").value = amountOriginal;
            document.getElementById("txtAmountsMatched").value = amountMatched;
            frmOrders.action = "OrdersCancelJS" + sExt + "?ts=" + iTimestamp + "&src=mkt";
            frmOrders.submit()
        }
    } else {
        forceLogin()
    }
};
function server_cancelAllBets() {
    if (top.bLoggedIn) {
        if (BeginProcessing()) {
            var frmOrders = document.getElementById("frmOrders");
            frmOrders.reset();
            document.getElementById("txtMarketID").value = sMarketID;
            frmOrders.action = "OrdersCancelJS" + sExt + "?cmd=DoCancelAll" + "&ts=" + iTimestamp + "&src=mkt";
            frmOrders.submit()
        }
    } else { forceLogin() }
};
function html_getBetTableNoHeader(rowDataHTML) {
    var betTableHTML = document.getElementById("taBetTableNoHeader").value;
    var re = new RegExp("@RowData", "g");
    betTableHTML = betTableHTML.replace(re, rowDataHTML);
    return betTableHTML
};
function html_getBetTable(betState, betSide, rowDataHTML, editable) {
    var blDesc = ""; var showLossRadio = "";
    var showDummyRadio = ""; var betTableHTML = "";
    var bShowProfitChecked = true; var bAllowSort = true;
    switch (betSide) {
        case betSide_Lay:
            betTableHTML = document.getElementById("taLayBetTable").value;
            blDesc = sLay;
            showLossRadio = "";
            showDummyRadio = "none";
            break;
        case betSide_Back:
            betTableHTML = document.getElementById("taBetTable").value;
            blDesc = sBack;
            showDummyRadio = "";
            showLossRadio = "none";
            break
    }
    switch (betState) {
        case betState_UnPlaced:
            bShowProfitChecked = bBetPanelUnPlacedShowProfit;
            bAllowSort = false;
            break;
        case betState_UnMatched:
            bShowProfitChecked = bBetPanelUnMatchedShowProfit;
            break;
        case betState_Matched:
            bShowProfitChecked = bBetPanelMatchedShowProfit;
            break
    }
    var re = new RegExp("@Side", "g");
    var betTableHTML = betTableHTML.replace(re, betSide);
    re = new RegExp("@BL", "g");
    betTableHTML = betTableHTML.replace(re, blDesc);
    re = new RegExp("@ShowLossRadio", "g");
    betTableHTML = betTableHTML.replace(re, showLossRadio);
    re = new RegExp("@ShowDummyRadio", "g");
    betTableHTML = betTableHTML.replace(re, showDummyRadio);
    re = new RegExp("@State", "g");
    betTableHTML = betTableHTML.replace(re, betState);
    var betBookPercentageHTML = "";
    if (editable && betState == betState_UnPlaced) {
        betBookPercentageHTML = document.getElementById("taBookPercentage").value;
        re = new RegExp("@BL", "g");
        betBookPercentageHTML = betBookPercentageHTML.replace(re, betSide);
        re = new RegExp("@StakeInputStyle", "g");
        betBookPercentageHTML = betBookPercentageHTML.replace(re, "StakeInput");
        re = new RegExp("@Side", "g");
        betBookPercentageHTML = betBookPercentageHTML.replace(re, betSide);
        if (document.getElementById("txtValue_" + betSide) != null) {
            if (betSide == betSide_Back) {
                dProfit = document.getElementById("txtValue_" + betSide).value;
                dBookProfit = document.getElementById("spBookPercentage_" + betSide).value
            } else if (betSide == betSide_Lay) {
                dLiability = document.getElementById("txtValue_" + betSide).value;
                dBookLiability = document.getElementById("spBookPercentage_" + betSide).value
            }
        }
        re = new RegExp("@txtValue", "g");
        betBookPercentageHTML = betBookPercentageHTML.replace(re, (betSide == betSide_Back ? dProfit : dLiability));
        re = new RegExp("@BookPercentage", "g");
        betBookPercentageHTML = betBookPercentageHTML.replace(re, (betSide == betSide_Back ? dBookProfit : dBookLiability));
        re = new RegExp("@BetType", "g");
        betBookPercentageHTML = betBookPercentageHTML.replace(re, (betSide == betSide_Back ? sTotalProfit : ""))
    }
    re = new RegExp("@RowData", "g");
    betTableHTML = betTableHTML.replace(re, rowDataHTML + betBookPercentageHTML);
    var sProfitChecked = "checked";
    var sLossChecked = "";
    if (!bShowProfitChecked) {
        sProfitChecked = ""; sLossChecked = "checked"
    }
    re = new RegExp("checked=\"@ProfitChecked\"", "g");
    betTableHTML = betTableHTML.replace(re, sProfitChecked);
    re = new RegExp("checked=\"@LossChecked\"", "g");
    betTableHTML = betTableHTML.replace(re, sLossChecked);
    if (!bAllowSort) {
        re = new RegExp("<a href=[^>]*>", "g");
        betTableHTML = betTableHTML.replace(re, "");
        re = new RegExp("</a>", "g");
        betTableHTML = betTableHTML.replace(re, "")
    }
    return betTableHTML
};
function html_getBetRow(id, betState, betSide, arrBetData, editable) {
    var betRowHTML = "";
    if (editable) {
        betRowHTML = document.getElementById("taBetRowEdit").value
    } else {
        betRowHTML = document.getElementById("taBetRowRead").value
    }
    var etID = arrBetData[iBetEntrantID];
    var desc = htEntrants[etID][iEtDesc];
    var stake = arrBetData[iBetStake];
    var odds = arrBetData[iBetOdds];
    var pl = space;
    var freeBetID = arrBetData[iBetFreeBetID];
    var bFreeBet = (freeBetID != null && freeBetID.length > 0);
    var prefixSign = '';
    if (!editable && (stake == "" || odds == "")) { betRowHTML = ""; return betRowHTML }
    var bIsPeningOrder = false;
    if (betState == betState_UnMatched && arrBetData[iEtSP] != 'undefined') {
        if (arrBetData[iEtSP]) bIsPeningOrder = true
    }
    var bIsPendingCancel = false;
    if (betState == betState_UnMatched && arrBetData[iEtSCR] != 'undefined') {
        if (arrBetData[iEtSCR]) { bIsPendingCancel = true }
    }
    var bShowProfitChecked;
    switch (betState) {
        case betState_UnPlaced:
            bShowProfitChecked = bBetPanelUnPlacedShowProfit;
            break;
        case betState_UnMatched:
            bShowProfitChecked = bBetPanelUnMatchedShowProfit;
            break;
        case betState_Matched:
            bShowProfitChecked = bBetPanelMatchedShowProfit;
            break
    }
    if (betSide == betSide_Lay) {
        pl = calcPayoutLiability(stake, odds, !bShowProfitChecked)
    } else {
        pl = calcProfit(stake, odds, betSide)
    }
    if (pl != space) {
        pl = formatCurrency(pl, sSymbol, numDec)
    }
    var convertedOdds = getNearestDecimalOdds(odds, ((betSide == betSide_Back) ? betSide_Lay : betSide_Back));
    if (!editable) {
        stake = formatCurrency(stake, sSymbol, numDec);
        if (oddsFormat == "Decimal" || oddsFormat == "Indon" || oddsFormat == "HK") {
            odds = (odds * 1).toFixed(5); prefix = ""
        } else {
            odds = (odds * 1).toFixed(5);
            if (betState == betState_Matched) prefixSign = (convertedOdds == odds) ? "" : (oddsFormat == "Percentage") ? ((convertedOdds > odds) ? ">" : "<") : ((convertedOdds > odds) ? "<" : ">")
        }
    } else {
        if ((arrBetData[9] != "") && (arrBetData[9] != null)) {
            prefixSign = (convertedOdds == odds) ? "" : (oddsFormat == "Percentage") ? ((convertedOdds > odds) ? ">" : "<") : ((convertedOdds > odds) ? "<" : ">")
        }
    }
    var re = new RegExp("@BL", "g");
    betRowHTML = betRowHTML.replace(re, betSide);
    re = new RegExp("@ID", "g");
    betRowHTML = betRowHTML.replace(re, id);
    re = new RegExp("@State", "g");
    betRowHTML = betRowHTML.replace(re, betState);
    re = new RegExp("@DE", "g");
    if (bFreeBet) betRowHTML = betRowHTML.replace(re, desc + "<br /><span class='spFreeBet'>(" + sFreeBet + ")</span> ");
    else if (bIsPeningOrder && !bIsPendingCancel) betRowHTML = betRowHTML.replace(re, desc + "<br /><span class='spPendingOrder'>(" + sPendingOrder + ")</span> ");
    else if (bIsPendingCancel) betRowHTML = betRowHTML.replace(re, desc + "<br /><span class='spPendingCancel'>(" + sPendingCancel + ")</span> ");
    else betRowHTML = betRowHTML.replace(re, desc);
    var stakeInputStyle = "StakeInput";
    var oddsInputStyle = "OddsInput";
    var stakeReadOnly = "readonly";
    var oddsReadOnly = "y";
    if (bIsPeningOrder) {
        stakeInputStyle = "StakeInputDisabled";
        oddsInputStyle = "OddsInputDisabled";
        oddsReadOnly = "readonly"
    } else if ((stake != "") && (editable && (stake < minBet)) && (betState != betState_UnPlaced)) {
        stakeInputStyle = "StakeInputDisabled"
    } else if (editable && bFreeBet) {
        stakeInputStyle = "StakeInputDisabled"
    } else {
        if (!bFreeBet) stakeReadOnly = "x"
    }
    re = new RegExp("@StakeInputStyle", "g");
    betRowHTML = betRowHTML.replace(re, stakeInputStyle);
    re = new RegExp("stakereadonly", "g");
    if (!bFreeBet) betRowHTML = betRowHTML.replace(re, stakeReadOnly);
    re = new RegExp("@OddsInputStyle", "g");
    betRowHTML = betRowHTML.replace(re, oddsInputStyle);
    if (oddsInputStyle == "OddsInputDisabled") {
        betRowHTML = betRowHTML.replace("ArrowUp.gif", "spacer.gif");
        betRowHTML = betRowHTML.replace("ArrowDown.gif", "spacer.gif")
    }
    re = new RegExp("oddsreadonly", "g");
    betRowHTML = betRowHTML.replace(re, oddsReadOnly);
    re = new RegExp("@Stake", "g");
    betRowHTML = betRowHTML.replace(re, stake);
    re = new RegExp("@Odds", "g");
    if (enableOddsFormat) {
        if ((oddsFormat == "Decimal" || oddsFormat == "Indon" || oddsFormat == "HK") && !editable) {
            var convertedNumber = ConvertOddsFromAlgorithm(odds);
            betRowHTML = betRowHTML.replace(re, (odds != "" ? convertedNumber : ""))
        } else {
            betRowHTML = betRowHTML.replace(re, prefixSign + ConvertOdds(convertedOdds, ((betSide == betSide_Back) ? betSide_Lay : betSide_Back)))
        }
    } else {
        var convertedNumber = parseFloat(parseFloat(odds).toFixed(2));
        betRowHTML = betRowHTML.replace(re, (odds != "" ? convertedNumber : ""))
    }
    re = new RegExp("@PL", "g");
    betRowHTML = betRowHTML.replace(re, pl);
    return betRowHTML
};
function calcProfit(stake, odds, side) {
    var pl = space;
    if (isNum(stake) && isNum(odds)) {
        if (side == betSide_Back) {
            pl = stake * (odds - 1)
        } else { pl = stake }
    }
    return pl
};
function calcPayoutLiability(stake, odds, bIsPayout) {
    var pl = space;
    if (isNum(stake) && isNum(odds)) {
        if (bIsPayout) {
            pl = stake * odds
        } else {
            pl = stake * (odds - 1)
        }
    }
    return pl
};
var bLastPlaceBetsEditable = true;
function client_renderPlaceBets(editable) {
    bLastPlaceBetsEditable = editable;
    var backHTML = ""; var layHTML = "";
    for (var entrantID in htPlaceBetsBack) {
        backHTML += html_getBetRow(entrantID, betState_UnPlaced, betSide_Back, htPlaceBetsBack[entrantID], editable)
    }
    for (var entrantID in htPlaceBetsLay) {
        layHTML += html_getBetRow(entrantID, betState_UnPlaced, betSide_Lay, htPlaceBetsLay[entrantID], editable)
    }
    document.getElementById("tblPlaceBetsResult").style.display = "none";
    document.getElementById("tblPlaceBets").style.display = "";
    if (!checkHasUnPlacedBets()) {
        bLastPlaceBetsEditable = true;
        document.getElementById("tdPlaceBetsItems").innerHTML = "";
        document.getElementById("trPlaceBetsEmpty").style.display = "";
        document.getElementById("trPlaceBetsItems").style.display = "none";
        document.getElementById("tdPlaceBetsItems").innerHTML = "";
        document.getElementById("trPlaceBetsConfirmControls").style.display = "none";
        document.getElementById("trPlaceBetsSubmitControls").style.display = "none"
    } else {
        if (backHTML != "") {
            backHTML = html_getBetTable(betState_UnPlaced, betSide_Back, backHTML, editable)
        }
        if (layHTML != "") {
            layHTML = html_getBetTable(betState_UnPlaced, betSide_Lay, layHTML, editable)
        }
        document.getElementById("trPlaceBetsEmpty").style.display = "none";
        document.getElementById("tdPlaceBetsItems").innerHTML = backHTML + layHTML;
        document.getElementById("trPlaceBetsItems").style.display = "";
        if (editable) {
            document.getElementById("trPlaceBetsSubmitControls").style.display = "";
            document.getElementById("trPlaceBetsConfirmControls").style.display = "none";
            document.getElementById("trCurrentUsingOddsFormat").style.display = "none"
        } else {
            document.getElementById("trPlaceBetsSubmitControls").style.display = "none";
            document.getElementById("trPlaceBetsConfirmControls").style.display = "";
            if (oddsFormat != 'Decimal') document.getElementById("trCurrentUsingOddsFormat").style.display = ""
        }
    }
};
function ui_doDeleteBet(state, side, ID) {
    if (IsProcessing()) return;
    switch (state) {
        case betState_UnPlaced:
            client_removeUnPlacedBet(side, ID);
            calculateProfitLoss();
            break;
        case betState_UnMatched:
            server_cancelBet(side, ID);
            break
    }
    var bIsBookPercentageShow = top.userGetSetting(top.cShowBookPercentage);
    ui_doBookPercentage(bIsBookPercentageShow);
    var iStakeFillTool = top.userGetSettingInt(top.cShowStakeFillTool);
    if (iStakeFillTool == PerBet_Show || iStakeFillTool == DutchProfit_Show || iStakeFillTool == DutchLiability_Show) {
        ui_setShowStakeFillTool(true)
    } else {
        ui_setShowStakeFillTool(false)
    }
};
function client_removeUnPlacedBet(side, entrantID) {
    if (side == betSide_Back) {
        delete htPlaceBetsBack[entrantID]
    } else {
        delete htPlaceBetsLay[entrantID]
    }
    client_renderPlaceBets(true)
};
function ui_doCancelAllUnMatched() {
    if (IsProcessing()) return;
    server_cancelAllBets()
};
function ui_doBackAll() {
    if (IsProcessing()) return;
    var firstEntrantID = "";
    if (iPLMarketDepth == MarketDepth_BestPriceOnly || iPLMarketDepth == MarketDepth_BestPriceAndStake) {
        firstEntrantID = doBackLayAllBestPrice(htPlaceBetsBack, betSide_Back)
    } else {
        firstEntrantID = doBackLayAll(htPlaceBetsBack, betSide_Back)
    }
    if (firstEntrantID != "") { client_renderPlaceBets(true); ui_doClickBPTab("PlaceBets") }
};
function ui_doLayAll() {
    if (IsProcessing()) return;
    var firstEntrantID = "";
    if (iPLMarketDepth == MarketDepth_BestPriceOnly || iPLMarketDepth == MarketDepth_BestPriceAndStake) {
        firstEntrantID = doBackLayAllBestPrice(htPlaceBetsLay, betSide_Lay)
    } else {
        firstEntrantID = doBackLayAll(htPlaceBetsLay, betSide_Lay)
    }
    if (firstEntrantID != "") {
        client_renderPlaceBets(true);
        ui_doClickBPTab("PlaceBets")
    }
};
function doBackLayAll(htPlaceBets, side) {
    var firstEntrantBetID = "";
    if (sStatus == "Open" || sStatus == "Live") {
        for (var entrantID in htEntrants) {
            var entrantStatus = htEntrants[entrantID][iEtStatus];
            if (entrantStatus == "Open") {
                var ok;
                ok = doBackLay(entrantID, htPlaceBets, side, null, false, null);
                if (ok) { firstEntrantBetID = entrantID }
            }
        }
    }
    return firstEntrantBetID
};
function doBackLayAllBestPrice(htPlaceBets, side) {
    var firstEntrantBetID = "";
    if (sStatus == "Open" || sStatus == "Live") {
        for (var entrantID in htEntrants) {
            var entrantStatus = htEntrants[entrantID][iEtStatus];
            var entrantIdentifier = htEntrants[entrantID][iEtIdent];
            if (entrantStatus == "Open") {
                var ok;
                var BestBet = CalMinWin(iPLMarketDepth, entrantID, side, htMinWin[iPLMinWin], entrantIdentifier);
                ok = doBackLayForBestBet(entrantID, htPlaceBets, side, BestBet[iBestOddPos], false);
                if (ok) { firstEntrantBetID = entrantID }
            }
        }
    }
    return firstEntrantBetID
};

function ui_doBack(entrantID, position, myOdds) {
    if (IsProcessing()) return;
    if (htEntrants[entrantID] == null) return;
    if (position == null) position = 1;
    if (iPLMarketDepth == MarketDepth_BestPriceOnly || iPLMarketDepth == MarketDepth_BestPriceAndStake) {
        var entrantIdentifier = htEntrants[entrantID][iEtIdent];
        var BestBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Back, htMinWin[iPLMinWin], entrantIdentifier);
        var odds = BestBet[iBestOddPos];
        if (myOdds != null && myOdds != "") { odds = myOdds }
        if (!freeBackMode) {
            var ok = false;
            ok = doBackLayForBestBet(entrantID, htPlaceBetsBack, betSide_Back, odds, true);
            if (ok) {
                if (!document.getElementById("inOdds_" + betState_UnPlaced + "_Back_" + entrantID)) { client_renderPlaceBets(true) }
                ui_doClickBPTab("PlaceBets");
                document.getElementById("inStake_" + betState_UnPlaced + "_Back_" + entrantID).focus()
            }
        } else {
            doFreeBackForBestBet(entrantID, odds)
        }
    } else {
        var oddsIndex = iEtBO1;
        switch (position) {
            case 2: oddsIndex = iEtBO2; break;
            case 3: oddsIndex = iEtBO3; break;
            default: oddsIndex = iEtBO1; break
        }
        if (!freeBackMode) {
            var ok = false;
            ok = doBackLay(entrantID, htPlaceBetsBack, betSide_Back, oddsIndex, true, myOdds);
            if (ok) {
                if (!document.getElementById("inOdds_" + betState_UnPlaced + "_Back_" + entrantID)) { client_renderPlaceBets(true) }
                ui_doClickBPTab("PlaceBets");
                document.getElementById("inStake_" + betState_UnPlaced + "_Back_" + entrantID).focus()
            }
        } else {
            doFreeBack(entrantID, oddsIndex)
        }
    }
};

function ui_doLay(entrantID, position, myOdds) {
    if (IsProcessing()) return;
    if (position == null) position = 1;
    var ok = false;
    if (iPLMarketDepth == MarketDepth_BestPriceOnly || iPLMarketDepth == MarketDepth_BestPriceAndStake) {
        var entrantIdentifier = htEntrants[entrantID][iEtIdent];
        var BestBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Lay, htMinWin[iPLMinWin], entrantIdentifier);
        var odds = BestBet[iBestOddPos];
        if (myOdds != null && myOdds != "") { odds = myOdds }
        ok = doBackLayForBestBet(entrantID, htPlaceBetsLay, betSide_Lay, odds, true)
    } else {
        var oddsIndex = iEtLO1;
        switch (position) {
            case 2: oddsIndex = iEtLO2; break;
            case 3: oddsIndex = iEtLO3; break;
            default: oddsIndex = iEtLO1; break
        }
        ok = doBackLay(entrantID, htPlaceBetsLay, betSide_Lay, oddsIndex, true, myOdds)
    }
    if (ok) {
        if (!document.getElementById("inOdds_" + betState_UnPlaced + "_Lay_" + entrantID)) {
            client_renderPlaceBets(true)
        }
        ui_doClickBPTab("PlaceBets");
        document.getElementById("inStake_" + betState_UnPlaced + "_Lay_" + entrantID).focus();
    }
};
function doBackLay(entrantID, htPlaceBets, side, oddsIndex, bChangeIfExist, myOdds) {
    if (sStatus == "Open" || sStatus == "Live") {
        if (oddsIndex == null) {
            if (side == betSide_Back) { oddsIndex = iEtBO1 } else { oddsIndex = iEtLO1 }
        }
        if (bChangeIfExist == null) bChangeIfExist = false;
        var bExisted = true;
        if (htPlaceBets[entrantID] == 'undefined' || htPlaceBets[entrantID] == null) { bExisted = false }
        if (!bExisted || bChangeIfExist) {
            var bet = new Array();
            bet[iBetEntrantID] = entrantID;
            bet[iBetStake] = "";
            if (myOdds != null && myOdds != "") {
                bet[iBetOdds] = myOdds
            } else {
                if (side == betSide_Back) {
                    bet[iBetOdds] = htOdds[entrantID][oddsIndex]
                } else {
                    bet[iBetOdds] = htOdds[entrantID][oddsIndex]
                }
            }
            htPlaceBets[entrantID] = bet;
            if (bExisted) {
                document.getElementById("inOdds_" + betState_UnPlaced + "_" + side + "_" + entrantID).value = ConvertOdds(bet[iBetOdds], side)
            }
            return true
        }
    }
    return false
};

function ui_doUpdatePL(state, side, ID) {
    if (IsProcessing()) return;
    var stakeID = "";
    var oddsID = "";
    var plID = "";
    var bShowProfitChecked;
    switch (state) {
        case betState_UnPlaced:
            stakeID = "inStake_" + betState_UnPlaced + "_" + side + "_" + ID;
            oddsID = "inOdds_" + betState_UnPlaced + "_" + side + "_" + ID;
            plID = "tdPL_" + betState_UnPlaced + "_" + side + "_" + ID;
            bShowProfitChecked = bBetPanelUnPlacedShowProfit;
            break;
        case betState_UnMatched:
            stakeID = "inStake_" + betState_UnMatched + "_" + side + "_" + ID;
            oddsID = "inOdds_" + betState_UnMatched + "_" + side + "_" + ID;
            plID = "tdPL_" + betState_UnMatched + "_" + side + "_" + ID;
            bShowProfitChecked = bBetPanelUnMatchedShowProfit;
            break
    }
    var stake = trim(document.getElementById(stakeID).value);
    var odds = trim(document.getElementById(oddsID).value);
    var pl = space;
    if (side == betSide_Lay) {
        pl = calcPayoutLiability(stake, odds, !bShowProfitChecked)
    } else {
        pl = calcProfit(stake, odds, side)
    }
    if (pl != space) { pl = formatCurrency(pl, sSymbol, numDec) }
    document.getElementById(plID).innerHTML = pl
};
function renderDirtyText() {
    for (var id in htMyBetsBackUnMatched) {
        if (htMyBetsBackUnMatched[id][iBetEdited] == true) {
            if (htMyBetsBackUnMatched[id][iBetAmountBeforeEdit] != htMyBetsBackUnMatched[id][iBetStake]) {
                var ctlID = "inStake_" + betState_UnMatched + "_" + betSide_Back + "_" + id;
                if (document.getElementById(ctlID))
                    document.getElementById(ctlID).style.color = "red"
            }
            if (htMyBetsBackUnMatched[id][iBetOddsBeforeEdit] != htMyBetsBackUnMatched[id][iBetOdds]) {
                var ctlID = "inOdds_" + betState_UnMatched + "_" + betSide_Back + "_" + id;
                if (document.getElementById(ctlID)) document.getElementById(ctlID).style.color = "red"
            }
        }
    }
    for (var id in htMyBetsLayUnMatched) {
        if (htMyBetsLayUnMatched[id][iBetEdited] == true) {
            if (htMyBetsLayUnMatched[id][iBetAmountBeforeEdit] != htMyBetsLayUnMatched[id][iBetStake]) {
                var ctlID = "inStake_" + betState_UnMatched + "_" + betSide_Lay + "_" + id;
                if (document.getElementById(ctlID))
                    document.getElementById(ctlID).style.color = "red"
            }
            if (htMyBetsLayUnMatched[id][iBetOddsBeforeEdit] != htMyBetsLayUnMatched[id][iBetOdds]) {
                var ctlID = "inOdds_" + betState_UnMatched + "_" + betSide_Lay + "_" + id;
                if (document.getElementById(ctlID)) document.getElementById(ctlID).style.color = "red"
            }
        }
    }
};

var todoUpdateStakeOddsDelayed;
var sCommandBuffer = "";
function ui_doUpdateStakeOddsDelayed(state, side, ID, bKeyPressUpdate, numDec, throwError) {
    if (IsProcessing()) return;
    window.clearTimeout(todoUpdateStakeOddsDelayed);
    sCommandBuffer = "ui_doUpdateStakeOdds('" + state + "','" + side + "','" + ID + "'," + bKeyPressUpdate + "," + numDec + "," + throwError + ")";
    todoUpdateStakeOddsDelayed = window.setTimeout("ui_doUpdateStakeOdds('" + state + "','" + side + "','" + ID + "'," + bKeyPressUpdate + "," + numDec + "," + throwError + ")", 500)
};

function ui_doUpdateStakeOdds(state, side, ID, bKeyPressUpdate, numDec, throwError) {
    if (IsProcessing()) return;
    window.clearTimeout(todoUpdateStakeOddsDelayed);
    sCommandBuffer = "";
    var stakeID = "";
    var oddsID = "";
    var plID = "";
    var htBets;
    var error = false;
    var bShowProfitChecked;
    switch (state) {
        case betState_UnPlaced:
            stakeID = "inStake_" + betState_UnPlaced + "_" + side + "_" + ID;
            oddsID = "inOdds_" + betState_UnPlaced + "_" + side + "_" + ID;
            plID = "tdPL_" + betState_UnPlaced + "_" + side + "_" + ID;
            bShowProfitChecked = bBetPanelUnPlacedShowProfit;
            if (side == betSide_Back) {
                htBets = htPlaceBetsBack
            } else {
                htBets = htPlaceBetsLay
            }
            break;
        case betState_UnMatched:
            stakeID = "inStake_" + betState_UnMatched + "_" + side + "_" + ID;
            oddsID = "inOdds_" + betState_UnMatched + "_" + side + "_" + ID;
            plID = "tdPL_" + betState_UnMatched + "_" + side + "_" + ID;
            bShowProfitChecked = bBetPanelUnMatchedShowProfit;
            if (side == betSide_Back) {
                htBets = htMyBetsBackUnMatched
            } else {
                htBets = htMyBetsLayUnMatched
            }
            break
    }
    var stake = trim(document.getElementById(stakeID).value);
    var originalOdds = htBets[ID][iBetOdds];
    var odds = trim(document.getElementById(oddsID).value);
    var prefix = "";
    if (odds != "" && (odds.charAt(0) == '<' || odds.charAt(0) == '>')) {
        prefix = odds.charAt(0); odds = odds.substring(1)
    }
    if (odds != "") {
        try {
            odds = ConvertOddsToDec(odds, side, throwError)
        } catch (ex) {
            if (!bKeyPressUpdate) {
                top.showMessageBox(ex, null, oddsFormat + "OddsFormat",
                function () { document.getElementById(oddsID).focus() });
                document.getElementById(oddsID).value = "";
                document.getElementById(plID).innerHTML = ""
            }
            return
        }
    }
    var bRecalcPL = false;
    if (htBets[ID][iBetStake] != stake || htBets[ID][iBetOdds] != odds) { bRecalcPL = true }
    if (!isNum(stake)) {
        if (stake != "") {
            htBets[ID][iBetStake] = "";
            if (bKeyPressUpdate) return;
            top.showMessageBox(sStakeNumeric, null, null, function () {
                document.getElementById(stakeID).focus()
            });
            document.getElementById(stakeID).value = "";
            document.getElementById(plID).innerHTML = "";
            return
        }
    } else {
        if (!bKeyPressUpdate) {
            var newStake = doRoundOdds(stake);
            if (stake != newStake) {
                stake = newStake;
                document.getElementById(stakeID).value = newStake;
                bRecalcPL = true
            }
        }
    }
    htBets[ID][iBetStake] = stake;
    var stakeEdit = false;
    if (state == betState_UnMatched && htBets[ID][iBetAmountBeforeEdit] != stake) {
        stakeEdit = true;
        htBets[ID][iBetEdited] = true
    }
    if (!isNum(odds)) {
        if (odds != "") {
            htBets[ID][iBetOdds] = "";
            if (bKeyPressUpdate) return;
            top.showMessageBox(sOddsNumeric, null, "BettingOddsFormat", function () {
                document.getElementById(oddsID).focus()
            });
            document.getElementById(oddsID).value = ""; return
        }
    } else {
        if (!bKeyPressUpdate) {
            var convertedOdds = ConvertOdds(odds, side);
            bRecalcPL = true;
            var inputOdds = document.getElementById(oddsID).value;
            if (inputOdds != prefix + convertedOdds) document.getElementById(oddsID).value = prefix + convertedOdds;
        }
    }
    htBets[ID][iBetOdds] = odds;
    var oddsEdit = false;
    if (state == betState_UnMatched && htBets[ID][iBetOddsBeforeEdit] != odds) {
        oddsEdit = true;
        htBets[ID][iBetEdited] = true
    }
    if (state == betState_UnMatched && !stakeEdit && !oddsEdit) {
        htBets[ID][iBetEdited] = false
    }
    if (state == betState_UnMatched) {
        if (stakeEdit) {
            document.getElementById(stakeID).style.color = "red"
        } else {
            document.getElementById(stakeID).style.color = ""
        }
        if (oddsEdit) {
            document.getElementById(oddsID).style.color = "red"
        } else {
            document.getElementById(oddsID).style.color = ""
        }
    }
    var pl = space;
    if (side == betSide_Lay) {
        pl = calcPayoutLiability(stake, odds, !bShowProfitChecked)
    } else {
        pl = calcProfit(stake, odds, side)
    }
    if (pl != space) { pl = formatCurrency(pl, sSymbol, numDec) }
    document.getElementById(plID).innerHTML = pl;
    if (bRecalcPL) calculateProfitLoss();
    updateBookPercentage(side)
};

function doRoundOdds(odds) { return odds };

function doRoundStake(stake) {
    stake = stake > maxBet ? maxBet : stake;
    stake = stake < minBet ? minBet : stake;
    return stake
};

function ui_switchBetPanelProfitLoss(state, showProfit) {
    if (IsProcessing()) return;
    var htBets;
    switch (state) {
        case betState_UnPlaced:
            htBets = htPlaceBetsLay;
            bBetPanelUnPlacedShowProfit = showProfit;
            break;
        case betState_UnMatched:
            htBets = htMyBetsLayUnMatched;
            bBetPanelUnMatchedShowProfit = showProfit;
            break;
        case betState_Matched:
            htBets = htMyBetsLayMatched;
            switch (matchedBetView) {
                case sBetViewConsolidated:
                    htBets = calculateConsolidatedMatchedBets(htBets);
                    break;
                case sBetViewAverage:
                    htBets = calculateAverageMatchedBets(htBets);
                    break
            }
            bBetPanelMatchedShowProfit = showProfit;
            break
    }
    switchBetPanelProfitLoss(state, showProfit, htBets)
};

function ui_switchBetPanelPayoutLiability(state, showProfit) {
    if (IsProcessing()) return;
    var htBets;
    switch (state) {
        case betState_UnPlaced:
            htBets = htPlaceBetsLay;
            bBetPanelUnPlacedShowProfit = showProfit;
            break;
        case betState_UnMatched:
            htBets = htMyBetsLayUnMatched;
            bBetPanelUnMatchedShowProfit = showProfit;
            break;
        case betState_Matched:
            htBets = htMyBetsLayMatched;
            switch (matchedBetView) {
                case sBetViewConsolidated:
                    htBets = calculateConsolidatedMatchedBets(htBets);
                    break;
                case sBetViewAverage:
                    htBets = calculateAverageMatchedBets(htBets);
                    break
            }
            bBetPanelMatchedShowProfit = showProfit;
            break
    }
    switchBetPanelPayoutLiability(state, showProfit, htBets)
};

function switchBetPanelPayoutLiability(state, showProfit, htBets) {
    var stake;
    var odds;
    for (var id in htBets) {
        stake = htBets[id][iBetStake];
        odds = htBets[id][iBetOdds];
        var pl = space;
        if (!showProfit) {
            pl = calcPayoutLiability(stake, odds, true);
        } else {
            pl = calcPayoutLiability(stake, odds, false)
        }
        if (pl != space) {
            pl = formatCurrency(pl, sSymbol, numDec)
        }
        document.getElementById("tdPL_" + state + "_" + betSide_Lay + "_" + id).innerHTML = pl
    }
};
function switchBetPanelProfitLoss(state, showProfit, htBets) {
    var stake; var odds;
    for (var id in htBets) {
        stake = htBets[id][iBetStake];
        odds = htBets[id][iBetOdds];
        var pl = space;
        if (!showProfit) {
            pl = +calcProfit(stake, odds, betSide_Back)
        } else {
            pl = calcProfit(stake, odds, betSide_Lay)
        }
        if (pl != space) {
            pl = formatCurrency(pl, sSymbol, numDec)
        }
        document.getElementById("tdPL_" + state + "_" + betSide_Lay + "_" + id).innerHTML = pl
    }
};
function ui_doClearUnPlacedBets() {
    if (IsProcessing()) return;
    client_clearUnPlacedBets();
    calculateProfitLoss()
};
function client_clearUnPlacedBets() {
    htPlaceBetsBack = {};
    htPlaceBetsLay = {};
    for (var id in htMyFreeBets) {
        htMyFreeBets[id][iFreeEdited] = false;
        htMyFreeBets[id][iFreeEntrant] = "";
        htMyFreeBets[id][iFreeOdds] = ""
    }
    freeBackMode = false;
    selectedFreeBetID = "";
    client_renderFreeBets(iRenderModeEdit);
    client_renderPlaceBets(true);
    document.getElementById("trCurrentUsingOddsFormat").style.display = "none"
};
function ui_doSubmitUnPlacedBets(numDec) {
    if (IsProcessing()) return;
    try { eval(sCommandBuffer) }
    catch (e) { }
    bHasValidUnplacedBets = false;
    bHasBetsOverWarnAmount = false;
    if (!(validateUnPlacedBets(htPlaceBetsBack, betSide_Back, numDec) && validateUnPlacedBets(htPlaceBetsLay, betSide_Lay, numDec) && validateUnPlacedBetsFree()))
    { return }
    if (document.getElementById("txtValue_" + betSide_Back) != null) {
        dProfit = document.getElementById("txtValue_" + betSide_Back).value
    }
    if (document.getElementById("txtValue_" + betSide_Lay) != null) {
        dLiability = document.getElementById("txtValue_" + betSide_Lay).value
    }
    if (document.getElementById("spBookPercentage_" + betSide_Back) != null) {
        dBookProfit = document.getElementById("spBookPercentage_" + betSide_Back).innerHTML
    }
    if (document.getElementById("spBookPercentage_" + betSide_Lay) != null) {
        dBookLiability = document.getElementById("spBookPercentage_" + betSide_Lay).innerHTML
    }
    if (!bHasValidUnplacedBets) return;
    if (bHasBetsOverWarnAmount && top.userGetSetting(top.cShowOddsWarning)) {
        top.showConfirmMsgBox(sOddsWarningMsg[oddsFormat],
        function () {
            if (top.userGetSetting(top.cShowBetConfirm) || oddsFormat != 'Decimal') {
                client_renderPlaceBets(false);
                client_renderFreeBets(iRenderModeRead)
            } else { server_addBets() }
        },
        function () { return }, null, null)
    } else {
        if (top.userGetSetting(top.cShowBetConfirm) || oddsFormat != 'Decimal') {
            client_renderPlaceBets(false);
            client_renderFreeBets(iRenderModeRead)
        } else {
            server_addBets()
        }
    }
};
var bHasValidUnplacedBets = false;
var bHasBetsOverWarnAmount = false;
function validateUnPlacedBets(htPlaceBets, side, numDec) {
    for (var entrantID in htPlaceBets) {
        var breakOut = false;
        var stake = htPlaceBets[entrantID][iBetStake].toString();
        var odds = htPlaceBets[entrantID][iBetOdds].toString();
        stake = trim(stake);
        stake = round(stake, numDec);
        odds = trim(odds);
        if (stake == "" || odds == "") { continue; }
        if (!breakOut && !validateStake(stake)) {
            breakOut = true;
            stake = "";
            top.showMessageBox(sStakeNumeric, null, null)
        }
        if (!breakOut && (CT2[odds] == null)) {
            breakOut = true;
            odds = "";
            top.showMessageBox(sOddsNumeric, null, null)
        }
        htPlaceBets[entrantID][iBetStake] = stake;
        htPlaceBets[entrantID][iBetOdds] = odds;
        if (breakOut) { return false }
        if (odds > iWarningOdds) bHasBetsOverWarnAmount = true;
        bHasValidUnplacedBets = true
    }
    return true
};
function validDecimalOdds(odds) { return true };
function validateStake(stake) { return true };
function ui_doConfirmUnPlacedBetsYes() { if (IsProcessing()) return; server_addBets() };
function ui_doConfirmUnPlacedBetsNo() {
    if (IsProcessing()) return;
    client_renderPlaceBets(true);
    client_renderFreeBets(iRenderModeEdit)
};
function BeginProcessing() {
    if (!bMemberRequestProcessing) {
        bMemberRequestProcessing = true;
        document.getElementById("dvWaitHolder").style.display = "";
        return true
    } else { return false }
};
function EndProcessing() {
    bMemberRequestProcessing = false;
    document.getElementById("dvWaitHolder").style.display = "none"
};
function IsProcessing() { return bMemberRequestProcessing };
function server_receiveAddResult(showResult, arrAddResult, arrAddResultFree, statusQueueDelayEnable) {
    showResult = top.userGetSetting(top.cShowBetResult);
    htPlaceBetsBack = {};
    htPlaceBetsLay = {};
    for (var i = 0; i < arrAddResult.length; i++) {
        arrAddResult[i][1] = htEntrants[arrAddResult[i][6]][iEtDesc]
    }
    client_stopInPlayDelay();
    client_clearMyBets();
    if (showResult) {
        client_renderAddResult(arrAddResult)
    } else {
        client_renderPlaceBets(false);
        ui_doClickBPTab("MyBets")
    }
};
function client_renderAddResult(arrAddResult) {
    var tableHTML = document.getElementById("taOrderResultTable").value;
    var rowDataHTML = "";
    var i;
    for (i = 0; i < arrAddResult.length; i++) {
        rowDataHTML = rowDataHTML + getOrderResultRowHTML(arrAddResult[i])
    }
    var re = new RegExp("@RowData", "g");
    tableHTML = tableHTML.replace(re, rowDataHTML);
    document.getElementById("tblPlaceBetsResult").style.display = "";
    document.getElementById("tblPlaceBets").style.display = "none";
    document.getElementById("tblPlaceFreeBets").style.display = "none";
    document.getElementById("tdPlaceBetsResultItems").innerHTML = tableHTML;
};
var iResultBetID = 0; var iResultEntrant = 1;
var iResultSide = 2; var iResultStake = 3;
var iResultOdds = 4; var iResultComment = 5;
function getOrderResultRowHTML(arrResultRowData) {
    var rowHTML = document.getElementById("taOrderResultRow").value;
    var re = new RegExp("@BetID", "g");
    rowHTML = rowHTML.replace(re, arrResultRowData[iResultBetID]);
    re = new RegExp("@DE", "g");
    rowHTML = rowHTML.replace(re, arrResultRowData[iResultEntrant]);
    re = new RegExp("@Side", "g");
    rowHTML = rowHTML.replace(re, htTranslations[arrResultRowData[iResultSide]]);
    re = new RegExp("@Stake", "g");
    rowHTML = rowHTML.replace(re, formatCurrency(arrResultRowData[iResultStake], sSymbol, numDec));
    re = new RegExp("@Odds", "g");
    var prefixSign = "";
    var odds = arrResultRowData[iResultOdds];
    var side = arrResultRowData[iResultSide] == "Back" ? "Lay" : "Back";
    var roundedOdds = getNearestDecimalOdds(odds, side);
    prefixSign = (roundedOdds == odds) ? "" : (oddsFormat == "Percentage") ? ((roundedOdds > odds) ? ">" : "<") : ((roundedOdds > odds) ? "<" : ">");
    if (enableOddsFormat) {
        if (oddsFormat == "Decimal" || oddsFormat == "Indon" || oddsFormat == "HK") rowHTML = rowHTML.replace(re, ConvertOddsFromAlgorithm(odds));
        else rowHTML = rowHTML.replace(re, prefixSign + ConvertOdds(odds, side))
    } else {
        var convertedNumber = parseFloat(parseFloat(odds).toFixed(2));
        rowHTML = rowHTML.replace(re, (odds == "" ? "" : convertedNumber))
    }
    re = new RegExp("@Comment", "g");
    rowHTML = rowHTML.replace(re, htTranslations[arrResultRowData[iResultComment]]);
    return rowHTML
};
function ui_doOrderAddResultContinue() {
    if (IsProcessing()) return;
    client_clearUnPlacedBets();
    ui_doClickBPTab("MyBets")
};
function ui_doOrderAmendResultContinue() {
    if (IsProcessing()) return;
    client_clearUnPlacedBets();
    ui_doRefreshMyBets();
    ui_doClickBPTab("MyBets")
};
function ui_setShowBetConfirm(show) { top.userSetSetting(top.cShowBetConfirm, show) };
function ui_setShowBetResult(show) { top.userSetSetting(top.cShowBetResult, show) };
function ui_setOddsWarning(show) { top.userSetSetting(top.cShowOddsWarning, show) };
function ui_setBookPercentage(show) {
    top.userSetSetting(top.cShowBookPercentage, show);
    ui_doBookPercentage(show)
};
var bHasMatchedBets = false; var bHasUnMatchedBets = false;
function server_receiveMyBets(sMyBetsTimestamp, noOfBets, htUnMatchedBackBets, htUnMatchedLayBets, htMatchedBackBets, htMatchedLayBets) {
    if (mybetsTimestamp == null || mybetsTimestamp != sMyBetsTimestamp) {
        iNoOfPlacedBets = noOfBets;
        htMyBetsBackUnMatched = htUnMatchedBackBets;
        htMyBetsLayUnMatched = htUnMatchedLayBets;
        htMyBetsBackMatched = htMatchedBackBets;
        htMyBetsLayMatched = htMatchedLayBets;
        backupUnMatchedData(htMyBetsBackUnMatched);
        backupUnMatchedData(htMyBetsLayUnMatched);
        var hasUnMatchedBets = client_renderMyBetsSection(betState_UnMatched, betSide_Back, htMyBetsBackUnMatched, sSortUnMatchedBackBetsBy, bSortUnMatchedBackBetsDesc);
        hasUnMatchedBets = (client_renderMyBetsSection(betState_UnMatched, betSide_Lay, htMyBetsLayUnMatched, sSortUnMatchedLayBetsBy, bSortUnMatchedLayBetsDesc) || hasUnMatchedBets);
        var htMatchedBetsBackView = htMyBetsBackMatched;
        var htMatchedBetsLayView = htMyBetsLayMatched;
        switch (matchedBetView) {
            case sBetViewConsolidated:
                htMatchedBetsBackView = calculateConsolidatedMatchedBets(htMatchedBetsBackView);
                htMatchedBetsLayView = calculateConsolidatedMatchedBets(htMyBetsLayMatched);
                break;
            case sBetViewAverage:
                htMatchedBetsBackView = calculateAverageMatchedBets(htMatchedBetsBackView);
                htMatchedBetsLayView = calculateAverageMatchedBets(htMyBetsLayMatched);
                break
        }
        var hasMatchedBets = client_renderMyBetsSection(betState_Matched, betSide_Back, htMatchedBetsBackView, sSortMatchedBackBetsBy, bSortMatchedBackBetsDesc);
        hasMatchedBets = (client_renderMyBetsSection(betState_Matched, betSide_Lay, htMatchedBetsLayView, sSortMatchedLayBetsBy, bSortMatchedLayBetsDesc) || hasMatchedBets);
        client_renderMyBets(hasUnMatchedBets, hasMatchedBets);
        bHasMatchedBets = hasMatchedBets;
        bHasUnMatchedBets = hasUnMatchedBets;
        setProfitLossUI()
    }
};
function server_initPlaceBetsPanel() {
    if (!top.bLoggedIn) { ui_doClickBPTab("Details") } else { if (iNoOfFreeBets > 0) { ui_doClickBPTab("PlaceBets") } else if (iNoOfPlacedBets > 0) { ui_doClickBPTab("MyBets") } else { ui_doClickBPTab("Details") } }
};

function backupUnMatchedData(htBets) {
    for (var orderID in htBets) { htBets[orderID][iBetEdited] = false; htBets[orderID][iBetAmountBeforeEdit] = htBets[orderID][iBetStake]; htBets[orderID][iBetOddsBeforeEdit] = htBets[orderID][iBetOdds] }
};

function ui_doUndoChangesUnMatched() {
    restoreUnMatchedData(htMyBetsBackUnMatched); restoreUnMatchedData(htMyBetsLayUnMatched); client_renderMyBetsSection(betState_UnMatched, betSide_Back, htMyBetsBackUnMatched, null, null); client_renderMyBetsSection(betState_UnMatched, betSide_Lay, htMyBetsLayUnMatched, null, null); calculateProfitLoss()
};

function restoreUnMatchedData(htBets) {
    for (var orderID in htBets) { htBets[orderID][iBetEdited] = false; htBets[orderID][iBetStake] = htBets[orderID][iBetAmountBeforeEdit]; htBets[orderID][iBetOdds] = htBets[orderID][iBetOddsBeforeEdit] }
};

function client_renderMyBets(hasUnMatchedBets, hasMatchedBets) {
    if (hasUnMatchedBets) { document.getElementById("trNoUnMatchedBets").style.display = "none"; document.getElementById("trUnMatchedBetsControls").style.display = "" } else { document.getElementById("trNoUnMatchedBets").style.display = ""; document.getElementById("trUnMatchedBetsControls").style.display = "none" } if (hasMatchedBets) { document.getElementById("trNoMatchedBets").style.display = "none" } else { document.getElementById("trNoMatchedBets").style.display = "" }
};

function client_clearMyBets() {
    htMyBetsBackUnMatched = null; htMyBetsLayUnMatched = null; htMyBetsBackMatched = null; htMyBetsLayMatched = null; document.getElementById("spMyUMBBackBets").innerHTML = ""; document.getElementById("spMyUMBLayBets").innerHTML = ""; document.getElementById("spMyMBBackBets").innerHTML = ""; document.getElementById("spMyMBLayBets").innerHTML = ""; document.getElementById("trNoUnMatchedBets").style.display = "none"; document.getElementById("trNoMatchedBets").style.display = "none"; document.getElementById("trUnMatchedBetsControls").style.display = "none"
};

function client_renderMyBetsSection(state, side, htBets, sortBy, sortAsc) {
    var HTML = ""; var editable = (state == betState_UnMatched ? true : false); if (htBets != null) { var arrSortedIds = sortBetsIds(htBets, sortBy, sortAsc); for (var i = 0; i < arrSortedIds.length; i++) { var orderID = arrSortedIds[i]; HTML += html_getBetRow(orderID, state, side, htBets[orderID], editable) } if (HTML != "") { HTML = html_getBetTable(state, side, HTML, editable) } } document.getElementById("spMy" + state + side + "Bets").innerHTML = HTML; if (state == betState_UnMatched) { renderDirtyText() } if (HTML != "") return true; return false
};

function ui_doRefreshMyBets() {
    document.getElementById("tblAmendBetsResult").style.display = "none"; document.getElementById("tblMyBets").style.display = ""; server_getBetData()
};

var callCount = 0;
function ui_doAmendUnMatched() {
    if (IsProcessing()) return; try { eval(sCommandBuffer) } catch (e) { } server_amendBets()
};

function server_receiveCancelResult(arrCancelResult, arrCancelArchiveResult) {
    for (var i = 0; i < arrCancelResult.length; i++) { arrCancelResult[i][1] = htEntrants[arrCancelResult[i][6]][iEtDesc] } if (arrCancelArchiveResult != null && arrCancelArchiveResult.length > 0) { for (var i = 0; i < arrCancelArchiveResult.length; i++) { var htUnMatchedOrders = null; var orderID = arrCancelArchiveResult[i][0]; var betSide = ""; if (htMyBetsBackUnMatched[orderID] != null) { htUnMatchedOrders = htMyBetsBackUnMatched; betSide = betSide_Back } else if (htMyBetsLayUnMatched[orderID] != null) { htUnMatchedOrders = htMyBetsLayUnMatched; betSide = betSide_Lay } if (htUnMatchedOrders != null) { var entrantID = arrCancelArchiveResult[i][1]; var comments = arrCancelArchiveResult[i][2]; var amountUnmatched = htUnMatchedOrders[orderID][iBetStake] - htUnMatchedOrders[orderID][iBetAmountMatched]; var odds = new Number(htUnMatchedOrders[orderID][iBetOdds]); arrCancelResult[arrCancelResult.length] = new Array("-", htEntrants[entrantID][iEtDesc], betSide, amountUnmatched, odds.toFixed(5), comments, entrantID) } } } client_clearMyBets(); client_renderAmendResult(arrCancelResult, true)
};

function server_receiveAmendResult(arrAmendResult, arrAmendArchiveResult) {
    for (var i = 0; i < arrAmendResult.length; i++) { arrAmendResult[i][1] = htEntrants[arrAmendResult[i][6]][iEtDesc] } if (arrAmendArchiveResult != null && arrAmendArchiveResult.length > 0) { for (var i = 0; i < arrAmendArchiveResult.length; i++) { var htUnMatchedOrders = null; var orderID = arrAmendArchiveResult[i][0]; var betSide = ""; if (htMyBetsBackUnMatched[orderID] != null) { htUnMatchedOrders = htMyBetsBackUnMatched; betSide = betSide_Back } else if (htMyBetsLayUnMatched[orderID] != null) { htUnMatchedOrders = htMyBetsLayUnMatched; betSide = betSide_Lay } if (htUnMatchedOrders != null) { var entrantID = arrAmendArchiveResult[i][1]; var comments = arrAmendArchiveResult[i][2]; var amountUnmatched = htUnMatchedOrders[orderID][iBetStake] - htUnMatchedOrders[orderID][iBetAmountMatched]; var odds = new Number(htUnMatchedOrders[orderID][iBetOdds]); arrAmendResult[arrAmendResult.length] = new Array("-", htEntrants[entrantID][iEtDesc], betSide, amountUnmatched, odds, comments, entrantID) } } } client_clearMyBets(); client_stopInPlayDelay(); client_renderAmendResult(arrAmendResult, false)
};

function client_renderAmendResult(arrAmendResult, bCancel) {
    var tableHTML = document.getElementById("taOrderResultTable").value; var rowDataHTML = ""; var i; for (i = 0; i < arrAmendResult.length; i++) { rowDataHTML = rowDataHTML + getOrderResultRowHTML(arrAmendResult[i]) } if (rowDataHTML.length == 0 && bCancel) { rowDataHTML = document.getElementById("taCancelEmptyResultRow").value } var re = new RegExp("@RowData", "g"); tableHTML = tableHTML.replace(re, rowDataHTML); document.getElementById("tblAmendBetsResult").style.display = ""; document.getElementById("tblMyBets").style.display = "none"; document.getElementById("tdAmendBetsResultItems").innerHTML = tableHTML
};
var timeoutInPlayDelay; var currentInPlayDelay = -1;

function client_startInPlayDelay() {
    document.getElementById("dvInPlayDelayHolder").style.display = ""; currentInPlayDelay = iDelay; client_updateInPlayDelay()
};

function client_updateInPlayDelay() {
    if (currentInPlayDelay >= 0) { document.getElementById("dvInPlayDelayCounter").innerHTML = currentInPlayDelay; currentInPlayDelay = currentInPlayDelay - 1; timeoutInPlayDelay = window.setTimeout("client_updateInPlayDelay()", 1000) } else { client_stopInPlayDelay() }
};

function client_stopInPlayDelay() {
    document.getElementById("dvInPlayDelayHolder").style.display = "none"; window.clearTimeout(timeoutInPlayDelay); currentInPlayDelay = -1
};
var iNoOfFreeBets = 0; var htMyFreeBets = {};
function server_receiveMyFreeBets(noOfFreeBets, htFreeBets) {
    iNoOfFreeBets = noOfFreeBets;
    for (var myFreeBetID in htMyFreeBets) {
        for (var freeBetID in htMyFreeBets) { if (myFreeBetID == freeBetID) { htFreeBets[freeBetID][iFreeEdited] = htMyFreeBets[myFreeBetID][iFreeEdited]; htFreeBets[freeBetID][iFreeOdds] = htMyFreeBets[myFreeBetID][iFreeOdds]; continue } }
    } htMyFreeBets = htFreeBets; var newMyFreeBets = {}; for (var myFreeBetID in htMyFreeBets) { if (htMyFreeBets[myFreeBetID][iMemberID] == top.dmid) { newMyFreeBets[myFreeBetID] = htMyFreeBets[myFreeBetID] } else { var imgSrc = "//{Host}/logging/fberr.jpg?LoginMemberID={LoginMemberID}&FreeBetMemberID={FreeBetMemberID}&FreeBetTokenID={FreeBetTokenID}&FreeBetTitle={FreeBetTitle}"; imgSrc = imgSrc.replace("{Host}", window.location.host.replace("www.", "m.")).replace("{LoginMemberID}", top.dmid).replace("{FreeBetMemberID}", htMyFreeBets[myFreeBetID][iMemberID]).replace("{FreeBetTokenID}", myFreeBetID).replace("{FreeBetTitle}", htMyFreeBets[myFreeBetID][iFreePromotion]); (new Image()).src = imgSrc } } htMyFreeBets = newMyFreeBets; client_renderFreeBets((bLastPlaceBetsEditable ? iRenderModeEdit : iRenderModeRead))
};
var iFreePromotion = 0; var iFreeExpiry = 1;
var iFreeUsedAmount = 2; var iFreeOutstandingAmount = 3;
var iFreeEntrant = 4; var iFreeOdds = 5; var iFreeEdited = 6;
var iMemberID = 7; var iRenderModeEdit = 0; var iRenderModeRead = 1;
function client_renderFreeBets(mode) {
    var freeBetsHTML = "";
    for (var freeBetID in htMyFreeBets) { var edited = htMyFreeBets[freeBetID][iFreeEdited]; if (edited || mode == iRenderModeEdit) { freeBetsHTML += html_getFreeBetRow(mode, freeBetID, htMyFreeBets[freeBetID]) } } if (freeBetsHTML.length > 0) { document.getElementById("tblPlaceFreeBets").style.display = ""; var hasPendingFree = checkHasFreeBetsPending(); if (hasPendingFree) { freeBetsHTML = html_getBetTable(betState_UnPlaced, betSide_Back, freeBetsHTML, false) } else { freeBetsHTML = html_getBetTableNoHeader(freeBetsHTML) } document.getElementById("tdFreeBetsItems").innerHTML = freeBetsHTML } else { document.getElementById("tblPlaceFreeBets").style.display = "none"; document.getElementById("tdFreeBetsItems").innerHTML = "" }
};

function html_getFreeBetRow(mode, id, arrFreeBet) {
    var edited = arrFreeBet[iFreeEdited]; var freeBetRowHTML = ""; if (mode == iRenderModeEdit) { if (!edited) { freeBetRowHTML = document.getElementById("taFreeBetRowUnused").value } else { freeBetRowHTML = document.getElementById("taFreeBetRowEdit").value } } else { freeBetRowHTML = document.getElementById("taFreeBetRowRead").value } var dExpiry = parseDateTime(arrFreeBet[iFreeExpiry]); dExpiry.setMinutes(dExpiry.getMinutes() + iGMTOffsetMins); var re = new RegExp("@Desc", "g"); var freeBetRowHTML = freeBetRowHTML.replace(re, arrFreeBet[iFreePromotion]); re = new RegExp("@ID", "g"); freeBetRowHTML = freeBetRowHTML.replace(re, id); re = new RegExp("@Expiry", "g"); freeBetRowHTML = freeBetRowHTML.replace(re, formatDateTimeMins(dExpiry)); re = new RegExp("@Used", "g"); freeBetRowHTML = freeBetRowHTML.replace(re, formatCurrency(arrFreeBet[iFreeUsedAmount], sSymbol, numDec)); re = new RegExp("@Remaining", "g"); freeBetRowHTML = freeBetRowHTML.replace(re, formatCurrency(arrFreeBet[iFreeOutstandingAmount], sSymbol, numDec)); re = new RegExp("@State", "g"); freeBetRowHTML = freeBetRowHTML.replace(re, betState_UnPlacedFree); re = new RegExp("@Entrant", "g"); var entrant = arrFreeBet[iFreeEntrant]; if (entrant.length > 0) { entrant = htEntrants[entrant][iEtDesc] } else { entrant = sBackEntrant } freeBetRowHTML = freeBetRowHTML.replace(re, entrant); re = new RegExp("@Odds", "g"); freeBetRowHTML = freeBetRowHTML.replace(re, ConvertOdds(arrFreeBet[iFreeOdds])); var stake = arrFreeBet[iFreeOutstandingAmount]; var odds = trim(arrFreeBet[iFreeOdds]); if (mode == iRenderModeRead && odds == "") { freeBetRowHTML = ""; return freeBetRowHTML } var pl = space; pl = calcProfit(stake, odds, betSide_Back); if (pl != space) pl = formatCurrency(pl, sSymbol, numDec); re = new RegExp("@PL", "g"); freeBetRowHTML = freeBetRowHTML.replace(re, pl); return freeBetRowHTML
};

function ui_doPlaceFree(state, id) {
    if (IsProcessing()) return; calculateProfitLoss(); htMyFreeBets[id][iFreeEdited] = true; client_renderFreeBets(iRenderModeEdit); ui_doSelectFreeBet(state, id); client_renderPlaceBets(true)
};
var selectedFreeBetID = ""; var freeBackMode = false;
function ui_doSelectFreeBet(state, id) {
    if (IsProcessing()) return; freeBackMode = true; if (selectedFreeBetID.length > 0) document.getElementById("inEntrant_" + state + "_" + selectedFreeBetID).className = "inFreeEntrantOff"; selectedFreeBetID = id; document.getElementById("inEntrant_" + state + "_" + id).className = "inFreeEntrantOn"
};

function ui_doDeleteFreeBet(state, id) {
    if (IsProcessing()) return; htMyFreeBets[id][iFreeEdited] = false; htMyFreeBets[id][iFreeEntrant] = ""; htMyFreeBets[id][iFreeOdds] = ""; selectedFreeBetID = ""; freeBackMode = false; client_renderFreeBets(iRenderModeEdit); client_renderPlaceBets(true)
};

function checkHasUnPlacedBets() {
    var hasBets = false; for (var i in htPlaceBetsBack) { return true } var hasBets = false; for (var i in htPlaceBetsLay) { return true } return checkHasFreeBetsPending()
};

function checkHasFreeBetsPending() {
    for (var i in htMyFreeBets) { if (htMyFreeBets[i][iFreeEdited]) { return true } } return false
};

function doFreeBackForBestBet(entrantID, odd) {
    htMyFreeBets[selectedFreeBetID][iFreeEntrant] = entrantID; htMyFreeBets[selectedFreeBetID][iFreeOdds] = odd; document.getElementById("inEntrant_" + betState_UnPlacedFree + "_" + selectedFreeBetID).value = htEntrants[entrantID][iEtDesc]; document.getElementById("inOdds_" + betState_UnPlacedFree + "_" + selectedFreeBetID).value = iif(htMyFreeBets[selectedFreeBetID][iFreeOdds] > 0, ConvertOdds(htMyFreeBets[selectedFreeBetID][iFreeOdds]), ""); ui_doClickBPTab("PlaceBets"); document.getElementById("inOdds_" + betState_UnPlacedFree + "_" + selectedFreeBetID).focus(); document.getElementById("inEntrant_" + betState_UnPlacedFree + "_" + selectedFreeBetID).className = "inFreeEntrantOff"; ui_doUpdatePLFree(betState_UnPlacedFree, selectedFreeBetID); selectedFreeBetID = ""; freeBackMode = false
};

function doFreeBack(entrantID, oddsIndex) {
    htMyFreeBets[selectedFreeBetID][iFreeEntrant] = entrantID; htMyFreeBets[selectedFreeBetID][iFreeOdds] = htOdds[entrantID][oddsIndex]; document.getElementById("inEntrant_" + betState_UnPlacedFree + "_" + selectedFreeBetID).value = htEntrants[entrantID][iEtDesc]; document.getElementById("inOdds_" + betState_UnPlacedFree + "_" + selectedFreeBetID).value = ConvertOdds(htMyFreeBets[selectedFreeBetID][iFreeOdds]); ui_doClickBPTab("PlaceBets"); document.getElementById("inOdds_" + betState_UnPlacedFree + "_" + selectedFreeBetID).focus(); document.getElementById("inEntrant_" + betState_UnPlacedFree + "_" + selectedFreeBetID).className = "inFreeEntrantOff"; ui_doUpdatePLFree(betState_UnPlacedFree, selectedFreeBetID); selectedFreeBetID = ""; freeBackMode = false
};

function ui_doUpdatePLFree(state, id) {
    if (IsProcessing()) return; var stake = htMyFreeBets[id][iFreeOutstandingAmount]; var odds = htMyFreeBets[id][iFreeOdds]; var oddsObj = document.getElementById("inOdds_" + state + "_" + id).value; if (isNum(oddsObj)) { try { odds = ConvertOddsToDec(document.getElementById("inOdds_" + state + "_" + id).value) } catch (ex) { } } var pl = space; pl = calcProfit(stake, odds, betSide_Back); if (pl != space) pl = formatCurrency(pl, sSymbol, numDec); document.getElementById("tdPLFree_" + state + "_" + id).innerHTML = pl
};

function ui_doUpdateOddsFree(state, id, bKeyPressUpdate) {
    if (IsProcessing()) return; var oddsID = "inOdds_" + state + "_" + id; try { var odds = ConvertOddsToDec(document.getElementById(oddsID).value) } catch (ex) {
        top.showMessageBox(ex, null, oddsFormat + "OddsFormat",
        function () { document.getElementById(oddsID).focus(); document.getElementById(oddsID).value = "" }); return
    } if (!isNum(odds)) {
        if (odds != "") {
            htMyFreeBets[id][iFreeOdds] = ""; if (bKeyPressUpdate) return; top.showMessageBox(sOddsNumeric, null, null,
            function () { document.getElementById(oddsID).focus() }); document.getElementById(oddsID).value = ""; return
        }
    } else { document.getElementById(oddsID).value = CT2[odds]; htMyFreeBets[id][iFreeOdds] = odds.toString() }
    ui_doUpdatePLFree(state, id)
};
function validateUnPlacedBetsFree() {
    for (var freeBetID in htMyFreeBets) { var edited = htMyFreeBets[freeBetID][iFreeEdited]; if (edited) { var breakOut = false; var entrantID = htMyFreeBets[freeBetID][iFreeEntrant].toString(); var odds = trim(htMyFreeBets[freeBetID][iFreeOdds]); if (entrantID == "") { breakOut = true; top.showMessageBox(sEntrantRequiredFree, null, null) } if (!breakOut && odds == "") { continue; } if (!breakOut && (CT2[odds] == null)) { breakOut = true; odds = ""; top.showMessageBox(sOddsNumeric, null, null) } if (breakOut) { return false } if (odds > iWarningOdds) bHasBetsOverWarnAmount = true; bHasValidUnplacedBets = true } } return true
};
var htHandicapPLMatched = {};

function calculateProfitLossHandicapMarket() {
    if (htMyBetsBackMatched == null || htMyBetsLayMatched == null) { ui_doRefreshMyBets() } else { htHandicapPLMatched = {}; helper_calculateWinLoseForHandicaps(htHandicapPLMatched, htMyBetsBackMatched, betSide_Back); helper_calculateWinLoseForHandicaps(htHandicapPLMatched, htMyBetsLayMatched, betSide_Lay); if (top.userGetSetting(top.cPLIncUnMatched)) { helper_calculateWinLoseForHandicaps(htHandicapPLMatched, htMyBetsBackUnMatched, betSide_Back); helper_calculateWinLoseForHandicaps(htHandicapPLMatched, htMyBetsLayUnMatched, betSide_Lay) } helper_splitQuarteredHandicaps(htHandicapPLMatched) }
};

function helper_virtualSettleHandicapMarket(score, htHandicapPL) {
    var pl = 0; for (var handicap in htHandicapPL) { var arrValues = htHandicapPL[handicap]; if (sBetType == "Totals") { if (handicap * 1 + score * 1 < 0) { pl += arrValues[0] } else { pl += arrValues[1] } } else { if (score * 1 + handicap * 1 > 0) { pl += arrValues[0] } else if (score * 1 + handicap * 1 < 0) { pl += arrValues[1] } } }; if (top.userGetSetting(top.cPLIncCommision)) { if (pl > 0) { pl = pl - (pl * iMarketCommission / 100) } } return pl
};

function helper_splitQuarteredHandicaps(htHandicapPL) {
    for (var handicap in htHandicapPL) { if (handicap.toString().indexOf(".25") > -1 || handicap.toString().indexOf(".75") > -1) { var arrValues = htHandicapPL[handicap]; var nextHandicap = handicap * 1 + 0.25; if (htHandicapPL[nextHandicap] == null) { var arrNextValues = [0, 0]; htHandicapPL[nextHandicap] = arrNextValues } var arrNextValues = htHandicapPL[nextHandicap]; arrNextValues[0] += arrValues[0] / 2; arrNextValues[1] += arrValues[1] / 2; var prevHandicap = handicap * 1 - 0.25; if (htHandicapPL[prevHandicap] == null) { var arrPrevValues = [0, 0]; htHandicapPL[prevHandicap] = arrPrevValues } var arrPrevValues = htHandicapPL[prevHandicap]; arrPrevValues[0] += arrValues[0] / 2; arrPrevValues[1] += arrValues[1] / 2; delete htHandicapPL[handicap] } }
};

function helper_calculateWinLoseForHandicaps(htHandicapPL, htBets, side) {
    var entrantID; var team; var entrantWin; var entrantLose;
    var stake; var odds; var handicap; for (var id in htBets) { entrantID = htBets[id][iBetEntrantID]; if (htEntrants[entrantID] == null) break; stake = htBets[id][iBetStake]; odds = htBets[id][iBetOdds]; if (stake != "" && odds != "") { team = htEntrants[entrantID][iEtIdent]; handicap = htEntrants[entrantID][iEtHandicap] * 1; if (side == betSide_Back) { entrantWin = stake * odds - stake; entrantLose = -stake * 1 } else { entrantWin = -(stake * odds - stake); entrantLose = stake * 1 } if (team == sTeam2 && sBetType != "Totals") { handicap = handicap * -1 } if (htHandicapPL[handicap] == null) { var values = [0, 0]; htHandicapPL[handicap] = values } var arrValues = htHandicapPL[handicap]; if (team == sTeam1) { arrValues[0] += entrantWin; arrValues[1] += entrantLose } else { arrValues[0] += entrantLose; arrValues[1] += entrantWin } } }
};
var iMLAll = 0; var iUMLAll = 0; var iUPLAll = 0; var marketProfit = 0;

function calculateProfitLoss() {
    if (!top.bLoggedIn || top.userGetSetting(top.cHideProfitLoss)) { return } if (sBetType == "Odds") { calculateProfitLossOddsMarket() } else { calculateProfitLossHandicapMarket(); client_renderHandicapProfitLoss() }
};

function helper_roundHandicap(handicap) {
    if (handicap > 0) { handicap = Math.ceil(handicap) } else { handicap = Math.floor(handicap) } return handicap
};

function helper_wholeNumber(num) { return num == Math.ceil(num) };
function NumberCompareDesc(a, b) { return b - a }; function NumberCompareAsc(a, b) { return a - b };

function client_renderHandicapProfitLoss() {
    if (bHasMatchedBets || (bHasUnMatchedBets && top.userGetSetting(top.cPLIncUnMatched))) { document.getElementById("trOverallPLTable").style.display = ""; document.getElementById("trOverallPLTableNA").style.display = "none" } else { document.getElementById("trOverallPLTable").style.display = "none"; document.getElementById("trOverallPLTableNA").style.display = ""; return } var arrTeam1Handicaps = new Array(); var arrTeam2Handicaps = new Array(); var bZeroExist = false; for (var handicap in htHandicapPLMatched) { if (handicap * 1 == 0) { bZeroExist = true } if (handicap * 1 <= 0 || sBetType == "Totals") { arrTeam1Handicaps.push(handicap * 1) } else { arrTeam2Handicaps.push(handicap * 1) } } if (!bZeroExist) { arrTeam1Handicaps.push(0) } var templatePrefix = "Handicap"; if (sBetType == "Totals") { templatePrefix = "Totals"; arrTeam1Handicaps.sort(NumberCompareDesc) } else { if (arrTeam1Handicaps.toString().indexOf("-0.5") == -1) { arrTeam1Handicaps.push(-0.5) } if (arrTeam2Handicaps.toString().indexOf("0.5") == -1) { arrTeam2Handicaps.push(0.5) } arrTeam1Handicaps.sort(NumberCompareAsc); arrTeam2Handicaps.sort(NumberCompareDesc) } var team1HTML = document.getElementById("ta" + templatePrefix + "OverallPLIFRow").value; var re = new RegExp("@Team", "g"); team1HTML = team1HTML.replace(re, sTeam1); team1HTML += html_getHandicapPLTeam(arrTeam1Handicaps, htHandicapPLMatched); re = new RegExp("@Unit", "g"); team1HTML = team1HTML.replace(re, sUnit); var team2HTML = ""; if (sBetType != "Totals") { team2HTML = document.getElementById("ta" + templatePrefix + "OverallPLIFRow").value; re = new RegExp("@Team", "g"); team2HTML = team2HTML.replace(re, sTeam2); re = new RegExp("@Unit", "g"); team2HTML = team2HTML.replace(re, sUnit); team2HTML += html_getHandicapPLTeam(arrTeam2Handicaps, htHandicapPLMatched) } var tableHTML = document.getElementById("taOverallPLTable").value; re = new RegExp("@RowData", "g"); tableHTML = tableHTML.replace(re, team1HTML + team2HTML); document.getElementById("tdOverallPLTable").innerHTML = tableHTML
};

function html_getHandicapPLTeam(arrTeamHandicaps, htHandicapPL) {
    var prevSettleScore = null; var currentSettleScore = null; var resultHTML = ""; for (i = 0; i < arrTeamHandicaps.length; i++) { var currentHandicap = arrTeamHandicaps[i]; if (helper_wholeNumber(currentHandicap) && currentHandicap != 0) { if (currentHandicap > 0) { currentSettleScore = currentHandicap + 1 } else { currentSettleScore = currentHandicap - 1 } if (prevSettleScore != currentSettleScore) { var pl = helper_virtualSettleHandicapMarket(-currentSettleScore, htHandicapPL); resultHTML += html_getHandicapPLRow((prevSettleScore == null ? null : Math.abs(prevSettleScore)), Math.abs(currentSettleScore), pl); prevSettleScore = currentSettleScore } currentSettleScore = currentHandicap; var pl = helper_virtualSettleHandicapMarket(-currentSettleScore, htHandicapPL); resultHTML += html_getHandicapPLRow((prevSettleScore == null ? null : Math.abs(prevSettleScore)), Math.abs(currentHandicap), pl); prevSettleScore = currentSettleScore } else { currentSettleScore = helper_roundHandicap(currentHandicap); if (prevSettleScore != currentSettleScore) { var pl = helper_virtualSettleHandicapMarket(-currentSettleScore, htHandicapPL); resultHTML += html_getHandicapPLRow((prevSettleScore == null ? null : Math.abs(prevSettleScore)), Math.abs(currentSettleScore), pl); prevSettleScore = currentSettleScore } } } return resultHTML
};

function html_getHandicapPLRow(prevBoundary, boundary, pl) {
    var resultHTML = ""; var templatePrefix = "Handicap"; if (sBetType == "Totals") { templatePrefix = "Totals" } if (prevBoundary == null) { resultHTML = document.getElementById("ta" + templatePrefix + "OverallPLMoreRow").value } else if (boundary == 0 && sBetType != "Totals") { resultHTML = document.getElementById("ta" + templatePrefix + "OverallPLTieRow").value } else if (prevBoundary - boundary > 1) { resultHTML = document.getElementById("ta" + templatePrefix + "OverallPLRangeRow").value } else { resultHTML = document.getElementById("ta" + templatePrefix + "OverallPLExactRow").value } var re = new RegExp("@Score2", "g"); resultHTML = resultHTML.replace(re, prevBoundary - 1); re = new RegExp("@PLHandWin", "g"); resultHTML = resultHTML.replace(re, htTranslations["PLWin"]); re = new RegExp("@Score", "g"); resultHTML = resultHTML.replace(re, boundary); re = new RegExp("@Unit", "g"); if (boundary > 1) { resultHTML = resultHTML.replace(re, sUnit) } else { resultHTML = resultHTML.replace(re, sUnitSingle) } re = new RegExp("@PLValue", "g"); resultHTML = resultHTML.replace(re, formatCurrency(pl, sSymbol, numDec)); var result = ""; if (pl > 0) { result = "Win" } if (pl < 0) { result = "Lose" } re = new RegExp("@Result", "g"); resultHTML = resultHTML.replace(re, result); return resultHTML
};

function calculateProfitLossOddsMarket() {
    resetWinLoseForEntrants(); var lose; if (htMyBetsBackMatched == null || htMyBetsLayMatched == null) { ui_doRefreshMyBets(); iMLAll = 0 } else { lose = calculateWinLoseForEntrants(htMyBetsBackMatched, betSide_Back, iEtMWin, iEtMLose); iMLAll = iMLAll - lose; lose = calculateWinLoseForEntrants(htMyBetsLayMatched, betSide_Lay, iEtMWin, iEtMLose); iMLAll = iMLAll + lose } if (top.userGetSetting(top.cPLIncUnMatched)) { if (htMyBetsBackUnMatched == null || htMyBetsLayUnMatched == null) { iUMLAll = 0 } else { lose = calculateWinLoseForEntrants(htMyBetsBackUnMatched, betSide_Back, iEtUMWin, iEtUMLose); iUMLAll = iUMLAll - lose; lose = calculateWinLoseForEntrants(htMyBetsLayUnMatched, betSide_Lay, iEtUMWin, iEtUMLose); iUMLAll = iUMLAll + lose } } if (top.userGetSetting(top.cPLIncUnPlaced)) { lose = calculateWinLoseForEntrants(htPlaceBetsBack, betSide_Back, iEtUPWin, iEtUPLose); iUPLAll = iUPLAll - lose; lose = calculateWinLoseForEntrants(htPlaceBetsLay, betSide_Lay, iEtUPWin, iEtUPLose); iUPLAll = iUPLAll + lose } for (var entrantID in htEntrants) { if (document.getElementById("sp" + entrantID + "WinValue") == null) ui_doRefreshMyBets() } try { for (var entrantID in htEntrants) { var entrantStatus = htEntrants[entrantID][iEtStatus]; if (entrantStatus == "Open") { var pl = htEntrants[entrantID][iEtMWin] * 1 + iMLAll - htEntrants[entrantID][iEtMLose]; var win = htEntrants[entrantID][iEtMWin]; var lose = htEntrants[entrantID][iEtMLose]; if (top.userGetSetting(top.cPLIncSettled)) { pl = pl + marketProfit } if (top.userGetSetting(top.cPLIncUnMatched)) { pl = pl + htEntrants[entrantID][iEtUMWin] * 1 + iUMLAll - htEntrants[entrantID][iEtUMLose]; win = win + htEntrants[entrantID][iEtUMWin]; lose = lose + htEntrants[entrantID][iEtUMLose] } if (top.userGetSetting(top.cPLIncUnPlaced)) { pl = pl + htEntrants[entrantID][iEtUPWin] * 1 + iUPLAll - htEntrants[entrantID][iEtUPLose]; win = win + htEntrants[entrantID][iEtUPWin]; lose = lose + htEntrants[entrantID][iEtUPLose] } if (top.userGetSetting(top.cPLIncCommision)) { if (pl > 0) { pl = pl - (pl * iMarketCommission / 100) } if (win > 0) { win = win - (win * iMarketCommission / 100) } if (lose > 0) { lose = lose - (lose * iMarketCommission / 100) } } htEntrants[entrantID][iEtProfitLossDisplay] = pl; htEntrants[entrantID][iEtWinDisplay] = win; htEntrants[entrantID][iEtLoseDisplay] = lose; if (iNoOfWinners == 1) { if (pl == 0) { document.getElementById("sp" + entrantID + "WinValue").innerHTML = ""; document.getElementById("sp" + entrantID + "LoseValue").innerHTML = "" } else if (pl > 0) { document.getElementById("sp" + entrantID + "WinValue").innerHTML = formatCurrency(pl, sSymbol, numDec); document.getElementById("sp" + entrantID + "LoseValue").innerHTML = ""; document.getElementById("sp" + entrantID + "WinValue").className = "PLWin" } else { document.getElementById("sp" + entrantID + "WinValue").innerHTML = ""; document.getElementById("sp" + entrantID + "LoseValue").innerHTML = formatCurrency(pl, sSymbol, numDec); document.getElementById("sp" + entrantID + "WinValue").className = "PLLose" } } else { if (win == 0 && lose == 0) { document.getElementById("sp" + entrantID + "WinValue").innerHTML = ""; document.getElementById("sp" + entrantID + "LoseValue").innerHTML = "" } else { document.getElementById("sp" + entrantID + "WinValue").innerHTML = formatCurrency(win, sSymbol, numDec) + ", "; document.getElementById("sp" + entrantID + "LoseValue").innerHTML = formatCurrency(lose, sSymbol, numDec) } if (win < 0) { document.getElementById("sp" + entrantID + "WinValue").className = "PLLose" } else { document.getElementById("sp" + entrantID + "WinValue").className = "PLWin" } if (lose >= 0) { document.getElementById("sp" + entrantID + "LoseValue").className = "PLWin" } else { document.getElementById("sp" + entrantID + "LoseValue").className = "PLLose" } } } } } catch (ex) { ui_doRefreshMyBets() }
}

function calculateWinLoseForEntrants(htBets, side, winIndex, loseIndex) {
    var entrantID; var entrantWin;
    var entrantLose; var stake; var odds;
    var loseALL = 0;
    for (var id in htBets) {
        entrantID = htBets[id][iBetEntrantID];
        if (htEntrants[entrantID] == null) break;
        stake = htBets[id][iBetStake]; odds = htBets[id][iBetOdds];
        if (stake != "" && odds != "") {
            entrantWin = htEntrants[entrantID][winIndex] * 1; entrantLose = htEntrants[entrantID][loseIndex] * 1;
            if (side == betSide_Back) {
                entrantWin = entrantWin + (stake * odds - stake); entrantLose = entrantLose - stake * 1
            } else {
                entrantWin = entrantWin - (stake * odds - stake); entrantLose = entrantLose + (stake * 1)
            }
            loseALL = loseALL + (stake * 1);
            htEntrants[entrantID][winIndex] = entrantWin;
            htEntrants[entrantID][loseIndex] = entrantLose
        }
    } return loseALL
};

function resetWinLoseForEntrants() {
    iMLAll = 0; iUMLAll = 0; iUPLAll = 0;
    for (var entrantID in htEntrants) {
        htEntrants[entrantID][iEtUPWin] = 0; htEntrants[entrantID][iEtUPLose] = 0;
        htEntrants[entrantID][iEtMWin] = 0; htEntrants[entrantID][iEtMLose] = 0;
        htEntrants[entrantID][iEtUMWin] = 0; htEntrants[entrantID][iEtUMLose] = 0;
        htEntrants[entrantID][iEtProfitLossDisplay] = 0;
        htEntrants[entrantID][iEtWinDisplay] = 0; htEntrants[entrantID][iEtLoseDisplay] = 0
    }
};

function setProfitLossUI() {
    var bHideProfitLoss = (top.userGetSetting(top.cHideProfitLoss) || !top.bLoggedIn);
    if (bHideProfitLoss) {
        document.getElementById("chkPLMinusComm").disabled = true;
        $("#chkPLMinusComm").parent().next().css("color", "#999999");
        document.getElementById("chkPLIncUnMatched").disabled = true;
        $("#chkPLIncUnMatched").parent().next().css("color", "#999999");
        document.getElementById("chkPLIncUnPlaced").disabled = true;
        $("#chkPLIncUnPlaced").parent().next().css("color", "#999999");
        document.getElementById("chkPLIncSettled").disabled = true; $("#chkPLIncSettled").parent().next().css("color", "#999999");
        if (top.bLoggedIn) {
            if (sBetType == "Odds") {
                for (var entrantID in htEntrants) {
                    document.getElementById("dv" + entrantID + "ProfitLoss").style.display = "none"
                }
            } else {
                document.getElementById("trOverallPLHeader").style.display = "none";
                document.getElementById("trOverallPLTable").style.display = "none";
                document.getElementById("trOverallPLTableNA").style.display = "none"; document.getElementById("trOverallPLGap").style.display = "none";
                document.getElementById("trOverallPLGap2").style.display = "none"
            }
        }
    } else {
        document.getElementById("chkPLMinusComm").disabled = false;
        $("#chkPLMinusComm").parent().next().css("color", "#000000");
        document.getElementById("chkPLIncUnMatched").disabled = false;
        $("#chkPLIncUnMatched").parent().next().css("color", "#000000");
        document.getElementById("chkPLIncUnPlaced").disabled = !top.userGetSetting(top.cPLIncUnMatched);
        $("#chkPLIncUnPlaced").parent().next().css("color", !top.userGetSetting(top.cPLIncUnMatched) ? "#999999" : "#000000");
        document.getElementById("chkPLIncSettled").disabled = false;
        $("#chkPLIncSettled").parent().next().css("color", "#000000"); calculateProfitLoss();
        if (sBetType == "Odds") {
            for (var entrantID in htEntrants) {
                var winValue = ""; if (document.getElementById("sp" + entrantID + "WinValue") != null) {
                    winValue = document.getElementById("sp" + entrantID + "WinValue").innerText
                } var lostValue = "";
                if (document.getElementById("sp" + entrantID + "LoseValue") != null) {
                    lostValue = document.getElementById("sp" + entrantID + "LoseValue").innerText
                }
                document.getElementById("dv" + entrantID + "ProfitLoss").style.display = ((winValue == "") && (lostValue == "")) ? "none" : ""
            }
        } else {
            document.getElementById("trOverallPLHeader").style.display = "";
            document.getElementById("trOverallPLGap").style.display = "";
            document.getElementById("trOverallPLGap2").style.display = ""
        }
    }
    ui_doResize()
};

var sBetViewNormal = "Normal"; var sBetViewConsolidated = "Consolidated";
var sBetViewAverage = "Average"; var matchedBetView = sBetViewNormal;

function ui_changeMatchedBetsView(viewType) {
    if (IsProcessing()) return; matchedBetView = viewType;
    var htMatchedBetsBackView = htMyBetsBackMatched;
    var htMatchedBetsLayView = htMyBetsLayMatched;
    switch (matchedBetView) {
        case sBetViewConsolidated:
            htMatchedBetsBackView = calculateConsolidatedMatchedBets(htMatchedBetsBackView);
            htMatchedBetsLayView = calculateConsolidatedMatchedBets(htMyBetsLayMatched); break;
        case sBetViewAverage:
            htMatchedBetsBackView = calculateAverageMatchedBets(htMatchedBetsBackView);
            htMatchedBetsLayView = calculateAverageMatchedBets(htMyBetsLayMatched);
            break
    }
    client_renderMyBetsSection(betState_Matched, betSide_Back, htMatchedBetsBackView, sSortMatchedBackBetsBy, bSortMatchedBackBetsDesc);
    client_renderMyBetsSection(betState_Matched, betSide_Lay, htMatchedBetsLayView, sSortMatchedLayBetsBy, bSortMatchedLayBetsDesc)
};

function calculateAverageMatchedBets(htBets) {
    var htAverageBets = {};
    if (htBets != null) {
        var key = "";
        for (var oId in htBets) {
            var etId = htBets[oId][iBetEntrantID]; var stake = htBets[oId][iBetStake] * 1;
            var odds = htBets[oId][iBetOdds] * 1; key = etId;
            if (htAverageBets[key] == null) {
                htAverageBets[key] = new Array(); htAverageBets[key][iBetEntrantID] = etId;
                htAverageBets[key][iBetStake] = stake; htAverageBets[key][iBetOdds] = odds * stake
            } else {
                htAverageBets[key][iBetStake] = htAverageBets[key][iBetStake] * 1 + stake;
                htAverageBets[key][iBetOdds] = htAverageBets[key][iBetOdds] * 1 + odds * stake
            }
        }
        for (var avgBetID in htAverageBets) {
            htAverageBets[avgBetID][iBetOdds] = htAverageBets[avgBetID][iBetOdds] * 1 / htAverageBets[avgBetID][iBetStake] * 1
        }
    }
    return htAverageBets
};

function calculateConsolidatedMatchedBets(htBets) {
    var htConsolidatedBets = {};
    if (htBets != null) {
        var key = ""; for (var oId in htBets) {
            var etId = htBets[oId][iBetEntrantID]; var stake = htBets[oId][iBetStake] * 1;
            var odds = htBets[oId][iBetOdds] * 1; key = etId + odds.toString();
            if (htConsolidatedBets[key] == null) {
                htConsolidatedBets[key] = new Array();
                htConsolidatedBets[key][iBetEntrantID] = etId;
                htConsolidatedBets[key][iBetStake] = stake;
                htConsolidatedBets[key][iBetOdds] = odds
            } else {
                htConsolidatedBets[key][iBetStake] = htConsolidatedBets[key][iBetStake] * 1 + stake
            }
        }
    }
    return htConsolidatedBets
};

function server_receiveExposure(sMyBetsTimestamp, exposure) {
    if (mybetsTimestamp == null || mybetsTimestamp != sMyBetsTimestamp) {
        document.getElementById("spMyExposure").innerHTML = formatCurrency(exposure, sSymbol, numDec);
        mybetsTimestamp = sMyBetsTimestamp
    }
    window.clearTimeout(timeoutMyBets);
    timeoutMyBets = window.setTimeout("ui_refreshMyBets()", mybetsTimeout)
};

function ui_openPLSettings() {
    if (IsProcessing()) return;
    if (document.getElementById("dvPLSettingsHolder").style.display == "") {
        ui_closePLSettings()
    } else {
        document.getElementById("chkPLIncSettled").checked = top.userGetSetting(top.cPLIncSettled);
        document.getElementById("chkPLMinusComm").checked = top.userGetSetting(top.cPLIncCommision);
        document.getElementById("chkPLIncUnMatched").checked = top.userGetSetting(top.cPLIncUnMatched);
        document.getElementById("chkPLIncUnPlaced").checked = top.userGetSetting(top.cPLIncUnPlaced);
        document.getElementById("chkPLIncUnPopupWin").checked = top.userGetSetting(top.cPLIncUnPopupWin);
        document.getElementById("chkPLShow").checked = !top.userGetSetting(top.cHideProfitLoss);
        if (iPLMarketView == top.MarketView_SportsbookView) {
            iPLMarketDepth = top.userGetSettingInt(top.cPLSportsbookMarketDepth);
            bPLShowTotalMatched = top.userGetSetting(top.cPLSportsbookShowTotalMatched);
            bPLShowBook = top.userGetSetting(top.cPLSportsbookShowBook)
        } else {
            iPLMarketDepth = top.userGetSettingInt(top.cPLExchangeMarketDepth);
            bPLShowTotalMatched = top.userGetSetting(top.cPLExchangeShowTotalMatched);
            bPLShowBook = top.userGetSetting(top.cPLExchangeShowBook)
        } ui_changeMarketViewUI(iPLMarketView, true, false);
        document.getElementById("dvPLSettingsHolder").style.display = ""
    }
    if (top.bLoggedIn) {
        document.getElementById("tbProfitLoss").style.display = ""
    } else {
        document.getElementById("tbProfitLoss").style.display = "none"
    }
    if (document.getElementById("tbProfitLoss").className != "here") {
        document.getElementById("tbDisplay").className = "here"
    }
};

function ui_closePLSettings() {
    if (IsProcessing()) return;
    document.getElementById("dvPLSettingsHolder").style.display = "none"
};

function ui_setIncludeSettled(show) {
    top.userSetSetting(top.cPLIncSettled, show); setProfitLossUI()
}

function ui_setIncludeCommission(show) {
    top.userSetSetting(top.cPLIncCommision, show); setProfitLossUI()
}

function ui_setIncludeUnmatched(show) {
    top.userSetSetting(top.cPLIncUnMatched, show); setProfitLossUI()
}

function ui_setIncludeUnPlaced(show) { top.userSetSetting(top.cPLIncUnPlaced, show); setProfitLossUI() }
function ui_setIncludeUnPopupWin(show) { top.userSetSetting(top.cPLIncUnPopupWin, show); setProfitLossUI() }
function ui_setShowBookPercentage(show) { top.userSetSetting(top.cShowBookPercentage, show); setProfitLossUI() }
function ui_setShowProfitLoss(show) { top.userSetSetting(top.cHideProfitLoss, !show); setProfitLossUI() }
function fnSetCookies(position, value) { top.userSetSetting(position, value) }
function updateSettings(settingPositions, settingsValues) {
    var settings = readCookie("S"); var newSetting = ""; for (var i = 0; i < settingPositions.length; i++) { newSetting = settingsValues[i].toString(); settings = settings.substr(0, settingPositions[i]) + newSetting + settings.substr(settingPositions[i] + 1, settings.length - 1) } writeCookie("S", settings, top.iCookieDays, top.sCookieDomain)
};

function ui_savePLSettings(isMarketGridSetting) {
    if (IsProcessing()) return; top.userSetSetting(top.cShowStakeFillTool, iShowStakeFillTool); var iPrevMarketView = iPLMarketView; var iPrevMarketDepth = iPLMarketDepth; var iPrevMinWin = iPLMinWin; var rdPLMarketView = document.getElementsByName("rdPLMarketView"); iPLMarketView = getCheckedValue(rdPLMarketView); var ddlPLMarketDepth = document.getElementById("ddlPLMarketDepth"); iPLMarketDepth = ddlPLMarketDepth.options[ddlPLMarketDepth.selectedIndex].value; var ddlPLMinWin = document.getElementById("ddlPLMinWin"); iPLMinWin = ddlPLMinWin.options[ddlPLMinWin.selectedIndex].value; bPLShowBook = document.getElementById("chkPLShowBook").checked ? '1' : '0'; bPLShowTotalMatched = document.getElementById("chkPLShowTotalMatched").checked ? '1' : '0'; var posa = new Array(); var va = new Array(); posa[0] = top.cPLMinWin; va[0] = iPLMinWin; if (iPLMarketView == top.MarketView_SportsbookView) { posa[1] = top.cPLSportsbookMarketDepth; va[1] = iPLMarketDepth; posa[2] = top.cPLSportsbookShowBook; va[2] = bPLShowBook; posa[3] = top.cPLSportsbookShowTotalMatched; va[3] = bPLShowTotalMatched } else { posa[1] = top.cPLExchangeMarketDepth; va[1] = iPLMarketDepth; posa[2] = top.cPLExchangeShowBook; va[2] = bPLShowBook; posa[3] = top.cPLExchangeShowTotalMatched; va[3] = bPLShowTotalMatched } updateSettings(posa, va); if (!(iPLMarketView == iPrevMarketView && iPLMarketDepth == iPrevMarketDepth && iPrevMinWin == iPLMinWin)) { posa[4] = top.cPLMarketView; va[4] = iPLMarketView; ui_changeMarketViewUI(iPLMarketView, false, true); ui_doReOrderEntrants() } else { changeBookPercentage(bPLShowBook == '1'); changeTotalMatched(bPLShowTotalMatched == '1') } ui_closePLSettings()
};

var sSortByEntrant = "Entrant"; var sSortByStake = "Stake";
var sSortByOdds = "Odds"; var sSortUnMatchedBackBetsBy = sSortByEntrant;
var bSortUnMatchedBackBetsDesc = false; var sSortUnMatchedLayBetsBy = sSortByEntrant;
var bSortUnMatchedLayBetsDesc = false; var sSortMatchedBackBetsBy = sSortByEntrant;
var bSortMatchedBackBetsDesc = false; var sSortMatchedLayBetsBy = sSortByEntrant;
var bSortMatchedLayBetsDesc = false; var htTempBetsForSorting;

function ui_sortBets(betState, betSide, sortBy) {
    if (IsProcessing()) return; var sortDesc = false; var htBetsToReRender; switch (betState) { case betState_Matched: if (betSide == betSide_Back) { if (sSortMatchedBackBetsBy == sortBy) { bSortMatchedBackBetsDesc = !bSortMatchedBackBetsDesc; sortDesc = bSortMatchedBackBetsDesc } else { sSortMatchedBackBetsBy = sortBy; bSortMatchedBackBetsDesc = false } htBetsToReRender = htMyBetsBackMatched } else { if (sSortMatchedLayBetsBy == sortBy) { bSortMatchedLayBetsDesc = !bSortMatchedLayBetsDesc; sortDesc = bSortMatchedLayBetsDesc } else { sSortMatchedLayBetsBy = sortBy; bSortMatchedLayBetsDesc = false } htBetsToReRender = htMyBetsLayMatched } break; case betState_UnMatched: if (betSide == betSide_Back) { if (sSortUnMatchedBackBetsBy == sortBy) { bSortUnMatchedBackBetsDesc = !bSortUnMatchedBackBetsDesc; sortDesc = bSortUnMatchedBackBetsDesc } else { sSortUnMatchedBackBetsBy = sortBy; bSortUnMatchedBackBetsDesc = false } htBetsToReRender = htMyBetsBackUnMatched } else { if (sSortUnMatchedLayBetsBy == sortBy) { bSortUnMatchedLayBetsDesc = !bSortUnMatchedLayBetsDesc; sortDesc = bSortUnMatchedLayBetsDesc } else { sSortUnMatchedLayBetsBy = sortBy; bSortUnMatchedLayBetsDesc = false } htBetsToReRender = htMyBetsLayUnMatched } break } client_renderMyBetsSection(betState, betSide, htBetsToReRender, sortBy, sortDesc)
};

function sortBetsIds(htBets, sortBy, reverse) {
    var arrIds = new Array(); for (var id in htBets) { arrIds.push(id) } htTempBetsForSorting = htBets; switch (sortBy) { case sSortByEntrant: arrIds = arrIds.sort(sortFn_BetEntrant); break; case sSortByOdds: arrIds = arrIds.sort(sortFn_BetOdds); break; case sSortByStake: arrIds = arrIds.sort(sortFn_BetStake); break } htTempBetsForSorting = null; if (reverse) { return arrIds.reverse() } return arrIds
};

function sortFn_BetEntrant(id1, id2) {
    var etId1 = htTempBetsForSorting[id1][iBetEntrantID]; var etId2 = htTempBetsForSorting[id2][iBetEntrantID]; var et1 = htEntrants[etId1][iEtDesc]; var et2 = htEntrants[etId2][iEtDesc]; return (et1 > et2 ? 1 : et1 < et2 ? -1 : 0)
};

function sortFn_BetStake(id1, id2) {
    var stake1 = htTempBetsForSorting[id1][iBetStake]; var stake2 = htTempBetsForSorting[id2][iBetStake]; return (stake1 > stake2 ? 1 : stake1 < stake2 ? -1 : 0)
};

function sortFn_BetOdds(id1, id2) {
    var odds1 = htTempBetsForSorting[id1][iBetOdds]; var odds2 = htTempBetsForSorting[id2][iBetOdds]; return (odds1 > odds2 ? 1 : odds1 < odds2 ? -1 : 0)
};

function hideAllBPTabs() {
    if (document.getElementById("tdDetails")) { document.getElementById("tdDetails").className = "bpOffTab"; document.getElementById("tblDetails").style.display = "none" } document.getElementById("spPlaceBets").style.display = "none"; document.getElementById("spMyBets").style.display = "none"; document.getElementById("tdPlaceBets").className = "bpOffTab"; document.getElementById("tdMyBets").className = "bpOffTab"; ui_closeMarketSnapshotSettings()
};

function ui_doRefreshMarket(bExtendSession) {
    if (bExtendSession == null) { bExtendSession = false } window.clearTimeout(timeoutRefreshOdds); if (!IsProcessing()) { var sURL = "OddsDataJS" + sExt + "?marketID=" + sMarketID + "&ts=" + iTimestamp + "&ExtendSession=" + bExtendSession; var evalResponse = true; GetLatestMarketData(sURL, frames["ifraMarketRequest"], evalResponse); client_renderTimeToStart() } timeoutRefreshOdds = window.setTimeout("ui_doRefreshMarket()", iRefreshOddsInterval)
};

function ui_togglePL() {
    if (IsProcessing()) return; if (top.bLoggedIn) { top.userSetSetting(top.cHideProfitLoss, !(top.userGetSetting(top.cHideProfitLoss))); setProfitLossUI() } else { forceLogin() }
};

function ui_doReOrderEntrants() {
    if (IsProcessing()) return; var entrantOrder = document.getElementById("selEntrantOrder").value; if (entrantOrder == "") return; if (NewGenEntrantMethod) { if (iPLMarketView == top.MarketView_SportsbookView) { client_renderEntrantDataWithOddsForSportsbookView(sMarketID, true) } else { client_renderEntrantDataWithOdds1(sMarketID, true) } } else { client_renderEntrantDataWithOdds(sMarketID) }
};
var betPanelTabID = "";

function ui_doClickBPTab(tabID) {
    if ((tabID == "Details") && websiteTabs) { tabID = "PlaceBets" } if (tabID == "MyBets" && !top.bLoggedIn) { forceLogin(); if (websiteTabs) return false } else if (tabID == "MyBets" && top.bLoggedIn) { hideAllBPTabs(); document.getElementById("spMyBets").style.display = ""; document.getElementById("tdMyBets").className = "bpOnTab"; if (htMyBetsBackMatched == null || htMyBetsBackUnMatched == null) { window.clearTimeout(timeoutMyBets); server_getBetData(); timeoutMyBets = window.setTimeout("ui_refreshMyBets()", mybetsTimeout) } } else if (tabID == "PlaceBets") { hideAllBPTabs(); document.getElementById("spPlaceBets").style.display = ""; document.getElementById("tdPlaceBets").className = "bpOnTab"; bSetDutchTool(); ui_doBookPercentage(top.userGetSetting(top.cShowBookPercentage)) } else { hideAllBPTabs(); document.getElementById("tblDetails").style.display = ""; document.getElementById("tdDetails").className = "bpOnTab" } betPanelTabID = tabID; if (top.syncBetPanelTab) { top.syncBetPanelTab() }
};

function client_renderBetSettings() {
    document.getElementById("chkShowBetConfirm").checked = top.userGetSetting(top.cShowBetConfirm); document.getElementById("chkShowBetResult").checked = top.userGetSetting(top.cShowBetResult); document.getElementById("chkShowOddsWarning").checked = top.userGetSetting(top.cShowOddsWarning); document.getElementById("chkShowBookPercentage").checked = top.userGetSetting(top.cShowBookPercentage); ui_doBookPercentage(top.userGetSetting(top.cShowBookPercentage)); var iStakeFillTool = top.userGetSettingInt(top.cShowStakeFillTool); if (iStakeFillTool == PerBet_Show || iStakeFillTool == DutchProfit_Show || iStakeFillTool == DutchLiability_Show) { document.getElementById("chkShowStakeFillTool").checked = true; ui_setShowStakeFillTool(true) } else { document.getElementById("chkShowStakeFillTool").checked = false; ui_setShowStakeFillTool(false) } bShowBetSettings = top.userGetSetting(top.cShowBetSettings); bShowPLSettings = top.userGetSetting(top.cShowProfitLossSettings); ui_openBetSettings(bShowBetSettings); ui_openProfitLossSettings(bShowPLSettings); var PLSettingLinkObj = document.getElementById("taPLSetting"); document.getElementById("dvPLSettingsHolder").style.top = findPosY(PLSettingLinkObj) + 18; document.getElementById("dvPLSettingsHolder").style.left = findPosX(PLSettingLinkObj)
};

function client_renderPLSettings() {
    iPLMarketView = top.userGetSettingInt(top.cPLMarketView); if (document.getElementById("tbProfitLoss").className == "") { document.getElementById("tbDisplay").className = "tbOnHeader"; document.getElementById("tbProfitLoss").className = "tbOffHeader" } setCheckedValue(document.getElementsByName("rdPLMarketView"), iPLMarketView); setCheckedValue(document.getElementsByName("rdMarketView"), iPLMarketView); if (iPLMarketView == top.MarketView_SportsbookView) { iPLMarketDepth = top.userGetSettingInt(top.cPLSportsbookMarketDepth); bPLShowTotalMatched = top.userGetSetting(top.cPLSportsbookShowTotalMatched); bPLShowBook = top.userGetSetting(top.cPLSportsbookShowBook) } else { iPLMarketDepth = top.userGetSettingInt(top.cPLExchangeMarketDepth); bPLShowTotalMatched = top.userGetSetting(top.cPLExchangeShowTotalMatched); bPLShowBook = top.userGetSetting(top.cPLExchangeShowBook) }
};

function client_renderMemberDetails() {
    document.getElementById("spLoyaltyDiscount").innerHTML = top.iDiscount + "%"
};
var minOdds = 1.01; var maxOdds = 10000; var arrOddsLadder;

function ResetScrollBar() { document.getElementById("dvEntrantsTable").scrollTop = 0; bMarketIsChanged = false };
function initLadder() { arrOddsLadder = sLadder.split(';') };
function getNextOdds(iFrom) {
    if (oddsFormat == "Percentage") {
        if (isNaN(iFrom) || iFrom.length == 0) { iFrom = Math.abs(validDecimalOdds[0]) } for (var i = 0; i < validDecimalOdds.length; i++) { if (Math.abs(validDecimalOdds[i]) == iFrom) { if (i < validDecimalOdds.length - 1) { return Math.abs(validDecimalOdds[i + 1]) } else { return Math.abs(validDecimalOdds[i]) } } }
    } else { if (isNaN(iFrom) || iFrom.length == 0) { iFrom = validDecimalOdds[0] } for (var i = 0; i < validDecimalOdds.length; i++) { if (validDecimalOdds[i] == iFrom) { if (i < validDecimalOdds.length - 1) { return validDecimalOdds[i + 1] } else { return validDecimalOdds[i] } } } }
};

function getPrevOdds(iFrom) {
    if (oddsFormat == "Percentage") { if (isNaN(iFrom) || iFrom.length == 0) { iFrom = Math.abs(validDecimalOdds[0]) } for (var i = validDecimalOdds.length - 1; i >= 0; i--) { if ((Math.abs(validDecimalOdds[i]) == iFrom) && (i >= 0)) { return (i == 0) ? Math.abs(validDecimalOdds[0]) : Math.abs(validDecimalOdds[i - 1]) } } } else { if (isNaN(iFrom) || iFrom.length == 0) { iFrom = validDecimalOdds[0] } for (var i = validDecimalOdds.length - 1; i >= 0; i--) { if ((validDecimalOdds[i] == iFrom) && (i >= 0)) { return (i == 0) ? validDecimalOdds[0] : validDecimalOdds[i - 1] } } }
};

function getNextXOdds(iFrom, x) {
    var iReturn = iFrom; var i; for (i = 0; i < x; i++) { iReturn = getNextOdds(iReturn); if (iReturn != oddsLimitCheck(iReturn)) { return oddsLimitCheck(iReturn) } } return iReturn
};

function getPrevXOdds(iFrom, x) {
    var iReturn = iFrom; var i; for (i = 0; i < x; i++) { iReturn = getPrevOdds(iReturn); if (iReturn != oddsLimitCheck(iReturn)) { return oddsLimitCheck(iReturn) } } return iReturn
};

function getLastOddsForStep(iPosition) {
    var iUpperStep = getStepByPosition(iPosition); var iLowerStep = 1; if (iPosition - 1 >= 0) { iLowerStep = getStepByPosition(iPosition - 1) } var iOddsIncrement = getIncrementByPosition(iPosition); var iNoOfIncrements = Math.ceil((iUpperStep - iLowerStep) / iOddsIncrement); return round(iLowerStep + (iNoOfIncrements - 1) * iOddsIncrement, 5)
};

function getStepByPosition(position) {
    var arrPair = arrOddsLadder[position].split(":"); return round(arrPair[0], 5)
};

function getIncrementByPosition(position) {
    var arrPair = arrOddsLadder[position].split(":"); return round(arrPair[1], 5)
};

function round(num, noOfdec) {
    return Math.round(num * Math.pow(10, noOfdec)) / Math.pow(10, noOfdec)
};

function getLadder(startFromValue, count) {
    var arrReturnOdds = new Array(); var i = 0; for (i = 0; i < count; i++) { if (i == 0) { arrReturnOdds[0] = round(startFromValue, 5) } else { var odds = round(getNextOdds(arrReturnOdds[i - 1]), 5); if (odds != oddsLimitCheck(odds)) { count = count - 1; i = i - 1 } else { arrReturnOdds[i] = odds } } } return arrReturnOdds
};

function ui_OddsUp(ctlID) {
    if (IsProcessing()) return; if (currentControlId != ctlID) { hideLadder() } if (document.getElementById("dvLadderHolder").style.display == "none") { showLadder(ctlID, true) } else { ladderVisible = true; $('#aOddsIncSmall').trigger('click') }
};

function ui_OddsDown(ctlID) {
    if (IsProcessing()) return; if (currentControlId != ctlID) { hideLadder() } if (document.getElementById("dvLadderHolder").style.display == "none") { showLadder(ctlID, false) } else { ladderVisible = true; $('#aOddsDecSmall').trigger('click') }
};

function renderLadder(arrCurrentLadder) {
    var ladderHTML = document.getElementById("taLadderTemplate").value; var sLadderValueTemplate = document.getElementById("taLadderValueTemplate").value; var iNextStartingOdds = arrCurrentLadder[5]; var iPrevStartingOdds = oddsLimitCheck(arrCurrentLadder[3]); var re = new RegExp("@IncSmall", "g"); ladderHTML = ladderHTML.replace(re, iNextStartingOdds); re = new RegExp("@DecSmall", "g"); ladderHTML = ladderHTML.replace(re, iPrevStartingOdds); iNextStartingOdds = oddsLimitCheck(getNextXOdds(arrCurrentLadder[4], 5), 3); iPrevStartingOdds = oddsLimitCheck(getPrevXOdds(arrCurrentLadder[4], 5), 3); re = new RegExp("@IncBig", "g"); ladderHTML = ladderHTML.replace(re, iNextStartingOdds); re = new RegExp("@DecBig", "g"); ladderHTML = ladderHTML.replace(re, iPrevStartingOdds); var sValuesHTML = ""; for (i = arrCurrentLadder.length - 1; i >= 0; i--) { if (arrCurrentLadder[i] > 0) { var re = new RegExp("@value", "g"); var sValue = sLadderValueTemplate.replace(re, ConvertOdds(arrCurrentLadder[i])); if (i != 4) { sValue = sValue.replace("LadderSelected", "") } sValuesHTML += sValue } } re = new RegExp("@values", "g"); ladderHTML = ladderHTML.replace(re, sValuesHTML); document.getElementById("dvLadderHolder").innerHTML = ladderHTML; document.getElementById("ifraLadderShim").style.height = document.getElementById("dvLadderHolder").offsetHeight + "px"; $(".OddsLadderMovingButton").click(
    function (e) { ui_setLadder($(this).attr('val')); if (!e) e = window.event; if (e.cancelBubble) e.cancelBubble = true; else e.stopPropagation() })
};
var oddsPerLadder = 9;
function ui_setLadder(startValue) {
    if (IsProcessing()) return; if (!startValue) { return false } var arrNewOddsLadder = new Array(oddsPerLadder); for (i = 0; i < arrNewOddsLadder.length; i++) { arrNewOddsLadder[i] = 0 } startValue = round(oddsLimitCheck(startValue), 5); document.getElementById(currentControlId).value = ConvertOdds(startValue); if (document.getElementById(currentControlId)) { document.getElementById(currentControlId).onkeyup() } arrNewOddsLadder[4] = startValue; for (j = 3; j >= 0; j--) { var newOdds = oddsLimitCheck(getPrevOdds(arrNewOddsLadder[j + 1])); if (newOdds != arrNewOddsLadder[j + 1] && !isNaN(newOdds)) { arrNewOddsLadder[j] = round(newOdds, 5) } else { break } } for (k = 5; k < 9; k++) { var newOdds = oddsLimitCheck(getNextOdds(arrNewOddsLadder[k - 1])); if (newOdds != arrNewOddsLadder[k - 1] && !isNaN(newOdds)) { arrNewOddsLadder[k] = round(newOdds, 5) } else { break } } renderLadder(arrNewOddsLadder); return false
};
document.onclick = hideLadder;

function hideLadder() {
    if (!ladderVisible) {
        if (document.getElementById("dvLadderHolder")) {
            document.getElementById("dvLadderHolder").style.display = "none";
            document.getElementById("ifraLadderShim").style.display = "none"
        }
    } else { ladderVisible = false }
};

function ui_selectOdds(value) {
    if (IsProcessing()) return;
    if (document.getElementById(currentControlId)) {
        document.getElementById(currentControlId).value = value;
        document.getElementById(currentControlId).focus();
        document.getElementById(currentControlId).onkeyup()
    } hideLadder()
};

function getNearestOdds(iFrom, bRoundUp) {
    if (iFrom != oddsLimitCheck(iFrom)) { return oddsLimitCheck(iFrom) }
    for (i = 0; i < arrOddsLadder.length; i++) {
        var iUpperStep = getStepByPosition(i);
        var iLowerStep = 1; if (i - 1 >= 0) iLowerStep = getStepByPosition(i - 1); if (iFrom <= iUpperStep && iFrom >= iLowerStep) {
            var iOddsIncrement = getIncrementByPosition(i); var iNoOfIncrements = Math.round((iFrom - iLowerStep) / iOddsIncrement);
            if (bRoundUp) {
                var ladderValue = round(iLowerStep + iNoOfIncrements * iOddsIncrement, 5); if (ladderValue < iFrom) {
                    return round(iLowerStep + (iNoOfIncrements + 1) * iOddsIncrement, 5)
                } else { return ladderValue }
            } else {
                var ladderValue = round(iLowerStep + iNoOfIncrements * iOddsIncrement, 5);
                if (round(iLowerStep + iNoOfIncrements * iOddsIncrement, 5) > iFrom) {
                    return round(iLowerStep + (iNoOfIncrements - 1) * iOddsIncrement, 5)
                } else {
                    return ladderValue
                }
            }
        }
    }
};

function oddsLimitCheck(value) {
    if (oddsFormat == "Percentage") {
        if (value > Math.abs(validDecimalOdds[0])) {
            return Math.abs(validDecimalOdds[0])
        }
        if (value < Math.abs(validDecimalOdds[validDecimalOdds.length - 1])) {
            return Math.abs(validDecimalOdds[validDecimalOdds.length - 1])
        }
    } else {
        if (value < validDecimalOdds[0]) {
            return validDecimalOdds[0]
        } if (value > validDecimalOdds[validDecimalOdds.length - 1]) {
            return validDecimalOdds[validDecimalOdds.length - 1]
        }
    } return value
};
var currentControlId = ""; var ladderVisible = false;

function showLadder(id, bGoUp) {
    ladderVisible = true; currentControlId = id; var inCtrlOddsValue = document.getElementById(id).value;
    if (inCtrlOddsValue != "") {
        var firstChar = inCtrlOddsValue.charAt(0); if (firstChar == '<' || firstChar == '>') {
            inCtrlOddsValue = inCtrlOddsValue.substring(1)
        }
    } var currentOdds = ConvertOddsToDec(inCtrlOddsValue, "Back");
    if (currentOdds == "" || !currentOdds) { currentOdds = getNextOdds(1) } else {
        if (bGoUp) { currentOdds = getNextOdds(currentOdds) } else {
            currentOdds = getPrevOdds(currentOdds)
        }
    } currentOdds = oddsLimitCheck(currentOdds); var start = currentOdds; ui_setLadder(start);
    var xPos = 0; var yPos = 0; var tdTag = document.getElementById("img" + id);
    do { tdTag = tdTag.offsetParent; xPos += tdTag.offsetLeft; yPos += tdTag.offsetTop }
    while (tdTag.tagName != "BODY"); yPos = yPos - 68; var dvHolder = document.getElementById("dvLadderHolder");
    dvHolder.style.display = ""; dvHolder.style.top = yPos; dvHolder.style.left = xPos - 59;
    var dvShim = document.getElementById("ifraLadderShim"); dvShim.style.display = ""; dvShim.style.top = dvHolder.style.top;
    dvShim.style.left = dvHolder.style.left; dvShim.style.height = dvHolder.offsetHeight + "px"
};
var Current = null;

function ui_OnMouseOverEntrantTip(event, obj) {
    var dvObj = document.getElementById('dvEntrantTip'); Current = obj;
    var iLength = 15; if (dvObj != null && dvObj.innerHTML != "") {
        if (navigator.userAgent.indexOf("Firefox") > -1) {
            var x = event.pageX - parseInt(dvObj.style.width.replace("px", "")) / 2; if (x < 0) x = 0; x = x + document.body.scrollLeft;
            var y = event.pageY + document.body.scrollTop + iLength;
            var dDeadline = document.body.offsetHeight - document.body.scrollTop;
            if (document.getElementById("trOddsHeader") != null)
                dDeadline = document.getElementById("trOddsHeader").offsetTop;
            var divHeight = 12.5 * (1 + StringCount(dvObj.innerHTML.toLowerCase(), "<br>", ""));
            var objHeight = 0; if (obj != null) objHeight = obj.offsetHeight;
            if ((findPos(obj) + objHeight + divHeight + iLength - document.getElementById("dvEntrantsTable").scrollTop) > dDeadline) {
                y = event.pageY - document.body.scrollTop - iLength - divHeight
            } document.getElementById("dvEntrantTip").style.top = y + "px";
            document.getElementById("dvEntrantTip").style.left = x + "px";
            document.getElementById("dvEntrantTip").style.display = ""
        } else {
            var x = event.clientX - parseInt(dvObj.style.width.replace("px", "")) / 2; if (x < 0) x = 0;
            x = x + document.body.scrollLeft; var y = event.clientY + document.body.scrollTop + iLength;
            var dDeadline = document.body.offsetHeight - document.body.scrollTop; if (document.getElementById("selEntrantOrder") != null)
                dDeadline = document.getElementById("selEntrantOrder").offsetTop; var divHeight = 12.5 * (1 + StringCount(dvObj.innerHTML.toLowerCase(), "<br>", ""));
            var objHeight = 0; if (obj != null) objHeight = obj.offsetHeight;
            if (findPos(obj) + objHeight + divHeight + iLength - document.getElementById("dvEntrantsTable").scrollTop > dDeadline) {
                y = event.clientY - document.body.scrollTop - iLength - divHeight
            } document.getElementById("dvEntrantTip").style.top = y + "px";
            document.getElementById("dvEntrantTip").style.left = x + "px";
            document.getElementById("dvEntrantTip").style.display = "";
        }
    }
};

function StringCount(ObjValue, strTarget, strSubString) {
    var strText = ObjValue; var intIndexOfMatch = strText.indexOf(strTarget);
    var iCount = 0; while (intIndexOfMatch != -1) {
        strText = strText.replace(strTarget, strSubString); intIndexOfMatch = strText.indexOf(strTarget);
        iCount++
    } return iCount
};

function ui_ShowEntrantTip(obj, msg, event) {
    var bIsShow = 0; if (top.userGetSetting(top.cPLIncUnPopupWin)) bIsShow = 1; var iLength = 15;
    if (msg != "" && bIsShow == 1) {
        document.getElementById('dvEntrantTip').innerHTML = msg;
        var dvObj = document.getElementById('dvEntrantTip'); if (navigator.userAgent.indexOf("Firefox") > -1) {
            var x = event.pageX - parseInt(dvObj.style.width.replace("px", "")) / 2; if (x < 0) { x = 0 } x = x + document.body.scrollLeft;
            var y = event.pageY + document.body.scrollTop + iLength; var dDeadline = document.body.offsetHeight - document.body.scrollTop;
            if (document.getElementById("trOddsHeader") != null) { dDeadline = document.getElementById("trOddsHeader").offsetTop }
            var divHeight = 12.5 * (1 + StringCount(dvObj.innerHTML.toLowerCase(), "<br>", "")); var objHeight = 0; if (obj != null)
                objHeight = obj.offsetHeight; if ((findPos(obj) + objHeight + divHeight + iLength - document.getElementById("dvEntrantsTable").scrollTop) > dDeadline) {
                y = event.pageY - document.body.scrollTop - iLength - divHeight
            }
            document.getElementById("dvEntrantTip").style.top = y + "px";
            document.getElementById("dvEntrantTip").style.left = x + "px";
            document.getElementById("dvEntrantTip").style.display = ""
        } else {
            var x = event.clientX - parseInt(dvObj.style.width.replace("px", "")) / 2;
            if (x < 0) { x = 0 } x = x + document.body.scrollLeft; var y = event.clientY + document.body.scrollTop + iLength;
            var dDeadline = document.body.offsetHeight - document.body.scrollTop;
            if (document.getElementById("selEntrantOrder") != null) { dDeadline = findPos(document.getElementById("selEntrantOrder")); }
            var divHeight = 12.5 * (1 + StringCount(dvObj.innerHTML.toLowerCase(), "<br>", ""));
            var objHeight = 0; if (obj != null) { objHeight = obj.offsetHeight } if ((findPos(obj) + objHeight + divHeight + iLength - document.getElementById("dvEntrantsTable").scrollTop) > dDeadline) {
                y = event.clientY - document.body.scrollTop - iLength - divHeight
            }
            document.getElementById("dvEntrantTip").style.top = y + "px"; document.getElementById("dvEntrantTip").style.left = x + "px";
            document.getElementById("dvEntrantTip").style.display = ""
        }
    } else {
        document.getElementById('dvEntrantTip').innerHTML = ""; ui_EntrantHideTip()
    }
};

function ui_EntrantHideTip(command) {
    if (command == null) {
        document.getElementById("dvEntrantTip").style.display = "none"
    } else if (command == 'Body') {
        if (Current == null && document.getElementById("dvEntrantTip") != null) {
            document.getElementById("dvEntrantTip").style.display = "none"
        }
    }
};

function ui_EntrantHideIcon(entrantID, objImg) {
    objImg.style.display = "none"; htEntrants[entrantID][iEtIconError] = true;
    window.setTimeout("client_renderWindowResize()", timeoutResize)
};

function ui_setShowStakeFillTool(show) {
    var array_radio = document.getElementsByName("rdStakeFillTool"); var iChoice = 1;
    var bIsBoolean = isBoolean(show); if (bIsBoolean) {
        array_radio[0].disabled = !show; array_radio[1].disabled = !show; array_radio[2].disabled = !show;
        var iChoice = top.userGetSettingInt(top.cShowStakeFillTool); iShowStakeFillTool = iChoice; if (show) {
            if (iChoice == PerBet_Hide) iShowStakeFillTool = PerBet_Show; if (iChoice == DutchProfit_Hide) iShowStakeFillTool = DutchProfit_Show;
            if (iChoice == DutchLiability_Hide) iShowStakeFillTool = DutchLiability_Show
        } else {
            if (iChoice == PerBet_Show) iShowStakeFillTool = PerBet_Hide; if (iChoice == DutchProfit_Show) iShowStakeFillTool = DutchProfit_Hide;
            if (iChoice == DutchLiability_Show) iShowStakeFillTool = DutchLiability_Hide
        }
        if (iShowStakeFillTool == PerBet_Show || iShowStakeFillTool == PerBet_Hide) {
            array_radio[0].checked = true; array_radio[1].checked = false; array_radio[2].checked = false
        }
        if (iShowStakeFillTool == DutchProfit_Show || iShowStakeFillTool == DutchProfit_Hide) {
            array_radio[0].checked = false;
            array_radio[1].checked = true; array_radio[2].checked = false
        }
        if (iShowStakeFillTool == DutchLiability_Show || iShowStakeFillTool == DutchLiability_Hide) {
            array_radio[0].checked = false;
            array_radio[1].checked = false; array_radio[2].checked = true
        }
    } else { iShowStakeFillTool = show }
    top.userSetSettingInt(top.cShowStakeFillTool, iShowStakeFillTool); bSetDutchTool()
};

function ui_doBookPercentage(bIsEnabled) {
    if (document.getElementById("lbBookPercentage_" + betSide_Back) != null) {
        document.getElementById("lbBookPercentage_" + betSide_Back).innerHTML = (bIsEnabled ? sBook : "&nbsp;");
        bIsEnabled ? updateBookPercentage(betSide_Back) : document.getElementById("spBookPercentage_" + betSide_Back).innerHTML = "&nbsp;"
    }
    if (document.getElementById("lbBookPercentage_" + betSide_Lay) != null) {
        document.getElementById("lbBookPercentage_" + betSide_Lay).innerHTML = (bIsEnabled ? sBook : "&nbsp;");
        bIsEnabled ? updateBookPercentage(betSide_Lay) : document.getElementById("spBookPercentage_" + betSide_Lay).innerHTML = "&nbsp;"
    } var iChoice = top.userGetSettingInt(top.cShowStakeFillTool); var IsShowStakeFillTool = false;
    if (iChoice == PerBet_Show || iChoice == DutchProfit_Show || iChoice == DutchLiability_Show) { IsShowStakeFillTool = true }
    if (document.getElementById("trBookPercentage_" + betSide_Back) != null) {
        document.getElementById("trBookPercentage_" + betSide_Back).style.display = (!bIsEnabled && !IsShowStakeFillTool ? "none" : "")
    }
    if (document.getElementById("trBookPercentage_" + betSide_Lay) != null) {
        document.getElementById("trBookPercentage_" + betSide_Lay).style.display = (!bIsEnabled && !IsShowStakeFillTool ? "none" : "")
    }
};

function ui_doCalculateStake(betSide) {
    iShowStakeFillTool = top.userGetSettingInt(top.cShowStakeFillTool);
    var TargetPrice = document.getElementById("txtValue_" + betSide).value; var msg = "";
    var htPlaceBets; var StakeHeader = ""; if (betSide == betSide_Back) {
        htPlaceBets = htPlaceBetsBack;
        StakeHeader = "inStake_" + betState_UnPlaced + "_Back_"; msg = msgProfitRequired
    } else if (betSide == betSide_Lay) {
        htPlaceBets = htPlaceBetsLay; StakeHeader = "inStake_" + betState_UnPlaced + "_Lay_"; msg = msgLiabilityRequired
    }
    if (TargetPrice.length <= 0 || isNaN(TargetPrice)) { top.showMessageBox(msg, null, null); return }
    if (!bIsAllOddsFilled(htPlaceBets)) { top.showMessageBox(msgAllOddsRequired, null, null); return }
    if (TargetPrice <= 0) {
        var iChoice = top.userGetSettingInt(top.cShowStakeFillTool);
        if (iChoice == DutchLiability_Show || iChoice == DutchLiability_Hide) {
            top.showMessageBox(msgTotalStakeNumeric, null, null)
        } else {
            top.showMessageBox(msgTotalProfitNumeric, null, null)
        } return
    } if (iShowStakeFillTool == PerBet_Show) {
        for (var entrantID in htPlaceBets) {
            var Odds = htPlaceBets[entrantID][iBetOdds].toString(); if (Odds != "") {
                document.getElementById(StakeHeader + entrantID).value = cutDecimalPlace(calStakePerBet(Odds, TargetPrice, betSide), 2);
                document.getElementById(StakeHeader + entrantID).onblur()
            }
        } return
    } if (iShowStakeFillTool == DutchProfit_Show || iShowStakeFillTool == DutchLiability_Show) {
        var type = "Profit"; if (iShowStakeFillTool == 5) type = "Liability"; var CValue = 0;
        CValue = calStakeDutchCValue(htPlaceBets); if (type == "Profit") {
            if (calBookPercentage(betSide) > 100 && betSide == betSide_Back) {
                top.showMessageBox(msgBookPercentageBelow100, null, null)
            }
            else if (calBookPercentage(betSide) < 100 && betSide == betSide_Lay) {
                top.showMessageBox(msgBookPercentageOver100, null, null)
            } else {
                for (var entrantID in htPlaceBets) {
                    var Odds = htPlaceBets[entrantID][iBetOdds].toString(); if (Odds != "") {
                        document.getElementById(StakeHeader + entrantID).value = cutDemicalPlace(calDutchProfit(Odds, CValue, TargetPrice, betSide), 2);
                        document.getElementById(StakeHeader + entrantID).onblur()
                    }
                }
            }
        } else {
            if (calBookPercentage(betSide) > 100 && betSide == betSide_Back) {
                top.showMessageBox(msgBookPercentageBelow100, null, null)
            } else if (calBookPercentage(betSide) > 100 && betSide == betSide_Lay) {
                top.showMessageBox(msgBookPercentageBelow100, null, null)
            } else {
                for (var entrantID in htPlaceBets) {
                    var Odds = parseFloat(ConvertOdds(htPlaceBets[entrantID][iBetOdds])); if (Odds != "") {
                        document.getElementById(StakeHeader + entrantID).value = cutDemicalPlace(calDutchLiability(Odds, CValue, TargetPrice, betSide), 2);
                        document.getElementById(StakeHeader + entrantID).onblur()
                    }
                }
            }
        }
    }
};

function bIsAllOddsFilled(htObjs) {
    var bIsValid = true; for (var entrantID in htObjs) {
        var Odds = htObjs[entrantID][iBetOdds].toString(); if (Odds == "") { bIsValid = false; break }
    }
    return bIsValid
};

function calStakePerBet(Odds, TargetPrice, betSide) {
    if (betSide == betSide_Back) { return (TargetPrice / (Odds - 1)) } else { return TargetPrice }
};

function calStakeDutch(Odds, CValue, TargetPrice) {
    var DValue = (100 / CValue) - 1; return ((100 / Odds) / CValue) * (TargetPrice / DValue)
};

function calDutchLiability(Odds, CValue, TargetPrice, betSide) {
    if (betSide == betSide_Back) {
        return TargetPrice * (100 / Odds) / CValue
    } else {
        var DValue = (100 / CValue) - 1; return ((100 / Odds) / CValue) * (TargetPrice / DValue)
    }
};

function calDutchProfit(Odds, CValue, TargetPrice, betSide) {
    if (betSide == betSide_Back) {
        var DValue = (100 / CValue) - 1;
        return ((100 / Odds) / CValue) * (TargetPrice / DValue)
    } else {
        return ((100 / Odds)) * (TargetPrice / (CValue - 100))
    }
};

function calStakeDutchCValue(htObjs) {
    var CValue = 0; for (var entrantID in htObjs) {
        var Odds = htObjs[entrantID][iBetOdds].toString(); if (Odds != "") { CValue += 100 / Odds }
    } return CValue
};

function updateBookPercentage(betSide) {
    var BookPercentage = 0;
    var bIsBookPercentageShow = top.userGetSetting(top.cShowBookPercentage);
    var spBookPercentage = document.getElementById("spBookPercentage_" + betSide); if (bIsBookPercentageShow) {
        if (spBookPercentage != null) { spBookPercentage.innerHTML = calBookPercentage(betSide) + '%' }
    }
};

function calBookPercentage(betSide) {
    var BookPercentage = 0; var htPlaceBets; var StakeHeader = "";
    if (betSide == betSide_Back) {
        htPlaceBets = htPlaceBetsBack;
        StakeHeader = "inOdds_" + betState_UnPlaced + "_Back_"
    } else if (betSide == betSide_Lay) {
        htPlaceBets = htPlaceBetsLay; StakeHeader = "inOdds_" + betState_UnPlaced + "_Lay_"
    }
    for (var entrantID in htPlaceBets) {
        var Odds = htPlaceBets[entrantID][iBetOdds].toString();
        if (Odds != null && Odds != "") {
            BookPercentage += 100 / ConvertOddsToDec(document.getElementById(StakeHeader + entrantID).value, betSide)
        }
    }
    return cutDecimalPlace(BookPercentage, 2)
};

function bSetDutchTool() {
    try {
        bUseDutchTool = true; var array_radio = document.getElementsByName("rdStakeFillTool");
        var bIsShowFillTool = document.getElementById("chkShowStakeFillTool").checked; if (sBetType != "Odds") bUseDutchTool = false;
        if (iNoOfWinners > 1) bUseDutchTool = false; if (arrEntrantOrderByName != null && arrEntrantOrderByName.length <= 2) bUseDutchTool = false;
        var ProfitValue = document.getElementById("txtValue_" + betSide_Back); var ProfitBtn = document.getElementById("btnFill_" + betSide_Back);
        var LiabilityValue = document.getElementById("txtValue_" + betSide_Lay); var LiabilityBtn = document.getElementById("btnFill_" + betSide_Lay);
        if (bUseDutchTool) {
            if (!bIsShowFillTool) {
                array_radio[0].disabled = true;
                array_radio[1].disabled = true; array_radio[2].disabled = true; ui_SetStakeFillTool(false)
            } else {
                array_radio[0].disabled = false; array_radio[1].disabled = false;
                array_radio[2].disabled = false; ui_SetStakeFillTool(true)
            }
        } else {
            array_radio[2].disabled = true; array_radio[1].disabled = true;
            array_radio[0].disabled = !bIsShowFillTool;
            if (bIsShowFillTool) {
                array_radio[0].checked = true; top.userSetSetting(top.cShowStakeFillTool, array_radio[0].value); ui_SetStakeFillTool(true)
            } else { ui_SetStakeFillTool(false) }
        }
    } catch (e) { top.showMessageBox("bSetDutchTool:" + e.message, null, null) }
};

function ui_SetStakeFillTool(bIsEnabled) {
    var iChoice = top.userGetSettingInt(top.cShowStakeFillTool);
    var lbBetType = sTotalProfit; if (iChoice == DutchLiability_Show || iChoice == DutchLiability_Hide) {
        lbBetType = sTotalStake
    } if (document.getElementById("txtValue_" + betSide_Back) != null) {
        document.getElementById("lbBetType_" + betSide_Back).innerHTML = (bIsEnabled ? lbBetType : "&nbsp;");
        document.getElementById("txtValue_" + betSide_Back).style.visibility = (bIsEnabled ? "visible" : "hidden");
        document.getElementById("btnFill_" + betSide_Back).style.visibility = (bIsEnabled ? "visible" : "hidden")
    }
    if (document.getElementById("txtValue_" + betSide_Lay) != null) {
        document.getElementById("lbBetType_" + betSide_Lay).innerHTML = (bIsEnabled ? lbBetType : "&nbsp;");
        document.getElementById("txtValue_" + betSide_Lay).style.visibility = (bIsEnabled ? "visible" : "hidden");
        document.getElementById("btnFill_" + betSide_Lay).style.visibility = (bIsEnabled ? "visible" : "hidden")
    }
    if (document.getElementById("trBookPercentage_" + betSide_Back) != null) {
        document.getElementById("trBookPercentage_" + betSide_Back).style.display = (!bIsEnabled && !top.userGetSetting(top.cShowBookPercentage) ? "none" : "")
    }
    if (document.getElementById("trBookPercentage_" + betSide_Lay) != null) {
        document.getElementById("trBookPercentage_" + betSide_Lay).style.display = (!bIsEnabled && !top.userGetSetting(top.cShowBookPercentage) ? "none" : "")
    }
};

function cutDecimalPlace(iValue, dp) {
    var index = iValue.toString().indexOf(".");
    var bIsInt = (iValue.toString().indexOf(".00") >= 0 ? true : false); if (index < 0) {
        return iValue.toString()
    } else {
        if (iValue.toString().length < index + 1 + dp) {
            if (bIsInt) {
                return iValue.toString().substring(0, index)
            } else {
                return iValue.toString()
            }
        } else {
            if (bIsInt) {
                return iValue.toString().substring(0, index)
            } else {
                return iValue.toString().substring(0, index + 1 + dp)
            }
        }
    }
};

function ui_openProfitLossSettings(bIsOpen) {
    bShowPLSettings = (bIsOpen == null ? !bShowPLSettings : bIsOpen);
    if (bShowPLSettings && top.bLoggedIn) {
        $("#imgPLSettings").attr("src", "UI/Black/Img/collapse.gif"); $("#profitlossPanelContent").show()
    } else {
        $("#imgPLSettings").attr("src", "UI/Black/Img/expand.gif"); $("#profitlossPanelContent").hide()
    }
    top.userSetSetting(top.cShowProfitLossSettings, bShowPLSettings)
}

function ui_openBetSettings(bIsOpen) {
    bShowBetSettings = (bIsOpen == null ? !bShowBetSettings : bIsOpen);
    if (bShowBetSettings) {
        document.getElementById("imgBetSettings").src = "UI/Black/Img/collapse.gif";
        document.getElementById("trPlaceBetsOptions").style.display = ""
    } else {
        document.getElementById("imgBetSettings").src = "UI/Black/Img/expand.gif";
        document.getElementById("trPlaceBetsOptions").style.display = "none"
    }
    top.userSetSetting(top.cShowBetSettings, bShowBetSettings)
};

function ui_openSettings(tabName, imgName) {
    if (document.getElementById(tabName).style.display != "") {
        document.getElementById(imgName).src = "UI/Black/Img/minus.gif";
        document.getElementById(tabName).style.display = ""
    } else {
        document.getElementById(imgName).src = "UI/Black/Img/plus.gif";
        document.getElementById(tabName).style.display = "none"
    }
};

function changeMarketView(ddlValue, bSetPLSettingsMarketView, bSetMarketView) {
    changeMarketView(ddlValue, bSetPLSettingsMarketView, bSetMarketView)
}
function changeMarketView(ddlValue, bSetPLSettingsMarketView, bSetMarketView) {
    iPLMarketView = ddlValue; if (NewGenEntrantMethod) {
        top.userSetSetting(top.cPLMarketView, iPLMarketView);
        if (iPLMarketView == top.MarketView_SportsbookView) {
            iPLMarketDepth = top.userGetSettingInt(top.cPLSportsbookMarketDepth);
            bPLShowBook = top.userGetSetting(top.cPLSportsbookShowBook); bPLShowTotalMatched = top.userGetSetting(top.cPLSportsbookShowTotalMatched);
            client_renderEntrantDataWithOddsForSportsbookView(sMarketID, false)
        } else {
            iPLMarketDepth = top.userGetSettingInt(top.cPLExchangeMarketDepth);
            bPLShowBook = top.userGetSetting(top.cPLExchangeShowBook);
            bPLShowTotalMatched = top.userGetSetting(top.cPLExchangeShowTotalMatched); client_renderEntrantDataWithOdds1(sMarketID, false)
        }
        ui_changeMarketViewUI(iPLMarketView, bSetPLSettingsMarketView, bSetMarketView);
        changeTotalMatched(bPLShowTotalMatched)
    } else client_renderEntrantDataWithOdds(sMarketID)
};

function ui_changeMarketViewUI(ddlValue, bSetPLSettingsMarketView, bSetMarketView) {
    if (bSetPLSettingsMarketView == true) {
        setCheckedValue(document.getElementsByName("rdPLMarketView"), ddlValue)
    }
    if (bSetMarketView == true) {
        setCheckedValue(document.getElementsByName("rdMarketView"), ddlValue)
    }
    if (NewGenEntrantMethod) {
        if (ddlValue == MarketView_SportsbookView) {
            document.getElementById("chkPLShowTotalMatched").checked = top.userGetSetting(top.cPLSportsbookShowTotalMatched);
            document.getElementById("chkPLShowBook").checked = top.userGetSetting(top.cPLSportsbookShowBook);
            ui_changeMarketDepth(top.userGetSettingInt(top.cPLSportsbookMarketDepth), true)
        } else {
            document.getElementById("chkPLShowTotalMatched").checked = top.userGetSetting(top.cPLExchangeShowTotalMatched);
            document.getElementById("chkPLShowBook").checked = top.userGetSetting(top.cPLExchangeShowBook);
            ui_changeMarketDepth(top.userGetSettingInt(top.cPLExchangeMarketDepth), true)
        }
        ui_changeMinWin(top.userGetSettingInt(top.cPLMinWin))
    }
};

function ui_changeMarketDepth(ddlValue, bPLSettingMarketDepth) {
    if (bPLSettingMarketDepth == true) set_DropDownList("ddlPLMarketDepth", ddlValue);
    if (ddlValue == MarketDepth_BestPriceOnly || ddlValue == MarketDepth_BestPriceAndStake) {
        ddlPLName = "ddlPLMinWin"; set_DropDownList(ddlPLName, iPLMinWin); $('#ddlPLMinWin').parent().parent().show()
    } else {
        $('#ddlPLMinWin').parent().parent().hide()
    }
    $("#MarketSettingsDialogHelp").css('top', ($("#MarketSettingsDialog").parent().outerHeight() - 30) + 'px')
};

function ui_changeMinWin(ddlValue) {
    var ddlName = "ddlPLMinWin"; set_DropDownList(ddlName, ddlValue); iPLMinWin = ddlValue
};

function set_DropDownList(ddlName, SelectedValue) {
    var ddl = document.getElementById(ddlName);
    for (var i = 0; i < ddl.options.length; i++) {
        if (SelectedValue == ddl.options[i].value) { ddl.options[i].selected = true }
    }
};

function client_renderEntrantDataWithOddsForSportsbookView(marketID, bUpdateEntrantOrder) {
    var start; var start1; var bShowIdentifier = false;
    try {
        if (sMarketID.toLowerCase() != marketID.toLowerCase()) return;
        var orderByDefault = false; var openEntrantsHTML = "";
        var closedEntrantsHTML = ""; var openTemplate = ""; var voidTemplate = "";
        var blankTemplate = ""; if (iPLMarketDepth == MarketDepth_BestPriceOnly || iPLMarketDepth == MarketDepth_BestPriceAndStake) {
            openTemplate = document.getElementById("taEntrantWithOneSideOneOddOpenRowTemplate").value;
            voidTemplate = document.getElementById("taEntrantVoidRowOneSideOneOddTemplate").value;
            blankTemplate = document.getElementById("taEntrantBlankRowOneSideOneOddTemplate").value
        } else {
            openTemplate = document.getElementById("taEntrantWithOneSideThreeOddsOpenRowTemplate").value;
            voidTemplate = document.getElementById("taEntrantVoidRowOneSideThreeOddsTemplate").value;
            blankTemplate = document.getElementById("taEntrantBlankRowOneSideThreeOddsTemplate").value
        }
        var openEntrantHTML = new StringBuffer(); var voidEntrantHTML = new StringBuffer();
        var blankEntrantHTML = blankTemplate; var index = 0; arrEntrantIDs = new Array();
        arrEntrantIDsToIndex = new Array(); for (var entrantID in htEntrants) {
            arrEntrantIDs[index] = entrantID; arrEntrantIDsToIndex[entrantID] = index; index = index + 1
        }
        arrEntrantOrderDefault = new Array();
        for (i = 0; i < arrEntrantIDs.length; i++) { arrEntrantOrderDefault[i] = i }
        var arrEntrantOrder = arrEntrantOrderDefault; var entrantOrder = "";
        entrantOrder = document.getElementById("selEntrantOrder").value;
        switch (entrantOrder) {
            case "Selection":
                arrEntrantOrder = arrEntrantOrderByName; break; case "Identifier":
                arrEntrantOrder = arrEntrantOrderByIdentifier; htLastBestBetOrder = {}; break; case "Odds":
                if (bUpdateEntrantOrder || arrEntrantOrderByOdds.length == 0) {
                    arrBestBetObj = new Array();
                    for (i = 0; i < arrEntrantOrder.length; i++) {
                        var entrantID = arrEntrantIDs[arrEntrantOrder[i]];
                        if (htEntrants[entrantID] == null) break; var identifier = htEntrants[entrantID][iEtIdent];
                        var BestBackBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Back, htMinWin[iPLMinWin], identifier);
                        arrBestBetObj[arrBestBetObj.length] = new BestBetObj(BestBackBet[iBestEntrantIDPos], BestBackBet[iBestStakePos], BestBackBet[iBestOddPos], BestBackBet[iBestIdentifierPos])
                    }
                    arrBestBetObj.sort(sortByOdds);
                    htLastBestBetOrder = {}; arrEntrantOrderByOdds = new Array();
                    for (var i = 0; i < arrBestBetObj.length; i++) {
                        arrEntrantOrderByOdds[i] = arrEntrantIDsToIndex[arrBestBetObj[i].entrantID];
                        htLastBestBetOrder[arrBestBetObj[i].entrantID] = arrBestBetObj[i].odds
                    }
                } else {
                    var bIsChanged = false; for (i = 0; i < arrBestBetObj.length; i++) {
                        var entrantID = arrBestBetObj[i].entrantID; var identifier = htEntrants[entrantID][iEtIdent];
                        var BestBackBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Back, htMinWin[iPLMinWin], identifier);
                        arrBestBetObj[i] = new BestBetObj(BestBackBet[iBestEntrantIDPos], BestBackBet[iBestStakePos], BestBackBet[iBestOddPos], BestBackBet[iBestIdentifierPos]);
                        if (htLastBestBetOrder[entrantID] != null) {
                            if (htLastBestBetOrder[entrantID] != arrBestBetObj[i].odds) {
                                bIsChanged = true || bIsChanged
                            }
                        } else { bIsChanged = true || bIsChanged }
                    } if (bIsChanged) {
                        document.getElementById("selEntrantOrder").value = ""
                    }
                }
                arrEntrantOrder = arrEntrantOrderByOdds; break;
            case "": if (htLastBestBetOrder.length > 0) arrEntrantOrder = arrEntrantOrderByOdds; break
        }
        var allBacked = true; var allLayed = true; var bookLay = 0; var bookBack = 0;
        if (sBetType == "Handicap" || sBetType == "Asian" || sBetType == "Totals") { allBacked = false }
        iNoOfOpenEntrants = 0; var VoidTemplateArray = voidTemplate.split('@'); var OpenTemplateArray = openTemplate.split('@');
        var marketIDLowerCase = marketID.toLowerCase(); var iTime = 0; if (iPLMarketDepth == MarketDepth_BestPriceOnly) {
            OpenTemplateArray[32] = OpenTemplateArray[32].replace("Odds", "OddsOnly");
        } for (i = 0; i < arrEntrantOrder.length; i++) {
            if (htEntrants[entrantID] == null) break; var entrantID = arrEntrantIDs[arrEntrantOrder[i]];
            var entrantHTML = null; var entrantStatus = htEntrants[entrantID][iEtStatus];
            var plDisplay = htEntrants[entrantID][iEtProfitLossDisplay] == null ? 0 : htEntrants[entrantID][iEtProfitLossDisplay];

            var winDisplay = htEntrants[entrantID][iEtWinDisplay] == null ? 0 : htEntrants[entrantID][iEtWinDisplay];
            var loseDisplay = htEntrants[entrantID][iEtLoseDisplay] == null ? 0 : htEntrants[entrantID][iEtLoseDisplay];
            var arrOpenTemplate = null; if (entrantStatus != "Open") {
                arrOpenTemplate = VoidTemplateArray; var entrantIDLowerCase = entrantID.toLowerCase(); arrOpenTemplate[3] = entrantIDLowerCase;
                arrOpenTemplate[9] = entrantIDLowerCase; arrOpenTemplate[13] = entrantIDLowerCase;
                arrOpenTemplate[17] = entrantIDLowerCase; arrOpenTemplate[19] = entrantIDLowerCase;
                arrOpenTemplate[21] = entrantIDLowerCase; arrOpenTemplate[5] = marketIDLowerCase;
                arrOpenTemplate[11] = marketIDLowerCase
            } else {
                arrOpenTemplate = OpenTemplateArray;
                var entrantIDLowerCase = entrantID.toLowerCase();
                arrOpenTemplate[3] = entrantIDLowerCase; arrOpenTemplate[9] = entrantIDLowerCase; arrOpenTemplate[13] = entrantIDLowerCase;
                arrOpenTemplate[29] = entrantIDLowerCase; arrOpenTemplate[31] = entrantIDLowerCase; arrOpenTemplate[35] = entrantIDLowerCase;
                arrOpenTemplate[17] = entrantIDLowerCase; arrOpenTemplate[21] = entrantIDLowerCase;
                arrOpenTemplate[25] = entrantIDLowerCase; arrOpenTemplate[5] = marketIDLowerCase;
                arrOpenTemplate[11] = marketIDLowerCase; if (iPLMarketDepth == MarketDepth_FullDepth) {
                    arrOpenTemplate[39] = entrantIDLowerCase; arrOpenTemplate[49] = entrantIDLowerCase;
                    arrOpenTemplate[41] = entrantIDLowerCase; arrOpenTemplate[45] = entrantIDLowerCase;
                    arrOpenTemplate[51] = entrantIDLowerCase; arrOpenTemplate[55] = entrantIDLowerCase
                }
            } var ident = ""; if (sBetType == "Odds") { ident = htEntrants[entrantID][iEtIdent] }
            if (ident.length > 0) { ident = "[" + ident + "] "; bShowIdentifier = bShowIdentifier || true }
            arrOpenTemplate[15] = htEntrants[entrantID][iEtDesc]; arrOpenTemplate[7] = ident;
            if (htEntrants[entrantID][iEtIconError] == null) {
                var entrantIcon = htEntrants[entrantID][iEtIcon];
                var entrantIconHtml = ""; if (entrantIcon.length > 0) {
                    entrantIconHtml = "<img width=\"29\" height=\"21\" src=\"" + sEntrantIconsURL + entrantIcon + "\" onerror=\"ui_EntrantHideIcon('" + entrantID + "',this)\"  />"
                }
                arrOpenTemplate[1] = entrantIconHtml
            } else { arrOpenTemplate[1] = "" }
            if (entrantStatus == "Open") {
                if (htOdds[entrantID] == null) {
                    if (iPLMarketDepth == MarketDepth_FullDepth) {
                        arrOpenTemplate[53] = "&nbsp;"; arrOpenTemplate[57] = "&nbsp;";
                        arrOpenTemplate[43] = "&nbsp;"; arrOpenTemplate[47] = "&nbsp;"; arrOpenTemplate[33] = "&nbsp;";
                        arrOpenTemplate[37] = "&nbsp;";
                    }
                } else {
                    var BO1 = "";
                    if (iPLMarketDepth == MarketDepth_BestPriceOnly) {
                        var BestBackBet;
                        if (entrantOrder == "Odds") {
                            BO1 = iif(arrBestBetObj[i].odds > 0, arrBestBetObj[i].odds, "");
                            arrOpenTemplate[33] = iif(arrBestBetObj[i].odds > 0, arrBestBetObj[i].odds, "&nbsp;");
                            arrOpenTemplate[33] = iif(BO1 == "", "&nbsp;", ConvertOdds(BO1))
                        } else {
                            var BestBackBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Back, htMinWin[iPLMinWin]);
                            BO1 = iif(BestBackBet[iBestOddPos] > 0, BestBackBet[iBestOddPos], "");
                            arrOpenTemplate[33] = iif(BO1 == "", "&nbsp;", ConvertOdds(BO1))
                        } arrOpenTemplate[37] = ""
                    } else if (iPLMarketDepth == MarketDepth_BestPriceAndStake) {
                        var BestBackBet; if (entrantOrder == "Odds") {
                            BA1 = iif(arrBestBetObj[i].stake > 0, sSymbol + formatDollars(arrBestBetObj[i].stake), "&nbsp;");
                            BO1 = iif(arrBestBetObj[i].odds > 0, arrBestBetObj[i].odds, ""); arrOpenTemplate[33] = iif(BO1 == "", "&nbsp;",
           ConvertOdds(BO1)); arrOpenTemplate[37] = iif(arrBestBetObj[i].stake > 0, arrBestBetObj[i].stake, "&nbsp;")
                        } else {
                            BestBackBet = CalMinWin(iPLMarketDepth, entrantID, betSide_Back, htMinWin[iPLMinWin]);
                            BO1 = iif(BestBackBet[iBestOddPos] > 0, BestBackBet[iBestOddPos], "");
                            arrOpenTemplate[33] = iif(BO1 == "", "&nbsp;", ConvertOdds(BO1));
                            arrOpenTemplate[37] = iif(BestBackBet[iBestStakePos] > 0, sSymbol + formatDollars(BestBackBet[iBestStakePos]), "&nbsp;")
                        }
                    } else {
                        BO1 = (htOdds[entrantID][iEtBO1] == '') ? '' : parseFloat(htOdds[entrantID][iEtBO1]);
                        var BA1 = (htOdds[entrantID][iEtBA1] == '') ? '' : parseFloat(htOdds[entrantID][iEtBA1]);
                        var BO2 = (htOdds[entrantID][iEtBO2] == '') ? '' : parseFloat(htOdds[entrantID][iEtBO2]);
                        var BA2 = (htOdds[entrantID][iEtBA2] == '') ? '' : parseFloat(htOdds[entrantID][iEtBA2]);
                        var BO3 = (htOdds[entrantID][iEtBO3] == '') ? '' : parseFloat(htOdds[entrantID][iEtBO3]);
                        var BA3 = (htOdds[entrantID][iEtBA3] == '') ? '' : parseFloat(htOdds[entrantID][iEtBA3]);
                        var BO4 = (htOdds[entrantID][iEtBO4] == '') ? '' : parseFloat(htOdds[entrantID][iEtBO4]);
                        var BA4 = (htOdds[entrantID][iEtBA4] == '') ? '' : parseFloat(htOdds[entrantID][iEtBA4]);
                        var BO5 = (htOdds[entrantID][iEtBO5] == '') ? '' : parseFloat(htOdds[entrantID][iEtBO5]);
                        var BA5 = (htOdds[entrantID][iEtBA5] == '') ? '' : parseFloat(htOdds[entrantID][iEtBA5]);
                        if (BO1 == "") {
                            arrOpenTemplate[53] = "&nbsp;"; arrOpenTemplate[57] = ""
                        } else {
                            arrOpenTemplate[53] = ConvertOdds(BO1); arrOpenTemplate[57] = sSymbol + formatDollars(BA1)
                        } if (BO2 == "") {
                            arrOpenTemplate[43] = "&nbsp;"; arrOpenTemplate[47] = ""
                        } else {
                            arrOpenTemplate[43] = ConvertOdds(BO2); arrOpenTemplate[47] = sSymbol + formatDollars(BA2)
                        }
                        if (BO3 == "") {
                            arrOpenTemplate[33] = "&nbsp;";
                            arrOpenTemplate[37] = ""
                        } else {
                            arrOpenTemplate[33] = ConvertOdds(BO3); arrOpenTemplate[37] = sSymbol + formatDollars(BA3)
                        }
                    } if (BO1 != "" && allBacked) { bookBack = bookBack + 1 / BO1 } else {
                        allBacked = false
                    }
                }
            }
            if (entrantStatus == "Open") {
                if (sBetType == "Odds" && top.bLoggedIn && !top.userGetSetting(top.cHideProfitLoss) && plDisplay != 0) {
                    arrOpenTemplate[19] = ""; if (iNoOfWinners == 1) {
                        if (plDisplay == 0) {
                            arrOpenTemplate[23] = ""; arrOpenTemplate[27] = ""
                        } else if (plDisplay >= 0) {
                            arrOpenTemplate[23] = formatCurrency(plDisplay, sSymbol, numDec); arrOpenTemplate[27] = ""
                        } else { arrOpenTemplate[23] = ""; arrOpenTemplate[27] = formatCurrency(plDisplay, sSymbol, numDec) }
                    } else {
                        if (winDisplay == 0 && loseDisplay == 0) {
                            arrOpenTemplate[23] = ""; arrOpenTemplate[27] = ""
                        }
                        arrOpenTemplate[23] = formatCurrency(winDisplay, sSymbol, numDec) + ", ";
                        arrOpenTemplate[27] = formatCurrency(loseDisplay, sSymbol, numDec)
                    }
                } else {
                    arrOpenTemplate[19] = "display:none"
                }
            } entrantHTML = arrOpenTemplate.join("");
            if (entrantStatus == "Open" && sBetType == "Odds" && top.bLoggedIn && !top.userGetSetting(top.cHideProfitLoss) && iNoOfWinners > 1) {
                if (winDisplay < 0) entrantHTML = entrantHTML.replace("PLWin", "PLTmpLose");
                if (loseDisplay >= 0) entrantHTML = entrantHTML.replace("PLLose", "PLWin");
                entrantHTML = entrantHTML.replace("PLTmpLose", "PLLose")
            }
            if (entrantStatus == "Open") {
                if (sBetType != "Odds" && i > 0 && i % 2 == 0) {
                    if (tpMarketFlag == "N") openEntrantHTML.append(blankEntrantHTML)
                }
                iNoOfOpenEntrants = iNoOfOpenEntrants + 1; openEntrantHTML.append(entrantHTML)
            } else {
                if (sBetType != "Odds" && i > 0 && i % 2 == 0) {
                    if (tpMarketFlag == "N") voidEntrantHTML.append(blankEntrantHTML)
                } voidEntrantHTML.append(entrantHTML)
            }
        }
        document.getElementById("spSelections").innerHTML = iNoOfOpenEntrants;
        var entrantTable = "";
        if (iPLMarketDepth == MarketDepth_BestPriceOnly || iPLMarketDepth == MarketDepth_BestPriceAndStake) {
            entrantTable = document.getElementById("taEntrantTableOneSideOneOddTemplate").value
        } else {
            entrantTable = document.getElementById("taEntrantTableTemplate1").value
        }
        ui_changeSortingDropDown("Identifier", bShowIdentifier);
        var arrEntrantTable = entrantTable.split('@');
        document.getElementById("dvEntrantsTable").innerHTML = arrEntrantTable[0] + openEntrantHTML.toString() + voidEntrantHTML.toString() + arrEntrantTable[2];
        if (allBacked) {
            bookBack = formatPercentage(bookBack * 100);
            document.getElementById("spBackBook").innerHTML = bookBack;
            if (bPLShowBook) {
                document.getElementById("spBackBook").style.display = ""
            } else {
                document.getElementById("spBackBook").style.display = "none"
            }
        } else {
            document.getElementById("spBackBook").innerHTML = ""
        }
    } catch (e) {
        top.showMessageBox("client_renderEntrantDataWithOddsForSportsbook:" + e.message, null, null)
    }
    client_renderWindowResize(); window.setTimeout("client_renderWindowResize()", timeoutResize)
};

function CalMinWin(iMarketDepth, entrantID, betSide, MinWin, identifier) {
    var BestBet = [-1, -1, entrantID, identifier];
    var O1, O2, O3, O4, O5; var A1, A2, A3, A4, A5; var sum = -1;
    if (betSide == betSide_Lay && htOdds[entrantID] != null) {
        O1 = htOdds[entrantID][iEtLO1]; O2 = htOdds[entrantID][iEtLO2];
        O3 = htOdds[entrantID][iEtLO3]; O4 = htOdds[entrantID][iEtLO4]; O5 = htOdds[entrantID][iEtLO5];
        A1 = htOdds[entrantID][iEtLA1]; A2 = htOdds[entrantID][iEtLA2]; A3 = htOdds[entrantID][iEtLA3];
        A4 = htOdds[entrantID][iEtLA4]; A5 = htOdds[entrantID][iEtLA5]; BestBet[iBestOddPos] = iif(O1 == "", -1, O1);
        BestBet[iBestStakePos] = iif(A1 == "", -1, A1); sum = iif(O1 == "", 0, A1);
        if (sum < MinWin && O2 != "") {
            sum = parseFloat(sum) + parseFloat(A2); BestBet[iBestOddPos] = O2;
            BestBet[iBestStakePos] = sum;
        }
        if (sum < MinWin && O3 != "") {
            sum = parseFloat(sum) + parseFloat(A3); BestBet[iBestOddPos] = O3;
            BestBet[iBestStakePos] = sum;
        }
        if (sum < MinWin && O4 != "") {
            sum = parseFloat(sum) + parseFloat(A4); BestBet[iBestOddPos] = O4; BestBet[iBestStakePos] = sum;
        }
        if (sum < MinWin && O5 != "") {
            sum = parseFloat(sum) + parseFloat(A5); BestBet[iBestOddPos] = O5; BestBet[iBestStakePos] = sum;
        }
    } else if (htOdds[entrantID] != null) {
        var sumOfStake = -1; O1 = htOdds[entrantID][iEtBO1]; O2 = htOdds[entrantID][iEtBO2];
        O3 = htOdds[entrantID][iEtBO3]; O4 = htOdds[entrantID][iEtBO4]; O5 = htOdds[entrantID][iEtBO5];
        A1 = htOdds[entrantID][iEtBA1]; A2 = htOdds[entrantID][iEtBA2]; A3 = htOdds[entrantID][iEtBA3];
        A4 = htOdds[entrantID][iEtBA4]; A5 = htOdds[entrantID][iEtBA5];
        if (iMarketDepth == MarketDepth_FullDepth) {
            BestBet[iBestOddPos] = iif(O1 == "", -1, O1); BestBet[iBestStakePos] = iif(A1 == "", -1, A1)
        } else {
            BestBet[iBestOddPos] = iif(O1 == "", -1, O1);
            BestBet[iBestStakePos] = iif(A1 == "", -1, A1);
            sum = iif(O1 == "", 0, parseFloat((O1 - 1) * A1)); sumOfStake = parseFloat(A1);
            if (sum < MinWin && O2 != "") {
                sum = parseFloat(sum) + parseFloat((O2 - 1) * A2);
                sumOfStake = parseFloat(sumOfStake) + parseFloat(A2); BestBet[iBestOddPos] = O2;
                BestBet[iBestStakePos] = sumOfStake
            } if (sum < MinWin && O3 != "") {
                sum = parseFloat(sum) + parseFloat((O3 - 1) * A3); sumOfStake = parseFloat(sumOfStake) + parseFloat(A3);
                BestBet[iBestOddPos] = O3; BestBet[iBestStakePos] = sumOfStake
            } if (
        sum < MinWin && O4 != "") {
                sum = parseFloat(sum) + parseFloat((O4 - 1) * A4);
                sumOfStake = parseFloat(sumOfStake) + parseFloat(A4); BestBet[iBestOddPos] = O4;
                BestBet[iBestStakePos] = sumOfStake
            } if (sum < MinWin && O5 != "") {
                sum = parseFloat(sum) + parseFloat((O5 - 1) * A5);
                sumOfStake = parseFloat(sumOfStake) + parseFloat(A5); BestBet[iBestOddPos] = O5;
                BestBet[iBestStakePos] = sumOfStake
            }
        }
    }
    if (iMarketDepth == MarketDepth_BestPriceOnly && sum < MinWin) {
        BestBet[iBestOddPos] = -1; BestBet[iBestStakePos] = -1
    } return BestBet
};

function changeBookPercentage(bShowBookPercentage) {
    if (bShowBookPercentage && document.getElementById("spBackBook").innerHTML != "") {
        document.getElementById("spBackBook").style.display = ""
    } else {
        document.getElementById("spBackBook").style.display = "none"
    }
    if (bShowBookPercentage && document.getElementById("spLayBook").innerHTML != "") {
        document.getElementById("spLayBook").style.display = ""
    } else { document.getElementById("spLayBook").style.display = "none" }
};

function changeTotalMatched(bShowTotalMatched) {
    var txtHideShow = "none"; if (bShowTotalMatched && dTotalMatched > 0) txtHideShow = "";
    document.getElementById("trTotalMatched").style.display = txtHideShow
};

function iif(i, j, k) { if (i) return j; else return k };
function doBackLayForBestBet(entrantID, htPlaceBets, side, odds, bChangeIfExist) {
    if (sStatus == "Open" || sStatus == "Live") {
        if (bChangeIfExist == null) bChangeIfExist = false;
        var bExisted = true;
        if (htPlaceBets[entrantID] == 'undefined' || htPlaceBets[entrantID] == null) {
            bExisted = false
        }
        if (!bExisted || bChangeIfExist) {
            var bet = new Array(); bet[iBetEntrantID] = entrantID; bet[iBetStake] = "";
            if (side == betSide_Back && odds > 0) {
                bet[iBetOdds] = odds
            } else if (side == betSide_Lay && odds > 0) {
                bet[iBetOdds] = odds
            } else { bet[iBetOdds] = "" }
            htPlaceBets[entrantID] = bet;
            if (bExisted) {
                document.getElementById("inOdds_" + betState_UnPlaced + "_" + side + "_" + entrantID).value = ConvertOdds(bet[iBetOdds])
            } return true
        }
    } return false
};

function cutDemicalPlace(iValue, dp) {
    var index = iValue.toString().indexOf(".");
    var bIsInt = (iValue.toString().indexOf(".00") >= 0 ? true : false);
    if (index < 0) {
        return iValue.toString()
    } else {
        if (iValue.toString().length < index + 1 + dp) {
            if (bIsInt) {
                return iValue.toString().substring(0, index)
            } else { return iValue.toString() }
        } else {
            if (bIsInt) {
                return iValue.toString().substring(0, index)
            } else { return iValue.toString().substring(0, index + 1 + dp) }
        }
    }
};

function getCheckedValue(radioObj) {
    if (!radioObj) return ""; var radioLength = radioObj.length;
    if (radioLength == undefined)
        if (radioObj.checked) return radioObj.value;
        else return ""; for (var i = 0; i < radioLength; i++) {
        if (radioObj[i].checked) { return radioObj[i].value }
    } return ""
};

function setCheckedValue(radioObj, newValue) {
    if (!radioObj) return; var radioLength = radioObj.length;
    if (radioLength == undefined) {
        radioObj.checked = (radioObj.value == newValue.toString()); return
    }
    for (var i = 0; i < radioLength; i++) {
        radioObj[i].checked = false;
        if (radioObj[i].value == newValue.toString()) { radioObj[i].checked = true }
    }
};

function ui_doClickPLTab(tabID) {
    if (tabID == "ProfitLoss") {
        document.getElementById("trProfitLoss").style.display = "";
        document.getElementById("trDisplay").style.display = "none";
        document.getElementById("tbDisplay").className = "";
        document.getElementById("tbProfitLoss").className = "here"
    } else if (tabID == "Display") {
        document.getElementById("trProfitLoss").style.display = "none";
        document.getElementById("trDisplay").style.display = "";
        document.getElementById("tbDisplay").className = "here";
        document.getElementById("tbProfitLoss").className = ""
    }
};

function GenerateTextSnapshotData() {
    var TextSnapshotData = "";
    TextSnapshotData += document.getElementById("spEvent").innerHTML + " - " + document.getElementById("spMarket").innerHTML + "\r\n";
    var BestBackBet = ""; var BO1 = "";
    for (var entrantID in htEntrants) {
        var entrantStatus = htEntrants[entrantID][iEtStatus];
        if (entrantStatus == "Open") {
            BestBackBet = CalMinWin(MarketDepth_BestPriceOnly, entrantID, betSide_Back, htMinWin[0]);
            BO1 = iif(BestBackBet[iBestOddPos] > 0, ConvertOdds(BestBackBet[iBestOddPos]), betSide_Back);
            TextSnapshotData += FormatSnapshotData(htEntrants[entrantID][iEtDesc], BO1, 35) + "\r\n"
        }
    }
    CopyToClipboard(TextSnapshotData, 'TextSummary', "")
};

function FormatSnapshotData(entrantDescription, backOdds, totalLength) {
    var remainLength = totalLength - entrantDescription.length - backOdds.length;
    var spaceString = "                                     ";
    return entrantDescription + spaceString.substring(0, remainLength) + backOdds
};

function setOpacity(value) {
    var dvObj = document.getElementById("dvMarketViewResult");
    if (value == 0) {
        dvObj.style.display = "none"
    } else {
        var temp = "alpha(opacity=" + value * 10 + ")";
        dvObj.style.filter = temp; dvObj.style.opacity = value / 10
    }
};

function GetShortcutMarketLink(bIncludeAffiliateCode) {
    var shortcutMarketLink = "http://" + location.host + "/m.ashx/" + sMarketID.substr(0, 5);
    if (bIncludeAffiliateCode) { shortcutMarketLink += "/" + sAffiliateCode }
    return shortcutMarketLink
};

function ui_changeSortingDropDown(sSortOption, bIsShow) {
    var textPos = 0; var valuePos = 0; if (currentBrowser == null)
        currentBrowser = new BrowserObj();
    if (sSortOption == "Identifier") {
        textPos = iIdentifierText; valuePos = iIdentifierValue
    } else { }
    if (htSortBy[textPos] == null) htSortBy[textPos] = "";
    if (bIsShow) {
        if (htSortBy[textPos].length > 0) {
            var sortOption = document.createElement('option');
            sortOption.text = htSortBy[textPos];
            sortOption.value = htSortBy[valuePos];
            var ddlObj = document.getElementById("selEntrantOrder");
            if (currentBrowser.isIE) {
                ddlObj.add(sortOption)
            } else {
                ddlObj.add(sortOption, null);
            } htSortBy[textPos] = ""
        }
    } else {
        var ddlObj = document.getElementById("selEntrantOrder");
        for (var i = ddlObj.length - 1; i >= 0; i--) {
            if (ddlObj.options[i].value == sSortOption) {
                htSortBy[textPos] = ddlObj.options[i].text;
                htSortBy[valuePos] = ddlObj.options[i].value;
                ddlObj.remove(i)
            }
        }
    }
};

function sortByOdds(a, b) {
    var x = parseFloat(a.odds); var y = parseFloat(b.odds);
    if (x <= 0 && y <= 0) {
        if (a.identifier > b.identifier) return 1;
        else return -1
    } else if (x <= 0) return 1;
    else if (y <= 0) return -1;
    else if ((x < y) && (y > 0)) return -1;
    else if ((x > y) && (y > 0)) return 1;
    else {
        if (a.identifier > b.identifier) return 1;
        else return -1
    }
};

function BestBetObj(_entrantID, _stake, _odds, _identifier) {
    this.entrantID = _entrantID;
    this.stake = _stake;
    this.odds = _odds;
    this.identifier = parseFloat(_identifier)
};

function findPos(obj) {
    var curtop = 0; if (obj != null) {
        if (obj.offsetParent) {
            curtop = obj.offsetTop;
            while (obj = obj.offsetParent) { curtop += obj.offsetTop }
        }
    }
    return curtop
};

function BrowserObj() {
    var nVer = navigator.appVersion; var nAgt = navigator.userAgent; this.browserName = '';
    this.isIE = false;
    if ((verOffset = nAgt.indexOf("MSIE")) != -1) {
        this.browserName = "Microsoft Internet Explorer";
        this.isIE = true
    } else if ((verOffset = nAgt.indexOf("Opera")) != -1) {
        this.browserName = "Opera"
    } else if ((verOffset = nAgt.indexOf("Firefox")) != -1) {
        this.browserName = "Firefox"
    } else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) < (verOffset = nAgt.lastIndexOf('/'))) {
        this.browserName = nAgt.substring(nameOffset, verOffset)
    }
    if (this.browserName.toLowerCase() == this.browserName.toUpperCase()) {
        this.browserName = navigator.appName
    }
};

function ui_openMarketSnapshotSettings(clipboardFormat) {
    if (IsProcessing()) return;
    if (document.getElementById("dvMarketSnapshotHolder").style.display == "") {
        ui_closeMarketSnapshotSettings()
    } else {
        if (sMarketSnapshotChoice == "") {
            document.getElementById("txtMarketSnapshotContent").value = sPleaseWait
        } sMarketSnapshotChoice = clipboardFormat;
        var SettingLinkObj = document.getElementById("BBCode");
        document.getElementById("dvMarketSnapshotHolder").style.top = findPosY(SettingLinkObj);
        document.getElementById("dvMarketSnapshotHolder").style.left = findPosX(SettingLinkObj);
        document.getElementById("dvMarketSnapshotHolder").style.display = "";
        ui_doClickMarketSnapshotTab(clipboardFormat, true)
    }
};

function ui_closeMarketSnapshotSettings() {
    if (IsProcessing()) return;
    document.getElementById("dvMarketSnapshotHolder").style.display = "none"
};

function ui_doClickMarketSnapshotTab(clipboardFormat, bIsOpen) {
    if (!bIsOpen && document.getElementById("dvMarketSnapshotHolder").style.display == "" && sMarketSnapshotChoice == clipboardFormat) {
        ui_closeMarketSnapshotSettings()
    } else {
        if (clipboardFormat == 'TextSummary') {
            sMarketSnapshotChoice = clipboardFormat; GenerateTextSnapshotData()
        } else if (clipboardFormat == 'Refresh' || sClipboardData == "") {
            if (clipboardFormat != 'Refresh') sMarketSnapshotChoice = clipboardFormat;
            if (sMarketSnapshotChoice == "" || sMarketSnapshotChoice == "TextSummary") {
                GenerateTextSnapshotData()
            } else {
                document.getElementById("txtMarketSnapshotContent").value = sPleaseWait;
                frames["ifraMarketRequest"].location = "MarketSnapshotJS" + sExt + "?marketID=" + sMarketID + "&clipboardFormat=" + sMarketSnapshotChoice;
            }
        } else if (clipboardFormat == 'BBCode' || clipboardFormat == 'HTMLCode' || clipboardFormat == 'DirectLink') {
            if (sMarketSnapshotChoice == "") {
                sMarketSnapshotChoice = clipboardFormat;
                document.getElementById("txtMarketSnapshotContent").value = sPleaseWait;
                frames["ifraMarketRequest"].location = "MarketSnapshotJS" + sExt + "?marketID=" + sMarketID + "&clipboardFormat=" + sMarketSnapshotChoice;
            } else {
                sMarketSnapshotChoice = clipboardFormat;
                CopyToClipboard(sClipboardData, sMarketSnapshotChoice, "")
            }
        }
    }
};

function CopyToClipboard(imageLink, clipboardFormat, message) {
    var bSetClipboard = true; var CopyData = "";
    var btnObj = null; document.getElementById("tbBBCode").className = "";
    document.getElementById("tbHTMLCode").className = "";
    document.getElementById("tbDirectLink").className = "";
    document.getElementById("tbTextSummary").className = "";
    if (clipboardFormat == 'BBCode') {
        document.getElementById("spMarketSnapshotDesc").innerHTML = msgHelpMarketSnapshotBBCode;
        btnObj = document.getElementById("tbBBCode")
    } else if (clipboardFormat == 'HTMLCode') {
        document.getElementById("spMarketSnapshotDesc").innerHTML = msgHelpMarketSnapshotHTMLCode;
        btnObj = document.getElementById("tbHTMLCode")
    } else if (clipboardFormat == 'DirectLink') {
        document.getElementById("spMarketSnapshotDesc").innerHTML = msgHelpMarketSnapshotDirectLink;
        btnObj = document.getElementById("tbDirectLink")
    } else if (clipboardFormat == 'TextSummary') {
        document.getElementById("spMarketSnapshotDesc").innerHTML = msgHelpMarketSnapshotText;
        btnObj = document.getElementById("tbTextSummary")
    }
    if (imageLink == null || imageLink == "") {
        imageLink = "";
    }
    if (true) {
        var sEventMarket = document.getElementById("spEvent").innerHTML + " - " + document.getElementById("spMarket").innerHTML;
        if (clipboardFormat != 'TextSummary' && imageLink != "") {
            sClipboardData = imageLink
        } if (clipboardFormat != 'TextSummary' && imageLink == "") {
            imageLink = sClipboardData
        }
        if (clipboardFormat == 'BBCode') {
            CopyData = "[IMG]" + imageLink + "[/IMG]";
            CopyData += "\r\n[URL=" + GetShortcutMarketLink(true) + "]" + sEventMarket + "[/URL]"
        } else if (clipboardFormat == 'HTMLCode') {
            CopyData = "<IMG SRC='" + imageLink + "' />";
            CopyData += "\r\n<A HREF='" + GetShortcutMarketLink(true) + "'>" + sEventMarket + "</A>"
        } else if (clipboardFormat == 'DirectLink') {
            CopyData = imageLink
        } else if (clipboardFormat == 'TextSummary') {
            CopyData = imageLink; CopyData += "\r\n" + GetShortcutMarketLink(true);
            CopyData += "\r\n" + sCourtesy
        }
    } if (message == "") {
        document.getElementById("txtMarketSnapshotContent").value = CopyData;
        document.getElementById("txtMarketSnapshotContent").select(); btnObj.className = "here"
    } else {
        if (imageLink.length == 0) {
            document.getElementById("txtMarketSnapshotContent").value = ""
        } else { document.getElementById("txtMarketSnapshotContent").value = CopyData }
        top.showMessageBox(message, null, null)
    }
};

function PLGroupControl(bIsEnabled) {
    document.getElementById("chkPLMinusComm").disabled = !bIsEnabled
};
function getCellWidthByName(tagName, cellName) {
    var cellWidth = 0;
    var cellObjs = document.getElementById(tagName).getElementsByTagName("td");
    for (i = 0; i < cellObjs.length; i++) {
        if (cellObjs[i].getAttribute("name") == cellName) {
            cellWidth = cellObjs[i].offsetWidth; break
        }
    }
    return cellWidth
};

function forceLogin() { top.ui_doPopLoginOpen(); }
function ui_showMarketDetails(obj) {
    document.getElementById("dvMarketDetails").style.display = "";
};

function ui_hideMarketDetails() {
    document.getElementById("dvMarketDetails").style.display = "none"
};

function ui_refreshMyBets() { server_getBetData() };

function ui_doRefreshAll() {
    try {
        if (BeginProcessing()) {
            var sURL = "OddsDataJS" + sExt + "?marketID=" + sMarketID + "&ts=" + iTimestamp + "&ExtendSession=true";
            var evalResponse = true; GetLatestMarketData(sURL, frames["ifraMarketRequest"], evalResponse);
            client_renderTimeToStart();
            if (top.bLoggedIn) {
                bMemberRequestProcessing = false;
                ui_doRefreshMyBets()
            }
        }
    } catch (e) {
        top.showMessageBox("ui_doRefreshAll:" + e.message, null, null)
    }
};

function calcuateFakeBorderPosition() {
    var dockCtrl = $("#taPLSetting");
    var dockCtrlPos = dockCtrl.position();
    var offsetX = dockCtrl.outerWidth();
    var offsetY = dockCtrl.outerHeight();
    var fakebordertop = parseInt(dockCtrlPos.top + offsetY) - $("#tbPLSettingTopBorderPart").outerHeight();
    $("#tbPLSettingTopBorderPart").css("display", ""); $("#tbPLSettingTopBorderPart").css("top", fakebordertop - 1);
    $("#tbPLSettingTopBorderPart").css("left", dockCtrlPos.left + offsetX);
    $("#tbPLSettingTopBorderPart").css("width", $('#MarketSettingsDialog').parent().outerWidth() - offsetX)
}

function ceMarketSettings() {
    if ($('#MarketSettingsDialog').dialog('isOpen') == true) {
        $('#MarketSettingsDialog').dialog('close');
        $("#tbPLSettingTopBorderPart").css("display", "none");
        $("#taPLSetting").attr("style", "border: 1px solid #CCC; padding: 2px 5px 3px 5px;")
    } else {
        $('#MarketSettingsDialog').dialog('open')
    }
}
