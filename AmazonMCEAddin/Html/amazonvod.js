

//amazon 1
var boxw = "100%";
var boxh = "100%";

var ue_t0 = ue_t0 || +new Date();
var ue_id = '1GQFBYFG06YC31MX9QB6',
ue_sid = sessionID,
ue_mid = 'ATVPDKIKX0DER',
ue_url = '/gp/product/B0021NMEJY/ref=s9_al_bw_g318_ir02/uedata/176-1378021-8485108/Detail',
ue_furl = 'fls-na.amazon.com',
ue_pr = 0,
ue_navtiming = 0,
ue_rnt = 1,
ue_tm = 0,
ue_wn = 0;
var ueinit = (ueinit || 0) + 1;
var ue = { t0: ue_t0, id: ue_id, url: ue_url, a: "", b: "", e: "", ec: 0, h: {}, r: { ld: 0, oe: 0, ul: 0 }, s: 1, t: {}, sc: {}, iel: [] };
function uet(c, e, g) {
    var f = (new Date()).getTime();
    var a = !e && typeof g != "undefined";
    if (a) { return }
    if (c) {
        var d = e ? ues("t", e) || ues("t", e, {}) : ue.t;
        d[c] = f;
        for (var b in g) {
            ues(b, e, g[b])
        }
    }
    return f
}
function ues(b, c, d) {
    var e, a;
    if (b) {
        e = a = ue;
        if (c && c != e.id) {
            a = e.sc[c];
            if (!a) {
                a = {};
                d ? (e.sc[c] = a) : a
            }
        }
        e = d ? (a[b] = d) : a[b]
    }
    return e
}
function ueh(e, f, d, b, a) {
    var c = "on" + d;
    var g = f[c];
    if (typeof (g) == "function") {
        if (e) {
            ue.h[e] = g
        }
    }
    else
    {
        g = function () { }
    }
    f[c] = a ? function (h) {
        b(h); g(h)
    } : function (h) {
        g(h); b(h)
    };
    f[c].isUeh = 1
}
function uex(i, e, h) {
    function d(G, E) {
        var C = [G], w = 0, D = {};
        if (E) {
            C.push("m=1");
            D[E] = 1
        }
        else {
            D = ue.sc
        }
        var u;
        var z;
        for (var v in D) {
            var x = ues("wb", v), B = ues("t", v) || {}, A = ues("t0", v) || ue.t0; if (E || x == 2) {
                var F = x ? w++ : "";
                C.push("sc" + F + "=" + v);
                for (var y in B) {
                    if (y.length <= 3 && B[y]) {
                        C.push(y + F + "=" + (B[y] - A))
                    }
                }
                C.push("t" + F + "=" + B[i]);
                if (ues("ctb", v) || ues("wb", v)) {
                    u = 1
                }
                x == 2 && (z = 1) && delete ue.sc[v]
            }
        }
        if (!f && u) {
            C.push("ctb=1")
        }
        if (!E) {
            return z ? C.join("&") : ""
        }
        return C.join("&")
    }
    function k(v, u) {
        if (v == "") {
            return
        }
        var w = new Image();
        if (ue.b) {
            w.onload = function () {
                if (ue.b == "") {
                    return
                }
                var x = ue.b; ue.b = ""; k(x)
            }
        }
        ue.iel.push(w);
        w.src = v;
        if (window.ue_err && !ue_err.ts) {
            ue_err.startTimer()
        }
    }
    function q(u) {
        var w = u.timing;
        if (w) {
            ue.t.na_ = w.navigationStart;
            ue.t.ul_ = w.unloadEventStart;
            ue.t._ul = w.unloadEventEnd;
            ue.t.rd_ = w.redirectStart;
            ue.t._rd = w.redirectEnd;
            ue.t.fe_ = w.fetchStart;
            ue.t.lk_ = w.domainLookupStart;
            ue.t._lk = w.domainLookupEnd;
            ue.t.co_ = w.connectStart;
            ue.t._co = w.connectEnd;
            ue.t.sc_ = w.secureConnectionStart;
            ue.t.rq_ = w.requestStart;
            ue.t.rs_ = w.responseStart;
            ue.t._rs = w.responseEnd;
            ue.t.dl_ = w.domLoading;
            ue.t.di_ = w.domInteractive;
            ue.t.de_ = w.domContentLoadedEventStart;
            ue.t._de = w.domContentLoadedEventEnd;
            ue.t._dc = w.domComplete;
            ue.t.ld_ = w.loadEventStart;
            ue.t._ld = w.loadEventEnd
        }
        var v = u.navigation;
        if (v) {
            ue.t.ty = v.type + ue.t0; ue.t.rc = v.redirectCount + ue.t0
        }
    }
    var t = !e && typeof h != "undefined";
    if (t) {
        return
    }
    for (var a in h) {
        ues(a, e, h[a])
    }
    uet("pc", e, h);
    var m = ues("id", e) || ue.id; var g = ue.url + "?" + i + "&v=19&id=" + m; var f = ues("ctb", e) || ues("wb", e);
    if (f) {
        g += "&ctb=" + f
    }
    if (ueinit > 1) {
        g += "&ic=" + ueinit
    }
    var p;
    if (document.ue_backdetect && document.ue_backdetect.ue_back) {
        p = document.ue_backdetect.ue_back;
        if (p.value > 1) {
            g += "&bf=" + (p.value - 1)
        }
    }
    if (ue._fi && i == "at" && (!e || e == m)) {
        g += ue._fi()
    }
    var b;
    if (i == "ld" && (!e || e == m)) {
        if (!window.ue_tm) {
            if (window.onbeforeunload && window.onbeforeunload.isUeh) {
                window.onbeforeunload = null
            }
        }
        else {
            var s = g; setInterval(function () {
                k(d(s, null))
            }, ue_tm)
        }
        if (p) {
            p.value++
        }
        if (window._uess) {
            b = _uess()
        }
        var o = window.performance || window.webkitPerformance;
        if (window.ue_navtiming && o && o.timing) {
            ues("ctb", m, "1");
            if (ue_navtiming == 1) {
                ue.t.tc = o.timing.fetchStart
            }
            else {
                if (ue_navtiming == 3) {
                    ue.t.tc = o.timing.navigationStart
                }
                else {
                    if (ue_navtiming == 4) {
                        ue.t.tc = o.timing.requestStart
                    }
                }
            }
        }
        if (window.ue_rnt && o) {
            q(o)
        }
    }
    uet(i, e, h);
    var l = (i == "ld" && e && ues("wb", e));
    if (l) {
        ues("wb", e, 2)
    }
    var n = 1;
    var c = 0;
    for (var j in ue.sc) {
        if (!window.ue_tm) {
            if (ues("wb", j) == 1) {
                n = 0; break
            }
        }
        else {
            if (ues("wb", j) == 2) { c++ }
        }
    }
    window.ue_tm && (n = c >= ue_wn ? 1 : 0);
    if (l) {
        if (ue.s != 0 || !n) {
            return
        }
        g = d(g, null)
    }
    else {
        if (n || i == "ul") {
            var r = d(g, null);
            if (r != g) {
                ue.b = r
            }
        }
        if (b) {
            g += b
        }
        g = d(g, e || ue.id)
    }
    if (!l) {
        ue.s = 0;
        if (!window.ue_wl_jserr && ue.e) {
            g += "&ec=" + ue.ec + ue.e; ue.e = ""; ue.ec = 0
        }
        if (window.ue_wl_jserr && window.ue_err && ue_err.ec > 0) {
            g += "&ec=" + ue_err.ec
        }
        ues("t", e, {})
    }
    ue.a = g;
    k(g, i)
}
function uei() {
    var c = ue.r;
    function b(e) {
        return function () {
            if (!c[e]) {
                c[e] = 1; uex(e)
            }
        }
    }
    window.onLd = b("ld");
    window.onLdEnd = b("ld");
    var a = {
        beforeunload: b("ul"), error: function (j, h, i) {
            ue.ec++;
            if (!ue.r.oe) {
                ue.r.oe = 1;
                ue.e += "&em=" + escape(j) + "&eu=" + escape(h) + "&el=" + i
            }
            return false
        },
        stop: function () {
            uex("os")
        }
    };
    for (var d in a) {
        ueh(0, window, d, a[d])
    }
    if (window.addEventListener) {
        window.addEventListener("load", window.onLd, false)
    }
    else {
        if (window.attachEvent) {
            window.attachEvent("onload", window.onLd)
        }
    }
    ue._uep = function () {
        new Image().src = (window.ue_md ? ue_md : "http://uedata.amazon.com/uedata/?tp=") + (+new Date)
    };
    if (window.ue_pr && (ue_pr == 2 || ue_pr == 4)) {
        ue._uep()
    }
    if (window.ue_wl_jserr) {
        window.onerror = window.ueLogError
    }
    uet("ue")
}
uei();
ue.rid = ue_id;
ue.sid = ue_sid;
ue.mid = ue_mid;
ue.furl = ue_furl;
ue.lr = [];
ue.log = function (c, b, a) {
    if (ue.lr.length == 500) {
        return
    }
    ue.lr.push(["l", c, b, a, ue.d(), ue.rid])
};
ue.reset = function (b, a) {
    if (!b) {
        return
    }
    window.ue_cel && ue_cel.reset(); ue.t0 = ue.d(1); ue.rid = b
};
ue.d = function (a) {
    return +new Date - (a ? 0 : ue.t0)
};


