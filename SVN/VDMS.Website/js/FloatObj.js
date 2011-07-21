/* Script by: www.jtricks.com 
* Version: 20071017 
* Latest version: 
* www.jtricks.com/javascript/navigation/floating.html 
*/

// The x and y variables represent the desired distance 
// from the top left corner of the menu to the window borders. 
// Positive values mean distance from left and top border to top-left corner
// and negative: from right and bottom to bottom-right corner.
// Special string value 'center' means the floating menu will be centered on respective axis.
// Special string value 'none' means the floating menu will use the original value.
function SetFloatingObjectPosition(controler, x, y) {
    controler.targetX = x;
    controler.targetY = (y == 'init') ? parseInt(controler.menu.offsetTop) + parseInt(document.body.clientTop) : y;
    //alert(document.body.clientTop);
}

//targetId: id of Object (div,...) to be floated
//controlerName: name of vaiable holder controler for floatingObj
//moveSmoothly: moving Object smoothly or not
//exp: var menu = CreateFloatingObject('divMenuId', 'menu');
function CreateFloatingObject(targetId, controlerName, moveSmoothly) {
    var action = controlerName + '.doFloat()';

    var floatingObj =
{
    targetX: -250,
    targetY: 10,

    smoothly: moveSmoothly,

    hasInner: typeof (window.innerWidth) == 'number',
    hasElement: typeof (document.documentElement) == 'object'
        && typeof (document.documentElement.clientWidth) == 'number',

    menu:
        document.getElementById
        ? document.getElementById(targetId)
        : document.all
          ? document.all[targetId]
          : document.layers[targetId]
};


    floatingObj.move = function() {
        if (floatingObj.targetX != 'none') floatingObj.menu.style.left = floatingObj.nextX + 'px';
        if (floatingObj.targetY != 'none') floatingObj.menu.style.top = floatingObj.nextY + 'px';
    }

    floatingObj.computeShifts = function() {
        var de = document.documentElement;

        floatingObj.shiftX =
        floatingObj.hasInner
        ? pageXOffset
        : floatingObj.hasElement
          ? de.scrollLeft
          : document.body.scrollLeft;
        if (floatingObj.targetX < 0) {
            floatingObj.shiftX +=
            floatingObj.hasElement
            ? de.clientWidth
            : document.body.clientWidth;
        }

        floatingObj.shiftY =
        floatingObj.hasInner
        ? pageYOffset
        : floatingObj.hasElement
          ? de.scrollTop
          : document.body.scrollTop;
        if (floatingObj.targetY < 0) {
            if (floatingObj.hasElement && floatingObj.hasInner) {
                // Handle Opera 8 problems  
                floatingObj.shiftY +=
                de.clientHeight > window.innerHeight
                ? window.innerHeight
                : de.clientHeight
            }
            else {
                floatingObj.shiftY +=
                floatingObj.hasElement
                ? de.clientHeight
                : document.body.clientHeight;
            }
        }
    }

    floatingObj.calculateCornerX = function() {
        if ((floatingObj.targetX != 'center') && (floatingObj.targetX != 'none'))
            return floatingObj.shiftX + floatingObj.targetX;

        var width = parseInt(floatingObj.menu.offsetWidth);

        var cornerX =
        floatingObj.hasElement
        ? (floatingObj.hasInner
           ? pageXOffset
           : document.documentElement.scrollLeft) +
          (document.documentElement.clientWidth - width) / 2
        : document.body.scrollLeft +
          (document.body.clientWidth - width) / 2;
        return cornerX;
    };

    floatingObj.calculateCornerY = function() {
        if ((floatingObj.targetY != 'center') && (floatingObj.targetY != 'none'))
            return floatingObj.shiftY + floatingObj.targetY;

        var height = parseInt(floatingObj.menu.offsetHeight);

        // Handle Opera 8 problems  
        var clientHeight =
        floatingObj.hasElement && floatingObj.hasInner
        && document.documentElement.clientHeight
            > window.innerHeight
        ? window.innerHeight
        : document.documentElement.clientHeight

        var cornerY =
        floatingObj.hasElement
        ? (floatingObj.hasInner
           ? pageYOffset
           : document.documentElement.scrollTop) +
          (clientHeight - height) / 2
        : document.body.scrollTop +
          (document.body.clientHeight - height) / 2;
        return cornerY;
    };

    floatingObj.doFloat = function() {
        var stepX, stepY;

        floatingObj.computeShifts();
        var height = parseInt(floatingObj.menu.offsetHeight);
        var width = parseInt(floatingObj.menu.offsetWidth);

        var cornerX = floatingObj.calculateCornerX();
        var cornerY = floatingObj.calculateCornerY();
        if (floatingObj.targetY < 0) cornerY = cornerY - height;
        if (floatingObj.targetX < 0) cornerX = cornerX - width;

        stepX = (cornerX - floatingObj.nextX) * .07;
        if (Math.abs(stepX) < .5) {
            stepX = cornerX - floatingObj.nextX;
        }

        stepY = (cornerY - floatingObj.nextY) * .07;
        if (Math.abs(stepY) < .5) {
            stepY = cornerY - floatingObj.nextY;
        }

        if (Math.abs(stepX) > 0 ||
        Math.abs(stepY) > 0) {
            if (floatingObj.smoothly) {
                floatingObj.nextX += stepX;
                floatingObj.nextY += stepY;
            }
            else {
                floatingObj.nextX = cornerX;
                floatingObj.nextY = cornerY;
            }
            floatingObj.move();
        }
        setTimeout(action, 20);

    };

    // addEvent designed by Aaron Moore  
    floatingObj.addEvent = function(element, listener, handler) {
        if (typeof element[listener] != 'function' ||
       typeof element[listener + '_num'] == 'undefined') {
            element[listener + '_num'] = 0;
            if (typeof element[listener] == 'function') {
                element[listener + 0] = element[listener];
                element[listener + '_num']++;
            }
            element[listener] = function(e) {
                var r = true;
                e = (e) ? e : window.event;
                for (var i = element[listener + '_num'] - 1; i >= 0; i--) {
                    if (element[listener + i](e) == false)
                        r = false;
                }
                return r;
            }
        }

        //if handler is not already stored, assign it  
        for (var i = 0; i < element[listener + '_num']; i++)
            if (element[listener + i] == handler)
            return;
        element[listener + element[listener + '_num']] = handler;
        element[listener + '_num']++;
    };

    floatingObj.init = function() {
        floatingObj.initSecondary();
        floatingObj.doFloat();
    };

    // Some browsers init scrollbars only after  
    // full document load.  
    floatingObj.initSecondary = function() {
        floatingObj.computeShifts();
        floatingObj.nextX = floatingObj.calculateCornerX();
        floatingObj.nextY = floatingObj.calculateCornerY();
        floatingObj.move();
    }


    if (document.layers)
        floatingObj.addEvent(window, 'onload', floatingObj.init);
    else {
        floatingObj.init();
        floatingObj.addEvent(window, 'onload',
        floatingObj.initSecondary);
    }

    return floatingObj;
}