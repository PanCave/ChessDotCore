# ChessDotCore
A chess engine written in C# (DotNetCore), optimized for creating bots



###Getting Started

To start a game, you can just create a chess object. This chess object is capable of creating multiple games:

```c#
IChess chess = new Chess();
IGame myGame = chess.CreateGame("MyGameName");
IGame myOtherGame = chess.CreateGame("MyOtherGameName");
```

Once you have a game, you are free to play the match. Each game has a property "Board", which holds all necessary information for the current board position. If for example you want to get all possible moves for the current board position, you can get them from the board:

```c#
List<IMove> legalMoves = myGame.Board.LegalMoves;
```

The board holds a lot more information, like "IsCheckMate", "Fen" and many more, which can be read in the documentation.

Moves can be played by either using the UCI-format or by passing move object to the game:

```c#
// Move using UCI-format
myGame.MoveFromUCI("e2e4");

// Move using move object
List<IMove> moves = MyGame.Board.LegalMoves;
MyGame.Move(move.First()); // this moves the pawn on Square a7 to a6
```



### Writing a Bot

As mention at the beginning, this engine is specifically designed to aid in writing efficient bots, who for example use the Minimax-Algorithm. For this case the engine provides a unifying interface for bots:

```c#
public interface IChessBot
{
  IMove MakeMove();
}
```

So, writing a simple bot is extremely easy. Let's have look at a bot playing random legal moves:

```c#
public class RandomBot : IChessBot
{
  private readonly IGame game;
  private readonly Random random;

  public RandomBot(IGame game)
  {
    this.game = game;
    random = new Random();
  }
  
  public IMove MakeMove()
  {
    IMove[] moves = game.Board.LegalMoves.ToArray();
    return moves[random.Next(moves.Length)];
  }
}

public class Program
{
    public static void main(string[] args)
    {
      IChess chess = new Chess();
      IGame botGame = chess.CreateGame("Bot Showdown");
      IChessBot white = new RandomBot(botGame);
      IChessBot black = new RandomBot(botGame);
        
      while(true)
      {
        // White's move
        IMove whiteMove = white.MakeMove();
        botGame.Move(whiteMove);
        if(botGame.IsCheckMate)
          break;
          
        // Black's move
        IMove blackMove = black.MakeMove();
        botGame.Move(whiteMove);
        if(botGame.IsCheckMate)
          break;  
      }
      
      Console.WriteLine($"{game.Turn} has lost");
    }
}
```

