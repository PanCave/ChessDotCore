namespace ChessDotCore.Engine.Interfaces
{
  public interface IGame
  {
    IBoard Board { get; }

    string Name { get; }

    IGameInformation GameInformation { get; }

    IGame Clone();

    bool Move(IMove[] moves);

    bool Move(string[] uciStrings);

    bool Move(IMove move, bool isOneTimeMove = false);

    bool MoveFromUCI(string uciString);

    void UndoMove();
  }
}