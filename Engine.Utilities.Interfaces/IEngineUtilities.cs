using ChessDotCore.Engine.Interfaces;
using System.Collections.Generic;

namespace ChessDotCore.Engine.Utilities.Interfaces
{
  internal interface IEngineUtilities
  {
    bool CanBlackStillKingSideCastle(IMove move, IBoard board);

    bool CanBlackStillQueenSideCastle(IMove move, IBoard board);

    bool CanWhiteStillKingSideCastle(IMove move, IBoard board);

    bool CanWhiteStillQueenSideCastle(IMove move, IBoard board);

    List<IPiece> CreatePieces(ISquare[,] squares);

    void CreatePiecesList(Board board);

    List<IMove> GetLegalMoves(Board board);

    List<IMove> GetPseudoLegalMoves(IBoard board);

    bool IsCheck(IBoard board, Color turn);
    bool IsCheck(List<IPiece> pieces, ISquare[,] squares, Color turn);

    bool IsCheckMate(IBoard board);

    bool MakeMove(IMove move, Board board);

    IBoard UndoMove(IBoard board);

    void UndoMove(IMove move, Board board);
  }
}