//amazon 2
var old_url = "http://z-ecx.images-amazon.com/images/G/01/digital/video/javascript/AC_OETags_V1._V221005236_.js";
amznJQ.declareAvailable('ATVFlashPlayerJS');

function hasRequestedPlayerVersion() {
    return DetectFlashVer(10, 1, 0, 0);
}
function removePlayer() {
    jQuery("#streaming_container").children().remove();
}
function createPlayer() {
    var hasProductInstall = DetectFlashVer(6, 0, 65);

    if (hasRequestedPlayerVersion()) {
        document.getElementById("streamBoxTable").style.height = boxh;
        document.getElementById("streaming_container").style.height = boxh;
        document.getElementById("streaming_container").style.width = boxw;
        flashvars = getflashvars();
        AC_FL_RunContent(
            'src', player_url,
            'width', boxw,
            'height', boxh,
            'align', 'middle',
            'id', 'streaming',
            'quality', 'high',
            'bgcolor', '#000000',
            'name', 'streaming',
            'allowFullScreen', 'true',
            'allowScriptAccess', 'always',
            'type', 'application/x-shockwave-flash',
            'pluginspage', 'http://www.adobe.com/go/getflashplayer',
            'divID', 'streaming_container',
            'flashVars', flashvars
             )
    }
    else if (hasProductInstall) {
        document.getElementById("upgradeFlashBox").style.display = "block";
        sendMetric('UpgradeFlash');
    }
    else {
        document.getElementById("noFlashBox").style.display = "block";
        sendMetric('NoFlash');
    }
}
function watchFromBeginning() {
    if (typeof
        nJQ !== 'undefined') {
        amznJQ.available('ATVFlashPlayerJS', function () {


            jQuery("#streaming_container").children().remove();


            if (hasRequestedPlayerVersion()) {
                flashvars = getflashvars();

                AC_FL_RunContent(
'src', player_url,
'width', boxw,
'height', boxw,
'align', 'middle',
'id', 'streaming',
'quality', 'high',
'bgcolor', '#000000',
'name', 'streaming',
'allowFullScreen', 'true',
'allowScriptAccess', 'always',
'type', 'application/x-shockwave-flash',
'pluginspage', 'http://www.adobe.com/go/getflashplayer',
'flashVars', flashvars + '&setTimeCode=0',
'divID', 'streaming_container'
);
            }
        });
    }
    return watchNow();
}


