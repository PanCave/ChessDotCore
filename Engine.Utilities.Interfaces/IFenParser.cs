using ChessDotCore.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDotCore.Engine.Utilities.Interfaces
{
  internal interface IFenParser
  {
    IGame FenToGame(string fen);
    string GameToFen(IGame game);
    bool Validate(string fen);
  }
}
