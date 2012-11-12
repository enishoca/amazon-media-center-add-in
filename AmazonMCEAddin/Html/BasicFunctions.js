/////////////////////////////////////////////////////////////
// Identify page as enabled for Windows Media Center. This avoids a warning dialog to user.
function IsMCEEnabled() {
    return true
}

/////////////////////////////////////////////////////////////////
// Scaling elements for page resize 
function onScaleEvent(vScale) {
    try {
        body.style.zoom = vScale;
        // when page gets resized, reset positions and sizes of focusable elements
        setArray()
    }
    catch (e) {
        // ignore error
    }
}

var nFullScreen = 0
function backFromFullScreen() {
    if (nFullScreen == 0) {
        nFullScreen = 1
        return false
    }
    else return true
}


/////////////////////////////////////////////////////////////////
// determine which remote control key the user selected 
// and take appropriate action
function onRemoteEvent(keyChar) {
    /* Call optional "doOnFocus" function if needed; you can locate this function on the HTML page, and use it to tie some
   custom functionality to a remote-control keypress. If you want to stop onRemoteEvent from moving focus after that, 
   return true in the doOnFocus function to indicate that remote control keypress is already being handled in some other way */
    try {
        if (doOnFocus(keyChar) == true) {
            return true;
        }
    }
    catch (e) {
        // if doOnFocus function is not present on page, ignore error
    }
    try {
        // this switch tests to see which button on the remote is pressed
        switch (keyChar) {
            case 38:  // Up button selected
                changeFocus("up");
                break;

            case 40:  // Down button selected
                changeFocus("down");
                break;

            case 37:  // Left button selected
                changeFocus("left");
                break;

            case 39:  // Right button selected
                changeFocus("right");
                break;

            case 13:    // Enter button selected, execute link to content/page
                doSelect();
                return true;
                break;

            case 8:  // Keyboard Backspace selected
                return false;
                break;

            case 166:             // Remote Control Back button selected; Windows Media Center will already perform a Back
                return false;      // navigation when this is pressed, but this case can be used to add additional
                break;             //functionality to Back button

            case 33:    // Page up (plus) selected; page-up scrolling menu
                pageUpDown("up");
                return true;
                break;

            case 34:    // Page down (minus) selected; page-down scrolling menu
                pageUpDown("down");
                return true;
                break;

            default:
                return false;
                // ignore all other clicks
        }
    }
    catch (ex) {
        //ignore error
    }
    return true
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Set background color for Shared Viewport and MCE toolbars that appear when you move mouse
function setBGColor(Color) {
    try {
        window.external.MediaCenter.BGColor = Color
    }
    catch (e) {
        // not using Windows Media Center, or attribute not set
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/* When the user returns via the back button to an HTML page from a Windows Media Center view such as full screen video, the page 
receives an onload event. This is not a normal onload event, because the values for global variables are not reset. This can 
cause problems if the page's onload functionality depends on any global variables being in their default state. The following 
function, which you can call from the onload event, tests whether the values for global variables have returned to their default 
state. If they have not, you can skip the rest of your onload functionality.  */

var backFromMediaCenter = false
function continueOnloadFunctions() {
    if (backFromMediaCenter == false) // meaning variable has been reset to its default state
    {
        // set variable to true and return
        backFromMediaCenter = true
        return
    }
    /* otherwise, page variables have not been reset, so you do not want to continue to call
    your page initilizing functions. Return false */
    return false
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This function tests to see if you need to open a Viewport for media playback
function needViewport() {
    // if Shared or Custom Viewport is already visible, return false
    if (window.external.MediaCenter.SharedViewPort.Visible == true || window.external.MediaCenter.CustomViewPort.Visible == true) {
        return false
    }
    // if there is no media playing, return false
    if (window.external.MediaCenter.Experience.PlayState == -1) return false
    // otherwise return true
    return true
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This function detects what version of Windows Media Center the user is running and returns a string.
function showMCEVersion() {
    // variable for version
    var sVersion;
    try {
        // find major and minor versions
        var nMajVer = window.external.MediaCenter.MajorVersion;
        var nMinVer = window.external.MediaCenter.MinorVersion;
    }
    catch (e) {
        return null;
    }
    // set return string according to major and minor versions
    if (nMajVer < 7) sVersion = "2004";
    if (nMajVer == 7 && nMinVer < 5) sVersion = "2005";
    if (nMajVer == 7 && nMinVer == 5) sVersion = "Update Rollup 2";
    if (nMajVer > 7) sVersion = "Windows Vista";
    // return correct string
    return sVersion;
    // in case above fails, return null
    return null;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This function detects whether user is in a remote session on a Windows Media Center Extender device
// detect whether user is in a remote session on a Windows Media Center Extender device
function IsMCExtender() {
    try {
        // if this is not a console session ...
        if (window.external.MediaCenter.Capabilities.IsConsole == false) {
            /* ...then it is either a Windows Media Center Extender session or a traditional Remote Desktop session.
             To tell which type of session it is, check if video is allowed. If video is allowed... */
            if (window.external.MediaCenter.Capabilities.IsVideoAllowed == true) {
                // ... then it is an extender session, so return true    
                return true
            }
                // Windows Media Center does not allow video in a traditional Remote Desktop session. So if video is not allowed ...
            else {
                /* IsConsole and IsVideoAllowed are both false false, so user is accessing through a traditional Remote 
                Desktop session, rather than from an extender device. That means that they probably have access to a keyboard 
                and mouse, but they cannot play video. If your application features video playback, you may want to 
                adjust your functionality for this user accordingly. 
                Returning false simply indicates that this is not an Extender session.  */
                return false
            }
        }
        else {
            // If not, this is a Windows Media Center session on the console PC, so return false
            return false
        }
    }
    catch (e) {
        /* If above cause errors, user is probably accessing from a browser outside of Windows Media Center.
        Return false to indicate that it is not an extender session. */
        return false
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Function for reloading page
function reloadPage() {
    /* This function refreshes the page, and calls the onScaleEvent function
    to manage resizing of the elements on the page */
    window.location.reload()
    // determine width of page
    var newWidth = body.getBoundingClientRect().right
    // determine how much the page needs to be resized, by comparing page width to 1024
    var sizeAmount = (newWidth / 1024)
    // call onScaleEvent function
    onScaleEvent(sizeAmount)
}
