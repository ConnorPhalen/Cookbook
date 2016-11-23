$(document).ready(function () {
    var bt = $('#mainmenu').position().top;

    $(window).scroll(function () {
        var wst = $(window).scrollTop();

        (wst >= bt) ?
        $('#mainmenu').css({ position: 'fixed', top: 60 + 'px' }) :
        $('#mainmenu').css({ position: 'absolute', top: bt + 'px' })
    });
});