var board,
  game = new Chess();

var makeRandomMove = function () {
    var possibleMoves = game.moves();

    // exit if the game is over
    if (game.game_over() === true ||
      game.in_draw() === true ||
      possibleMoves.length === 0) return;

    var randomIndex = Math.floor(Math.random() * possibleMoves.length);
    game.move(possibleMoves[randomIndex]);
    board.position(game.fen());

    window.setTimeout(makeRandomMove, 500);
};

var pieceTheme = function (piece) {
    return '../Infrastructure/ChessBoard/img/chesspieces/wikipedia/' + piece + '.png';
};
var cfg = {
    pieceTheme: pieceTheme,
    position: 'start'
};
board = new ChessBoard('board', cfg);

window.setTimeout(makeRandomMove, 500);