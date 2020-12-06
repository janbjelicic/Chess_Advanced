var board;
var game = new Chess();
var pgnMoves = $('#pgn');
var MatchData = {};
var hub = $.connection.matchHub;

// do not pick up pieces if  the game is over
// only pick up pieces for the side to move
var onDragStart = function (source, piece, position, orientation) {    
    if (game.game_over() === true ||
        (game.turn() === 'w' && piece.search(/^b/) !== -1) ||
        (game.turn() === 'b' && piece.search(/^w/) !== -1) ||
        ($("#white-connection-id").val() === "" && $("#is-white").val() === "False") ||
        (game.turn() === 'w' && $("#is-white").val() === "False") ||
        ($("#black-connection-id").val() === "" && $("#is-white").val() === "True") ||
        (game.turn() === 'b' && $("#is-white").val() === "True")) {
        return false;
    }
};

function checkIfMoveInValid(source, target) {
    // See if the move is legal.
    var move = game.move({
        from: source,
        to: target,
        promotion: 'q'
    });

    return move === null;
}

function sendMoveToOpponent(source, target, piece) {
    opponentConnectionId = $("#is-white").val() === "True" ? $("#black-connection-id").val() : $("#white-connection-id").val();
    hub.server.playMove(source, target, piece, opponentConnectionId);
}

function movePiece(source, target, piece) {
    var pieces = $("#pieces-played").val();
    $("#pieces-played").val(pieces + ' ' + piece);
    dealWithClock();
    updateStatus();
    checkIfEnd();
}

function finishMove() {
    board.position(game.fen());
}

var onDrop = function (source, target, piece) {
    if (!checkIfMoveInValid(source, target)) {
        movePiece(source, target, piece);
        sendMoveToOpponent(source, target, piece);        
    } else {
        return 'snapback';
    }       
};

// update the board position after the piece snap 
// for castling, en passant, pawn promotion
var onSnapEnd = function () {
    finishMove();
};

$(document).ready(function () {
    var pieceTheme = function (piece) {
        return '../Infrastructure/ChessBoard/img/chesspieces/wikipedia/' + piece + '.png';
    };

    var cfg = {
        pieceTheme: pieceTheme,
        position: 'start',
        draggable: true,
        onDragStart: onDragStart,
        onDrop: onDrop,
        onSnapEnd: onSnapEnd
    };
    board = new ChessBoard('board-play', cfg);
    var seconds = $("#match-time").val();
    var gain = $("#match-gain").val();
    MatchData.whiteTime = new Timer({
        seconds: seconds,
        gain: gain,
        id: "#white-time",
        onTimerEnd: function () {
            $("#result").html('0 : 1');
            sendResult('0 : 1');
        }
    });
    MatchData.blackTime = new Timer({
        seconds: seconds,
        gain: gain,
        id: "#black-time",
        onTimerEnd: function () {
            $("#result").html('1 : 0');
            sendResult('1 : 0');
        }
    });

    $.connection.hub.start().done(function () {
        console.log("Connection initialized...");
        var isWhite = $("#is-white").val();
        if (isWhite === "True") {
            hub.server.registerForMatch($("#match-identifier").val(), isWhite);
        } else {
            hub.server.registerForMatch($("#match-identifier").val(), isWhite);
        }
    });

    hub.client.playMove = function (source, target, piece) {
        if (!checkIfMoveInValid(source, target)) {
            movePiece(source, target, piece);
            finishMove();
        } else {
            return 'snapback';
        }
    };

    hub.client.setWhiteConnectionId = function (message) {
        $("#white-connection-id").val(message.ID);
    };

    hub.client.setBlackConnectionId = function (message) {
        $("#black-connection-id").val(message.ID);
    };
});

var updateStatus = function () {
    pgnMoves.html(game.pgn());
};

function checkIfEnd() {
    if (game.game_over() == true) {
        var result = '';
        if (game.in_draw() || game.in_stalemate() || game.in_threefold_repetition())
            result = '1/2';
        if (game.in_checkmate()) {
            if (game.turn() === 'w') {
                result = '0 : 1';
            } else {
                result = '1 : 0';
            }
        }
        MatchData.blackTime.stop();
        MatchData.whiteTime.stop();
        $("#result").html(result);
        sendResult(result);
    }
}

var dealWithClock = function () {
    if (game.turn() === 'w') {
        MatchData.whiteTime.start();
        MatchData.blackTime.stop();
    } else {
        MatchData.blackTime.start();
        MatchData.whiteTime.stop();
    }
}

updateStatus();

$("#draw-btn").on("click", function () {
    $("#result").html('1/2');
    sendResult('1/2');
});

$("#resign-btn").on("click", function () {
    $("#result").html('0 : 1');
    sendResult('0 : 1');
});

function sendResult(result) {
    $.post('/Match/Match', {
        whiteId: $("#white-id").val(),
        blackId:  $("#black-id").val(),
        result: result,
        pieces: $("#pieces-played").val(),
        moves: game.pgn(),
        isRated: true
    });
}