function avodUpgradeFlash() {
    amznJQ.onReady('ATVFlashPlayerJS', function () {
        document.getElementById("upgradeFlashBox").style.display = "none";

        var MMPlayerType = (is_IE == true) ? "ActiveX" : "PlugIn";
        var MMredirectURL = window.location;
        document.getElementById("streamBoxTable").style.height = "100%";
        document.getElementById("streaming_container").style.height = "100%";
        document.getElementById("streaming_container").style.width = "100%";

        AC_FL_RunContent(
    'src', 'http://g-ecx.images-amazon.com/images/G/01/digital/video/streaming/playerProductInstall._V12750109_.swf',
    'FlashVars', 'MMredirectURL=' + MMredirectURL + '&MMplayerType=' + MMPlayerType,
    'width', boxw,
    'height', boxh,
    'align', 'middle',
    'id', 'streaming',
    'quality', 'high',
    'bgcolor', '#ffffff',
    'name', 'streaming',
    'allowFullScreen', 'true',
    'allowScriptAccess', 'always',
    'type', 'application/x-shockwave-flash',
    'pluginspage', 'http://www.adobe.com/go/getflashplayer',
    'divID', 'streaming_container'
);

        if (typeof amznJQ !== 'undefined') {
            amznJQ.available('jQuery', function () {
                jQuery("#noFlashBox").css("display", "none");
            });
        }
    });
}

