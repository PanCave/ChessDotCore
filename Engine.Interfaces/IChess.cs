using System.Collections.Generic;

namespace ChessDotCore.Engine.Interfaces
{
  public interface IChess
  {
    IGame CreateGame(string name);

    void DeleteGame(string name);

    IGame ImportPgn(string filename);

    List<IGame> Games { get; }
  }
}