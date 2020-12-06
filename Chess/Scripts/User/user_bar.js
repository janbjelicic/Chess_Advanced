$("#profile-information-item").on("click", function () {
    if ($("#profile-information-item").hasClass("active"))
        return;
    $("#profile-information-item").siblings(".active").removeClass("active");
    $("#profile-information-item").addClass("active");

    var activeSibling = $("#profile-information").siblings(".active");
    activeSibling.removeClass("active in");
    activeSibling.addClass("fade");

    $("#profile-information").removeClass("fade");
    $("#profile-information").addClass("active in");
});

$("#profile-password-item").on("click", function () {
    if ($("#profile-password-item").hasClass("active"))
        return;
    $("#profile-password-item").siblings(".active").removeClass("active");
    $("#profile-password-item").addClass("active");

    var activeSibling = $("#profile-password").siblings(".active");
    activeSibling.removeClass("active in");
    activeSibling.addClass("fade");

    $("#profile-password").removeClass("fade");
    $("#profile-password").addClass("active in");
});

$("#profile-matches-item").on("click", function () {
    if ($("#profile-matches-item").hasClass("active"))
        return;
    $("#profile-matches-item").siblings(".active").removeClass("active");
    $("#profile-matches-item").addClass("active");

    var activeSibling = $("#profile-matches").siblings(".active");
    activeSibling.removeClass("active in");
    activeSibling.addClass("fade");

    $("#profile-matches").removeClass("fade");
    $("#profile-matches").addClass("active in");
});

$("#profile-comments-item").on("click", function () {
    if ($("#profile-comments-item").hasClass("active"))
        return;
    $("#profile-comments-item").siblings(".active").removeClass("active");
    $("#profile-comments-item").addClass("active");

    var activeSibling = $("#profile-comments").siblings(".active");
    activeSibling.removeClass("active in");
    activeSibling.addClass("fade");

    $("#profile-comments").removeClass("fade");
    $("#profile-comments").addClass("active in");
});