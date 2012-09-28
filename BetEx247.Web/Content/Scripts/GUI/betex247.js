betex247 = {
    Url: '',
    init: function () { },
    //#region common function
    getallsport: function () {
//        var key = "sportlist";
//        if (checkCookie(key)) {
//            var jData = $.parseJSON(getCookie(key));
//            var output = betex247.bindsport(jData);
//            $("#left_column").html(output);
//            //hide all league
//            $(".navsports").hide();
//            //toggle the componenet with class msg_body
//            $(".sport-heading").click(function () {
//                $(".navsports").hide();
//                $(this).next(".navsports").slideToggle(250);
//            });
//        }

        $.ajax({
            type: "GET",
            url: this.Url + 'common/getAllSport', // {"content"="Hello!"}
            dataType: 'json',
            cache: true,
            success: function (data) {
//                setCookie(key, data, 1);
                var output = betex247.bindsport(data);
                $("#left_column").html(output);
                //hide all league
                $(".navsports").hide();
                //toggle the componenet with class msg_body
                $(".sport-heading").click(function () {
                    $(".navsports").hide();
                    $(this).next(".navsports").slideToggle(250);
                });
            }
        });
    },

    getsport: function (id) {
        $.ajax({
            type: "GET",
            url: this.Url + 'common/getSport', // {"content"="Hello!"}
            dataType: 'json',
            cache: true,
            success: function (data) {
                var outPut = betex247.bindbet(data);
                $("div.highlights-content").html(outPut);
            }
        });
    },
    //#endregion

    //#region support function
    bindsport: function (data) {
        var sb = new StringBuilder();
        sb.append("<div>");
        sb.append("    <div class=\"ChooseSportsHeader\">");
        sb.append("        <span class=\"orange\">Sports offer</span>");
        sb.append("    </div>");
        sb.append("</div>");
        var obj = data;
        for (var i = 0; i < obj.length; i++) {
            var twt = obj[i];
            if (twt.Name.length > 0) {
                var countLeague = twt.Leagues.length;
                var activeCountLeague = 0;
                if (countLeague > 0) {
                    for (var j = 0; j < countLeague; j++) {
                        var twtLeague = twt.Leagues[j];
                        if (twtLeague.Matches != null) {
                            activeCountLeague++;
                        }
                    }
                    sb.append(this.addStringEndLeague());
                }

                sb.append(this.addStringSport(twt.Name, activeCountLeague));
                if (countLeague > 0) {
                    sb.append(this.addStringStartLeague());
                    for (var j = 0; j < countLeague; j++) {
                        var twtLeague = twt.Leagues[j];
                        if (twtLeague.Matches != null) {
                            sb.append(this.addStringLeague("#", twtLeague.Name));
                        }
                    }
                    sb.append(this.addStringEndLeague());
                }
            }
        }
        return sb.toString();
    },

    addStringSport: function (sportname, leagueCount) {
        var className = "";
        switch (sportname) {
            case "Soccer":
                className = "sports-soccer";
                break;
            case "Baseball":
                className = "sports-baseball";
                break;
            case "Tennis":
                className = "sports-tennis";
                break;
            case "Horse Racing":
                className = "sports-horse";
                break;
            case "American Football":
                className = "sports-usfootball";
                break;
            case "Cricket":
                className = "sports-cricket";
                break;
            case "sports-hockey":
                className = "Ice Hockey";
                break;
            case "Handball":
                className = "sports-handball";
                break;
            case "Volleyball":
                className = "sports-volleyball";
                break;
            case "Rugby League":
                className = "sports-rugby";
                break;
            case "Rugby Union":
                className = "sports-rugby";
                break;
            default:
                className = "sports-soccer";
                break;
        }
        var sb = new StringBuilder();
        sb.append("<div class=\"sport-heading\">");
        sb.append("<a href=\"#\" class=\"sportlnk\">");
        sb.append("    <div style=\"padding: 6px 0px\">");
        sb.append("        <span class=\"" + className + "\"></span>" + sportname + " <span style=\"float: right; margin-right: 4px;\">" + leagueCount + "</span>");
        sb.append("    </div>");
        sb.append("</a></div>");
        return sb.toString();
    },

    addStringStartLeague: function () {
        var sb = new StringBuilder();
        sb.append("<div class=\"navsports\">");
        sb.append("    <ul>");
        return sb.toString();
    },

    addStringEndLeague: function () {
        var sb = new StringBuilder();
        sb.append("    </ul>");
        sb.append("</div>");
        return sb.toString();
    },

    addStringLeague: function (url, leagueName) {
        var sb = new StringBuilder();
        sb.append("        <li><a href=\"" + url + "\">");
        sb.append("            <div style=\"padding: 3px 0px 3px 4px;\">");
        sb.append("                " + leagueName + "</div>");
        sb.append("        </a></li>");
        return sb.toString();
    },

    bindbet: function (data) {
        var obj = data;
        var sb = new StringBuilder();
        if (obj.Name.length > 0) {
            sb.append(this.betTitle(obj.Name));

            var countLeague = obj.Leagues.length;
            if (countLeague > 0) {
                for (var j = 0; j < countLeague; j++) {
                    var twtLeague = obj.Leagues[j];
                    var leagueName = twtLeague.Name;
                    var countMatch = twtLeague.Matches != null ? twtLeague.Matches.length : 0;

                    if (countMatch > 0) {
                        sb.append(this.betdetailStart());
                        //test just get 5 record. 
                        var rowControl = countMatch > 3 ? 3 : countMatch;
                        for (var i = 0; i < rowControl; i++) {
                            var twtMatch = twtLeague.Matches[i];
                            if (twtMatch.Periods.length > 0 && (twtMatch.Periods[0].Spreads != null || twtMatch.Periods[0].Totals != null || twtMatch.Periods[0].MoneyLines != null)) {
                                sb.append(this.betdetail(leagueName, twtMatch));
                            }
                        }
                        sb.append(this.betdettailEnd());
                    }

                }
            }
        }
        return sb.toString();
    },

    betTitle: function (sportName) {
        var sb = new StringBuilder();

        sb.append("<div class=\"sports-cat-heading-underline\">");
        sb.append("<div class=\"title\">" + sportName + "</div>");
        sb.append("<div class=\"all\">");
        sb.append("    <table style=\"border-spacing: 2px; border-collapse: separate; width: 100%;\">");
        sb.append("        <tr>");
        sb.append("            <td height=\"26\" width=\"33%\">");
        sb.append("                <table width=\"100%\" cellpadding=\"1\" cellspacing=\"1\" border=\"1\" bgcolor=\"#000000\">");
        sb.append("                    <tr>");
        sb.append("                        <td colspan=\"2\" align=\"center\">Home</td>");
        sb.append("                    </tr>");
        sb.append("                    <tr>");
        sb.append("                        <td width=\"50%\" align=\"center\">Back</td>");
        sb.append("                        <td width=\"50%\" align=\"center\">Lay</td>");
        sb.append("                    </tr>");
        sb.append("                </table>");
        sb.append("            </td>");
        sb.append("            <td height=\"26\" width=\"34%\">");
        sb.append("                <table width=\"100%\" cellpadding=\"2\" cellspacing=\"2\">");
        sb.append("                    <tr>");
        sb.append("                        <td colspan=\"2\" align=\"center\">Draw</td>");
        sb.append("                    </tr>");
        sb.append("                    <tr>");
        sb.append("                        <td width=\"50%\" align=\"center\">Back</td>");
        sb.append("                        <td width=\"50%\" align=\"center\">Lay</td>");
        sb.append("                    </tr>");
        sb.append("                </table>");
        sb.append("           </td>");
        sb.append("            <td height=\"26\" width=\"33%\">");
        sb.append("                <table width=\"100%\" cellpadding=\"2\" cellspacing=\"2\">");
        sb.append("                    <tr>");
        sb.append("                        <td colspan=\"2\" align=\"center\">Away</td>");
        sb.append("                    </tr>");
        sb.append("                    <tr>");
        sb.append("                        <td width=\"50%\" align=\"center\">Back</td>");
        sb.append("                        <td width=\"50%\" align=\"center\">Lay</td>");
        sb.append("                    </tr>");
        sb.append("                </table>");
        sb.append("            </td>");
        sb.append("        </tr>");
        sb.append("    </table>");
        sb.append("</div>");
        sb.append("</div>");

        return sb.toString();
    },

    betdetailStart: function () {
        var sb = new StringBuilder();

        sb.append("<div class=\"sports-cat-content\">");
        sb.append("<table class=\"sports-cat-tbl\">");

        return sb.toString();
    },

    betdettailEnd: function () {

        var sb = new StringBuilder();
        sb.append("</table></div>");

        return sb.toString();
    },

    betdetail: function (leagueName, jData) {
        var sb = new StringBuilder();

        sb.append("<tr>");

        sb.append(this.bindMatch(leagueName, jData.HomeTeam, jData.AwayTeam, jData.StartDateTime));

        sb.append("    <td width=\"250\" style=\"vertical-align: top;\" align=\"center\">");
        sb.append("        <table style=\"border-spacing: 2px; border-collapse: separate; width: 100%;\">");
        sb.append("            <tr>");

        var twtSpread;
        var twtTotal;
        var twtMoneyLine;

        if (jData.Periods[0].Spreads != null) {
            twtSpread = jData.Periods[0].Spreads[0];
            sb.append(this.bindOdd(twtSpread.HomePrice, twtSpread.AwayPrice));
        }
        if (jData.Periods[0].Totals != null) {
            twtTotal = jData.Periods[0].Totals[0];
            sb.append(this.bindOdd(twtTotal.OverPrice, twtTotal.UnderPrice));
        }
        if (jData.Periods[0].MoneyLines != null) {
            twtMoneyLine = jData.Periods[0].MoneyLines[0];
            sb.append(this.bindOdd(twtMoneyLine.HomePrice, twtMoneyLine.AwayPrice));
        }

        sb.append("            </tr>");
        sb.append("        </table>");
        sb.append("    </td>");
        sb.append("</tr>");

        return sb.toString();
    },

    bindMatch: function (leagueName, HomeTeam, AwayTeam, Time) {
        var sb = new StringBuilder();
        var d = Time.toDateFromAspNet();

        sb.append("    <td width=\"50\">" + d.format("h:MM") + "<br/>" + d.format("mmm d") + "</td>");  //21:00<br />18 Sep
        sb.append("    <td width=\"200\">");
        sb.append("        <a href=\".\" class=\"sports-cat-lnk\">");
        sb.append("            <div>" + leagueName + "<br />");
        sb.append("            <span class=\"match-title-bigger\">" + HomeTeam + " - " + AwayTeam + "</span></div>");
        sb.append("        </a>");
        sb.append("    </td>");

        return sb.toString();
    },

    bindOdd: function (HomeTeamOdd, AwayTeamOdd) {
        var sb = new StringBuilder();

        sb.append("                <td height=\"30\" width=\"33%\">");
        sb.append("                    <table width=\"100%\" cellpadding=\"2\" cellspacing=\"2\">");
        sb.append("                        <tr>");
        sb.append("                            <td width=\"50%\" align=\"center\">");
        sb.append("                                <div style=\"margin: 0 auto; padding: 0px 0px; width: auto;\">");
        sb.append("                                    <a href=\"mysegments.html\" class=\"odds-button-back\">" + HomeTeamOdd + "</a>");
        sb.append("                                </div>");
        sb.append("                            </td>");
        sb.append("                            <td width=\"50%\" align=\"center\">");
        sb.append("                                <div style=\"margin: 0 auto; padding: 0px 0px; width: auto;\">");
        sb.append("                                    <a href=\"mysegments.html\" class=\"odds-button-lay\">" + AwayTeamOdd + "</a>");
        sb.append("                                </div>");
        sb.append("                            </td>");
        sb.append("                        </tr>");
        sb.append("                    </table>");
        sb.append("                </td>");

        return sb.toString();
    }
    //#endregion
}