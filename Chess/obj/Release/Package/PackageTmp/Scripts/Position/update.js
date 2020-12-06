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
    board = new ChessBoard('board-problem', cfg);
});

$("#start-btn").on("click", function () {
    board.start();
});

$("#clear-btn").on("click", function () {
    board.clear();
});

$("#btn-update-position").on("click", function () {
    $("#piece-positions-form-update").val(board.fen());
});