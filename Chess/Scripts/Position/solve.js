var moveNumber = 0;
var board;
var onDrop = function (source, target, piece, newPos, oldPos, orientation) {
    if (isForbidden(source, target, piece))
        return 'snapback';
    var result = checkMove(source, target, piece);
    return result;
}

function checkMove(source, target, piece) {
    var move = source + "-" + target;
    var id = $("#position-id-form-solving").val();
    var returnData = '';
    $.ajax({
        url: "../CheckMove",
        type: 'POST',
        async: false,
        data: {
            move: move,
            moveNumber: moveNumber,
            id: id
        },
        success: function (result) {
            if (result.result == false) {
                $("#success-message").html('<p>' + result.message + '</p>');
                $("#attempt-message").html('<p>' + result.attempt + '</p>');
                returnData = 'snapback';
            } else {
                if (moveNumber % 2 == 0) {
                    move = moveNumber / 2 + 1;
                    $("#table-solution-body").append(
                        "<tr class='move" + move + "'><td>" + move + ".</td><td>" + piece.charAt(1) + target + "</td></tr>"
                    );
                } else {
                    move = moveNumber / 2 + 0.5;
                    $("tr.move" + move).append(
                        "<td>" + piece.charAt(1) + target + "</td>"
                    );
                }
                $("#success-message").html('<p>' + result.message + '</p>');
                $("#attempt-message").html('<p>' + result.attempt + '</p>');
                moveNumber++;
                var whiteIsPlaying = $("#white-is-playing-form-solving").val();
                $("#white-is-playing-form-solving").val(whiteIsPlaying == 'False' ? 'True' : 'False')
            }
        },
        error: function (result) {
            $("#success-message").html('<p>' + result.message + '</p>');
        }
    });
    return returnData;
}

function isForbidden(source, target, piece) {
    if (source == target)
        return true;
    var numberOfMoves = $("#number-of-moves-form-solving").val();
    if (numberOfMoves <= moveNumber)
        return true;
    var whiteIsPlaying = $("#white-is-playing-form-solving").val();
    if (piece.charAt(0) == 'w' && whiteIsPlaying == 'False')
        return true;
    if (piece.charAt(0) == 'b' && whiteIsPlaying == 'True')
        return true;    
    return false;
}

$(document).ready(function () {
    var pieceTheme = function (piece) {
        return '../../Infrastructure/ChessBoard/img/chesspieces/wikipedia/' + piece + '.png';
    };
    var pieces = $("#piece-positions-form-solving").val();
    var cfg = {
        pieceTheme: pieceTheme,
        draggable: true,
        onDrop: onDrop,
        position: pieces
    };
    board = new ChessBoard('board-problem', cfg);
});

$('#btn-add-comment').click(function () {
    var comment = $('#comment-box').val();
    var id = $('#position-id-form-solving').val();
    $.ajax({
        type: "POST",
        data: {
            id: id,
            comment: comment
        },
        url: "/Position/AddComment",
        success: function (result) {
            $("#comments").append(
                "<hr />"
                +"<div class='text-left'>" + result.UserName + ", " + result.DateCreated + "</div>"
                + "<div class='text-center'>"
                + "<p>" + result.Content + "</p>"
                + "</div>");
        },
        error: function () {
            alert("Something went wrong, I hope this doesn't happen during the presentation.");
        }
    });
});

$('#btn-get-solution').on("click", function () {
    var id = $('#position-id-form-solving').val();
    $.ajax({
        type: "POST",
        data: {
            id: id
        },
        url: "/Position/GetSolution",
        success: function (result) {
            var moves = result.moves.split(", ");
            for (var i = moveNumber; i < moves.length; i++) {
                board.move(moves[i]);
            }
        },
        error: function () {
            alert("Something went wrong, I hope this doesn't happen during the presentation.");
        }
    });
});