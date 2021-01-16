using ChessDotCore.Engine.Interfaces;

namespace ChessDotCore.Bots.Interfaces
{
  public interface IChessBot
  {
    IMove MakeMove();
  }
}