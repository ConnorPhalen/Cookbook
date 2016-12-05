$(document).ready(function () {
    var close = document.getElementsByClassName("closebtn");
    $('.closebtn').click(function () {
        var div = this.parentElement;
        div.style.opacity = "0";
        setTimeout(function () { $('.alert').style.display = "none"; }, 600);
        $('body').animate({ marginTop: '-70px'}, 1000);
    });
});
