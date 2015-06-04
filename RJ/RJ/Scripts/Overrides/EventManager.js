Ext.define('RJ.overrides.ext.EventManager', {
    requires: [
        'Ext.EventManager'
    ]
}, function () {

    // EventManager is not a normally define()d class, so we can't use Ext.override().  Instead, we 'backup' its
    // original normalizeEvent() function, and replace it with our wrapper function.
    var origNormalizeEvent = Ext.EventManager.normalizeEvent;
    Ext.apply(Ext.EventManager, {
        normalizeEvent: function (eventName, fn) {
            // If this is Chrome 43, then it suffers from the bug logged as
            // https://code.google.com/p/chromium/issues/detail?id=475193, and discussed at length in
            // https://www.sencha.com/forum/showthread.php?301116-Submenus-disappear-in-Chrome-43-beta.
            // Here we workaround by putting a delay in processing any mouseover events so that other events
            // (namely mouseleave) can be processed first.  Then we call the original normalizeEvent method.
            if (Ext.chromeVersion >= 43 && eventName == 'mouseover') {
                var origFn = fn;
                fn = function () {
                    var me = this, args = arguments;
                    setTimeout(
                        function () {
                            origFn.apply(me || Ext.global, args);
                        },
                        0);
                };
            }
            return origNormalizeEvent.call(this, eventName, fn);
        }
    });
});