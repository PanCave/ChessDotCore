using ChessDotCore.Engine.Interfaces;
using ChessDotCore.Engine.Utilities;
using ChessDotCore.Engine.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ChessDotCore.Tests")]
namespace ChessDotCore.Engine
{
  internal class Game : IGame
  {
    private readonly IEngineUtilities engineUtilities;
    private readonly ISquare[,] squares;

    public Game(
      string name,
      string @event = "Unknown Event",
      string site = "Unknown Site",
      DateTime date = new DateTime(),
      string round = "Unknown Round",
      string white = "Unknown Player",
      string black = "Unknown Player"
      )
    {
      engineUtilities = new EngineUtilities();
      Name = name;
      squares = new Square[8, 8];
      FillSquares();
      Board = CreateNewBoard();
      GameInformation = new GameInformation(@event, site, date, round, white, black);
    }

    public Game(
      IBoard board,
      string name
      )
    {
      engineUtilities = new EngineUtilities();
      Name = name;
      squares = board.Squares;
      Board = board;
      GameInformation = new GameInformation();
    }

    public IGame Clone()
    {
      IGame game = new Game(Name);
      (game as Game).CopyBoard(Board);
      return game;
    }

    public bool Move(IMove move, bool isOneTimeMove = false)
    {
      bool isLegal = engineUtilities.MakeMove(move, Board as Board);
      if (isLegal)
      {
        if (!isOneTimeMove)
        {
          Color nextTurn = Board.Turn == Color.White ? Color.Black : Color.White;
          bool nextCanWhiteKingSideCastle;
          bool nextCanWhiteQueenSideCastle;
          bool nextCanBlackKingSideCastle;
          bool nextCanBlackQueenSideCastle;
          if (Board.Turn == Color.White)
          {
            nextCanWhiteKingSideCastle = engineUtilities.CanWhiteStillKingSideCastle(move, Board);
            nextCanWhiteQueenSideCastle = engineUtilities.CanWhiteStillQueenSideCastle(move, Board);
            nextCanBlackKingSideCastle = Board.CanBlackKingSideCastle;
            nextCanBlackQueenSideCastle = Board.CanBlackQueenSideCastle;
          }
          else
          {
            nextCanWhiteKingSideCastle = Board.CanWhiteKingSideCastle;
            nextCanWhiteQueenSideCastle = Board.CanWhiteQueenSideCastle;
            nextCanBlackKingSideCastle = engineUtilities.CanBlackStillKingSideCastle(move, Board);
            nextCanBlackQueenSideCastle = engineUtilities.CanBlackStillQueenSideCastle(move, Board);
          }
          int nextHalfTurnsSincePawnMovementOrCapture = (move is CapturingMove || move.MovingPiece.PieceType == PieceType.Pawn) ? Board.HalfTurnsSincePawnMovementOrCapture + 1 : 0;

          bool nextIsCheck = engineUtilities.IsCheck(Board, nextTurn);

          int len = Board.MoveHistory.Length;
          string[] nextMoveHistory = new string[len + 1];
          Array.Copy(Board.MoveHistory, nextMoveHistory, len);
          nextMoveHistory[len] = move.ToString();

          Board board = new Board(
              nextCanBlackKingSideCastle,
              nextCanBlackQueenSideCastle,
              nextCanWhiteKingSideCastle,
              nextCanWhiteQueenSideCastle,
              (move is EnPassantEnablingMove m ? m.EnPassantSquare : null),
              nextHalfTurnsSincePawnMovementOrCapture,
              Board,
              move,
              nextTurn,
              (nextTurn == Color.White) ? Board.TurnNumber + 1 : Board.TurnNumber,
              Board.Pieces,
              nextIsCheck,
              squares,
              nextMoveHistory);
          UpdateGamestate();
          SetPiecesLists(board);
          List<IMove> legalMoves = engineUtilities.GetLegalMoves(board);
          board.LegalMoves = legalMoves;
          board.IsCheckMate = legalMoves.Count == 0;
          Board = board;
        }
        else
        {
          Color nextTurn = Board.Turn == Color.White ? Color.Black : Color.White;
          bool nextCanWhiteKingSideCastle = true;
          bool nextCanWhiteQueenSideCastle = true;
          bool nextCanBlackKingSideCastle = true;
          bool nextCanBlackQueenSideCastle = true;
          int nextHalfTurnsSincePawnMovementOrCapture = 0;

          bool nextIsCheck = engineUtilities.IsCheck(Board, nextTurn);

          Board board = new Board(
              nextCanWhiteKingSideCastle,
              nextCanWhiteQueenSideCastle,
              nextCanBlackKingSideCastle,
              nextCanBlackQueenSideCastle,
              null,
              nextHalfTurnsSincePawnMovementOrCapture,
              Board,
              move,
              nextTurn,
              Board.TurnNumber,
              Board.Pieces,
              nextIsCheck,
              squares,
              Array.Empty<string>());
          SetPiecesLists(board);
          board.LegalMoves = nextIsCheck ? engineUtilities.GetLegalMoves(board) : null;
          board.IsCheckMate = nextIsCheck && board.LegalMoves.Count == 0;
          Board = board;
        }
      }
      return isLegal;
    }

