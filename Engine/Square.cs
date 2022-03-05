using ChessDotCore.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDotCore.Engine
{
  internal class Square : ISquare
  {
    public Square(int rank, int file)
    {
      Rank = rank;
      File = file;
      Piece = null;
    }

    public override string ToString()
    {
      if (Piece == null) return $"{(char)(65 + File)}{Rank + 1}";
      string pieceString = "";
      string colorString = Piece.Color == Color.White ? "w" : "b";
      switch (Piece.PieceType)
      {
        case PieceType.Pawn: pieceString = "Bauer"; break;
        case PieceType.Bishop: pieceString = "Läufer"; break;
        case PieceType.Knight: pieceString = "Springer"; break;
        case PieceType.Rook: pieceString = "Turm"; break;
        case PieceType.Queen: pieceString = "Dame"; break;
        case PieceType.King: pieceString = "König"; break;
      }
      return $"{(char)(65 + File)}{Rank + 1} [{colorString}]{pieceString}";
    }

    public int File { get; }

    public IPiece Piece { get; set; }

    public int Rank { get; }

    public string UciCode => $"{(char)(65 + File)}{Rank + 1}";
  }
}
