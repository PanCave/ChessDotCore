using ChessDotCore.Engine.Interfaces;
using ChessDotCore.Engine.Utilities;
using ChessDotCore.Engine.Utilities.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChessDotCore.Engine
{
  public class Chess : IChess
  {
    private readonly IPgnParser pgnParser;

    public Chess()
    {
      Games = new List<IGame>();
      pgnParser = new PgnParser();
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

    public IGame ImportPgn(string filename)
    {
      IGame game = pgnParser.PgnToGame(File.ReadAllLines(filename));
      Games.Add(game);
      return game;
    }

    public IGame CreateGameFromFEN(string fen, string name)
    {
      throw new System.NotImplementedException();
    }

    public List<IGame> Games { get; }
  }
}