    public bool Move(IMove[] moves)
    {
      foreach (IMove move in moves)
      {
        if (!Move(move)) return false;
      }
      return true;
    }

    public bool Move(string[] uciStrings)
    {
      foreach (string uciString in uciStrings)
      {
        if (!MoveFromUCI(uciString)) return false;
      }
      return true;
    }

    public bool MoveFromUCI(string uciString)
    {
      if (uciString.Equals("O-O"))
      {
        return Move(new CastlingMove(Board[0, 4], Board[0, 6], Board[0, 4].Piece, Board[0, 7], Board[0, 5], Board[0, 7].Piece));
      }
      else if (uciString.Equals("o-o"))
      {
        return Move(new CastlingMove(Board[7, 4], Board[7, 6], Board[7, 4].Piece, Board[7, 7], Board[7, 5], Board[7, 7].Piece));
      }
      else if (uciString.Equals("O-O-O"))
      {
        return Move(new CastlingMove(Board[0, 4], Board[0, 2], Board[0, 4].Piece, Board[0, 0], Board[0, 3], Board[0, 0].Piece));
      }
      else if (uciString.Equals("o-o-o"))
      {
        return Move(new CastlingMove(Board[7, 4], Board[7, 2], Board[7, 4].Piece, Board[7, 0], Board[7, 3], Board[7, 0].Piece));
      }

      char fromFileChar = uciString[0];
      char fromRankChar = uciString[1];
      char toFileChar = uciString[2];
      char toRankChar = uciString[3];

      int fromFile = fromFileChar - 97;
      int fromRank = int.Parse(fromRankChar.ToString()) - 1;
      int toFile = toFileChar - 97;
      int toRank = int.Parse(toRankChar.ToString()) - 1;

      if (fromFile < 0 || fromFile > 7
        || fromRank < 0 || fromRank > 7
        || toFile < 0 || toFile > 7
        || toRank < 0 || toRank > 7) return false;

      ISquare fromSquare = Board[fromRank, fromFile];
      ISquare toSquare = Board[toRank, toFile];
      IPiece movingPiece = Board[fromRank, fromFile].Piece;

      if (toSquare.Piece != null)
      {
        return Move(new CapturingMove(fromSquare, toSquare, movingPiece, toSquare.Piece, true));
      }
      else if (uciString.Equals("e1g1"))
      {
        return Move(new CastlingMove(fromSquare, toSquare, movingPiece, Board[0, 7], Board[0, 5], Board[0, 7].Piece));
      }
      else if (uciString.Equals("e1c1"))
      {
        return Move(new CastlingMove(fromSquare, toSquare, movingPiece, Board[0, 0], Board[0, 2], Board[0, 0].Piece));
      }
      else if (uciString.Equals("e8g8"))
      {
        return Move(new CastlingMove(fromSquare, toSquare, movingPiece, Board[7, 7], Board[7, 5], Board[7, 7].Piece));
      }
      else if (uciString.Equals("e8c8"))
      {
        return Move(new CastlingMove(fromSquare, toSquare, movingPiece, Board[7, 0], Board[7, 2], Board[7, 0].Piece));
      }
      else if (movingPiece.PieceType == PieceType.Pawn && Math.Abs(fromRank - toRank) == 2)
      {
        return Move(new EnPassantEnablingMove(fromSquare, toSquare, movingPiece, Board[(fromRank + toRank) / 2, fromFile]));
      }
      else if (Board.EnPassantSquare != null
        && movingPiece.PieceType == PieceType.Pawn
        && movingPiece.Square.Rank == (movingPiece.Color == Color.White ? 4 : 3)
        && fromFile != toFile)
      {
        int direction = movingPiece.Color == Color.White ? -1 : 1;
        ISquare capturedPieceSquare = Board[Board.EnPassantSquare.Rank + direction, Board.EnPassantSquare.File];
        return Move(new EnPassantMove(fromSquare, toSquare, movingPiece, capturedPieceSquare.Piece, capturedPieceSquare));
      }
      else if (movingPiece.PieceType == PieceType.Pawn
        && movingPiece.Square.Rank == (movingPiece.Color == Color.White ? 6 : 1)
        && fromRank == (movingPiece.Color == Color.White ? 6 : 1)
        && toRank == (movingPiece.Color == Color.White ? 7 : 0)
        && uciString.Length > 4)
      {
        PieceType pieceType;
        switch (uciString[4])
        {
          case 'q': pieceType = PieceType.Queen; break;
          case 'r': pieceType = PieceType.Rook; break;
          case 'n': pieceType = PieceType.Knight; break;
          case 'b': pieceType = PieceType.Bishop; break;
          default: pieceType = PieceType.Queen; break;
        }
        return Move(new PromotionMove(fromSquare, toSquare, movingPiece, pieceType));
      }
      return Move(new StandardMove(fromSquare, toSquare, movingPiece));
    }

