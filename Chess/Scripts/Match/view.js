var board;
var game = new Chess();
$(document).ready(function () {
    var pieceTheme = function (piece) {
        return '../../Infrastructure/ChessBoard/img/chesspieces/wikipedia/' + piece + '.png';
    };

    var cfg = {
        pieceTheme: pieceTheme,
        position: 'start'
    };
   board = new ChessBoard('board-view', cfg);
});


function reverseMove(move) {
    return move.charAt(3) + move.charAt(4) + "-" + move.charAt(0) + move.charAt(1);
}

$("#game-start-btn").on("click", function () {
    board.position("start");
    game.reset();
    board.position(game.fen());
    $("#move-number").val(0);
});

$("#game-end-btn").on("click", function () {
    var moveNumber = $("#move-number").val();
    var moves = $("#moves").val().split(", ");
    if ((moveNumber) == moves.length)
        return;
    for (var i = moveNumber; i < moves.length; i++) {
        game.move(moves[i]);
        board.position(game.fen());
    }
    $("#move-number").val(moves.length);
});

$("#one-move-behind-btn").on("click", function () {
    var moveNumber = $("#move-number").val();
    if (moveNumber == 0)
        return;
    game.undo();
    board.position(game.fen());
    $("#move-number").val(--moveNumber);
});

$("#one-move-forward-btn").on("click", function () {
    var moveNumber = $("#move-number").val();
    var moves = $("#moves").val().split(", ");
    if ((moveNumber) == moves.length)
        return;
    game.move(moves[moveNumber++]);
    board.position(game.fen());
    $("#move-number").val(moveNumber);
});

$("#btn-add-comment").click(function () {
    var comment = $("#comment-box").val();
    var id = $("#match-identifier").val();
    $.ajax({
        type: "POST",
        data: {
            id: id,
            comment: comment
        },
        url: "/Match/AddComment",
        success: function (result) {
            $("#comments").append(
                "<hr />"
                + "<div class='text-left'>" + result.UserName + ", " + result.DateCreated + "</div>"
                + "<div class='text-center'>"
                + "<p>" + result.Content + "</p>"
                + "</div>");
        },
        error: function (result) {
            alert("Something went wrong, I hope this doesn't happen during the presentation.");
        }
    });
});