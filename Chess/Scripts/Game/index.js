$(document).ready(function () {
    var pieceTheme = function (piece) {
        return '../Infrastructure/ChessBoard/img/chesspieces/wikipedia/' + piece + '.png';
    };
    var numberOfPositions = $("#number-of-positions").val();
    for (var i = 1; i <= numberOfPositions; i++) {
        var pieces = $("#piece-positions-form-index"+i).val();
        var cfg = {
            pieceTheme: pieceTheme,
            position: pieces,
            showNotation: false
        };
        board = new ChessBoard('board-game'+i, cfg);
    }    
});