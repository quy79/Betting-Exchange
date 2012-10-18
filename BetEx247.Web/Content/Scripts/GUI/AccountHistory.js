var g_currentView = "All";
var g_periodType = "";
var g_startDate = "";
var g_endDate = "";
var g_displayType = "";
var g_betCategory = "";
var g_currentPage = 1;
var g_switchedViews = false;
var iNoOfPages = 0;
var sShowWarning = false;
var enableOddsFormat = true;
var bMemberRequestProcessing = false;

AccountHistory ={
    ui_doSearch:function() {
        var period = $("#selPeriod").val();
        var startDate = "";
        var endDate = "";
        var betCategory = $("#selBetCategory").val();
        var display = $("#selDisplay").val();
        if (ValidateInput()) {
            $("#dvAccountHistory").html(this.html_getAllTable("") + this.getLoadingTableHTML());
            if ($("#rdoCustom").prop('checked')) {
                period = "DateRange";
                startDate = $("#txtStartDate").val();
                endDate = $("#txtEndDate").val()
            }
            $("#dvAccountHistory").html(html_getAllTable(""));
            $("#dvAccountHistoryGroupedBets").html("");
            $("#dvAccountHistoryGroupedAccounts").html("");
            $("#dvAccountHistoryGroupedMisc").html("");
            g_switchedViews = false;
            g_startDate = startDate;
            g_endDate = endDate;
            sResultID = "";
            sShowWarning = true;
            if (g_currentView == "All") {
                this.server_getAllData(period, startDate, endDate, betCategory, display, 1, true)
            } else { }
        }
    },

    ui_doDisplayChange:function(displayType) {
        this.server_getAllData(g_periodType, g_startDate, g_endDate, g_betCategory, displayType, 1, true)
    },

    ui_doBetCatChange:function(betCategory) {
        this.server_getAllData(g_periodType, g_startDate, g_endDate, betCategory, g_displayType, 1, true)
    },

    ui_doPageChange:function(pageNo) {
        this.server_getAllData(g_periodType, g_startDate, g_endDate, g_betCategory, g_displayType, pageNo, false)
    },

    ui_popStartDate:function() {
        SetSelectedRadio("rdoCustom", "rdoPredefined");
        popUpCalendar(document.getElementById('txtStartDate'), document.getElementById('txtStartDate'), 'yyyy-mm-dd', -1, -1)
    },

    ui_popEndDate:function() {
        SetSelectedRadio("rdoCustom", "rdoPredefined");
        popUpCalendar(document.getElementById('txtEndDate'), document.getElementById('txtEndDate'), 'yyyy-mm-dd', -1, -1)
    },

    ui_doExcelDownload:function(downloadType) {
        if (downloadType != "") {
            var period = $("#selPeriod").val();
            var startDate = "";
            var endDate = "";
            var betCategory = $("#selBetCategory").val();
            var displayType = $("#selDisplay").val();
            if (ValidateInput()) {
                if ($("#rdoCustom").prop('checked')) {
                    period = "DateRange";
                    startDate = $("#txtStartDate").val();
                    endDate = $("#txtEndDate").val()
                }
                var displayMemberType = "";
                frames["ifraMemberRequest"].location = "MemberAccountHistory" + sExt + "?ExcelDowload=" + downloadType + "&Period=" + period + "&StartDate=" + startDate + "&EndDate=" + endDate + "&BetCategoryID=" + betCategory + "&Display=" + displayType + "&DisplayMemberType=" + displayMemberType + "&RecordsPerPage=" + sRecordsPerPage + "&MaxNumOfPage=" + sMaxNumOfPage + "&ResultID=" + sResultID
            }
        }
    },

    server_getAllData:function(periodType, startDate, endDate, betCategory, displayType, pageNo, refresh) {
        if (this.BeginProcessing()) {
            var displayMemberType = "";
            if (displayType.indexOf("Member") != -1) {
                displayMemberType = displayType.replace("Member", "");
                displayType = "Member"
            } 
            frames["ifraMemberRequest"].location = "MemberAccountHistoryGetJS" + sExt + "?Period=" + periodType + "&StartDate=" + startDate + "&EndDate=" + endDate + "&BetCategory=" + betCategory + "&Display=" + displayType + "&Page=" + pageNo + "&Refresh=" + refresh + "&DisplayMemberType=" + displayMemberType + "&RecordsPerPage=" + sRecordsPerPage + "&MaxNumOfPage=" + sMaxNumOfPage + "&ResultID=" + sResultID

        }
    },

    server_receiveBetCategories:function(arrIDs, arrDescs) {
        insertOptions(arrIDs, arrDescs, "selBetCategory", true, sBetCatAll)
    },

    server_receiveAllData:function(arrAllData, periodType, startDate, endDate, betCategory, displayType, pageNo, noOfPages, displayMemberType, resultID, numDec, oddsFormat) {
        try {
            g_periodType = periodType;
            g_displayType = displayType;
            g_betCategory = betCategory;
            g_currentPage = pageNo;
            iNoOfPages = noOfPages;
            sResultID = resultID;
            var historyHTML = "";
            for (var i = 0; i < arrAllData.length; i++) {
                var voucherType = arrAllData[i][0];
                switch (voucherType) {
                    case "Member":
                    case "Adjustment":
                    case "Fee":
                        historyHTML += html_getAllMemberRow(arrAllData[i], numDec);
                        break;
                    case "FBSettlement":
                    case "Settlement":
                        historyHTML += html_getAllSettlementRow(arrAllData[i], numDec, oddsFormat);
                        break;
                    case "Commission":
                    case "MarketRebate":
                    case "LoyaltyRebate":
                        historyHTML += html_getAllOthersRow(arrAllData[i], numDec);
                        break
                }
            }
            if (historyHTML == "") {
                historyHTML = html_getAllNoRows()
            }
            historyHTML = html_getAllTable(historyHTML);
            $("#dvAccountHistory").html(historyHTML);
            setSelectedOption("selBetCategory", betCategory, false);
            setSelectedOption("selDisplay", displayType + displayMemberType, false);
            var targetDrop = $("#selPager");
            targetDrop.selectedIndex = pageNo - 1;
            $("#dvPager").html(html_getFooterPager(pageNo, noOfPages));
        } catch (e) {
            top.showMessageBox(e.message)
        }
    },

    receiveWarningByPageCount:function(totalRecords) {
        if ((totalRecords + 1 > sRecordsPerPage * sMaxNumOfPage) && sShowWarning == true) {
            sShowWarning = false;
            var msssage = msgOverRecords;
            var re = new RegExp("@MaxNoOfPage", "g");
            msssage = msssage.replace(re, sMaxNumOfPage.toString());
            top.showMessageBox(msssage)
        }
    },

    html_getAllTable:function(rowHTML) {
        var tableHTML = $("#taAllTable").val();
        var re = new RegExp("@RowData@", "g");
        tableHTML = tableHTML.replace(re, rowHTML);
        return tableHTML
    },

    html_getAllMemberRow:function(arrRowData, numDec) {
        var rowHTML = $("#taAllMemberRow").val();
        var symbol = arrRowData[9];
        var re = new RegExp("@Date@", "g");
        rowHTML = rowHTML.replace(re, arrRowData[1]);
        re = new RegExp("@ID@", "g");
        rowHTML = rowHTML.replace(re, arrRowData[2]);
        var newLine = (arrRowData[4].length > 0 ? "<br/>" : "");
        re = new RegExp("@Desc@", "g");
        var status = "";
        if (arrRowData[6] != "Approved") status = "&#160;(" + htTranslations[arrRowData[6]] + ")";
        var desc = arrRowData[3] + newLine + arrRowData[4] + status;
        if (arrRowData[6] == "Rejected") desc = "<span class='spRejected'>" + desc + "</span>";
        rowHTML = rowHTML.replace(re, desc);
        var typeResult = (arrRowData[0] == "Member" ? htTranslations[arrRowData[5]] : htTranslations[arrRowData[0]]);
        re = new RegExp("@TypeResult@", "g");
        rowHTML = rowHTML.replace(re, typeResult);
        var debit = (arrRowData[5] != "Deposit" ? formatCurrency(arrRowData[7], symbol, numDec) : "");
        re = new RegExp("@Debit@", "g");
        rowHTML = rowHTML.replace(re, debit);
        var credit = (arrRowData[5] == "Deposit" ? formatCurrency(arrRowData[7], symbol, numDec) : "");
        re = new RegExp("@Credit@", "g");
        rowHTML = rowHTML.replace(re, credit);
        var amount = (arrRowData[5] == "Deposit" ? formatCurrency(arrRowData[7], symbol, numDec) : formatCurrency(0 - arrRowData[7], symbol, numDec));
        re = new RegExp("@Amount@", "g");
        rowHTML = rowHTML.replace(re, amount);
        var balance = (arrRowData[6] == "Approved" ? formatCurrency(arrRowData[8], symbol, numDec) : "");
        re = new RegExp("@Balance@", "g");
        rowHTML = rowHTML.replace(re, balance);

        return rowHTML 
    },

    html_getAllSettlementRow:function(arrRowData, numDec, oddsFormat) {
        var rowHTML = $("#taAllSettlementRow").val();
        var symbol = arrRowData[11];
        var re = new RegExp("@Date@", "g");
        rowHTML = rowHTML.replace(re, arrRowData[1]);
        re = new RegExp("@ID@", "g");
        rowHTML = rowHTML.replace(re, (arrRowData[2] == 0 ? "" : arrRowData[2]));
        re = new RegExp("@Desc@", "g");
        if (arrRowData[0] != "LoyaltyRebate") {
            var eventDesc = (arrRowData[5].length > 0 ? "<br/>" + arrRowData[5] : "");
            re = new RegExp("@Desc@", "g");
            var desc = "<b>" + arrRowData[6] + "</b>" + eventDesc;
            rowHTML = rowHTML.replace(re, desc)
        } else {
            rowHTML = rowHTML.replace(re, sRetroLoyaltyRebate)
        }
        var tooltipDesc = arrRowData[4];
        tooltipDesc = (arrRowData[3].length > 0 ? arrRowData[3] + " - " + tooltipDesc : tooltipDesc);
        re = new RegExp("@DGP@", "g");
        rowHTML = rowHTML.replace(re, tooltipDesc);
        var typeResult = arrRowData[0];
        if (arrRowData[0] == "Commission" || arrRowData[0] == "MarketRebate" || arrRowData[0] == "LoyaltyRebate") typeResult = htTranslations[arrRowData[0]];
        if (arrRowData[0] == "Settlement" && arrRowData[7] == "Deposit") typeResult = sWin;
        if (arrRowData[0] == "Settlement" && arrRowData[7] != "Deposit") typeResult = sLose;
        if (arrRowData[0] == "FBSettlement") typeResult = "<span class='spFreeBet'>" + sFreeBet + "</a>";
        re = new RegExp("@TypeResult@", "g");
        rowHTML = rowHTML.replace(re, typeResult);
        var debit = (arrRowData[7] != "Deposit" ? formatCurrency(arrRowData[9], symbol, numDec) : "");
        re = new RegExp("@Debit@", "g");
        rowHTML = rowHTML.replace(re, debit);
        var credit = (arrRowData[7] == "Deposit" ? formatCurrency(arrRowData[9], symbol, numDec) : "");
        re = new RegExp("@Credit@", "g");
        rowHTML = rowHTML.replace(re, credit);
        var amount = (arrRowData[7] == "Deposit" ? formatCurrency(arrRowData[9], symbol, numDec) : formatCurrency(0 - arrRowData[9], symbol, numDec));
        re = new RegExp("@Amount@", "g");
        rowHTML = rowHTML.replace(re, amount);
        var balance = (arrRowData[8] == "Approved" ? formatCurrency(arrRowData[10], symbol, numDec) : "");
        re = new RegExp("@Balance@", "g");
        rowHTML = rowHTML.replace(re, balance);
        re = new RegExp("@Side@", "g");
        rowHTML = rowHTML.replace(re, arrRowData[13]);
        if (arrRowData[0] == "Settlement" || arrRowData[0] == "FBSettlement") {
            re = new RegExp("@Entrant@", "g");
            if (arrRowData[12] != null) {
                rowHTML = rowHTML.replace(re, arrRowData[12])
            } else {
                rowHTML = rowHTML.replace(re, "")
            }
            re = new RegExp("@BL@", "g");
            if (arrRowData[13] != null && arrRowData[13] != "") {
                rowHTML = rowHTML.replace(re, htTranslations[arrRowData[13]])
            } else {
                rowHTML = rowHTML.replace(re, "")
            }
            re = new RegExp("@AmountMatched@", "g");
            if (arrRowData[14] != null) {
                rowHTML = rowHTML.replace(re, formatCurrency(arrRowData[14], symbol, numDec))
            } else {
                rowHTML = rowHTML.replace(re, "")
            }

            var refBL = htTranslations[arrRowData[13]].toLowerCase() == "back" ? "Lay" : "Back";
            var refOdds = arrRowData[15];
            var roundedDecimalOdds = getNearestDecimalOdds(refOdds, refBL);
            var oddsPrefix = (roundedDecimalOdds == refOdds) ? "" : (oddsFormat == "Percentage") ? ((roundedDecimalOdds > refOdds) ? ">" : "<") : ((roundedDecimalOdds > refOdds) ? "<" : ">");
            re = new RegExp("@OddsMatched@", "g");
            if (arrRowData[15] != null) {
                if (enableOddsFormat) {
                    if (oddsFormat == "Decimal" || oddsFormat == "Indon" || oddsFormat == "HK") rowHTML = rowHTML.replace(re, ConvertOddsFromAlgorithm(refOdds));
                    else rowHTML = rowHTML.replace(re, (roundedDecimalOdds == "") ? "" : oddsPrefix + ConvertOdds(roundedDecimalOdds, refBL))
                } else {
                    rowHTML = rowHTML.replace(re, parseFloat(refOdds))
                }
            } else { rowHTML = rowHTML.replace(re, "") }
        } else {
            re = new RegExp("@Entrant@", "g");
            rowHTML = rowHTML.replace(re, "&nbsp;");
            re = new RegExp("@BL@", "g");
            rowHTML = rowHTML.replace(re, "&nbsp;");
            re = new RegExp("@AmountMatched@", "g");
            rowHTML = rowHTML.replace(re, "&nbsp;");
            re = new RegExp("@OddsMatched@", "g");
            rowHTML = rowHTML.replace(re, "&nbsp;")
        }

        return rowHTML
    },

    html_getAllOthersRow:function(arrRowData, numDec) {
        var rowHTML = $("#taAllOthersRow").val();
        var symbol = arrRowData[11];
        var re = new RegExp("@Date@", "g");
        rowHTML = rowHTML.replace(re, arrRowData[1]);
        re = new RegExp("@ID@", "g");
        rowHTML = rowHTML.replace(re, (arrRowData[2] == 0 ? "" : arrRowData[2]));
        re = new RegExp("@Desc@", "g");
        if (arrRowData[0] != "LoyaltyRebate") {
            var eventDesc = (arrRowData[5].length > 0 ? "<br/>" + arrRowData[5] : "");
            re = new RegExp("@Desc@", "g");
            var desc = "<b>" + arrRowData[6] + "</b>" + eventDesc;
            rowHTML = rowHTML.replace(re, desc)
        } else {
            rowHTML = rowHTML.replace(re, sRetroLoyaltyRebate)
        }

        var tooltipDesc = arrRowData[4];
        tooltipDesc = (arrRowData[3].length > 0 ? arrRowData[3] + " - " + tooltipDesc : tooltipDesc);
        re = new RegExp("@DGP@", "g");
        rowHTML = rowHTML.replace(re, tooltipDesc);
        var typeResult = arrRowData[0];
        typeResult = htTranslations[arrRowData[0]];
        re = new RegExp("@TypeResult@", "g");
        rowHTML = rowHTML.replace(re, typeResult);
        var amount = (arrRowData[7] == "Deposit" ? formatCurrency(arrRowData[9], symbol, numDec) : formatCurrency(0 - arrRowData[9], symbol, numDec));
        re = new RegExp("@Amount@", "g");
        rowHTML = rowHTML.replace(re, amount);
        var balance = (arrRowData[8] == "Approved" ? formatCurrency(arrRowData[10], symbol, numDec) : "&nbsp;");
        re = new RegExp("@Balance@", "g");
        rowHTML = rowHTML.replace(re, balance);
        return rowHTML
    },

    html_getAllNoRows:function() {
        var rowHTML = $("#taAllNoRows").val();
        return rowHTML
    },

    html_getMarketDescription:function(gpe, pe, e, m) {
        if (gpe != "") gpe = gpe + " - ";
        if (pe != "") pe = pe + " - ";
        return "<b>" + m + "</b>" + " - " + gpe + pe + e
    },

    html_getEventDescription:function(gpe, pe, e) {
        var desc = e;
        if (pe.length > 0) {
            desc += " (";
            if (gpe.length > 0) desc += gpe + ", ";
            desc += pe; desc += ")"
        } return desc
    },

    html_getFooterPager:function(currentPage, noOfPages) {
        var pagerHTML = "&nbsp;";
        if (noOfPages > 0) {
            if (currentPage > 1 && noOfPages > 1) {
                pagerHTML += $("#taPagePreviousEnabled").val();
            } else {
                pagerHTML += $("#taPagePreviousDisabled").val();
            }
            pagerHTML += $("#taPager").val();
            if (currentPage < noOfPages && noOfPages > 1) {
                pagerHTML += $("#taPageNextEnabled").val();
            } else {
                pagerHTML += $("#taPageNextDisabled").val();
            }
            var re = new RegExp("@FirstPage", "g");
            pagerHTML = pagerHTML.replace(re, 1);
            re = new RegExp("@PrevPage", "g");
            pagerHTML = pagerHTML.replace(re, currentPage - 1);
            re = new RegExp("@CurrentPage", "g");
            pagerHTML = pagerHTML.replace(re, currentPage);
            re = new RegExp("@NoOfPages", "g");
            pagerHTML = pagerHTML.replace(re, noOfPages);
            re = new RegExp("@NextPage", "g");
            pagerHTML = pagerHTML.replace(re, currentPage + 1);
            re = new RegExp("@LastPage", "g");
            pagerHTML = pagerHTML.replace(re, noOfPages)
        } return pagerHTML
    },

    createPagerDropdown:function(noOfPages) {
        var sHTML = "";
        for (i = 0; i < noOfPages; i++) {
            var page = (i + 1);
            var desc = page.toString() + " " + sOf + " " + noOfPages.toString();
            sHTML += "<option value='" + page.toString() + "'>" + desc + "</option>"
        }
        sHTML = "<select id='selPager' onchange='ui_doPageChange(this.value)'>" + sHTML + "</select>";
        $("#spPagerHolder").html(sHTML);
    }, 
    
    BeginProcessing:function() {
        if (!bMemberRequestProcessing) {
            bMemberRequestProcessing = true;
            return true
        } else { return false }
    },

    EndProcessing:function() { bMemberRequestProcessing = false; },

    ValidateInput:function() {
        if ($("#rdoCustom").prop('checked')) return ValidateDateRange($("#txtStartDate").val(), 6);
        return true
    },

    ValidateDateRange:function(startDate, lastNumOfMonth) {
        var totalDays = lastNumOfMonth * 30;
        var currentDate = new Date();
        var currentYear = currentDate.getFullYear();
        var currentMonth = currentDate.getMonth() + 1;
        var currentDay = currentDate.getDate();
        currentMonth = currentMonth < 10 ? "0" + currentMonth : currentMonth;
        currentDay = currentDay < 10 ? "0" + currentDay : currentDay;
        var endDate = currentYear + "-" + currentMonth + "-" + currentDay;
        if (DateDifference(startDate, endDate, "d") > totalDays) {
            top.showMessageBox(msgOverMaxDateRangeLast180Days);
            return false
        } else return true
    },

    ShowBetCategory:function() {
        switch ($("#selDisplay").val()) {
            case "Adjustment":
            case "Deposit":
            case "Fee":
            case "LoyaltyRebate":
            case "Withdraw":
                setSelectedOption("selBetCategory", "", false);
                $("#selBetCategory").prop('disabled',true);
                break;
            default:
                $("#selBetCategory").prop('disabled',false);
                break
        }
    },

    getLoadingTableHTML:function() {
        return loadingTableHTML = $("#taLoadingTable").val(); 
    }
}