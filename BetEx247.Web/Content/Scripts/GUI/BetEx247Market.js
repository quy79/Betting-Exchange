BetEx247Market = {
    bMemberRequestProcessing: false,
    bMarketIsChanged: false,
    timeoutRefreshOdds : 10000,
    timeoutRedirect : 10000,
    htEntrants: {},

    IsProcessing: function () { return this.bMemberRequestProcessing },

    initMarket: function (marketID, bSyncTree) {
        server_getMarketData(marketID, bSyncTree, false)
    },
    server_getMarketData: function (marketID, bSyncTree, refresh) {
        if (sMarketID != marketID || refresh) {
            window.clearTimeout(BetEx247Market.timeoutRefreshOdds);
            window.clearTimeout(BetEx247Market.timeoutRedirect);
            frames["ifraOrdersRequest"].location = "Blank.html";
            frames["ifraMarketRequest"].location = "Blank.html";
            frames["ifraMarketRequest"].location = "MarketDataJS" + sExt + "?marketID=" + marketID + "&SyncTree=" + bSyncTree + "&src=mnu" + (refresh ? "&InitLoad=false" : "")
        }
    },

    server_receiveMarketData: function (htMarketData, htEnts, arrEntsOrderByName, arrRelatedMarkets, arrRelatedMarketGroups, initLoad, arrEntsOrderByIdentifier, mcSymbol, numDecPlaces) {
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
    },

    ui_doBack: function (entrantID, position, myOdds) {
        if (this.IsProcessing()) return;
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
    },

    ui_doLay: function (entrantID, position, myOdds) {
        if (this.IsProcessing()) return;
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
    },

    doBackLay: function (entrantID, htPlaceBets, side, oddsIndex, bChangeIfExist, myOdds) {
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
    }

}