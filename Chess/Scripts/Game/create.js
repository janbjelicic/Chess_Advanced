﻿var onDrop = function (source, target, piece, newPos, oldPos, orientation) {

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
    board = new ChessBoard('board-game', cfg);
});


$("#btn-create-new-game").on("click", function () {
    $("#piece-positions-form-create").val(board.fen());
});

$("#btn-create-game-start").on("click", function () {
    board.start(false);
});

$("#btn-create-game-clear").on("click", function () {
    board.clear(false);
});