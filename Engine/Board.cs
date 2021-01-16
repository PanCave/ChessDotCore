using ChessDotCore.Engine.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace ChessDotCore.Engine
{
  internal class Board : IBoard
  {
    private readonly ISquare[,] squares;
    private string fen;

    public Board(
        bool canBlackKingSideCastle,
        bool canBlackQueenSideCastle,
        bool canWhiteKingSideCastle,
        bool canWhiteQueenSideCastle,
        ISquare enPassantSquare,
        int halfTurnsSincePawnMovementOrCapture,
        IBoard lastBoard,
        IMove lastMove,
        Color turn,
        int turnNumber,
        List<IPiece> pieces,
        bool isCheck,
        ISquare[,] squares)
    {
      CanBlackKingSideCastle = canBlackKingSideCastle;
      CanBlackQueenSideCastle = canBlackQueenSideCastle;
      CanWhiteKingSideCastle = canWhiteKingSideCastle;
      CanWhiteQueenSideCastle = canWhiteQueenSideCastle;
      EnPassantSquare = enPassantSquare;
      HalfTurnsSincePawnMovementOrCapture = halfTurnsSincePawnMovementOrCapture;
      LastBoard = lastBoard;
      LastMove = lastMove;
      Turn = turn;
      TurnNumber = turnNumber;
      Pieces = pieces;
      IsCheck = isCheck;
      this.squares = squares;
      LegalPawnMoves = new List<IMove>();
      LegalBishopMoves = new List<IMove>();
      LegalKnightMoves = new List<IMove>();
      LegalRookMoves = new List<IMove>();
      LegalQueenMoves = new List<IMove>();
      LegalKingMoves = new List<IMove>();
    }

    public IPiece BlackKing { get; }

    public bool CanBlackKingSideCastle { get; }

    public bool CanBlackQueenSideCastle { get; }

    public bool CanWhiteKingSideCastle { get; }

    public bool CanWhiteQueenSideCastle { get; }

    public ISquare EnPassantSquare { get; internal set; }

    public string Fen => GetFen();

    public int HalfTurnsSincePawnMovementOrCapture { get; }

    public bool IsCheck { get; }

    public bool IsCheckMate { get; internal set; }

    public bool IsDraw => HalfTurnsSincePawnMovementOrCapture >= 50;

    public IBoard LastBoard { get; }

    public IMove LastMove { get; }

    public List<IMove> LegalMoves { get; set; }

    public List<IPiece> Pieces { get; }

    public Color Turn { get; }

    public int TurnNumber { get; }

    public IPiece WhiteKing { get; }

    public List<IMove> LegalPawnMoves { get; }

    public List<IMove> LegalBishopMoves { get; }

    public List<IMove> LegalKnightMoves { get; }

    public List<IMove> LegalRookMoves { get; }

    public List<IMove> LegalQueenMoves { get; }

    public List<IMove> LegalKingMoves { get; }

    public GameState GameState { get; internal set; }
    internal List<IPiece> AliveBlackPiecesList { get; set; }

    internal List<IPiece> AlivePiecesList { get; set; }

    internal List<IPiece> AliveWhitePiecesList { get; set; }

    internal List<IPiece> BlackPiecesList { get; set; }

    internal List<IPiece> DeadBlackPiecesList { get; set; }

    internal List<IPiece> DeadPiecesList { get; set; }

    internal List<IPiece> DeadWhitePiecesList { get; set; }

    internal List<IPiece> WhitePiecesList { get; set; }

    public ISquare this[int rank, int file]
    {
      get
      {
        if (rank < 0 || rank >= 8 || file < 0 || file >= 8) return null;
        return squares[rank, file];
      }
    }

    public List<IPiece> this[PieceType pieceType, Color color, bool alive]
    {
      get
      {
        switch (pieceType)
        {
          case PieceType.Pawn:
            switch (color)
            {
              case Color.White: return alive ? AliveWhitePawnList : DeadWhitePawnList;
              case Color.Black: return alive ? AliveBlackPawnList : DeadBlackPawnList;
            }
            break;

          case PieceType.Bishop:
            switch (color)
            {
              case Color.White: return alive ? AliveWhiteBishopList : DeadWhiteBishopList;
              case Color.Black: return alive ? AliveBlackBishopList : DeadBlackBishopList;
            }
            break;

          case PieceType.Knight:
            switch (color)
            {
              case Color.White: return alive ? AliveWhiteKnightList : DeadWhiteKnightList;
              case Color.Black: return alive ? AliveBlackKnightList : DeadBlackKnightList;
            }
            break;

          case PieceType.Rook:
            switch (color)
            {
              case Color.White: return alive ? AliveWhiteRookList : DeadWhiteRookList;
              case Color.Black: return alive ? AliveBlackRookList : DeadBlackRookList;
            }
            break;

          case PieceType.Queen:
            switch (color)
            {
              case Color.White: return alive ? AliveWhiteQueenList : DeadWhiteQueenList;
              case Color.Black: return alive ? AliveBlackQueenList : DeadBlackQueenList;
            }
            break;
        }
        return null;
      }
    }

    public List<IPiece> this[PieceType pieceType, Color color]
    {
      get
      {
        switch (pieceType)
        {
          case PieceType.Pawn: return color == Color.White ? WhitePawnList : BlackPawnList;
          case PieceType.Bishop: return color == Color.White ? WhiteBishopList : BlackBishopList;
          case PieceType.Knight: return color == Color.White ? WhiteKnightList : BlackKnightList;
          case PieceType.Rook: return color == Color.White ? WhiteRookList : BlackRookList;
          case PieceType.Queen: return color == Color.White ? WhiteQueenList : BlackQueenList;
        }
        return null;
      }
    }

    public List<IPiece> this[Color color]
    {
      get
      {
        return color == Color.White ? WhitePiecesList : BlackPiecesList;
      }
    }

    public List<IPiece> this[Color color, bool alive]
    {
      get
      {
        if (alive) return color == Color.White ? AliveWhitePiecesList : AliveBlackPiecesList;
        return color == Color.White ? DeadWhitePiecesList : DeadBlackPiecesList;
      }
    }

    public List<IPiece> this[bool alive]
    {
      get
      {
        return alive ? AlivePiecesList : DeadPiecesList;
      }
    }

    private string GetFen()
    {
      if (fen == null)
      {
        List<string> fenParts = new List<string>();
        List<string> boardParts = new List<string>();
        StringBuilder sb;
        for (int i = 8 - 1; i >= 0; i--)
        {
          sb = new StringBuilder();
          byte currentFreeSpace = 0;
          for (int j = 0; j < 8; j++)
          {
            IPiece piece = this[i, j].Piece;
            if (piece != null)
            {
              if (currentFreeSpace > 0)
              {
                sb.Append(currentFreeSpace);
                currentFreeSpace = 0;
              }
              sb.Append(piece.PieceChar);
            }
            else
            {
              currentFreeSpace++;
            }
          }
          if (currentFreeSpace > 0) sb.Append(currentFreeSpace);
          boardParts.Add(sb.ToString());
        }
        fenParts.Add(string.Join("/", boardParts));
        fenParts.Add(Turn == Color.White ? "w" : "b");
        sb = new StringBuilder();
        if (CanWhiteKingSideCastle) sb.Append("K");
        if (CanWhiteQueenSideCastle) sb.Append("Q");
        if (CanBlackKingSideCastle) sb.Append("k");
        if (CanBlackQueenSideCastle) sb.Append("q");
        fenParts.Add(sb.ToString());
        if (EnPassantSquare != null) fenParts.Add($"{(char)(EnPassantSquare.File + 97)}{EnPassantSquare.Rank + 1}");
        else fenParts.Add("-");
        fenParts.Add($"{HalfTurnsSincePawnMovementOrCapture}");
        fenParts.Add($"{TurnNumber}");
        fen = string.Join(" ", fenParts);
      }
      return fen;
    }

    #region Alive Pieces

    internal List<IPiece> AliveBlackBishopList { get; set; }
    internal List<IPiece> AliveBlackKnightList { get; set; }
    internal List<IPiece> AliveBlackPawnList { get; set; }
    internal List<IPiece> AliveBlackQueenList { get; set; }
    internal List<IPiece> AliveBlackRookList { get; set; }
    internal List<IPiece> AliveWhiteBishopList { get; set; }
    internal List<IPiece> AliveWhiteKnightList { get; set; }
    internal List<IPiece> AliveWhitePawnList { get; set; }
    internal List<IPiece> AliveWhiteQueenList { get; set; }
    internal List<IPiece> AliveWhiteRookList { get; set; }

    #endregion Alive Pieces

    #region Dead Pieces

    internal List<IPiece> DeadBlackBishopList { get; set; }
    internal List<IPiece> DeadBlackKnightList { get; set; }
    internal List<IPiece> DeadBlackPawnList { get; set; }
    internal List<IPiece> DeadBlackQueenList { get; set; }
    internal List<IPiece> DeadBlackRookList { get; set; }
    internal List<IPiece> DeadWhiteBishopList { get; set; }
    internal List<IPiece> DeadWhiteKnightList { get; set; }
    internal List<IPiece> DeadWhitePawnList { get; set; }
    internal List<IPiece> DeadWhiteQueenList { get; set; }
    internal List<IPiece> DeadWhiteRookList { get; set; }

    #endregion Dead Pieces

    #region White PieceType Lists

    internal List<IPiece> WhiteBishopList { get; set; }
    internal List<IPiece> WhiteKnightList { get; set; }
    internal List<IPiece> WhitePawnList { get; set; }
    internal List<IPiece> WhiteQueenList { get; set; }
    internal List<IPiece> WhiteRookList { get; set; }

    #endregion White PieceType Lists

    #region Black PieceType Lists

    internal List<IPiece> BlackBishopList { get; set; }
    internal List<IPiece> BlackKnightList { get; set; }
    internal List<IPiece> BlackPawnList { get; set; }
    internal List<IPiece> BlackQueenList { get; set; }
    internal List<IPiece> BlackRookList { get; set; }

    #endregion Black PieceType Lists
  }
}