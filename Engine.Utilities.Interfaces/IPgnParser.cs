using ChessDotCore.Engine.Interfaces;

namespace ChessDotCore.Engine.Utilities.Interfaces
{
  public interface IPgnParser
  {
    IGame PgnToGame(string[] pgnContent);
  }
}