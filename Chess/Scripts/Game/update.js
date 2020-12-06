var onDrop = function (source, target, piece, newPos, oldPos, orientation) {

}

var board;

$(document).ready(function () {
    var pieceTheme = function (piece) {
        return '../../Infrastructure/ChessBoard/img/chesspieces/wikipedia/' + piece + '.png';
    };

    var pieces = $("#piece-positions-form-update").val();

    var cfg = {
        pieceTheme: pieceTheme,
        draggable: true,
        dropOffBoard: 'trash',
        sparePieces: true,
        onDrop: onDrop,
        position: pieces
    };
    board = new ChessBoard('board-game', cfg);
});


$("#btn-update-game").on("click", function () {
    $("#piece-positions-form-update").val(board.fen());
});

$("#btn-update-game-start").on("click", function () {
    board.start(false);
});

$("#btn-update-game-clear").on("click", function () {
    board.clear(false);
});