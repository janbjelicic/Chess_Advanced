$(function () {
    var sparring = $.connection.sparringHub;
    sparring.server.registerClient($('#user-id').val());

    game.client.refreshAmountOfPlayers = function (message) {
        $("#amountOfClients").html(message.amountOfClients);
    };

    sparring.client.sparePartners = function (whiteId, blackId, minutes, gain, identifier) {
        debugger;
        $.get('/Match/Match', {
            whiteId: whiteId,
            blackId: blackId,
            minutes: minutes,
            gain: gain,
            identifier: identifier
        });
    };

    $(".play-link").click(function () {
        // Call the Send method on the hub.
        sparring.server.Spare(
            $(this).attr('white'),
            $(this).attr('black'),
            $(this).attr('minutes'),
            $(this).attr('gain')
            );
    });
    $.connection.hub.start().done();
});