    public void UndoMove()
    {
      if ((Board as Board).LastBoard != null)
      {
        engineUtilities.UndoMove(Board.LastMove, (Board as Board));
        Board = (Board as Board).LastBoard;
      }
    }

    internal void CopyBoard(IBoard copyBoard)
    {
      Board board = new Board(
        copyBoard.CanWhiteKingSideCastle,
        copyBoard.CanWhiteQueenSideCastle,
        copyBoard.CanBlackKingSideCastle,
        copyBoard.CanBlackQueenSideCastle,
        (copyBoard.EnPassantSquare == null ? null : Board[copyBoard.EnPassantSquare.Rank, copyBoard.EnPassantSquare.File]),
        copyBoard.HalfTurnsSincePawnMovementOrCapture,
        null,
        null,
        copyBoard.Turn,
        copyBoard.TurnNumber,
        Board.Pieces,
        copyBoard.IsCheck,
        squares,
        (string[])copyBoard.MoveHistory.Clone());

      foreach (IPiece piece in Board.Pieces)
      {
        piece.Square = null;
      }

      foreach (IPiece copyBoardPiece in copyBoard.Pieces)
      {
        foreach (IPiece piece in Board.Pieces)
        {
          if (piece.PieceType == copyBoardPiece.PieceType
            && piece.Color == copyBoardPiece.Color
            && piece.Square == null)
          {
            int rank = copyBoardPiece.Square.Rank;
            int file = copyBoardPiece.Square.File;

            piece.Square = Board[rank, file];
            Board[rank, file].Piece = piece;
          }
        }
      }
      List<IMove> legalMoves = engineUtilities.GetLegalMoves(board);
      board.LegalMoves = legalMoves;
      board.IsCheckMate = legalMoves.Count == 0;
      Board = board;
    }

    private void CheckForEndGame()
    {
      byte count = 0;

      foreach (IPiece piece in Board[PieceType.Queen, Color.White, false])
      {
        count += 3;
      }
      foreach (IPiece piece in Board[PieceType.Queen, Color.Black, false])
      {
        count += 3;
      }

      foreach (IPiece piece in Board[PieceType.Bishop, Color.White, false])
      {
        count++;
      }
      foreach (IPiece piece in Board[PieceType.Bishop, Color.Black, false])
      {
        count++;
      }

      foreach (IPiece piece in Board[PieceType.Knight, Color.White, false])
      {
        count++;
      }
      foreach (IPiece piece in Board[PieceType.Knight, Color.Black, false])
      {
        count++;
      }

      foreach (IPiece piece in Board[PieceType.Rook, Color.White, false])
      {
        count += 2;
      }
      foreach (IPiece piece in Board[PieceType.Rook, Color.Black, false])
      {
        count += 2;
      }

      if (count > 14) (Board as Board).GameState = GameState.EndGame;
    }

    private void CheckForMiddleGame()
    {
      byte count = 0;
      if (!Board.CanWhiteKingSideCastle) count++;
      if (!Board.CanWhiteQueenSideCastle) count++;
      if (!Board.CanBlackKingSideCastle) count++;
      if (!Board.CanBlackQueenSideCastle) count++;

      if (Board[0, 1].Piece?.PieceType != PieceType.Knight) count += 2;
      if (Board[0, 6].Piece?.PieceType != PieceType.Knight) count += 2;
      if (Board[0, 1].Piece?.PieceType != PieceType.Bishop) count += 2;
      if (Board[0, 6].Piece?.PieceType != PieceType.Bishop) count += 2;
      if (Board[0, 3].Piece?.PieceType != PieceType.Queen) count += 3;
      if (Board[7, 1].Piece?.PieceType != PieceType.Knight) count += 2;
      if (Board[7, 6].Piece?.PieceType != PieceType.Knight) count += 2;
      if (Board[7, 1].Piece?.PieceType != PieceType.Bishop) count += 2;
      if (Board[7, 6].Piece?.PieceType != PieceType.Bishop) count += 2;
      if (Board[7, 3].Piece?.PieceType != PieceType.Queen) count += 3;

      if (count > 18) (Board as Board).GameState = GameState.MiddleGame;
    }

