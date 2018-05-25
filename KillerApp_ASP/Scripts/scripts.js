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

        $.get("/user/getUser/" + id,
            function (data) {
                if (data.IsAdmin) {
                    $("#admin").append('<a class="nav-text" href="/user/adminpage"> Admin</a>');
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
            $("#Most").append(`
                    <div style="cursor: pointer;" onclick="NavigateToDetails(${result[i].Id})">
                        <p>
                            <strong>${i + 1}. ${result[i].TrackName}</strong>
                            <br />
                            ${result[i].ArtistName}
                            <br />
                            <a href='/shoppingcart/add/${result[i].Id}'><img class="rounded float-right shopping-cart-image" src="https://image.flaticon.com/icons/svg/70/70021.svg" alt="Alternate Text" /></a>
                            <hr>
                    </div>
            `);

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

    request.done(function (result) {
        for (var i = 0; i < result.length; i++) {
            $("#Latest").append(`
                    <div style="cursor: pointer;" onclick="NavigateToDetails(${result[i].Id})">
                        <p>
                            <strong>${i + 1}. ${result[i].TrackName}</strong>
                            <br />
                            ${result[i].ArtistName}
                            <br />
                            <a href='/shoppingcart/add/${result[i].Id}'><img class="rounded float-right shopping-cart-image" src="https://image.flaticon.com/icons/svg/70/70021.svg" alt="Alternate Text" /></a>
                            <hr>
                    </div>
            `);        }
    });

    request.fail(function (jqHxr, textStatus) {
        console.log("request failed: " + textStatus);
    });
}

function GetDeals() {
    var request = $.ajax({
        url: "/track/getdeals",
        type: "get",
        dataType: "json"
    });

    request.done(function (result) {
        $("#deal").append('<h3 class="text-center">Deal</h3>')
            .append("<hr />")
            .append(
            `<div class="text-truncate text-center" style="cursor: pointer" onclick="location.href = '/track/details/${result.Id}'">
                    <img class="homeImage" src="${result.CoverUrl}" alt="" />
                    <h3>
                        <strong>${result.TrackName}</strong>
                    </h3>
                    <p class="lead">${result.ArtistName}</p>
                </div>`);
    });

    request.fail(function (jqHxr, textStatus) {
        console.log("request failed: " + textStatus);
    });
}

function GetRecommended() {
    var request = $.ajax({
        url: "/track/GetRecommended",
        type: "get",
        dataType: "json"
    });

    request.done(function (result) {
        $("#recommended").append('<h3 class="text-center">Recommended</h3>')
            .append("<hr />")
            .append(
                `<div class="text-truncate text-center" style="cursor: pointer" onclick="location.href = '/track/details/${result.Id}'">
                    <img class="homeImage" src="${result.CoverUrl}" alt="" />
                    <h3>
                        <strong>${result.TrackName}</strong>
                    </h3>
                    <p class="lead">${result.ArtistName}</p>
                </div>`);
    });

    request.fail(function (jqHxr, textStatus) {
        console.log("request failed: " + textStatus);
    });
}


function NavigateToDetails(id) {
    location.href = "/track/details/" + id;
}

function Refresh() {
    $(document).ready(() => {
        UserLoggedIn();
        IsUserAdmin();
        GetLatestReleases();
        GetMostDownloaded();
        GetDeals();
        GetRecommended();
    });
}


//<h3 class="text-center">Recommended</h3>
//    <hr />
//    <div class="text-truncate text-center" style="cursor: pointer" onclick="location.href = '/track/details/22'">
//    <img class="homeImage" src="https://img.discogs.com/1ziYXqFjAa0ObgSsmUczeyyTD7s=/fit-in/300x300/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/R-10114855-1491862906-1409.jpeg.jpg" alt="" />
//    <h3>
//    <strong>Brutal 6.0</strong>
//    </h3>
//    <p class="lead">Radical Redemption</p>
//    </div>
