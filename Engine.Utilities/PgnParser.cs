using ChessDotCore.Engine.Interfaces;
using ChessDotCore.Engine.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChessDotCore.Engine.Utilities
{
  public class PgnParser : IPgnParser
  {
    private const string movesRegexPattern = @"[\s]*[1-9]+[0-9]*\.\s*(\S*)(\s*{.*?})*\s(\S*)(\s*{.*?})*";

    public IGame PgnToGame(string[] pgnContent)
    {
      IGame game;
      Dictionary<string, string> dict = new Dictionary<string, string>();

      int i = 0;
      for (; i < pgnContent.Length; i++)
      {
        if (!pgnContent[i].StartsWith("[")) break;
        string[] parts = pgnContent[i].Replace("[", "").Replace("]", "").Split("\"");
        string key = parts[0].Trim();
        string value = parts[1];
        dict[key] = value;
      }

      game = new Game($"{dict["Event"]} | {dict["Site"]} | {dict["Date"]}", dict["Event"], dict["Site"], DateTime.Parse(dict["Date"]), dict["Round"], dict["White"], dict["Black"]);

      i++;
      StringBuilder stringBuilder = new StringBuilder();
      for (; i < pgnContent.Length; i++)
      {
        stringBuilder.Append(pgnContent[i]);
        stringBuilder.Append(" ");
      }
      string s = stringBuilder.ToString();
      MatchCollection matches = Regex.Matches(s, movesRegexPattern);
      foreach (Match match in matches)
      {
        string whiteMoveText = match.Groups[1].Value;
        string whiteMoveComment = match.Groups[2].Value;
        string blackMoveText = match.Groups[3].Value;
        string blackMoveComment = match.Groups[4].Value;

        IMove whiteMove = ProcessMoveText(whiteMoveText, game);
        game.Move(whiteMove);

        if (game.Board.IsCheckMate || game.Board.IsDraw) break;

        IMove blackMove = ProcessMoveText(blackMoveText, game);
        game.Move(blackMove);

        if (game.Board.IsCheckMate || game.Board.IsDraw) break;
      }

      (game as Game).InitGamestate();

      return game;
    }

    private IMove ProcessMoveText(string moveText, IGame game)
    {
      if (moveText.Length < 2) throw new ArgumentException($"Invalid SAN literal {moveText}");

      if (moveText.EndsWith("+") || moveText.EndsWith("#")) moveText = moveText.Substring(0, moveText.Length - 1);

      if (moveText.Length == 2)
      {
        int toFile = CharToFile(moveText[0]);
        int toRank = CharToRank(moveText[1]);
        ISquare toSquare = game.Board[toRank, toFile];

        foreach (IMove move in game.Board.LegalMoves)
        {
          if (move.ToSquare == toSquare && move.MovingPiece.PieceType == PieceType.Pawn) return move;
        }
      }
      else if (moveText.Length == 3)
      {
        if (moveText.Equals("0-0") || moveText.Equals("O-O"))
        {
          foreach (IMove move in game.Board.LegalMoves)
          {
            if (move is CastlingMove castlingMove && castlingMove.Rook.Square.File == 7) return move;
          }
        }
        else
        {
          char pieceTypeChar = moveText[0];
          int toFile = CharToFile(moveText[1]);
          int toRank = CharToRank(moveText[2]);
          ISquare toSquare = game.Board[toRank, toFile];
          PieceType movingPieceType = CharToPieceType(pieceTypeChar);
          foreach (IMove move in game.Board.LegalMoves)
          {
            if (move.ToSquare == toSquare && move.MovingPiece.PieceType == movingPieceType) return move;
          }
        }
      }
      else if (moveText.Length == 4)
      {
        if (moveText.Contains("="))
        {
          PieceType promotingPieceType = CharToPieceType(moveText[3]);
          int toFile = CharToFile(moveText[0]);
          foreach (IMove move in game.Board.LegalMoves)
          {
            if (move is PromotionMove promotionMove && move.FromSquare.File == toFile && promotionMove.PromotedToPieceType == promotingPieceType) return move;
          }
        }
        else if (moveText.Contains("x"))
        {
          char fileOrPieceTypeChar = moveText[0];
          if (Char.IsUpper(fileOrPieceTypeChar))
          {
            PieceType movingPieceType = CharToPieceType(fileOrPieceTypeChar);
            int toFile = CharToFile(moveText[2]);
            int toRank = CharToRank(moveText[3]);
            ISquare toSquare = game.Board[toRank, toFile];
            foreach (IMove move in game.Board.LegalMoves)
            {
              if (move is CapturingMove capturingMove && capturingMove.MovingPiece.PieceType == movingPieceType && move.ToSquare == toSquare) return move;
            }
          }
          else
          {
            int fromFile = CharToFile(moveText[0]);
            int toFile = CharToFile(moveText[2]);
            int toRank = CharToRank(moveText[3]);
            ISquare toSquare = game.Board[toRank, toFile];
            foreach (IMove move in game.Board.LegalMoves)
            {
              if (move is CapturingMove capturingMove && capturingMove.MovingPiece.PieceType == PieceType.Pawn && move.ToSquare == toSquare && move.FromSquare.File == fromFile) return move;
            }
          }
        }
        else
        {
          if (Char.IsLetter(moveText[1]))
          {
            PieceType movingPieceType = CharToPieceType(moveText[0]);
            int fromFile = CharToFile(moveText[1]);
            int toFile = CharToFile(moveText[2]);
            int toRank = CharToRank(moveText[3]);
            ISquare toSquare = game.Board[toRank, toFile];
            foreach (IMove move in game.Board.LegalMoves)
            {
              if (move is StandardMove && move.MovingPiece.PieceType == movingPieceType && move.ToSquare == toSquare && move.FromSquare.File == fromFile) return move;
            }
          }
          else if (Char.IsDigit(moveText[1]))
          {
            PieceType movingPieceType = CharToPieceType(moveText[0]);
            int fromRank = CharToRank(moveText[1]);
            int toFile = CharToFile(moveText[2]);
            int toRank = CharToRank(moveText[3]);
            ISquare toSquare = game.Board[toRank, toFile];
            foreach (IMove move in game.Board.LegalMoves)
            {
              if (move is StandardMove && move.MovingPiece.PieceType == movingPieceType && move.ToSquare == toSquare && move.FromSquare.Rank == fromRank) return move;
            }
          }
        }
      }
      else if (moveText.Length == 5)
      {
        if (moveText.Equals("O-O-O") || moveText.Equals("0-0-0"))
        {
          foreach (IMove move in game.Board.LegalMoves)
          {
            if (move is CastlingMove castlingMove && castlingMove.Rook.Square.File == 0) return move;
          }
        }
        else if (moveText.Contains("x"))
        {
          char pieceTypeChar = moveText[0];
          PieceType movingPieceType = CharToPieceType(pieceTypeChar);
          int toFile = CharToFile(moveText[3]);
          int toRank = CharToRank(moveText[4]);
          ISquare toSquare = game.Board[toRank, toFile];
          if (Char.IsLetter(moveText[1]))
          {
            int fromFile = CharToFile(moveText[1]);
            foreach (IMove move in game.Board.LegalMoves)
            {
              if (move is CapturingMove capturingMove && capturingMove.MovingPiece.PieceType == movingPieceType && move.ToSquare == toSquare && move.FromSquare.File == fromFile) return move;
            }
          }
          else
          {
            int fromRank = CharToRank(moveText[1]);
            foreach (IMove move in game.Board.LegalMoves)
            {
              if (move is CapturingMove capturingMove && capturingMove.MovingPiece.PieceType == movingPieceType && move.ToSquare == toSquare && move.FromSquare.Rank == fromRank) return move;
            }
          }
        }
        else
        {
          PieceType movingPieceType = CharToPieceType(moveText[0]);
          int fromFile = CharToFile(moveText[1]);
          int fromRank = CharToRank(moveText[2]);
          int toFile = CharToFile(moveText[3]);
          int toRank = CharToRank(moveText[4]);
          ISquare fromSquare = game.Board[fromRank, fromFile];
          ISquare toSquare = game.Board[toRank, toFile];
          foreach (IMove move in game.Board.LegalMoves)
          {
            if (move is StandardMove && move.MovingPiece.PieceType == movingPieceType && move.ToSquare == toSquare && move.FromSquare == fromSquare) return move;
          }
        }
      }
      else if (moveText.Length == 6)
      {
        if (moveText.Contains("="))
        {
          int fromFile = CharToFile(moveText[0]);
          int toFile = CharToFile(moveText[2]);
          int toRank = CharToFile(moveText[3]);
          ISquare toSquare = game.Board[toRank, toFile];

          foreach (IMove move in game.Board.LegalMoves)
          {
            if (move is CapturingPromotionMove capturingPromotionMove && capturingPromotionMove.FromSquare.File == fromFile && capturingPromotionMove.ToSquare == toSquare) return move;
          }
        }
        else
        {
          PieceType movingPieceType = CharToPieceType(moveText[0]);
          int fromFile = CharToFile(moveText[1]);
          int fromRank = CharToFile(moveText[2]);
          int toFile = CharToFile(moveText[3]);
          int toRank = CharToFile(moveText[4]);
          ISquare fromSquare = game.Board[fromRank, fromFile];
          ISquare toSquare = game.Board[toRank, toFile];

          foreach (IMove move in game.Board.LegalMoves)
          {
            if (move is CapturingMove && move.MovingPiece.PieceType == movingPieceType && move.ToSquare == toSquare && move.FromSquare == fromSquare) return move;
          }
        }
      }

      return null;
    }

    private int CharToFile(char c) => c - 97;

    private int CharToRank(char c) => (int)c - 49;

    private PieceType CharToPieceType(char c)
    {
      switch (c)
      {
        case 'B': return PieceType.Bishop;
        case 'N': return PieceType.Knight;
        case 'R': return PieceType.Rook;
        case 'Q': return PieceType.Queen;
        case 'K': return PieceType.King;
      }

      return PieceType.Pawn;
    }
  }
}