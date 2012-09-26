betex247 = {
    init: function () { },
    //#region common function
    getallsport: function () {
        $.get(
            'common/getAllSport', // home là controller và action là getInfo
            function (data) {
                var output = betex247.bindsport(data);
                $("#left_column").html(output);
                //hide all league
                $(".navsports").hide();
                //toggle the componenet with class msg_body
                $(".sport-heading").click(function () {
                    $(this).next(".navsports").slideToggle(250);
                });
            }
        )
    },

    getsport: function (id) {
        $.get(
            'common/getSport', // home là controller và action là getInfo
            function (data) {
                //alert(data.name);
            }
        )
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
                sb.append(this.addStringSport(twt.Name, countLeague));
                if (countLeague > 0) {
                    sb.append(this.addStringStartLeague());
                    for (var j = 0; j < countLeague; j++) {
                        var twtLeague = twt.Leagues[j];
                        sb.append(this.addStringLeague("#", twtLeague.Name));
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
    }
    //#endregion
}