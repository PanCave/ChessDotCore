using ChessDotCore.Engine.Interfaces;
using ChessDotCore.Engine.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessDotCore.Engine.Utilities
{
  internal class EngineUtilities : IEngineUtilities
  {
    public bool CanBlackStillKingSideCastle(IMove move, IBoard board)
    {
      if (!board.CanBlackKingSideCastle) return false;
      return (move.MovingPiece.PieceType != PieceType.Rook
             || move.FromSquare.File != 7)
             && move.MovingPiece.PieceType != PieceType.King;
    }

    public bool CanBlackStillQueenSideCastle(IMove move, IBoard board)
    {
      if (!board.CanBlackQueenSideCastle) return false;
      return (move.MovingPiece.PieceType != PieceType.Rook
             || move.FromSquare.File != 0)
             && move.MovingPiece.PieceType != PieceType.King;
    }

    public bool CanWhiteStillKingSideCastle(IMove move, IBoard board)
    {
      if (!board.CanWhiteKingSideCastle) return false;
      return (move.MovingPiece.PieceType != PieceType.Rook
             || move.FromSquare.File != 7)
             && move.MovingPiece.PieceType != PieceType.King;
    }

    public bool CanWhiteStillQueenSideCastle(IMove move, IBoard board)
    {
      if (!board.CanWhiteQueenSideCastle) return false;
      return (move.MovingPiece.PieceType != PieceType.Rook
             || move.FromSquare.File != 0)
             && move.MovingPiece.PieceType != PieceType.King;
    }

    public List<IPiece> CreatePieces(ISquare[,] squares)
    {
      Color[] colors = (Color[])Enum.GetValues(typeof(Color));
      PieceType[] pieceTypes = (PieceType[])Enum.GetValues(typeof(PieceType));
      List<IPiece> pieces = new List<IPiece>();
      int rank;
      foreach (Color color in colors)
      {
        foreach (PieceType pieceType in pieceTypes)
        {
          switch (pieceType)
          {
            case PieceType.Pawn:
              rank = color == Color.White ? 1 : 6;
              for (int file = 0; file < 8; file++)
              {
                IPiece pawn = new Piece(color, pieceType, squares[rank, file]);
                squares[rank, file].Piece = pawn;
                pieces.Add(pawn);
              }
              break;

            case PieceType.Rook:
              rank = color == Color.White ? 0 : 7;
              IPiece leftRook = new Piece(color, pieceType, squares[rank, 0]);
              squares[rank, 0].Piece = leftRook;
              pieces.Add(leftRook);
              IPiece rightRook = new Piece(color, pieceType, squares[rank, 7]);
              squares[rank, 7].Piece = rightRook;
              pieces.Add(rightRook);
              break;

            case PieceType.Knight:
              rank = color == Color.White ? 0 : 7;
              IPiece leftKnight = new Piece(color, pieceType, squares[rank, 1]);
              squares[rank, 1].Piece = leftKnight;
              pieces.Add(leftKnight);
              IPiece rightKnight = new Piece(color, pieceType, squares[rank, 6]);
              squares[rank, 6].Piece = rightKnight;
              pieces.Add(rightKnight);
              break;

            case PieceType.Bishop:
              rank = color == Color.White ? 0 : 7;
              IPiece leftBishop = new Piece(color, pieceType, squares[rank, 2]);
              squares[rank, 2].Piece = leftBishop;
              pieces.Add(leftBishop);
              IPiece rightBishop = new Piece(color, pieceType, squares[rank, 5]);
              squares[rank, 5].Piece = rightBishop;
              pieces.Add(rightBishop);
              break;

            case PieceType.King:
              rank = color == Color.White ? 0 : 7;
              IPiece king = new Piece(color, pieceType, squares[rank, 4]);
              squares[rank, 4].Piece = king;
              pieces.Add(king);
              break;

            case PieceType.Queen:
              rank = color == Color.White ? 0 : 7;
              IPiece queen = new Piece(color, pieceType, squares[rank, 3]);
              squares[rank, 3].Piece = queen;
              pieces.Add(queen);
              break;
          }
        }
      }
      return pieces;
    }

    public void CreatePiecesList(Board board)
    {
      board.WhitePawnList = new List<IPiece>();
      board.WhiteKnightList = new List<IPiece>();
      board.WhiteBishopList = new List<IPiece>();
      board.WhiteRookList = new List<IPiece>();
      board.WhiteQueenList = new List<IPiece>();

      board.BlackPawnList = new List<IPiece>();
      board.BlackKnightList = new List<IPiece>();
      board.BlackBishopList = new List<IPiece>();
      board.BlackRookList = new List<IPiece>();
      board.BlackQueenList = new List<IPiece>();

      board.DeadWhitePawnList = new List<IPiece>();
      board.DeadWhiteKnightList = new List<IPiece>();
      board.DeadWhiteBishopList = new List<IPiece>();
      board.DeadWhiteRookList = new List<IPiece>();
      board.DeadWhiteQueenList = new List<IPiece>();

      board.DeadBlackPawnList = new List<IPiece>();
      board.DeadBlackKnightList = new List<IPiece>();
      board.DeadBlackBishopList = new List<IPiece>();
      board.DeadBlackRookList = new List<IPiece>();
      board.DeadBlackQueenList = new List<IPiece>();

      board.AliveWhitePawnList = new List<IPiece>();
      board.AliveWhiteKnightList = new List<IPiece>();
      board.AliveWhiteBishopList = new List<IPiece>();
      board.AliveWhiteRookList = new List<IPiece>();
      board.AliveWhiteQueenList = new List<IPiece>();

      board.AliveBlackPawnList = new List<IPiece>();
      board.AliveBlackKnightList = new List<IPiece>();
      board.AliveBlackBishopList = new List<IPiece>();
      board.AliveBlackRookList = new List<IPiece>();
      board.AliveBlackQueenList = new List<IPiece>();

      board.AliveWhitePiecesList = new List<IPiece>();
      board.AliveBlackPiecesList = new List<IPiece>();

      board.DeadWhitePiecesList = new List<IPiece>();
      board.DeadBlackPiecesList = new List<IPiece>();

      board.WhitePiecesList = new List<IPiece>();
      board.BlackPiecesList = new List<IPiece>();

      board.AlivePiecesList = new List<IPiece>();
      board.DeadPiecesList = new List<IPiece>();

      foreach (IPiece piece in board.Pieces)
      {
        AddPieceToCorrectList(board, piece);
      }
    }

    public List<IMove> GetLegalMoves(Board board)
    {
      List<IMove> pseudoLegalMoves = GetPseudoLegalMoves(board);

      TransformPseudoLegalMovesToLegalMoves(pseudoLegalMoves, board);

      return pseudoLegalMoves;
    }

    public List<IMove> GetPseudoLegalMoves(IBoard board)
    {
      List<IMove> pseudoLegalMoves = new List<IMove>();

      foreach (IPiece piece in board[true])
      {
        if (piece.Square != null)
        {
          List<IMove> movesToAdd = GetPseudoLegalPieceMoves(piece, board);
          pseudoLegalMoves.AddRange(movesToAdd);
        }
      }

      return pseudoLegalMoves;
    }

    public bool IsCheck(IBoard board, Color turn)
    {
      ISquare kingSquare = null;
      bool isCheck;
      foreach (IPiece piece in board.Pieces)
      {
        if (piece.Square != null && piece.PieceType == PieceType.King && piece.Color == turn)
        {
          kingSquare = piece.Square;
        }
      }

      isCheck = CheckPawnPutsInCheck(kingSquare, turn, board);
      isCheck |= CheckBishopLikePutsInCheck(kingSquare, turn, board);
      isCheck |= CheckRookLikePutsInCheck(kingSquare, turn, board);
      isCheck |= CheckKnightPutsInCheck(kingSquare, turn, board);

      return isCheck;
    }

    public bool IsCheck(List<IPiece> pieces, ISquare[,] squares, Color turn)
    { 
      ISquare kingSquare = null;
      bool isCheck;
      foreach (IPiece piece in pieces)
      {
        if (piece.Square != null && piece.PieceType == PieceType.King && piece.Color == turn)
        {
          kingSquare = piece.Square;
        }
      }

      isCheck = CheckPawnPutsInCheck(kingSquare, turn, squares);
      isCheck |= CheckBishopLikePutsInCheck(kingSquare, turn, squares);
      isCheck |= CheckRookLikePutsInCheck(kingSquare, turn, squares);
      isCheck |= CheckKnightPutsInCheck(kingSquare, turn, squares);

      return isCheck;
    }

    public bool IsCheckMate(IBoard board)
    {
      return board.LegalMoves.Count == 0;
    }

    public bool MakeMove(IMove move, Board board)
    {
      bool a = move is StandardMove standardMove && board.LegalMoves.Contains(standardMove);
      bool b = move is CapturingMove capturingMove && board.LegalMoves.Contains(capturingMove);
      bool c = move is CastlingMove castlingMove && board.LegalMoves.Contains(castlingMove);
      bool d = move is EnPassantEnablingMove enPassantEnablingMove && board.LegalMoves.Contains(enPassantEnablingMove);
      bool e = move is EnPassantMove enPassantMove && board.LegalMoves.Contains(enPassantMove);
      bool f = move is PromotionMove promotionMove && board.LegalMoves.Contains(promotionMove);
      bool g = move is CapturingPromotionMove capturingPromotionMove && board.LegalMoves.Contains(capturingPromotionMove);

      if (a || b || c || d || e || f || g)
      {
        MakeTheoMove(move, board);
        return true;
      }
      return false;
    }

    public IBoard UndoMove(IBoard board)
    {
      return (board as Board).LastBoard;
    }

    public void UndoMove(IMove move, Board board)
    {
      if (move is CapturingMove captureMove && (move as CapturingMove).Captured)
      {
        captureMove.FromSquare.Piece = captureMove.MovingPiece;
        captureMove.ToSquare.Piece = captureMove.CapturedPiece;
        captureMove.MovingPiece.Square = captureMove.FromSquare;
        captureMove.CapturedPiece.Square = captureMove.ToSquare;

        MovePieceFromDeadToAlive(captureMove.CapturedPiece, board);
      }
      else if (move is CastlingMove m)
      {
        m.FromSquare.Piece = m.MovingPiece;
        m.ToSquare.Piece = null;
        m.RookFromSquare.Piece = m.Rook;
        m.RookToSquare.Piece = null;
        m.MovingPiece.Square = m.FromSquare;
        m.Rook.Square = m.RookFromSquare;
      }
      else if (move is EnPassantMove enPassantMove)
      {
        enPassantMove.FromSquare.Piece = enPassantMove.MovingPiece;
        enPassantMove.ToSquare.Piece = null;
        enPassantMove.CapturedPieceSquare.Piece = enPassantMove.CapturedPiece;

        enPassantMove.MovingPiece.Square = enPassantMove.FromSquare;
        enPassantMove.CapturedPiece.Square = enPassantMove.CapturedPieceSquare;

        MovePieceFromDeadToAlive(enPassantMove.CapturedPiece, board);
      }
      else if (move is PromotionMove promotionMove)
      {
        promotionMove.FromSquare.Piece = promotionMove.MovingPiece;
        promotionMove.ToSquare.Piece = null;
        promotionMove.MovingPiece.Square = promotionMove.FromSquare;
        (promotionMove.MovingPiece as Piece).PieceType = PieceType.Pawn;

        MovePieceFromOtherToPawn(promotionMove.MovingPiece, promotionMove.PromotedToPieceType, board);
      }
      else if (move is CapturingPromotionMove capturingPromotionMove)
      {
        capturingPromotionMove.FromSquare.Piece = capturingPromotionMove.MovingPiece;
        capturingPromotionMove.ToSquare.Piece = capturingPromotionMove.CapturedPiece;
        capturingPromotionMove.MovingPiece.Square = capturingPromotionMove.FromSquare;
        capturingPromotionMove.CapturedPiece.Square = capturingPromotionMove.ToSquare;

        (capturingPromotionMove.MovingPiece as Piece).PieceType = PieceType.Pawn;

        MovePieceFromDeadToAlive(capturingPromotionMove.CapturedPiece, board);
        MovePieceFromOtherToPawn(capturingPromotionMove.MovingPiece, capturingPromotionMove.PromotedToPieceType, board);
      }
      else
      {
        move.FromSquare.Piece = move.MovingPiece;
        move.ToSquare.Piece = null;
        move.MovingPiece.Square = move.FromSquare;
      }
    }

    private void AddPieceToCorrectList(Board board, IPiece piece)
    {
      board.AlivePiecesList.Add(piece);
      if (piece.Color == Color.White)
      {
        board.AliveWhitePiecesList.Add(piece);
        board.WhitePiecesList.Add(piece);
        switch (piece.PieceType)
        {
          case PieceType.Pawn: board.AliveWhitePawnList.Add(piece); board.WhitePawnList.Add(piece); break;
          case PieceType.Bishop: board.AliveWhiteBishopList.Add(piece); board.WhiteBishopList.Add(piece); break;
          case PieceType.Knight: board.AliveWhiteKnightList.Add(piece); board.WhiteKnightList.Add(piece); break;
          case PieceType.Rook: board.AliveWhiteRookList.Add(piece); board.WhiteRookList.Add(piece); break;
          case PieceType.Queen: board.AliveWhiteQueenList.Add(piece); board.WhiteQueenList.Add(piece); break;
        }
      }
      else
      {
        board.AliveBlackPiecesList.Add(piece);
        board.BlackPiecesList.Add(piece);
        switch (piece.PieceType)
        {
          case PieceType.Pawn: board.AliveBlackPawnList.Add(piece); board.BlackPawnList.Add(piece); break;
          case PieceType.Bishop: board.AliveBlackBishopList.Add(piece); board.BlackBishopList.Add(piece); break;
          case PieceType.Knight: board.AliveBlackKnightList.Add(piece); board.BlackKnightList.Add(piece); break;
          case PieceType.Rook: board.AliveBlackRookList.Add(piece); board.BlackRookList.Add(piece); break;
          case PieceType.Queen: board.AliveBlackQueenList.Add(piece); board.BlackQueenList.Add(piece); break;
        }
      }
    }

    private bool CheckBishopLikePutsInCheck(ISquare kingSquare, Color turn, IBoard board)
    {
      int[] directionArray = new int[2] { -1, 1 };
      foreach (int direction in directionArray)
      {
        int offset;
        ISquare opponentSquare;
        IPiece opponentPiece;
        for (offset = 1; offset < 8; offset++)
        {
          opponentSquare = board[kingSquare.Rank + direction * offset, kingSquare.File + direction * offset];
          if (opponentSquare != null)
          {
            opponentPiece = opponentSquare.Piece;
            if (opponentPiece != null)
            {
              if (opponentPiece.Color == turn) break;
              if (opponentPiece.Color == turn) break;
              else
              {
                if (opponentPiece.PieceType == PieceType.Bishop || opponentPiece.PieceType == PieceType.Queen)
                  return true;
                else break;
              }
            }
          }
        }

        for (offset = 1; offset < 8; offset++)
        {
          opponentSquare = board[kingSquare.Rank + direction * offset, kingSquare.File - direction * offset];
          if (opponentSquare != null)
          {
            opponentPiece = opponentSquare.Piece;
            if (opponentPiece != null)
            {
              if (opponentPiece.Color == turn) break;
              else
              {
                if (opponentPiece.PieceType == PieceType.Bishop || opponentPiece.PieceType == PieceType.Queen)
                  return true;
                else break;
              }
            }
          }
        }
      }

      return false;
    }
    private bool CheckBishopLikePutsInCheck(ISquare kingSquare, Color turn, ISquare[,] squares)
    {
      int[] directionArray = new int[2] { -1, 1 };
      foreach (int direction in directionArray)
      {
        int offset;
        ISquare opponentSquare;
        IPiece opponentPiece;
        for (offset = 1; offset < 8; offset++)
        {
          opponentSquare = GetSquare(kingSquare.Rank + direction * offset, kingSquare.File + direction * offset, squares);
          if (opponentSquare != null)
          {
            opponentPiece = opponentSquare.Piece;
            if (opponentPiece != null)
            {
              if (opponentPiece.Color == turn) break;
              if (opponentPiece.Color == turn) break;
              else
              {
                if (opponentPiece.PieceType == PieceType.Bishop || opponentPiece.PieceType == PieceType.Queen)
                  return true;
                else break;
              }
            }
          }
        }

        for (offset = 1; offset < 8; offset++)
        {
          opponentSquare = GetSquare(kingSquare.Rank + direction * offset, kingSquare.File - direction * offset, squares);
          if (opponentSquare != null)
          {
            opponentPiece = opponentSquare.Piece;
            if (opponentPiece != null)
            {
              if (opponentPiece.Color == turn) break;
              else
              {
                if (opponentPiece.PieceType == PieceType.Bishop || opponentPiece.PieceType == PieceType.Queen)
                  return true;
                else break;
              }
            }
          }
        }
      }

      return false;
    }

    private bool CheckKnightPutsInCheck(ISquare kingSquare, Color turn, IBoard board)
    {
      ISquare opponentSquare;
      IPiece opponentPiece;
      int[,] possibleMoves = new int[,] { { -1, 2 }, { -1, -2 }, { 1, 2 }, { 1, -2 }, { 2, 1 }, { 2, -1 }, { -2, -1 }, { -2, 1 } };
      for (int index = 0; index < possibleMoves.GetLength(0); index++)
      {
        opponentSquare = board[kingSquare.Rank + possibleMoves[index, 0], kingSquare.File + possibleMoves[index, 1]];
        if (opponentSquare != null)
        {
          opponentPiece = opponentSquare.Piece;
          if (opponentPiece != null && opponentPiece.Color != turn && opponentPiece.PieceType == PieceType.Knight)
            return true;
        }
      }

      return false;
    }
    private bool CheckKnightPutsInCheck(ISquare kingSquare, Color turn, ISquare[,] squares)
    {
      ISquare opponentSquare;
      IPiece opponentPiece;
      int[,] possibleMoves = new int[,] { { -1, 2 }, { -1, -2 }, { 1, 2 }, { 1, -2 }, { 2, 1 }, { 2, -1 }, { -2, -1 }, { -2, 1 } };
      for (int index = 0; index < possibleMoves.GetLength(0); index++)
      {
        opponentSquare = GetSquare(kingSquare.Rank + possibleMoves[index, 0], kingSquare.File + possibleMoves[index, 1], squares);
        if (opponentSquare != null)
        {
          opponentPiece = opponentSquare.Piece;
          if (opponentPiece != null && opponentPiece.Color != turn && opponentPiece.PieceType == PieceType.Knight)
            return true;
        }
      }

      return false;
    }

    private bool CheckPawnPutsInCheck(ISquare kingSquare, Color kingColor, IBoard board)
    {
      int direction = kingColor == Color.White ? 1 : -1;
      bool isCheck = false;
      Color opponentColor = kingColor == Color.White ? Color.Black : Color.White;

      ISquare leftSquare = board[kingSquare.Rank + direction, kingSquare.File + 1];
      if (leftSquare != null)
      {
        IPiece leftPiece = leftSquare.Piece;
        isCheck |= leftPiece?.PieceType == PieceType.Pawn && leftPiece?.Color == opponentColor;
      }

      ISquare rightSquare = board[kingSquare.Rank + direction, kingSquare.File - 1];
      if (rightSquare != null)
      {
        IPiece rightPiece = rightSquare.Piece;
        isCheck |= rightPiece?.PieceType == PieceType.Pawn && rightPiece?.Color == opponentColor;
      }
      return isCheck;
    }
    private bool CheckPawnPutsInCheck(ISquare kingSquare, Color kingColor, ISquare[,] squares)
    {
      int direction = kingColor == Color.White ? 1 : -1;
      bool isCheck = false;
      Color opponentColor = kingColor == Color.White ? Color.Black : Color.White;

      ISquare leftSquare = GetSquare(kingSquare.Rank + direction, kingSquare.File + 1, squares);
      if (leftSquare != null)
      {
        IPiece leftPiece = leftSquare.Piece;
        isCheck |= leftPiece?.PieceType == PieceType.Pawn && leftPiece?.Color == opponentColor;
      }

      ISquare rightSquare = GetSquare(kingSquare.Rank + direction, kingSquare.File - 1, squares);
      if (rightSquare != null)
      {
        IPiece rightPiece = rightSquare.Piece;
        isCheck |= rightPiece?.PieceType == PieceType.Pawn && rightPiece?.Color == opponentColor;
      }
      return isCheck;
    }

    private bool CheckRookLikePutsInCheck(ISquare kingSquare, Color turn, IBoard board)
    {
      int[] directionArray = new int[2] { -1, 1 };
      foreach (int direction in directionArray)
      {
        int offset;
        ISquare opponentSquare;
        IPiece opponentPiece;
        for (offset = 1; offset < 8; offset++)
        {
          opponentSquare = board[kingSquare.Rank + direction * offset, kingSquare.File];
          if (opponentSquare != null)
          {
            opponentPiece = opponentSquare.Piece;
            if (opponentPiece != null)
            {
              if (opponentPiece.Color == turn) break;
              else
              {
                if (opponentPiece.PieceType == PieceType.Rook || opponentPiece.PieceType == PieceType.Queen)
                  return true;
                else break;
              }
            }
          }
        }

        for (offset = 1; offset < 8; offset++)
        {
          opponentSquare = board[kingSquare.Rank, kingSquare.File - direction * offset];
          if (opponentSquare != null)
          {
            opponentPiece = opponentSquare.Piece;
            if (opponentPiece != null)
            {
              if (opponentPiece.Color == turn) break;
              else
              {
                if (opponentPiece.PieceType == PieceType.Rook || opponentPiece.PieceType == PieceType.Queen)
                  return true;
                else break;
              }
            }
          }
        }
      }

      return false;
    }
    private bool CheckRookLikePutsInCheck(ISquare kingSquare, Color turn, ISquare[,] squares)
    {
      int[] directionArray = new int[2] { -1, 1 };
      foreach (int direction in directionArray)
      {
        int offset;
        ISquare opponentSquare;
        IPiece opponentPiece;
        for (offset = 1; offset < 8; offset++)
        {
          opponentSquare = GetSquare(kingSquare.Rank + direction * offset, kingSquare.File, squares);
          if (opponentSquare != null)
          {
            opponentPiece = opponentSquare.Piece;
            if (opponentPiece != null)
            {
              if (opponentPiece.Color == turn) break;
              else
              {
                if (opponentPiece.PieceType == PieceType.Rook || opponentPiece.PieceType == PieceType.Queen)
                  return true;
                else break;
              }
            }
          }
        }

        for (offset = 1; offset < 8; offset++)
        {
          opponentSquare = GetSquare(kingSquare.Rank, kingSquare.File - direction * offset, squares);
          if (opponentSquare != null)
          {
            opponentPiece = opponentSquare.Piece;
            if (opponentPiece != null)
            {
              if (opponentPiece.Color == turn) break;
              else
              {
                if (opponentPiece.PieceType == PieceType.Rook || opponentPiece.PieceType == PieceType.Queen)
                  return true;
                else break;
              }
            }
          }
        }
      }

      return false;
    }

    private IEnumerable<IMove> GetPseudoLegalBishopLikeMoves(IPiece piece, IBoard board)
    {
      List<IMove> moves = new List<IMove>();

      int[] directionArray = new int[2] { -1, 1 };
      foreach (int direction in directionArray)
      {
        int offset;
        ISquare toSquare;
        IPiece capturedPiece;
        for (offset = 1; offset < 8; offset++)
        {
          toSquare = board[piece.Square.Rank + direction * offset, piece.Square.File + direction * offset];
          if (toSquare != null)
          {
            capturedPiece = toSquare.Piece;
            if (capturedPiece == null)
            {
              piece.ReachableSquares.Add(toSquare);
              if (piece.Color == board.Turn) moves.Add(new StandardMove(piece.Square, toSquare, piece));
            }
            else
            {
              if (capturedPiece.Color != piece.Color)
              {
                piece.ReachableSquares.Add(toSquare);
                piece.AttackedSquares.Add(toSquare);
                if (piece.Color == board.Turn) moves.Add(new CapturingMove(piece.Square, toSquare, piece, capturedPiece, true));
                break;
              }
              else
              {
                piece.ProtectedSquares.Add(toSquare);
                break;
              }
            }
          }
          else
          {
            break;
          }
        }

        for (offset = 1; offset < 8; offset++)
        {
          toSquare = board[piece.Square.Rank + direction * offset, piece.Square.File - direction * offset];
          if (toSquare != null)
          {
            capturedPiece = toSquare.Piece;
            if (capturedPiece == null)
            {
              piece.ReachableSquares.Add(toSquare);
              if (piece.Color == board.Turn) moves.Add(new StandardMove(piece.Square, toSquare, piece));
            }
            else
            {
              if (capturedPiece.Color != piece.Color)
              {
                piece.ReachableSquares.Add(toSquare);
                piece.AttackedSquares.Add(toSquare);
                if (piece.Color == board.Turn) moves.Add(new CapturingMove(piece.Square, toSquare, piece, capturedPiece, true));
                break;
              }
              else
              {
                piece.ProtectedSquares.Add(toSquare);
                break;
              }
            }
          }
          else
          {
            break;
          }
        }
      }

      return moves;
    }

    private IEnumerable<IMove> GetPseudoLegalKingLikeMoves(IPiece piece, IBoard board)
    {
      List<IMove> moves = new List<IMove>();

      bool canKingMoveKingSide = false;
      bool canKingMoveQueenSide = false;

      IPiece opponentKing = board.Pieces.First(x => x.PieceType == PieceType.King && x.Color != piece.Color);

      ISquare toSquare;
      IPiece capturedPiece;
      for (int rankOffset = -1; rankOffset <= 1; rankOffset++)
      {
        for (int fileOffset = -1; fileOffset <= 1; fileOffset++)
        {
          if (rankOffset != 0 || fileOffset != 0)
          {
            toSquare = board[piece.Square.Rank + rankOffset, piece.Square.File + fileOffset];
            if (toSquare != null && Math.Abs(toSquare.Rank - opponentKing.Square.Rank) + Math.Abs(toSquare.File - opponentKing.Square.File) > 2)
            {
              capturedPiece = toSquare.Piece;
              if (capturedPiece != null)
              {
                if (capturedPiece.Color != piece.Color)
                {
                  piece.ReachableSquares.Add(toSquare);
                  piece.AttackedSquares.Add(toSquare);
                  if (piece.Color == board.Turn) moves.Add(new CapturingMove(piece.Square, toSquare, piece, capturedPiece, true));
                }
                else
                {
                  piece.ProtectedSquares.Add(toSquare);
                }
              }
              else
              {
                piece.ReachableSquares.Add(toSquare);
                if (piece.Color == board.Turn)
                {
                  moves.Add(new StandardMove(piece.Square, toSquare, piece));
                  if (rankOffset == 0 && fileOffset == 1) canKingMoveKingSide = true;
                  if (rankOffset == 0 && fileOffset == -1) canKingMoveQueenSide = true;
                }
              }
            }
          }
        }
      }
      if (piece.Color == board.Turn)
      {
        int rank = board.Turn == Color.White ? 0 : 7;
        if (((board.Turn == Color.White && board.CanWhiteKingSideCastle) || (board.Turn == Color.Black && board.CanBlackKingSideCastle)) && canKingMoveKingSide)
        {
          IPiece shouldBeKing = board[rank, 4].Piece;
          IPiece shouldBeEmpty1 = board[rank, 5].Piece;
          IPiece shouldBeEmpty2 = board[rank, 6].Piece;
          IPiece shouldBeRook = board[rank, 7].Piece;
          if (shouldBeKing != null && shouldBeKing.PieceType == PieceType.King
            && shouldBeEmpty1 == null && shouldBeEmpty2 == null
            && shouldBeRook != null && shouldBeRook.PieceType == PieceType.Rook)
            moves.Add(new CastlingMove(board[rank, 4], board[rank, 6], piece, board[rank, 7], board[rank, 5], shouldBeRook));
        }
        if (((board.Turn == Color.White && board.CanWhiteQueenSideCastle) || (board.Turn == Color.Black && board.CanBlackQueenSideCastle)) && canKingMoveQueenSide)
        {
          IPiece shouldBeKing = board[rank, 4].Piece;
          IPiece shouldBeEmpty1 = board[rank, 3].Piece;
          IPiece shouldBeEmpty2 = board[rank, 2].Piece;
          IPiece shouldBeEmpty3 = board[rank, 1].Piece;
          IPiece shouldBeRook = board[rank, 0].Piece;
          if (shouldBeKing != null && shouldBeKing.PieceType == PieceType.King
            && shouldBeEmpty1 == null && shouldBeEmpty2 == null && shouldBeEmpty3 == null
            && shouldBeRook != null && shouldBeRook.PieceType == PieceType.Rook)
            moves.Add(new CastlingMove(board[rank, 4], board[rank, 2], piece, board[rank, 0], board[rank, 3], shouldBeRook));
        }
      }

      return moves;
    }

    private IEnumerable<IMove> GetPseudoLegalKnightLikeMoves(IPiece piece, IBoard board)

    {
      List<IMove> moves = new List<IMove>();

      ISquare toSquare;
      IPiece capturedPiece;
      int[,] possibleMoves = new int[,] { { -1, 2 }, { -1, -2 }, { 1, 2 }, { 1, -2 }, { 2, 1 }, { 2, -1 }, { -2, -1 }, { -2, 1 } };
      for (int index = 0; index < possibleMoves.GetLength(0); index++)
      {
        toSquare = board[piece.Square.Rank + possibleMoves[index, 0], piece.Square.File + possibleMoves[index, 1]];
        if (toSquare != null)
        {
          capturedPiece = toSquare.Piece;
          if (capturedPiece != null)
          {
            if (capturedPiece.Color != piece.Color)
            {
              piece.ReachableSquares.Add(toSquare);
              piece.AttackedSquares.Add(toSquare);
              if (piece.Color == board.Turn) moves.Add(new CapturingMove(piece.Square, toSquare, piece, capturedPiece, true));
            }
            else
            {
              piece.ProtectedSquares.Add(toSquare);
            }
          }
          else
          {
            piece.ReachableSquares.Add(toSquare);
            if (piece.Color == board.Turn) moves.Add(new StandardMove(piece.Square, toSquare, piece));
          }
        }
      }

      return moves;
    }

    private IEnumerable<IMove> GetPseudoLegalPawnLikeMoves(IPiece piece, IBoard board)
    {
      List<IMove> moves = new List<IMove>();
      int startingRank = piece.Color == Color.White ? 1 : 6;
      int direction = piece.Color == Color.White ? 1 : -1;
      int almostPromotingRank = piece.Color == Color.White ? 6 : 1;

      if (board.EnPassantSquare != null
        && board.EnPassantSquare.Rank == (piece.Color == Color.White ? 5 : 2)
        && piece.Square.Rank == (piece.Color == Color.White ? 4 : 3)
        && (piece.Square.File == board.EnPassantSquare.File - 1 || piece.Square.File == board.EnPassantSquare.File + 1))
      {
        IPiece capturedPiece = board[board.EnPassantSquare.Rank - direction, board.EnPassantSquare.File].Piece;
        moves.Add(new EnPassantMove(piece.Square, board.EnPassantSquare, piece, capturedPiece, capturedPiece.Square));
      }

      ISquare toSquare = board[piece.Square.Rank + direction, piece.Square.File];
      if (toSquare != null && toSquare.Piece == null)
      {
        piece.ReachableSquares.Add(toSquare);
        if (piece.Color == board.Turn)
        {
          if (piece.Square.Rank != almostPromotingRank)
          {
            moves.Add(new StandardMove(piece.Square, toSquare, piece));
          }
          else
          {
            moves.Add(new PromotionMove(piece.Square, toSquare, piece, PieceType.Queen, true));
            moves.Add(new PromotionMove(piece.Square, toSquare, piece, PieceType.Knight, true));
            moves.Add(new PromotionMove(piece.Square, toSquare, piece, PieceType.Bishop, true));
            moves.Add(new PromotionMove(piece.Square, toSquare, piece, PieceType.Rook, true));
          }
        }
        if (piece.Square.Rank == startingRank)
        {
          toSquare = board[piece.Square.Rank + 2 * direction, piece.Square.File];
          if (toSquare != null && toSquare.Piece == null)
          {
            piece.ReachableSquares.Add(toSquare);
            if (piece.Color == board.Turn) moves.Add(new EnPassantEnablingMove(piece.Square, toSquare, piece, board[piece.Square.Rank + direction, piece.Square.File]));
          }
        }
      }
      int[] fileOffsetArray = new int[2] { -1, 1 };
      foreach (int fileOffset in fileOffsetArray)
      {
        toSquare = board[piece.Square.Rank + direction, piece.Square.File + fileOffset];
        if (toSquare != null)
        {
          IPiece capturedPiece = toSquare.Piece;
          if (capturedPiece != null)
          {
            if (capturedPiece.Color != piece.Color)
            {
              piece.AttackedSquares.Add(toSquare);
              if (piece.Color == board.Turn)
              {
                if (piece.Square.Rank != almostPromotingRank)
                {
                  moves.Add(new CapturingMove(piece.Square, capturedPiece.Square, piece, capturedPiece, true));
                }
                else
                {
                  moves.Add(new CapturingPromotionMove(piece.Square, toSquare, piece, capturedPiece, PieceType.Queen));
                  moves.Add(new CapturingPromotionMove(piece.Square, toSquare, piece, capturedPiece, PieceType.Knight));
                  moves.Add(new CapturingPromotionMove(piece.Square, toSquare, piece, capturedPiece, PieceType.Bishop));
                  moves.Add(new CapturingPromotionMove(piece.Square, toSquare, piece, capturedPiece, PieceType.Rook));
                }
              }
            }
            else
            {
              piece.ProtectedSquares.Add(toSquare);
            }
          }
        }
      }

      return moves;
    }

    private List<IMove> GetPseudoLegalPieceMoves(IPiece piece, IBoard board)
    {
      List<IMove> moves = new List<IMove>();
      ResetSquareLists(piece);
      if (piece.Square == null) return moves;
      switch (piece.PieceType)
      {
        case PieceType.Pawn:
          IEnumerable<IMove> pawnMoves = GetPseudoLegalPawnLikeMoves(piece, board);
          moves.AddRange(pawnMoves);
          board.LegalPawnMoves.AddRange(pawnMoves);
          break;

        case PieceType.Rook:
          IEnumerable<IMove> rookMoves = GetPseudoLegalRookLikeMoves(piece, board);
          moves.AddRange(rookMoves);
          board.LegalRookMoves.AddRange(rookMoves);
          break;

        case PieceType.Bishop:
          IEnumerable<IMove> bishopMoves = GetPseudoLegalBishopLikeMoves(piece, board);
          moves.AddRange(bishopMoves);
          board.LegalBishopMoves.AddRange(bishopMoves);
          break;

        case PieceType.Queen:
          IEnumerable<IMove> queenRookMoves = GetPseudoLegalRookLikeMoves(piece, board);
          IEnumerable<IMove> queenBishopMoves = GetPseudoLegalBishopLikeMoves(piece, board);
          moves.AddRange(queenRookMoves);
          board.LegalQueenMoves.AddRange(queenRookMoves);
          moves.AddRange(queenBishopMoves);
          board.LegalQueenMoves.AddRange(queenBishopMoves);
          break;

        case PieceType.King:
          IEnumerable<IMove> kingMoves = GetPseudoLegalKingLikeMoves(piece, board);
          moves.AddRange(kingMoves);
          board.LegalKingMoves.AddRange(kingMoves);
          break;

        case PieceType.Knight:
          IEnumerable<IMove> knightMoves = GetPseudoLegalKnightLikeMoves(piece, board);
          moves.AddRange(knightMoves);
          board.LegalKnightMoves.AddRange(knightMoves);
          break;
      }

      return moves;
    }

    private IEnumerable<IMove> GetPseudoLegalRookLikeMoves(IPiece piece, IBoard board)
    {
      List<IMove> moves = new List<IMove>();
      int[] directionArray = new int[2] { -1, 1 };
      foreach (int direction in directionArray)
      {
        int offset;
        ISquare toSquare;
        IPiece capturedPiece;
        for (offset = 1; offset < 8; offset++)
        {
          toSquare = board[piece.Square.Rank + direction * offset, piece.Square.File];
          if (toSquare != null)
          {
            capturedPiece = toSquare.Piece;
            if (capturedPiece == null)
            {
              piece.ReachableSquares.Add(toSquare);
              if (piece.Color == board.Turn) moves.Add(new StandardMove(piece.Square, toSquare, piece));
            }
            else
            {
              if (capturedPiece.Color != piece.Color)
              {
                piece.ReachableSquares.Add(toSquare);
                piece.AttackedSquares.Add(toSquare);
                if (piece.Color == board.Turn) moves.Add(new CapturingMove(piece.Square, toSquare, piece, capturedPiece, true));
                break;
              }
              else
              {
                piece.ProtectedSquares.Add(toSquare);
                break;
              }
            }
          }
          else
          {
            break;
          }
        }

        for (offset = 1; offset < 8; offset++)
        {
          toSquare = board[piece.Square.Rank, piece.Square.File + direction * offset];
          if (toSquare != null)
          {
            capturedPiece = toSquare.Piece;
            if (capturedPiece == null)
            {
              piece.ReachableSquares.Add(toSquare);
              if (piece.Color == board.Turn) moves.Add(new StandardMove(piece.Square, toSquare, piece));
            }
            else
            {
              if (capturedPiece.Color != piece.Color)
              {
                piece.ReachableSquares.Add(toSquare);
                piece.AttackedSquares.Add(toSquare);
                if (piece.Color == board.Turn) moves.Add(new CapturingMove(piece.Square, toSquare, piece, capturedPiece, true));
                break;
              }
              else
              {
                piece.ProtectedSquares.Add(toSquare);
                break;
              }
            }
          }
          else
          {
            break;
          }
        }
      }

      return moves;
    }

    private void MakeTheoMove(IMove move, Board board)
    {
      if (move is CastlingMove castlingMove)
      {
        castlingMove.FromSquare.Piece = null;
        castlingMove.ToSquare.Piece = castlingMove.MovingPiece;
        castlingMove.RookFromSquare.Piece = null;
        castlingMove.RookToSquare.Piece = castlingMove.Rook;
        castlingMove.MovingPiece.Square = castlingMove.ToSquare;
        castlingMove.Rook.Square = castlingMove.RookToSquare;
      }
      else if (move is CapturingMove captureMove)
      {
        captureMove.FromSquare.Piece = null;
        captureMove.ToSquare.Piece = captureMove.MovingPiece;

        captureMove.MovingPiece.Square = captureMove.ToSquare;
        captureMove.CapturedPiece.Square = null;

        MovePieceFromAliveToDead(captureMove.CapturedPiece, board);
      }
      else if (move is PromotionMove promotionMove)
      {
        ISquare fromSquare = promotionMove.FromSquare;
        ISquare toSquare = promotionMove.ToSquare;

        fromSquare.Piece = null;
        toSquare.Piece = promotionMove.MovingPiece;

        promotionMove.MovingPiece.Square = toSquare;
        (promotionMove.MovingPiece as Piece).PieceType = promotionMove.PromotedToPieceType;

        MovePieceFromPawnToOther(promotionMove.MovingPiece, promotionMove.PromotedToPieceType, board);
      }
      else if (move is EnPassantEnablingMove enPassantEnablingMove)
      {
        enPassantEnablingMove.FromSquare.Piece = null;
        enPassantEnablingMove.ToSquare.Piece = enPassantEnablingMove.MovingPiece;
        enPassantEnablingMove.MovingPiece.Square = enPassantEnablingMove.ToSquare;
      }
      else if (move is EnPassantMove enPassantMove)
      {
        enPassantMove.FromSquare.Piece = null;
        enPassantMove.ToSquare.Piece = enPassantMove.MovingPiece;

        enPassantMove.MovingPiece.Square = enPassantMove.ToSquare;
        enPassantMove.CapturedPiece.Square = null;
        enPassantMove.CapturedPieceSquare.Piece = null;

        MovePieceFromAliveToDead(enPassantMove.CapturedPiece, board);
      }
      else if (move is CapturingPromotionMove capturingPromotionMove)
      {
        capturingPromotionMove.FromSquare.Piece = null;
        capturingPromotionMove.ToSquare.Piece = capturingPromotionMove.MovingPiece;

        capturingPromotionMove.MovingPiece.Square = capturingPromotionMove.ToSquare;
        capturingPromotionMove.CapturedPiece.Square = null;

        (capturingPromotionMove.MovingPiece as Piece).PieceType = capturingPromotionMove.PromotedToPieceType;

        MovePieceFromAliveToDead(capturingPromotionMove.CapturedPiece, board);
        MovePieceFromPawnToOther(capturingPromotionMove.MovingPiece, capturingPromotionMove.PromotedToPieceType, board);
      }
      else
      {
        ISquare fromSquare = move.FromSquare;
        ISquare toSquare = move.ToSquare;

        fromSquare.Piece = null;
        toSquare.Piece = move.MovingPiece;

        move.MovingPiece.Square = toSquare;
      }
    }

    private void MovePieceFromAliveToDead(IPiece piece, Board board)
    {
      board.AlivePiecesList.Remove(piece);
      board.DeadPiecesList.Add(piece);
      if (piece.Color == Color.White)
      {
        board.AliveWhitePiecesList.Remove(piece);
        board.DeadWhitePiecesList.Add(piece);
        switch (piece.PieceType)
        {
          case PieceType.Pawn:
            board.AliveWhitePawnList.Remove(piece);
            board.DeadWhitePawnList.Add(piece);
            break;

          case PieceType.Bishop:
            board.AliveWhiteBishopList.Remove(piece);
            board.DeadWhiteBishopList.Add(piece);
            break;

          case PieceType.Knight:
            board.AliveWhiteKnightList.Remove(piece);
            board.DeadWhiteKnightList.Add(piece);
            break;

          case PieceType.Rook:
            board.AliveWhiteRookList.Remove(piece);
            board.DeadWhiteRookList.Add(piece);
            break;

          case PieceType.Queen:
            board.AliveWhiteQueenList.Remove(piece);
            board.DeadWhiteQueenList.Add(piece);
            break;
        }
      }
      else
      {
        board.AliveBlackPiecesList.Remove(piece);
        board.DeadBlackPiecesList.Add(piece);
        switch (piece.PieceType)
        {
          case PieceType.Pawn:
            board.AliveBlackPawnList.Remove(piece);
            board.DeadBlackPawnList.Add(piece);
            break;

          case PieceType.Bishop:
            board.AliveBlackBishopList.Remove(piece);
            board.DeadBlackBishopList.Add(piece);
            break;

          case PieceType.Knight:
            board.AliveBlackKnightList.Remove(piece);
            board.DeadBlackKnightList.Add(piece);
            break;

          case PieceType.Rook:
            board.AliveBlackRookList.Remove(piece);
            board.DeadBlackRookList.Add(piece);
            break;

          case PieceType.Queen:
            board.AliveBlackQueenList.Remove(piece);
            board.DeadBlackQueenList.Add(piece);
            break;
        }
      }
    }

    private void MovePieceFromDeadToAlive(IPiece piece, Board board)
    {
      board.AlivePiecesList.Add(piece);
      board.DeadPiecesList.Remove(piece);
      if (piece.Color == Color.White)
      {
        board.AliveWhitePiecesList.Add(piece);
        board.DeadWhitePiecesList.Remove(piece);
        switch (piece.PieceType)
        {
          case PieceType.Pawn:
            board.AliveWhitePawnList.Add(piece);
            board.DeadWhitePawnList.Remove(piece);
            break;

          case PieceType.Bishop:
            board.AliveWhiteBishopList.Add(piece);
            board.DeadWhiteBishopList.Remove(piece);
            break;

          case PieceType.Knight:
            board.AliveWhiteKnightList.Add(piece);
            board.DeadWhiteKnightList.Remove(piece);
            break;

          case PieceType.Rook:
            board.AliveWhiteRookList.Add(piece);
            board.DeadWhiteRookList.Remove(piece);
            break;

          case PieceType.Queen:
            board.AliveWhiteQueenList.Add(piece);
            board.DeadWhiteQueenList.Remove(piece);
            break;
        }
      }
      else
      {
        board.AliveBlackPiecesList.Add(piece);
        board.DeadBlackPiecesList.Remove(piece);
        switch (piece.PieceType)
        {
          case PieceType.Pawn:
            board.AliveBlackPawnList.Add(piece);
            board.DeadBlackPawnList.Remove(piece);
            break;

          case PieceType.Bishop:
            board.AliveBlackBishopList.Add(piece);
            board.DeadBlackBishopList.Remove(piece);
            break;

          case PieceType.Knight:
            board.AliveBlackKnightList.Add(piece);
            board.DeadBlackKnightList.Remove(piece);
            break;

          case PieceType.Rook:
            board.AliveBlackRookList.Add(piece);
            board.DeadBlackRookList.Remove(piece);
            break;

          case PieceType.Queen:
            board.AliveBlackQueenList.Add(piece);
            board.DeadBlackQueenList.Remove(piece);
            break;
        }
      }
    }

    private void MovePieceFromOtherToPawn(IPiece piece, PieceType promotedToPieceType, Board board)
    {
      if (piece.Color == Color.White)
      {
        board.AliveWhitePawnList.Add(piece);
        board.WhitePawnList.Add(piece);
        switch (promotedToPieceType)
        {
          case PieceType.Queen:
            board.AliveWhiteQueenList.Remove(piece);
            board.WhiteQueenList.Remove(piece);
            break;

          case PieceType.Rook:
            board.AliveWhiteRookList.Remove(piece);
            board.WhiteRookList.Remove(piece);
            break;

          case PieceType.Bishop:
            board.AliveWhiteBishopList.Remove(piece);
            board.WhiteBishopList.Remove(piece);
            break;

          case PieceType.Knight:
            board.AliveWhiteKnightList.Remove(piece);
            board.WhiteKnightList.Remove(piece);
            break;
        }
      }
      else
      {
        board.AliveBlackPawnList.Add(piece);
        board.BlackPawnList.Add(piece);
        switch (promotedToPieceType)
        {
          case PieceType.Queen:
            board.AliveBlackQueenList.Remove(piece);
            board.BlackQueenList.Remove(piece);
            break;

          case PieceType.Rook:
            board.AliveBlackRookList.Remove(piece);
            board.BlackRookList.Remove(piece);
            break;

          case PieceType.Bishop:
            board.AliveBlackBishopList.Remove(piece);
            board.BlackBishopList.Remove(piece);
            break;

          case PieceType.Knight:
            board.AliveBlackKnightList.Remove(piece);
            board.BlackKnightList.Remove(piece);
            break;
        }
      }
    }

    private void MovePieceFromPawnToOther(IPiece piece, PieceType promotedToPieceType, Board board)
    {
      if (piece.Color == Color.White)
      {
        board.AliveWhitePawnList.Remove(piece);
        board.WhitePawnList.Remove(piece);
        switch (promotedToPieceType)
        {
          case PieceType.Queen:
            board.AliveWhiteQueenList.Add(piece);
            board.WhiteQueenList.Add(piece);
            break;

          case PieceType.Rook:
            board.AliveWhiteRookList.Add(piece);
            board.WhiteRookList.Add(piece);
            break;

          case PieceType.Bishop:
            board.AliveWhiteBishopList.Add(piece);
            board.WhiteBishopList.Add(piece);
            break;

          case PieceType.Knight:
            board.AliveWhiteKnightList.Add(piece);
            board.WhiteKnightList.Add(piece);
            break;
        }
      }
      else
      {
        board.AliveBlackPawnList.Remove(piece);
        board.BlackPawnList.Remove(piece);
        switch (promotedToPieceType)
        {
          case PieceType.Queen:
            board.AliveBlackQueenList.Add(piece);
            board.BlackQueenList.Add(piece);
            break;

          case PieceType.Rook:
            board.AliveBlackRookList.Add(piece);
            board.BlackRookList.Add(piece);
            break;

          case PieceType.Bishop:
            board.AliveBlackBishopList.Add(piece);
            board.BlackBishopList.Add(piece);
            break;

          case PieceType.Knight:
            board.AliveBlackKnightList.Add(piece);
            board.BlackKnightList.Add(piece);
            break;
        }
      }
    }

    private void ResetSquareLists(IPiece piece)
    {
      piece.ReachableSquares.Clear();
      piece.AttackedSquares.Clear();
      piece.ProtectedSquares.Clear();
    }

    private void TransformPseudoLegalMovesToLegalMoves(List<IMove> pseudoLegalMoves, Board board)
    {
      List<IMove> toBeRemovedMoves = new List<IMove>();
      List<IMove> legalCapturingMoves = new List<IMove>();
      foreach (IMove move in pseudoLegalMoves)
      {
        if (move is CastlingMove)
        {
          if (IsCheck(board, board.Turn))
          {
            toBeRemovedMoves.Add(move);
          }
          else
          {
            int rank = move.ToSquare.Rank;
            int file = move.ToSquare.File == 6 ? 5 : 3;
            bool kingCanMoveSideways = true;
            /** This only works, because King's 'normal stepping' moves
             * are calculated BEFORE the castling moves.
             * This however is okay, since this method only gets called
             * by asking for legal moves and everything happens in one
             * un-interrupted go. 
             */

            foreach (IMove m in toBeRemovedMoves)
            {
              if (m.ToSquare.Rank == rank && m.ToSquare.File == file) kingCanMoveSideways = false;
            }
            if (!kingCanMoveSideways) toBeRemovedMoves.Add(move);
          }
        }
        MakeTheoMove(move, board);
        if (IsCheck(board, board.Turn))
        {
          toBeRemovedMoves.Add(move);
        }
        else if (move is CapturingMove || move is EnPassantMove || move is CapturingPromotionMove)
          legalCapturingMoves.Add(move);
        UndoMove(move, board);
      }
      board.LegalCapturingMoves = legalCapturingMoves;
      foreach (IMove move in toBeRemovedMoves)
      {
        pseudoLegalMoves.Remove(move);
        switch (move.MovingPiece.PieceType)
        {
          case PieceType.Pawn: board.LegalPawnMoves.Remove(move); break;
          case PieceType.Bishop: board.LegalBishopMoves.Remove(move); break;
          case PieceType.Knight: board.LegalKnightMoves.Remove(move); break;
          case PieceType.Rook: board.LegalRookMoves.Remove(move); break;
          case PieceType.Queen: board.LegalQueenMoves.Remove(move); break;
          case PieceType.King: board.LegalKingMoves.Remove(move); break;
        }
      }
    }

    private ISquare GetSquare(int rank, int file, ISquare[,] squares)
    {
      if (rank < 0 || rank >= 8 || file < 0 || file >= 8) return null;
      return squares[rank, file];
    }
  }
}