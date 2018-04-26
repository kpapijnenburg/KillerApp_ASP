function GetCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length === 2) {
        return parts.pop().split(";").shift();
    }

    return null;
}

function IsUserAdmin() {
    var cookie = GetCookie("userId");

    if (cookie != null) {
        var id = cookie.substr(cookie.length - 1);

        $.get("user/getUser/" + id,
            function (data) {
                if (data.IsAdmin) {
                    $("#admin").append('<a class="nav-text" href="user/adminpage"> Admin</a>');
                }
            });
    }
}

function UserLoggedIn() {
    var cookie = GetCookie("userId");

    if (cookie != null) {
        $(function () {
            $("#loggedIn").prepend('<a class="nav-text" href ="/user/logout"> Logout </a>');
            $("#loggedIn").prepend('<a class="nav-text" href ="/user/details"> Account |</a>');
        });
    }
    else {
        $(function () {
            $("#loggedIn").prepend('<a class="nav-text" href ="/user/loginForm"> Login </a>');
            $("#loggedIn").prepend('<a class="nav-text" href ="/user/registerForm"> Register |</a>');
        });
    }
}

function GetMostDownloaded() {
    var request = $.ajax({
        url: "/track/getmostdownloaded",
        type: "get",
        dataType: "json"
    });

    request.done(function (result) {

        for (var i = 0; i < result.length; i++) {
            $("#Most").append(`<div style="cursor: pointer;" onclick="NavigateToDetails(${result[i].Id})"><p><strong>${i + 1}. ${result[i].TrackName}</strong> </br> ${result[i].ArtistName}<hr> </div>`);

        }
    });

    request.fail(function (textStatus) {
        console.log("request failed" + textStatus);
    });
}

function GetLatestReleases() {
    var request = $.ajax({
        url: "/track/getlatestreleases",
        type: "get",
        dataType: "json"
    });

    request.done(function(result) {
        for (var i = 0; i < result.length; i++) {
            $("#Latest").append(`<div style="cursor: pointer;" onclick="NavigateToDetails(${result[i].Id})"><p><strong>${i + 1}. ${result[i].TrackName}</strong> </br> ${result[i].ArtistName}<hr> </div>`);
        }
    });

    request.fail(function (jqHxr, textStatus) {
        console.log("request failed: " + textStatus);
    });
}

function NavigateToDetails(id) {
    location.href = "/track/details/" + id;
}

function HasVoted() {
    var request = $.ajax({
        url: "/vote/hasvoted",
        type: "get",
        dataType: "json"
    });

    request.done(function(result) {
        console.log(result);
    });
}

function Refresh() {
    $(document).ready(() => {
        UserLoggedIn();
        IsUserAdmin();
        GetLatestReleases();
        GetMostDownloaded();
    });
}

