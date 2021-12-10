using ChessDotCore.Engine.Interfaces;
using ChessDotCore.Engine.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

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
    }

    public string GameToFen(IGame game)
    {
      throw new NotImplementedException();
    }

    public bool Validate(string fen)
    {
      return true;
    }
  }
}
