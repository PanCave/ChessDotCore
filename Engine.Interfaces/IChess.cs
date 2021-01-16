using System.Collections.Generic;

namespace ChessDotCore.Engine.Interfaces
{
  public interface IChess
  {
    IGame CloneGame(string name);

    IGame CreateGame(string name);

    void DeleteGame(string name);

    List<IGame> Games { get; }
  }
}