    private IBoard CreateNewBoard()
    {
      List<IPiece> pieces = engineUtilities.CreatePieces(squares);
      Board board = new Board(
          true,
          true,
          true,
          true,
          null,
          0,
          null,
          null,
          Color.White,
          1, pieces, false, squares, Array.Empty<string>())
      {
        IsCheckMate = false
      };
      engineUtilities.CreatePiecesList(board);
      List<IMove> legalMoves = engineUtilities.GetLegalMoves(board);
      board.LegalMoves = legalMoves;
      return board;
    }

    private void FillSquares()
    {
      for (int rank = 0; rank < 8; rank++)
      {
        for (int file = 0; file < 8; file++)
        {
          squares[rank, file] = new Square(rank, file);
        }
      }
    }

    private void SetPiecesLists(Board board)
    {
      Board b = Board as Board;
      board.AliveBlackBishopList = b.AliveBlackBishopList;
      board.AliveBlackKnightList = b.AliveBlackKnightList;
      board.AliveBlackPawnList = b.AliveBlackPawnList;
      board.AliveBlackQueenList = b.AliveBlackQueenList;
      board.AliveBlackRookList = b.AliveBlackRookList;
      board.AliveWhiteBishopList = b.AliveWhiteBishopList;
      board.AliveWhiteKnightList = b.AliveWhiteKnightList;
      board.AliveWhitePawnList = b.AliveWhitePawnList;
      board.AliveWhiteQueenList = b.AliveWhiteQueenList;
      board.AliveWhiteRookList = b.AliveWhiteRookList;
      board.DeadBlackBishopList = b.DeadBlackBishopList;
      board.DeadBlackKnightList = b.DeadBlackKnightList;
      board.DeadBlackPawnList = b.DeadBlackPawnList;
      board.DeadBlackQueenList = b.DeadBlackQueenList;
      board.DeadBlackRookList = b.DeadBlackRookList;
      board.DeadWhiteBishopList = b.DeadWhiteBishopList;
      board.DeadWhiteKnightList = b.DeadWhiteKnightList;
      board.DeadWhitePawnList = b.DeadWhitePawnList;
      board.DeadWhiteQueenList = b.DeadWhiteQueenList;
      board.DeadWhiteRookList = b.DeadWhiteRookList;
      board.AliveBlackPiecesList = b.AliveBlackPiecesList;
      board.AlivePiecesList = b.AlivePiecesList;
      board.AliveWhitePiecesList = b.AliveWhitePiecesList;
      board.BlackPiecesList = b.BlackPiecesList;
      board.DeadBlackPiecesList = b.DeadBlackPiecesList;
      board.DeadPiecesList = b.DeadPiecesList;
      board.DeadWhitePiecesList = b.DeadWhitePiecesList;
      board.WhitePiecesList = b.WhitePiecesList;
      board.WhiteBishopList = b.WhiteBishopList;
      board.WhiteKnightList = b.WhiteKnightList;
      board.WhitePawnList = b.WhitePawnList;
      board.WhiteQueenList = b.WhiteQueenList;
      board.WhiteRookList = b.WhiteRookList;
      board.BlackBishopList = b.BlackBishopList;
      board.BlackKnightList = b.BlackKnightList;
      board.BlackPawnList = b.BlackPawnList;
      board.BlackQueenList = b.BlackQueenList;
      board.BlackRookList = b.BlackRookList;
    }

    private void UpdateGamestate()
    {
      if (Board.GameState == GameState.Opening)
      {
        CheckForMiddleGame();
      }
      else if (Board.GameState == GameState.MiddleGame)
      {
        CheckForEndGame();
      }
    }

    internal void InitGamestate()
    {
      if (Board.GameState == GameState.Opening)
        CheckForMiddleGame();
      if (Board.GameState == GameState.MiddleGame)
        CheckForEndGame();
    }
    public IBoard Board { get; private set; }

    public IGameInformation GameInformation { get; }
    public string Name { get; }
  }
}