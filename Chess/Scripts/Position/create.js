var onDrop = function (source, target, piece, newPos, oldPos, orientation) {

}

var board;

$(document).ready(function () {
    var pieceTheme = function (piece) {
        return '../Infrastructure/ChessBoard/img/chesspieces/wikipedia/' + piece + '.png';
    };

    var cfg = {
        pieceTheme: pieceTheme,
        draggable: true,
        dropOffBoard: 'trash',
        sparePieces: true,
        onDrop: onDrop
    };
    board = new ChessBoard('board-problem', cfg);
});

$("#btn-create-position-start").on("click", function () {
    board.start();
});

$("#btn-create-position-clear").on("click", function () {
    board.clear();
});

$("#btn-create-position").on("click", function () {
    $("#piece-positions-form-create").val(board.fen());
});