$(document).ready(function () {
    $('#add-ingredient').click(function () {
        $.ajax({
            url: '@Url.Action("AddIngredient")',
            cache: false,
            method: 'GET',
            success: function (html) {
                $('#ingredients').append(html);
            }
        })
    });

    $('#ingredients').on('click', 'button.delete-ingredient', function () {
        var ingredientToRemove = $(this).closest('div.ingredient');
        ingredientToRemove.prev('input[type=hidden]').remove();
        ingredientToRemove.remove();
    });

    $('#add-ingredient').click(function () {
        $('.add').addClass("col-md-offset-2 ");
    });
});