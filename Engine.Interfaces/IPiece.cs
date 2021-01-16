using System.Collections.Generic;

namespace ChessDotCore.Engine.Interfaces
{
  public interface IPiece
  {
    string ToString();

    List<ISquare> AttackedSquares { get; set; }
    Color Color { get; }
    char PieceChar { get; }
    PieceType PieceType { get; }
    List<ISquare> ProtectedSquares { get; set; }
    List<ISquare> ReachableSquares { get; set; }
    ISquare Square { get; set; }
  }
}