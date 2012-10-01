var country_list = ["Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua &amp; Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas"
		, "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia &amp; Herzegovina", "Botswana", "Brazil", "British Virgin Islands"
		, "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Cape Verde", "Cayman Islands", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica"
		, "Cote D Ivoire", "Croatia", "Cruise Ship", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea"
		, "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana"
		, "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India"
		, "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kuwait", "Kyrgyz Republic", "Laos", "Latvia"
		, "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mauritania"
		, "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Namibia", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia"
		, "New Zealand", "Nicaragua", "Niger", "Nigeria", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal"
		, "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre &amp; Miquelon", "Samoa", "San Marino", "Satellite", "Saudi Arabia", "Senegal", "Serbia", "Seychelles"
		, "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "South Africa", "South Korea", "Spain", "Sri Lanka", "St Kitts &amp; Nevis", "St Lucia", "St Vincent", "St. Lucia", "Sudan"
		, "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad &amp; Tobago", "Tunisia"
		, "Turkey", "Turkmenistan", "Turks &amp; Caicos", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Virgin Islands (US)"
		, "Yemen", "Zambia", "Zimbabwe"];

var national_list = "World cup,Asian cup";
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
                $(".navsports").slideUp(500);
                $(".navsportssub").slideUp(500);
                //toggle the componenet with class msg_body
                $(".sport-heading").click(function () {
                    $(".navsports").hide();
                    $(this).next(".navsports").slideToggle(500);
                    $('html,body').animate({ scrollTop: $(this).offset().top - 200}, 1000);
                });

                $(".league-heading").click(function () {
                    $(".navsportssub").slideUp(500);
                    $(this).children(".navsportssub").slideToggle(500);
                    $('html,body').animate({ scrollTop: $(this).offset().top - 200 }, 1000);
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
            //check sport name
            if (twt.Name.length > 0) {
                var listCountry = "";
                var listNational = "";
                var countLeague = twt.Leagues.length;
                var activeCountLeague = 0;
                //count active league on sport
                if (countLeague > 0) {
                    for (var j = 0; j < countLeague; j++) {
                        var twtLeague = twt.Leagues[j];
                        if (twtLeague.Matches != null) {
                            activeCountLeague++;
                        }
                    }
                }
                //add sport name to list menu
                sb.append(this.addStringSport(twt.Name, activeCountLeague));
                if (countLeague > 0) {
                    listCountry = this.bindLeagueCountry(twt.Leagues);
                    listNational = this.bindLeagueNational(twt.Leagues);
                    //add start div league
                    sb.append(this.addStringStartLeague());
                    //add National league
                    if (listNational.length > 1) {
                        sb.append(this.addStringLeague("#", "National"));
                        for (var j = 0; j < countLeague; j++) {
                            sb.append(this.addStringStartSubLeague());
                            var twtLeague = twt.Leagues[j];
                            if (twtLeague.Matches != null && listNational.indexOf(twtLeague.Name) > -1) {
                                sb.append(this.addStringSubLeague("#", twtLeague.Name));
                            }
                            sb.append(this.addStringEndSubLeague());
                            sb.append("</li>");
                        }
                    }
                    //add Country league
                    if (listCountry.length > 1) {
                        var arrList = listCountry.split(',');
                        for (var k = 0; k < arrList.length; k++) {
                            var sCountry = arrList[k];
                            if (sCountry.length > 0) {
                                sb.append(this.addStringLeague("#", sCountry));
                                sb.append(this.addStringStartSubLeague());
                                for (var j = 0; j < countLeague; j++) {
                                    var twtLeague = twt.Leagues[j];
                                    if (twtLeague.Matches != null) {
                                        if (twtLeague.Name.indexOf(sCountry) > -1) {
                                            sb.append(this.addStringSubLeague("#", twtLeague.Name));
                                        }
                                    }
                                }
                                sb.append(this.addStringEndSubLeague());
                                sb.append("</li>");
                            }
                        }
                    }
                    //add end div league
                    sb.append(this.addStringEndLeague());
                }
            }
        }
        return sb.toString();
    },

    bindLeagueCountry: function (jLeagueData) {
        var list = "";
        var countLeague = jLeagueData.length;
        if (countLeague > 0) {
            for (var i = 0; i < countLeague; i++) {
                var twtLeague = jLeagueData[i];
                if (twtLeague.Matches != null) {
                    var name = twtLeague.Name;
                    if (name.indexOf(national_list) < 0) {
                        var arrName = name.split(' ');
                        if (arrName.length > 0) {
                            if (list.indexOf(arrName[0]) < 0) {
                                list += arrName[0] + ",";
                            }
                        } else {
                            if (list.indexOf(name) < 0) {
                                list += name + ",";
                            }
                        }
                    }
                }
            }
        }
        return list;
    },

    bindLeagueNational: function (jLeagueData) {
        var list = "";
        var countLeague = jLeagueData.length;
        if (countLeague > 0) {
            for (var i = 0; i < countLeague; i++) {
                var twtLeague = jLeagueData[i];
                if (twtLeague.Matches != null) {
                    var name = twtLeague.Name;
                    if (national_list.indexOf(name) > -1) {
                        list += name + ",";
                    }
                }
            }
        }
        return list;
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
        sb.append("        <li class=\"league-heading\"><a href=\"" + url + "\">");
        sb.append("            <div style=\"padding: 3px 0px 3px 8px;\">+&nbsp;<b>");
        sb.append("                " + leagueName + "</b></div>");
        //sb.append("        </a></li>");
        sb.append("        </a>");
        return sb.toString();
    },

    addStringSubLeague: function (url, leagueName) {
        var sb = new StringBuilder();
        sb.append("        <li><a href=\"" + url + "\">");
        sb.append("            <div style=\"padding: 3px 0px 3px 26px;\">");
        sb.append("                <i>" + leagueName + "</i></div>");
        sb.append("        </a></li>");
        return sb.toString();
    },

    addStringStartSubLeague: function () {
        var sb = new StringBuilder();
        sb.append("<div class=\"navsportssub\">");
        sb.append("    <ul>");
        return sb.toString();
    },

    addStringEndSubLeague: function () {
        var sb = new StringBuilder();
        sb.append("    </ul>");
        sb.append("</div>");
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