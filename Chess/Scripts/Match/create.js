var board;
var game = new Chess();
var pgnMoves = $('#match-moves-form-create');

// do not pick up pieces if the game is over
// only pick up pieces for the side to move
var onDragStart = function (source, piece, position, orientation) {
    if (game.game_over() === true ||
        (game.turn() === 'w' && piece.search(/^b/) !== -1) ||
        (game.turn() === 'b' && piece.search(/^w/) !== -1)) {
        return false;
    }
};

var onDrop = function (source, target) {
    // see if the move is legal
    var move = game.move({
        from: source,
        to: target,
        promotion: 'q' // NOTE: always promote to a queen for example simplicity
    });

    // illegal move
    if (move === null)
        return 'snapback';

    updateStatus();
};

// update the board position after the piece snap 
// for castling, en passant, pawn promotion
var onSnapEnd = function () {
    board.position(game.fen());
};

var updateStatus = function () {
    pgnMoves.val(game.pgn());
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
    board = new ChessBoard('board-game', cfg);
});

updateStatus();

$("#btn-create-match-start").on("click", function () {
    board.start(false);
    game = new Chess();
    $("#match-moves-form-create").val("");
});