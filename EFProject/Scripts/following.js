$(document).ready(function () {
    $("button[name='addFollowing']").on("click", function (event) {
        console.log('addFollowing' + "clicked");
        $parentRow = $(this).closest("tr");
        var id = $parentRow.attr("id");
        event.stopPropagation();

        var person = new Object();
        person.User_id = id
        if (person != null) {
            $.ajax({
                type: "POST",
                url: "/Tweet/AddFollowing",
                data: JSON.stringify(person),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    location.reload();
                },
                error: function (response) {
                    alert('error occured, please contact system administrator !!!');
                }
            });
        }
    });
});
