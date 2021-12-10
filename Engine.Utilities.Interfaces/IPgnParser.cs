using ChessDotCore.Engine.Interfaces;

namespace ChessDotCore.Engine.Utilities.Interfaces
{
  internal interface IPgnParser
  {
    IGame PgnToGame(string[] pgnContent);
  }
}