amznJQ.onReady('ATVFlashPlayerJS', function () {
    window.onbeforeunload = function (e) {
        var client = getSwfClient();

        if (client && client.exitVideoPlayer) {
            var closingCalls = client.exitVideoPlayer();
        }

        if (closingCalls && g_exitScreeningRoomCalled == false) {
            for (var i = 0; i < closingCalls.length; i++) {


                var action = "";
                if (closingCalls[i].indexOf("UpdateStream")) {
                    action = "updateStream";
                } else if (closingCalls[i].indexOf("ReportStreamingSessionEvent")) {
                    action = "reportStreamingSessionEvent";
                }

                if (action != "" && closingCalls[i].indexOf("?") != -1) {
                    jQuery.ajax({ url: "/gp/video/atv-ps-proxy.html?action=" + action + "&" + closingCalls[i].substr(closingCalls[i].indexOf("?") + 1), async: false });
                }

            }
            g_exitScreeningRoomCalled = true;
        }
    };

});


var g_exitScreeningRoomCalled = false;
var avodSwfUrl = player_url;




if (typeof amznJQ !== 'undefined') {
    amznJQ.onReady("jQuery", function () {
        jQuery(".prod-synopsis").click(function () {
            var synopsis = jQuery(this);
            var expander = synopsis.find(".synopsis-expand");

            if (expander != null) {
                var displayVal = expander.css("display");
                if (displayVal != null) {
                    if (displayVal == "none") {
                        expander.css("display", "inline");
                        synopsis.find(".synopsis-hidden").css("display", "none");
                    } else if (displayVal == "inline") {
                        expander.css("display", "none");
                        synopsis.find(".synopsis-hidden").css("display", "inline");
                    }
                }
            }
        });

        jQuery(".avod-preview-btn").click(function () {
            return watchPreview();
        });


    });

    var FlashQueue = function () {
        var available = false;
        var queue = [];

        var getPlayer = function () {
            if (navigator.appName.indexOf("Microsoft") != -1) {
                return window['streaming'];
            } else {
                return document['streaming'];
            }
        }

        var call = function (fStr) {
            var p = getPlayer();
            var success = false;
            if (p && typeof p[fStr] === 'function') {
                try {
                    p[fStr]();
                    success = true;
                } catch (e) { }
            }

            return success;
        }

        this.queue = function (fStr) {
            if (available === true) {
                if (!call(fStr)) {
                    available = false;
                    queue.unshift(fStr);
                }
            } else {
                queue.push(fStr);
            }
        }

        this.setAvailable = function () {
            var fStr = queue.shift();
            while (typeof fStr !== 'undefined') {
                if (!call(fStr)) {
                    available = false;
                    queue.unshift(fStr);
                    return;
                }
                fStr = queue.shift();
            }
            available = true;
        }

        this.setUnavailable = function () {
            available = false;
        }

        this.length = function () {
            return queue.length;
        }
        this.isAvailable = function () {
            return available;
        }
    }

    var playerQ = new FlashQueue();
    var rightsQ = new FlashQueue();
    var controlsQ = new FlashQueue();

    function play() {
        controlsQ.queue("avodPlay");
    }

    function stop() {
        controlsQ.queue("avodStop");
    }

    function pause() {
        controlsQ.queue("avodPause");
    }

    function playPause() {
        controlsQ.queue("avodPlayPause");
    }


    function startPlayback() {
        rightsQ.queue("watchNow");
    }

    function watchOrShowFlashWarning(watchFunction) {
        amznJQ.available('ATVFlashPlayerJS', function () {
            var hasOldVersion = DetectFlashVer(6, 0, 65);
            var hasRequestedVersion = DetectFlashVer(10, 1, 0, 0);
            if (hasRequestedVersion) {
                watchFunction();
            } else if (hasOldVersion) {
                showAvodAjaxDialog({
                    text: "      <p>You do not have a supported version of Adobe Flash, a requirement for      watching Amazon Instant Videos in your browser      (<a href=\"/gp/help/customer/display.html/ref=hp_3748_aivsysreq?nodeId=3748&#system\">learn  more</a>).</p><p>By clicking the button below,      you will upgrade your Flash Player so that you can instantly watch this and thousands       of your other favorite movies and TV shows in your browser.</p><br /><br />          <center><div class=\"ap_custom_close\" onclick=\"showPlayer(); avodUpgradeFlash();\">          <table class=\"avod-spritebox atv-new-flash-btn\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">  <tr height=\"19\">    <td class=\"content\" style=\"vertical-align: bottom;  ; background: url(http://g-ecx.images-amazon.com/images/G/01/digital/video/avod-1-5/dp-sprite-total._V156422041_.png) no-repeat -3px -153px; padding: 0px 4px 0px 11px;\">      Upgrade Your Flash Player    </td>    <td width=\"7\" style=\"vertical-align: bottom;  ; background: url(http://g-ecx.images-amazon.com/images/G/01/digital/video/avod-1-5/dp-sprite-total._V156422041_.png) no-repeat -396px -153px; line-height: 0.1px;\"><!-- --></td>  </tr>  <tr height=\"7\">    <td style=\"vertical-align: bottom;  ; background: url(http://g-ecx.images-amazon.com/images/G/01/digital/video/avod-1-5/dp-sprite-total._V156422041_.png) no-repeat -3px -174px\"></td>    <td width=\"7\" style=\"vertical-align: bottom;  ; background: url(http://g-ecx.images-amazon.com/images/G/01/digital/video/avod-1-5/dp-sprite-total._V156422041_.png) no-repeat -396px -174px; line-height: 0.1px;\"><!-- --></td>  </tr></table></div></center>",
                    title: "Your Flash Player is out of date"
                });
            } else {
                showAvodAjaxDialog({
                    text: "      <p>You do not have a supported version of Adobe Flash, a requirement for      watching Amazon Instant Videos in your browser      (<a href=\"/gp/help/customer/display.html/ref=hp_3748_aivsysreq?nodeId=3748&#system\">learn  more</a>).</p><p>Install the Adobe Flash       Player now so that you can instantly watch this and thousands of your other       favorite movies and TV shows in your browser. By clicking the button below, you       will be taken to Adobe's Flash installer website.</p><br /><br /><center><table class=\"avod-spritebox atv-new-flash-btn\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">  <tr height=\"19\">    <td class=\"content\" style=\"vertical-align: bottom;  ; background: url(http://g-ecx.images-amazon.com/images/G/01/digital/video/avod-1-5/dp-sprite-total._V156422041_.png) no-repeat -3px -153px; padding: 0px 4px 0px 11px;\">      <a href=\"http://get.adobe.com/flashplayer/\" style=\"text-decoration:none;color:#0C2149;padding-bottom:8px;\">Install Flash Player</a>    </td>    <td width=\"7\" style=\"vertical-align: bottom;  ; background: url(http://g-ecx.images-amazon.com/images/G/01/digital/video/avod-1-5/dp-sprite-total._V156422041_.png) no-repeat -396px -153px; line-height: 0.1px;\"><!-- --></td>  </tr>  <tr height=\"7\">    <td style=\"vertical-align: bottom;  ; background: url(http://g-ecx.images-amazon.com/images/G/01/digital/video/avod-1-5/dp-sprite-total._V156422041_.png) no-repeat -3px -174px\"></td>    <td width=\"7\" style=\"vertical-align: bottom;  ; background: url(http://g-ecx.images-amazon.com/images/G/01/digital/video/avod-1-5/dp-sprite-total._V156422041_.png) no-repeat -396px -174px; line-height: 0.1px;\"><!-- --></td>  </tr></table></center>",
                    title: "Flash Player is not installed"
                });
            }
        });
    }

    function autoPlay(new_asin) {
        target_asin = new_asin;
        ue_url = '/gp/product/' + target_asin + '/ref=s9_al_bw_g318_ir02/uedata/' + sessionID + '/Detail';
        createPlayer();
        watchNow();
    }

    function autoStop() {
        stop();
        removePlayer();
    }
    function watchNow() {

        watchOrShowFlashWarning(function () {
            showPlayer(); startPlayback();
        });
        return false;
    }

    function watchPreview() {

        watchOrShowFlashWarning(function () {



            showPlayerPreview(); startPlayback();
        });
        return false;
    }
    function updateRights() {
        controlsQ.setUnavailable();
        rightsQ.setUnavailable();

        playerQ.queue("reenqueue");
    }

    function playerControlsReady() {
        controlsQ.setAvailable();
    }

    function getPlayerControlsReady() {
        return (playerQ.isAvailable() && rightsQ.isAvailable()
                  && controlsQ.isAvailable());
    }

    function geoCheck() {
        if (typeof amznJQ !== 'undefined') {
            amznJQ.available('jQuery', function () {
                jQuery.ajax({
                    url: '/gp/video/detail/ajax/geo-check-swf.html',
                    dataType: 'json',
                    data: {},
                    success: function (data) {
                        var client = getSwfClient();

                        if (client && client.geoCheckHandler) {
                            client.geoCheckHandler(data);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        //var client = getSwfClient();

                        //if (client && client.geoCheckHandler) {
                        //    client.geoCheckHandler("true");
                        //}
                    }
                });
            });
        }
    }

    function rightsLoaded() {
        playerQ.setAvailable();
        rightsQ.setAvailable();
    }

    function isPlayerVisible() {
        if (typeof amznJQ !== 'undefined' && typeof jQuery !== 'undefined') {
            var videoPlayer = jQuery("#avod-video-player");
            var isVisible = videoPlayer.css("display") === "block";
            return isVisible;
        }

        return false;
    }

    function hideBuyBox() {
        if (typeof amznJQ !== 'undefined') {
            amznJQ.available('jQuery', function () {
                jQuery("#avod-buy-box-container").css("display", "none");
            });
        }
    }

    function showBuyBox() {
        if (typeof amznJQ !== 'undefined') {
            amznJQ.available('jQuery', function () {
                jQuery("#avod-buy-box-container").css("display", "block");
            });
        }
    }


    function showPlayer(useFullWindow) {
        if (typeof amznJQ !== 'undefined') {
            amznJQ.available('jQuery', function () {
                var videoPlayer = jQuery("#avod-video-player");

                g_exitScreeningRoomCalled = false;
                if (videoPlayer && videoPlayer.length) {
                    jQuery("#avod-atf-html").css("display", "none");
                    hideBuyBox();

                    if (useFullWindow != null && useFullWindow) {
                        rightsQ.setUnavailable();

                        document.body.style.overflow = "hidden";

                        videoPlayer.css("z-index", "9999")
                    .css("display", "block")
                    .css("position", "fixed")
                    .css("top", "0")
                    .css("left", "0")
                    .css("height", "100%")
                    .css("width", "100%");

                        jQuery("#streaming").css("height", "100%");
                    } else {



                        videoPlayer.css("display", "block")
                    .css("height", "100%")
                    .css("width", "100%")
                    .css("position", "static");
                    }
                }
            });
        }

    }

    function showPlayerPreview() {
        if (typeof amznJQ !== 'undefined') {
            amznJQ.available('jQuery', function () {
                var videoPlayer = jQuery("#avod-video-player");

                g_exitScreeningRoomCalled = false;
                if (videoPlayer && videoPlayer.length) {
                    jQuery("#prod-details").css("display", "none");
                    jQuery("#prod-img").css("display", "none");
                    jQuery("#avod-atf-html").css("display", "block");

                    videoPlayer
            .css("display", "block")
            .css("width", "100%")
            .css("height", "100%")
            .css("position", "static");
                    jQuery("#streaming").css("height", jQuery("#avod-main").height() || "410px");
                }
            });
        }
    }

    function shrinkFullWindowPlayer() {
        if (typeof amznJQ !== 'undefined') {
            amznJQ.available('jQuery', function () {
                var videoPlayer = jQuery("#avod-video-player");

                if (videoPlayer && videoPlayer.length && videoPlayer.css("position") === "fixed") {
                    rightsQ.setUnavailable();

                    document.body.style.overflow = "auto";

                    videoPlayer.css("display", "block")
         .css("position", "static")
         .css("height", "100%")
         .css("width", "100%")

                    startPlayback();
                }
            });
        }
    }

    function stopWatching() {
        if (typeof amznJQ !== 'undefined') {
            amznJQ.available('jQuery', function () {
                var client = getSwfClient();

                if (client && client.exitVideoPlayer) {
                    var closingCalls = client.exitVideoPlayer();
                }

                if (closingCalls && g_exitScreeningRoomCalled == false) {
                    for (i = 0; i < closingCalls.length; i++) {
                        var e = new Image();
                        e.src = closingCalls[i];
                    }
                    g_exitScreeningRoomCalled = true;
                }
            });
        }
    }

    function hidePlayer() {
        if (typeof amznJQ !== 'undefined') {
            amznJQ.available('jQuery', function () {
                var videoPlayer = jQuery("#avod-video-player");
                if (videoPlayer && videoPlayer.length) {
                    stopWatching();


                    videoPlayer.css("position", "absolute")
                   .css("left", "-3000px")
                   .css("top", "-3000px");


                    var container = jQuery("#avod-atf-html");
                    var img = jQuery("#prod-img");
                    var details = jQuery("#prod-details");

                    container.css("display", "block");
                    img.css("display", "block");
                    details.css("display", "block");
                    showBuyBox();
                }
            });
        }
    }

    function endOfVideoEvent() {
        if (typeof amznJQ !== 'undefined') {
            amznJQ.available('jQuery', function () {
            });
        }
    }
}

var is_IE = (navigator.appVersion.indexOf("MSIE") != -1) ? true : false;
var isWin = (navigator.appVersion.toLowerCase().indexOf("win") != -1) ? true : false;
var isOpera = (navigator.userAgent.indexOf("Opera") != -1) ? true : false;
function ControlVersion() {
    var version;
    var axo;
    var e;
    try {
        axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.7");
        version = axo.GetVariable("$version")
    }
    catch (e) { }
    if (!version) {
        try {
            axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.6");
            version = "WIN 6,0,21,0";
            axo.AllowScriptAccess = "always";
            version = axo.GetVariable("$version")
        } catch (e) { }
    } if (!version) {
        try {
            axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3");
            version = axo.GetVariable("$version")
        } catch (e) { }
    } if (!version) {
        try {
            axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3"); version = "WIN 3,0,18,0"
        } catch (e) { }
    } if (!version) {
        try {
            axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash"); version = "WIN 2,0,0,11"
        } catch (e) { version = -1 }
    } return version
}
function GetSwfVer() {
    var flashVer = -1;
    if (navigator.plugins != null && navigator.plugins.length > 0) {
        if (navigator.plugins["Shockwave Flash 2.0"] || navigator.plugins["Shockwave Flash"]) {
            var swVer2 = navigator.plugins["Shockwave Flash 2.0"] ? " 2.0" : "";
            var flashDescription = navigator.plugins["Shockwave Flash" + swVer2].description;
            var descArray = flashDescription.split(" ");
            var tempArrayMajor = descArray[2].split(".");
            var versionMajor = tempArrayMajor[0];
            var versionMinor = tempArrayMajor[1];
            if (descArray[3] != "") {
                tempArrayMinor = descArray[3].split("r")
            }
            else {
                tempArrayMinor = descArray[4].split("r")
            }
            var versionRevision = tempArrayMinor[1] > 0 ? tempArrayMinor[1] : 0;
            var flashVer = versionMajor + "." + versionMinor + "." + versionRevision
        }
    }
    else if (navigator.userAgent.toLowerCase().indexOf("webtv/2.6") != -1) flashVer = 4;
    else if (navigator.userAgent.toLowerCase().indexOf("webtv/2.5") != -1) flashVer = 3;
    else if (navigator.userAgent.toLowerCase().indexOf("webtv") != -1) flashVer = 2;
    else if (is_IE && isWin && !isOpera) { flashVer = ControlVersion() } return flashVer
}
function DetectFlashVer(reqMajorVer, reqMinorVer, reqRevision) {
    versionStr = GetSwfVer();
    if (versionStr == -1) {
        return false
    }
    else if (versionStr != 0) {
        if (is_IE && isWin && !isOpera) {
            tempArray = versionStr.split(" ");
            tempString = tempArray[1];
            versionArray = tempString.split(",")
        }
        else {
            versionArray = versionStr.split(".")
        }
        var versionMajor = versionArray[0];
        var versionMinor = versionArray[1];
        var versionRevision = versionArray[2];
        if (versionMajor > parseFloat(reqMajorVer)) {
            return true
        }
        else if (versionMajor == parseFloat(reqMajorVer)) {
            if (versionMinor > parseFloat(reqMinorVer)) return true;
            else if (versionMinor == parseFloat(reqMinorVer)) {
                if (versionRevision >= parseFloat(reqRevision)) return true
            }
        }
        return false
    }
}
function AC_AddExtension(src, ext) {
    if (src.indexOf('?') != -1) return src.replace(/\?/, ext + '?');
    else return src + ext
}
function AC_Generateobj(objAttrs, params, embedAttrs, dvAttrs) {
    var str = ''; if (is_IE && isWin && !isOpera) {
        str += '<object ';
        for (var i in objAttrs) str += i + '="' + objAttrs[i] + '" '; str += '>';
        for (var i in params) str += '<param name="' + i + '" value="' + params[i] + '" /> '
    }
    str += '<embed ';
    for (var i in embedAttrs) str += i + '="' + embedAttrs[i] + '" ';
    str += '> </embed>';
    if (is_IE && isWin && !isOpera) { str += '</object>' } document.getElementById(dvAttrs.divID).innerHTML = str
}
function AC_FL_RunContent() {
    var ret = AC_GetArgs(arguments, ".swf", "movie", "clsid:d27cdb6e-ae6d-11cf-96b8-444553540000", "application/x-shockwave-flash");
    AC_Generateobj(ret.objAttrs, ret.params, ret.embedAttrs, ret.dvAttrs)
}
function AC_SW_RunContent() {
    var ret = AC_GetArgs(arguments, ".dcr", "src", "clsid:166B1BCA-3F9C-11CF-8075-444553540000", null);
    AC_Generateobj(ret.objAttrs, ret.params, ret.embedAttrs, ret.dvAttrs)
}
function AC_GetArgs(args, ext, srcParamName, classid, mimeType) {
    var ret = new Object();
    ret.embedAttrs = new Object();
    ret.params = new Object();
    ret.objAttrs = new Object();
    ret.dvAttrs = new Object();
    for (var i = 0; i < args.length; i = i + 2) {
        var currArg = args[i].toLowerCase();
        switch (currArg) {
            case "divid":
                ret.dvAttrs[args[i]] = args[i + 1];
                break;
            case "classid":
                break;
            case "pluginspage":
                ret.embedAttrs[args[i]] = args[i + 1];
                break;
            case "src":
            case "movie":
                ret.embedAttrs["src"] = args[i + 1];
                ret.params[srcParamName] = args[i + 1];
                break;
            case "onafterupdate":
            case "onbeforeupdate":
            case "onblur":
            case "oncellchange":
            case "onclick":
            case "ondblClick":
            case "ondrag":
            case "ondragend":
            case "ondragenter":
            case "ondragleave":
            case "ondragover":
            case "ondrop":
            case "onfinish":
            case "onfocus":
            case "onhelp":
            case "onmousedown":
            case "onmouseup":
            case "onmouseover":
            case "onmousemove":
            case "onmouseout":
            case "onkeypress":
            case "onkeydown":
            case "onkeyup":
            case "onload":
            case "onlosecapture":
            case "onpropertychange":
            case "onreadystatechange":
            case "onrowsdelete":
            case "onrowenter":
            case "onrowexit":
            case "onrowsinserted":
            case "onstart":
            case "onscroll":
            case "onbeforeeditfocus":
            case "onactivate":
            case "onbeforedeactivate":
            case "ondeactivate":
            case "type":
            case "codebase":
                ret.objAttrs[args[i]] = args[i + 1];
                break;
            case "width":
            case "height":
            case "align":
            case "vspace":
            case "hspace":
            case "class":
            case "title":
            case "accesskey":
            case "name":
            case "id":
            case "tabindex":
                ret.embedAttrs[args[i]] = ret.objAttrs[args[i]] = args[i + 1];
                break;
            default:
                ret.embedAttrs[args[i]] = ret.params[args[i]] = args[i + 1]
        }
    }
    ret.objAttrs["classid"] = classid;
    if (mimeType) ret.embedAttrs["type"] = mimeType;
    return ret
}