$(function () {

    $('#login-form-link').click(function (e) {
        $("#login-form").delay(200).fadeIn(200);
        $("#register-form").fadeOut(200);
        $('#register-form-link').removeClass('active');
        $(this).addClass('active');
        redirect("login");
        e.preventDefault();
    });
    $('#register-form-link').click(function (e) {
        $("#register-form").delay(200).fadeIn(200);
        $("#login-form").fadeOut(200);
        $('#login-form-link').removeClass('active');
        $(this).addClass('active');
        redirect("register");
        e.preventDefault();

    });
    var redirect = (action) => {
        var selectedvalue;

        $.ajax({
            url: `/Account/${action}`, //@Url.Action("FunName","ControllerName")
        type: 'GET',
        dataType: 'html',
        data: {selectedvalue: selectedvalue },
        success: function (data) { //Make the function to return the partial view you want which would be fetched in the data
           
            var x = $(data).find(`.${action}-form`)
            console.log(data);
            $('.DynamicContent').html(x.html())
        }
    });

    }
        


});