using ChessDotCore.Engine.Interfaces;
using ChessDotCore.Engine.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ChessDotCore.Engine.Utilities
{
  internal class FenParser : IFenParser
  {
    private readonly IEngineUtilities engineUtilities;

    public FenParser(IEngineUtilities engineUtilities)
    {
      this.engineUtilities = engineUtilities;
    }

    public IGame FenToGame(string fen, string name)
    {
      if (!Validate(fen)) return null;

      string[] fenParts = fen.Trim(' ').Split(" ");

      #region Pieces
      List<IPiece> pieces = new List<IPiece>();
      ISquare[,] squares = new Square[8,8];
      string[] rows = fenParts[0].Split("/");

      int rank;
      int file;
      for (rank = 0; rank < 8; rank++)
      {
        for (file = 0; file < 8; file++)
        {
          squares[rank, file] = new Square(rank, file);
        }
      }

      file = 0;

      foreach (string row in rows)
      {
        rank = 0;

        foreach (char c in row)
        {
          if(char.IsLetter(c))
          {
            Color color = char.IsUpper(c) ? Color.White : Color.Black;
            IPiece piece;

            switch (char.ToLower(c))
            {
              case 'p':
                piece = new Piece(color, PieceType.Pawn, squares[rank, file]);
                pieces.Add(piece);
                squares[rank, file].Piece = piece;
                break;
              case 'r':
                piece = new Piece(color, PieceType.Rook, squares[rank, file]);
                pieces.Add(piece);
                squares[rank, file].Piece = piece;
                break;
              case 'b':
                piece = new Piece(color, PieceType.Bishop, squares[rank, file]);
                pieces.Add(piece);
                squares[rank, file].Piece = piece;
                break;
              case 'n':
                piece = new Piece(color, PieceType.Knight, squares[rank, file]);
                pieces.Add(piece);
                squares[rank, file].Piece = piece;
                break;
              case 'q':
                piece = new Piece(color, PieceType.Queen, squares[rank, file]);
                pieces.Add(piece);
                squares[rank, file].Piece = piece;
                break;
              case 'k':
                piece = new Piece(color, PieceType.King, squares[rank, file]);
                pieces.Add(piece);
                squares[rank, file].Piece = piece;
                break;
            }
            file++;
          }
          else
          {
            file += c - '0';
          }
          
        }
        rank++;
      }
      #endregion

      #region Turn
      Color turn = fenParts[1].Equals('w') ? Color.White : Color.Black;
      #endregion

      #region Castle rights
      bool canWhiteKingSideCastle = fenParts[2].Contains('K');
      bool canWhiteQueenSideCastle = fenParts[2].Contains('Q');
      bool canBlackKingSideCastle = fenParts[2].Contains('k');
      bool canBlackQueenSideCastle = fenParts[2].Contains('q');
      #endregion

      #region En-Passant square
      ISquare enPassantSquare = null;
      if(!fenParts[3].Equals("-"))
      {
        int ePFile = fenParts[3][0] - 'a';
        int ePRank = fenParts[3][0] - '0';
        enPassantSquare = squares[ePFile, ePRank];
      }
      #endregion

      #region Halfclock moves
      int halfClockMoves = int.Parse(fenParts[4]);
      #endregion

      #region Full move number
      int fullMoveNumber = int.Parse(fenParts[5]);
      #endregion

      IBoard board = new Board(
        canBlackKingSideCastle,
        canBlackQueenSideCastle,
        canWhiteKingSideCastle,
        canWhiteQueenSideCastle,
        enPassantSquare,
        halfClockMoves,
        null,
        null,
        turn,
        fullMoveNumber,
        pieces,
        engineUtilities.IsCheck(pieces, squares, turn),
        squares,
        new string[0] { }
        );
      return new Game(
        board,
        string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) ? $"Fen-Spiel_{DateTime.Now:dd.MM.yyyy_hh.mm.ss}" : name
        );
    }

    public string GameToFen(IGame game)
    {
      string[] fen = new string[6];
      StringBuilder sb = new StringBuilder();

      #region Pieces
      string[] piecesPart = new string[8];
      for (int rank = 0; rank < 8; rank++)
      {
        int skip = 0;
        for (int file = 0; file < 8; file++)
        {
          if (game.Board[rank, file] == null)
            skip++;
          else
          {
            if(skip > 0)
            {
              sb.Append(skip);
              skip = 0;
            }
            sb.Append(game.Board[rank, file].Piece.PieceChar);
          }
        }

        if (skip > 0)
          sb.Append(skip);

        piecesPart[rank] = sb.ToString();
        sb.Clear();
      }
      fen[0] = string.Join('/', piecesPart);
      #endregion

      #region Turn
      fen[1] = game.Board.Turn == Color.White ? "w" : "b";
      #endregion

      #region Castle rights
      bool castleRightsExist = false;
      if (game.Board.CanWhiteKingSideCastle)
      {
        sb.Append("K");
        castleRightsExist = true;
      }
      if (game.Board.CanWhiteQueenSideCastle)
      {
        sb.Append("Q");
        castleRightsExist = true;
      }
      if (game.Board.CanBlackKingSideCastle)
      {
        sb.Append("k");
        castleRightsExist = true;
      }
      if (game.Board.CanBlackQueenSideCastle)
      {
        sb.Append("q");
        castleRightsExist = true;
      }

      fen[2] = castleRightsExist ? sb.ToString() : "-";
      sb.Clear();
      #endregion

      #region En-Passant square
      fen[3] = game.Board.EnPassantSquare == null ? "-" : game.Board.EnPassantSquare.UciCode;
      #endregion

      #region Halfclock moves
      fen[4] = game.Board.HalfTurnsSincePawnMovementOrCapture.ToString();
      #endregion

      #region Full move number
      fen[5] = game.Board.TurnNumber.ToString();
      #endregion

      return string.Join(" ", fen);
    }

    public bool Validate(string fen)
    {
      string fenPattern = @"\s*^(((?:[rnbqkpRNBQKP1-8]+\/){7})[rnbqkpRNBQKP1-8]+)\s([b|w])\s(-|[K|Q|k|q]{1,4})\s(-|[a-h][1-8])\s(\d+\s\d+)$";
      Regex regex = new Regex(fenPattern);
      Match match = regex.Match(fen);

      if (!match.Success) return false;

      string[] fenParts = fen.Trim(' ').Split(" ");
      if (fenParts.Length != 6) return false;

      string[] piecesParts = fenParts[0].Split("/");
      foreach (string piecePart in piecesParts)
      {
        int fieldSum = 0;
        bool previousWasDigit = false;
        foreach (char c in piecePart)
        {
          if (Char.IsLetter(c))
          {
            fieldSum++;
            previousWasDigit = false;
          }
          else if (Char.IsDigit(c))
          {
            if (previousWasDigit) return false;
            // Black Magic
            fieldSum += c - '0';
            previousWasDigit = true;
          }
        }

        if (fieldSum > 8) return false;
      }

      return true;
    }
  }
}
