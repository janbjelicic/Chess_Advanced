$(function () {
    var sparring = $.connection.sparringHub;
    $.connection.hub.start().done(function () {
        console.log("Connection initialized...");
        $(".play-link").click(function () {
            sparring.server.spare(
                $(this).attr('white'),
                $(this).attr('black'),
                $(this).attr('minutes'),
                $(this).attr('gain'));
        });
        $("#filter-button").click(function () {
            sparring.server.registerClient($('#user-id').val());
            $("#online-users-table").load('/Match/ListOnlineUsers');
            return false;
        });
    });    

    sparring.client.refreshAmountOfPlayers = function (message) {
        $("#amountOfClients").html(message.amountOfClients);
    };

    sparring.client.sparePartners = function (whiteId, blackId, minutes, gain, identifier) {
        $.get('/Match/Match', {
            whiteId: whiteId,
            blackId: blackId,
            minutes: minutes,
            gain: gain,
            identifier: identifier
        });
    };    
});