using System.Collections.Generic;

namespace ChessDotCore.Engine.Interfaces
{
  public interface IBoard
  {
    IPiece BlackKing { get; }
    bool CanBlackKingSideCastle { get; }
    bool CanBlackQueenSideCastle { get; }
    bool CanWhiteKingSideCastle { get; }
    bool CanWhiteQueenSideCastle { get; }
    ISquare EnPassantSquare { get; }
    string Fen { get; }
    GameState GameState { get; }
    int HalfTurnsSincePawnMovementOrCapture { get; }
    bool IsCheck { get; }
    bool IsCheckMate { get; }
    bool IsDraw { get; }
    IMove LastMove { get; }
    List<IMove> LegalBishopMoves { get; }
    List<IMove> LegalKingMoves { get; }
    List<IMove> LegalKnightMoves { get; }
    List<IMove> LegalMoves { get; }
    List<IMove> LegalPawnMoves { get; }
    List<IMove> LegalQueenMoves { get; }
    List<IMove> LegalRookMoves { get; }
    string[] MoveHistory { get; }
    List<IPiece> Pieces { get; }
    Color Turn { get; }
    int TurnNumber { get; }
    IPiece WhiteKing { get; }
    ISquare[,] Squares { get; }
    ISquare this[int rank, int file] { get; }
    List<IPiece> this[PieceType pieceType, Color color, bool alive] { get; }
    List<IPiece> this[PieceType pieceType, Color color] { get; }
    List<IPiece> this[Color color, bool alive] { get; }
    List<IPiece> this[Color color] { get; }
    List<IPiece> this[bool alive] { get; }
  }
}