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

AccountHistory = {
    Url: 'http://localhost:4262/',
    init: function () { },
    row: 20,
    page: 1,

    ui_doSearch: function () {
        var action = $('#txtaction').val();
        var rptype = $('#txtrpType').val();
        var period = $("#selPeriod") ? $("#selPeriod").val() : "";
        var startDate = "";
        var endDate = "";
        var betCategory = "";
        if ($("#selBetCategory")) {
            betCategory = $("#selBetCategory").val();
        }
        var display = "";
        if ($("#selDisplay").length>0) {
            display = $("#selDisplay").val();
        }

        if ($("#rowspage").length > 0) {
            AccountHistory.row = $("#rowspage").val();
        } else {
            AccountHistory.row = 20;
        }

        //if (this.ValidateInput()) {
        $("#dvAccountHistory").html(this.getLoadingTableHTML());
        if ($("#rdoCustom").prop('checked')) {
            period = "DateRange";
            startDate = $("#sd").val();
            endDate = $("#ed").val()
        }

        if ($("#rdoCustom").prop('checked') && ($("#sd").val() == "" || $("#ed").val() == "")) {
            alert('You must specify start date, end date !');
            return;
        }

        $("#dvAccountHistory").html();
        $("#dvAccountHistoryGroupedBets").html("");
        $("#dvAccountHistoryGroupedAccounts").html("");
        $("#dvAccountHistoryGroupedMisc").html("");
        g_switchedViews = false;
        g_startDate = startDate;
        g_endDate = endDate;
        g_displayType = display;
        g_periodType = period;
        g_betCategory = betCategory;

        sResultID = "";
        sShowWarning = true;
        if (g_currentView == "All") {
            if (display != "") {
                rptype = display;
                this.server_getAllData(action, period, startDate, endDate, betCategory, display, 1, this.row, rptype)
            } else {                      
                this.server_getAllData(action, period, startDate, endDate, betCategory, display, 1, this.row, rptype)
            }
        } else { }
        //}
    },     

    ui_doSearchPL: function () {
        var action = $('#txtaction').val();
        var rptype = $('#txtrpType').val();
        var period = $("#selPeriod") ? $("#selPeriod").val() : "";
        var startDate = "";
        var endDate = "";

        //if (this.ValidateInput()) {
        $("#dvAccountHistory").html(this.getLoadingTableHTML());
        if ($("#rdoCustom").prop('checked')) {
            period = "DateRange";
            startDate = $("#sd").val();
            endDate = $("#ed").val()
        }

        if ($("#rdoCustom").prop('checked') && ($("#sd").val() == "" || $("#ed").val() == "")) {
            alert('You must specify start date, end date !');
            return;
        }

        this.server_getAllDataBL(action, period, startDate, endDate, rptype);
    },

    ui_doDisplayChange: function (displayType) {
        var action = $('#txtaction').val();
        var rptype = $('#txtrpType').val();
        this.server_getAllData(action, g_periodType, g_startDate, g_endDate, g_betCategory, displayType, 1, this.row, rptype)
    },

    ui_doBetCatChange: function (betCategory) {
        var action = $('#txtaction').val();
        var rptype = $('#txtrpType').val();
        this.server_getAllData(action, g_periodType, g_startDate, g_endDate, betCategory, g_displayType, 1, this.row, rptype)
    },

    ui_doPageChange: function (pageNo) {
        var action = $('#txtaction').val();
        //var rptype = $('#txtrpType').val();
        this.server_getAllData(action, g_periodType, g_startDate, g_endDate, g_betCategory, g_displayType, pageNo, this.row, g_displayType)
    },

    ui_selPeriod: function () {
        Common.SetSelectedRadio("rdoPredefined", "rdoCustom");
    },

    ui_popStartDate: function () {
        Common.SetSelectedRadio("rdoCustom", "rdoPredefined");
        //popUpCalendar(document.getElementById('txtStartDate'), document.getElementById('txtStartDate'), 'yyyy-mm-dd', -1, -1)
    },

    ui_popEndDate: function () {
        Common.SetSelectedRadio("rdoCustom", "rdoPredefined");
        //popUpCalendar(document.getElementById('txtEndDate'), document.getElementById('txtEndDate'), 'yyyy-mm-dd', -1, -1)
    },

    server_getAllData: function (action, periodType, startDate, endDate, betCategory, displayType, pageNo, noOfPages, rptype) {
        //if (this.BeginProcessing()) {
        var surl = this.Url + '';
        if (pageNo == "") {
            pageNo = this.page;
        }
        if (noOfPages == null || noOfPages == "") {
            noOfPages = this.row;
        }
        $.ajax({
            url: this.Url + 'ajax/' + action,
            type: 'GET',
            data: { pr: periodType, sd: startDate, ed: endDate, bcate: betCategory, bdis: displayType, pNo: pageNo, row: noOfPages, type: rptype },
            success: function (result) {
                AccountHistory.server_receiveAllData(result, rptype);
            }
        });
        //}
    },

    server_getAllDataBL: function (action, periodType, startDate, endDate, rptype) {
        var surl = this.Url + '';

        $.ajax({
            url: this.Url + 'ajax/' + action,
            type: 'GET',
            data: { pr: periodType, sd: startDate, ed: endDate, type: rptype },
            success: function (result) {
                AccountHistory.server_receiveAllData(result, rptype);
            }
        });
    },

    server_receiveAllData: function (data, action) {
        try {
            switch (action) {
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "betpl":
                    $("#dv-commission").html(data);
                    break;
                case "13":
                case "14":
                case "17":
                case "18":
                case "19":
                    $('#result').html(data);
                    break;
                case "FBSettlement":
                case "Settlement":
                    historyHTML += html_getAllSettlementRow(arrAllData[i], numDec, oddsFormat);
                    break;
            }
            //betex247.EndProcessing();
        } catch (e) {

        }
    },

    html_getAllSettlementRow: function (arrRowData, numDec, oddsFormat) {
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

    html_getAllOthersRow: function (arrRowData, numDec) {
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

    html_getAllNoRows: function () {
        var rowHTML = $("#taAllNoRows").val();
        return rowHTML
    },

    BeginProcessing: function () {
        if (!bMemberRequestProcessing) {
            bMemberRequestProcessing = true;
            return true
        } else { return false }
    },

    EndProcessing: function () { bMemberRequestProcessing = false; },

    ValidateInput: function () {
        if ($("#rdoCustom").prop('checked')) return this.ValidateDateRange($("#txtStartDate").val(), 6);
        return true
    },

    ValidateDateRange: function (startDate, lastNumOfMonth) {
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

    getLoadingTableHTML: function () {
        return loadingTableHTML = $("#taLoadingTable").val();
    }
}