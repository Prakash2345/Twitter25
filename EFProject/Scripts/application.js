$(document).ready(function () {
    function refreshTweet() {
        $.ajax({
            type: "POST",
            url: "/Tweet/GetTweetandFollowing",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#divTweet').text(response.totalTweet);
                $('#divFollowing').text(response.following);
                $('#divFollowers').text(response.followers);
            },
            error: function (response) {
                alert('error occured, please contact system administrator !!!');
            }
        });
    }
    var $message = $('#Message')

    $(document).on("focus", '#Message', function () {
        $message.siblings(".field-validation-error").css("visibility", "hidden")
    });
    $(document).on('click', "#btnAddTweet, #btnUpdateTweet", function () {
        var tweet = new Object();
        
        tweet.Message = $message.val();
        tweet.Tweet_id = $('#Tweet_id').val();
        if (!tweet.Message) {
            $message.siblings(".field-validation-error").text("Message is required").css("visibility", "visible");
            return;
        }


        if (tweet.Message.length < 130 || tweet.Message.length > 140) {
            $message.siblings(".field-validation-error").text("Please enter minimum of 130 and maximum of 140 charactors").css("visibility", "visible");
            return;
        }
            

        if (tweet != null) {
            $.ajax({
                type: "POST",
                url: "/Tweet/AddTweet",
                data: JSON.stringify(tweet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#tweetCategory").load('/Tweet/ListTweets');
                    $('#Message').val('');
                    refreshTweet();
                },
                failure: function (response) {
                    alert('error occured, please contact system administrator !!!');
                },
                error: function (response) {
                    alert('error occured, please contact system administrator !!!');
                }
            });
        }
    });
    $(document).on('click', "#btnDeleteTweet", function () {
        var tweet = new Object();
        tweet.Tweet_id = $('#Tweet_id').val();
        if (tweet != null) {
            $.ajax({
                type: "POST",
                url: "/Tweet/DeleteTweet",
                data: JSON.stringify(tweet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#tweetCategory").load('/Tweet/ListTweets');
                },
                failure: function (response) {
                    alert('error occured, please contact system administrator !!!');
                },
                error: function (response) {
                    alert('error occured, please contact system administrator !!!');
                }
            });
        }
    });

    $(document).on("click", "button[name='Edit']", function (event) {
        $parentRow = $(this).closest("tr");
        $parentRow.find("button[name='Cancel']").show();
        $parentRow.find("button[name='Update']").show();
        $parentRow.find("button[name='Delete']").hide();
        $parentRow.find("label[name='lblMessage']").hide();
        $parentRow.find("input[name='tweet.Message']").show();
        $(this).hide();
        event.stopPropagation();
    });
   
        $(document).on("click", "button[name='Cancel']", function (event) {
        $parentRow = $(this).closest("tr");
        $(this).hide();
        $parentRow.find("button[name='Update']").hide();
        $parentRow.find("button[name='Delete']").show();
        $parentRow.find("button[name='Edit']").show();
        $parentRow.find("label[name='lblMessage']").show();
        $parentRow.find("input[name='tweet.Message']").hide();
        event.stopPropagation();
    });
      
       $(document).on("click", "button[name='Update']", function (event) {
        $parentRow = $(this).closest("tr");
        $parentRow.find("button[name='Cancel']").hide();
        $parentRow.find("button[name='Delete']").show();
        $parentRow.find("button[name='Edit']").show();
        $parentRow.find("label[name='lblMessage']").show();
        $parentRow.find("input[name='tweet.Message']").hide();
        var id = $parentRow.attr("id");
        $(this).hide();
        event.stopPropagation();

        var tweet = new Object();
        tweet.Tweet_id = id
        tweet.Message = $parentRow.find("input[name='tweet.Message']").val();
        if (tweet != null) {
            $.ajax({
                type: "POST",
                url: "/Tweet/AddTweet",
                data: JSON.stringify(tweet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#tweetCategory").load('/Tweet/ListTweets');
                },
                error: function (response) {
                    alert('error occured, please contact system administrator !!!');
                }
            });
        }
    });
          
      $(document).on("click", "button[name='Delete']", function (event) {
        $parentRow = $(this).closest("tr");
        var id = $parentRow.attr("id");
        event.stopPropagation();

        var tweet = new Object();
        tweet.Tweet_id = id
        if (tweet != null) {
            $.ajax({
                type: "POST",
                url: "/Tweet/DeleteTweet",
                data: JSON.stringify(tweet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#tweetCategory").load('/Tweet/ListTweets');
                    refreshTweet();
                },
                error: function (response) {
                    alert('error occured, please contact system administrator !!!');
                }
            });
        }
    });
      $("#searchFollowing").on("click", function (event) {
        var querystr = $("#txtSearch").val();
        console.log('txtSearch' + querystr);
        if (querystr == "")
        {
            var urls = '/Tweet/Following';
            window.location.href = urls;
        }
        else
        {
            var urls = '/Tweet/Followings?searchName=' + querystr;
            window.location.href = urls;
        }
       
    });
    refreshTweet();

    $("#arFollowing").on("click", function (event) {
        var urls = '/Tweet/GetFollowing';
        window.location.href = urls;
    });
    $("#arFollowers").on("click", function (event) {
        var urls = '/Tweet/GetFollowers';
        window.location.href = urls;
    });
});