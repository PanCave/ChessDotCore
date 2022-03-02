using ChessDotCore.Engine.Interfaces;
using ChessDotCore.Engine.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ChessDotCore.Engine.Utilities
{
  public class FenParser : IFenParser
  {
    public IGame FenToGame(string fen)
    {
      if (!Validate(fen)) return null;

      string[] fenParts = fen.Trim(' ').Split(" ");

      // 1. Pieces
      string[] rows = fenParts[0].Split("/");
      foreach (string row in rows)
      {

      }

      return null;
    }

    public string GameToFen(IGame game)
    {
      throw new NotImplementedException();
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
