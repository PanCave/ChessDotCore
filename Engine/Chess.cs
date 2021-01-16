using ChessDotCore.Engine.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ChessDotCore.Engine
{
  public class Chess : IChess
  {
    public Chess()
    {
      Games = new List<IGame>();
    }

    public IGame CloneGame(string name)
    {
      var item = Games.SingleOrDefault(x => x.Name == name);
      if (item != null)
        return item.Clone();
      return null;
    }

    public IGame CreateGame(string name)
    {
      IGame game = new Game(name);
      Games.Add(game);
      return game;
    }

    public void DeleteGame(string name)
    {
      var item = Games.SingleOrDefault(x => x.Name == name);
      if (item != null)
        Games.Remove(item);
    }

    public List<IGame> Games { get; }
  }
}