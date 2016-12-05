$(document).ready(function () {
    // Get the modal
    var modal = $('.modal');
    

    /*document.getElementById("detail").onclick = function modals(id) {
       // var closedStatusId = @(id);
        //<h3>@Html.DisplayFor(modelItem => Model.Name)</h3>
        //Rating: @Html.DisplayFor(modelItem => Model.Rating)/10!
        modal.css({ "display": "block" });
    }*/
    $('.detail').click(function () {
        $('.modal-content').append("<h3>@Html.DisplayFor(modelItem => Model.Name)</h3>");
        modal.css({ "display": "block" });      
    });
   
    $('.close').click(function(){
        modal.css({ "display": "none" });
    });

    // When the user clicks anywhere outside of the modal, close it
    $(window).click(function (e) {
        if (e.target.nodeName === "modal") {
            modal.css({ "display": "none" });
        }
    });
    $('.recipeCard').keyup(function (e) {
        if (e.keycode === 13 || e.keycode === 27) {
            modal.css({ "display": "none" });
        }
    });
    /*$('.recipeCard').on({
        mouseenter: function () {
            $(this).animate({ backgroundColor: 'green' }, 1000)
        },
        mouseleave: function () {
            $(this).animate({ backgroundColor: 'red' }, 1000)
        }
    });*/
});