using ChessDotCore.Engine.Interfaces;
using System.Collections.Generic;

namespace ChessDotCore.Engine
{
  internal class Piece : IPiece
  {
    public Piece(Color color, PieceType pieceType, ISquare square)
    {
      Color = color;
      PieceType = pieceType;
      Square = square;
      AttackedSquares = new List<ISquare>();
      ProtectedSquares = new List<ISquare>();
      ReachableSquares = new List<ISquare>();
    }

    public override string ToString()
    {
      char pieceChar = GetPieceChar();

      return pieceChar + "_" + (char)(Square.File + 65) + (Square.Rank + 1).ToString() + ",";
    }

    private char GetPieceChar()
    {
      char pieceChar;
      switch (PieceType)
      {
        case PieceType.Pawn: pieceChar = 'P'; break;
        case PieceType.Rook: pieceChar = 'R'; break;
        case PieceType.Knight: pieceChar = 'N'; break;
        case PieceType.Bishop: pieceChar = 'B'; break;
        case PieceType.King: pieceChar = 'K'; break;
        case PieceType.Queen: pieceChar = 'Q'; break;
        default: pieceChar = 'X'; break;
      }
      return (Color == Color.White ? pieceChar : (char)(pieceChar + 32));
    }

    public List<ISquare> AttackedSquares { get; set; }

    public Color Color { get; }

    public char PieceChar => GetPieceChar();
    public PieceType PieceType { get; internal set; }

    public List<ISquare> ProtectedSquares { get; set; }

    public List<ISquare> ReachableSquares { get; set; }

    public ISquare Square { get; set; }
  }
}