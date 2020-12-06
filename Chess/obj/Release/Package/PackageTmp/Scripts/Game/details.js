$(document).ready(function () {
    var pieceTheme = function (piece) {
        return '../../Infrastructure/ChessBoard/img/chesspieces/wikipedia/' + piece + '.png';
    };

    var pieces = $("#piece-positions-form-details").val();

    var cfg = {
        pieceTheme: pieceTheme,
        position: pieces
    };
    board = new ChessBoard('board-game', cfg);
});