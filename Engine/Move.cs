using ChessDotCore.Engine.Interfaces;
using System;

namespace ChessDotCore.Engine
{
  public class CapturingMove : IMove
  {
    public CapturingMove(ISquare fromSquare, ISquare toSquare, IPiece movingPiece, IPiece capturedPiece, bool captured = true)
    {
      Captured = captured;
      CapturedPiece = capturedPiece;
      FromSquare = fromSquare;
      MovingPiece = movingPiece;
      ToSquare = toSquare;
    }

    public override bool Equals(object obj)
    {
      CapturingMove captureMove = obj as CapturingMove;
      return captureMove != null
             && captureMove.FromSquare == FromSquare
             && captureMove.ToSquare == ToSquare
             && captureMove.MovingPiece == MovingPiece
             && captureMove.Captured == Captured
             && captureMove.CapturedPiece == CapturedPiece;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(FromSquare, ToSquare, MovingPiece, Captured, CapturedPiece);
    }

    public override string ToString()
    {
      return FromSquare.UciCode + ToSquare.UciCode;
    }

    public bool Captured { get; }
    public IPiece CapturedPiece { get; }
    public ISquare FromSquare { get; }
    public IPiece MovingPiece { get; }
    public ISquare ToSquare { get; }
  }

  public class CapturingPromotionMove : IMove
  {
    public CapturingPromotionMove(ISquare fromSquare, ISquare toSquare, IPiece movingPiece, IPiece capturedPiece, PieceType promotedToPieceType)
    {
      FromSquare = fromSquare;
      ToSquare = toSquare;
      MovingPiece = movingPiece;
      CapturedPiece = capturedPiece;
      PromotedToPieceType = promotedToPieceType;
    }

    public override bool Equals(object obj)
    {
      CapturingPromotionMove capturingPromotionMove = obj as CapturingPromotionMove;
      return capturingPromotionMove != null
        && capturingPromotionMove.FromSquare == FromSquare
        && capturingPromotionMove.ToSquare == ToSquare
        && capturingPromotionMove.MovingPiece == MovingPiece
        && capturingPromotionMove.CapturedPiece == CapturedPiece
        && capturingPromotionMove.PromotedToPieceType == PromotedToPieceType;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(FromSquare, ToSquare, MovingPiece, CapturedPiece, PromotedToPieceType);
    }

    public override string ToString()
    {
      string pieceTypeChar = "";
      switch (PromotedToPieceType)
      {
        case PieceType.Queen: pieceTypeChar = "q"; break;
        case PieceType.Rook: pieceTypeChar = "r"; break;
        case PieceType.Knight: pieceTypeChar = "n"; break;
        case PieceType.Bishop: pieceTypeChar = "b"; break;
      }
      return FromSquare.UciCode + ToSquare.UciCode + pieceTypeChar;
    }

    public IPiece CapturedPiece { get; }
    public ISquare FromSquare { get; }
    public IPiece MovingPiece { get; }
    public PieceType PromotedToPieceType { get; }
    public ISquare ToSquare { get; }
  }

  public class CastlingMove : IMove
  {
    public CastlingMove(ISquare fromSquare, ISquare toSquare, IPiece movingPiece, ISquare rookFromSquare, ISquare rookToSquare, IPiece rook)
    {
      FromSquare = fromSquare;
      MovingPiece = movingPiece;
      Rook = rook;
      RookFromSquare = rookFromSquare;
      RookToSquare = rookToSquare;
      ToSquare = toSquare;
    }

    public override bool Equals(object obj)
    {
      if (obj is CastlingMove move)
      {
        return move.ToSquare == ToSquare
               && move.FromSquare == FromSquare
               && move.MovingPiece == MovingPiece
               && move.RookFromSquare == RookFromSquare
               && move.RookToSquare == RookToSquare
               && move.Rook == Rook;
      }
      return false;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(FromSquare, MovingPiece, Rook, RookFromSquare, RookToSquare, ToSquare);
    }

    public override string ToString()
    {
      return FromSquare.UciCode + ToSquare.UciCode;
    }

    public ISquare FromSquare { get; }
    public IPiece MovingPiece { get; }
    public IPiece Rook { get; }
    public ISquare RookFromSquare { get; }
    public ISquare RookToSquare { get; }
    public ISquare ToSquare { get; }
  }

  public class EnPassantEnablingMove : IMove
  {
    public EnPassantEnablingMove(ISquare fromSquare, ISquare toSquare, IPiece movingPiece, ISquare enPassantSquare)
    {
      FromSquare = fromSquare;
      ToSquare = toSquare;
      MovingPiece = movingPiece;
      EnPassantSquare = enPassantSquare;
    }

    public override bool Equals(object obj)
    {
      EnPassantEnablingMove enPassantEnablingMove = obj as EnPassantEnablingMove;
      return enPassantEnablingMove != null
        && enPassantEnablingMove.FromSquare == FromSquare
        && enPassantEnablingMove.ToSquare == ToSquare
        && enPassantEnablingMove.MovingPiece == MovingPiece
        && enPassantEnablingMove.EnPassantSquare == EnPassantSquare;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(FromSquare, ToSquare, MovingPiece);
    }

    public override string ToString()
    {
      return FromSquare.UciCode + ToSquare.UciCode;
    }

    public ISquare EnPassantSquare { get; }
    public ISquare FromSquare { get; }

    public IPiece MovingPiece { get; }
    public ISquare ToSquare { get; }
  }

  public class EnPassantMove : IMove
  {
    public EnPassantMove(ISquare fromSquare, ISquare toSquare, IPiece movingPiece, IPiece capturedPiece, ISquare capturedPieceSquare)
    {
      FromSquare = fromSquare;
      ToSquare = toSquare;
      MovingPiece = movingPiece;
      CapturedPiece = capturedPiece;
      CapturedPieceSquare = capturedPieceSquare;
    }

    public override bool Equals(object obj)
    {
      EnPassantMove enPassantMove = obj as EnPassantMove;
      return enPassantMove != null
        && enPassantMove.FromSquare == FromSquare
        && enPassantMove.ToSquare == ToSquare
        && enPassantMove.MovingPiece == MovingPiece
        && enPassantMove.CapturedPiece == CapturedPiece
        && enPassantMove.CapturedPieceSquare == CapturedPieceSquare;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(FromSquare, ToSquare, MovingPiece, CapturedPiece, CapturedPieceSquare);
    }

    public override string ToString()
    {
      return FromSquare.UciCode + ToSquare.UciCode;
    }

    public IPiece CapturedPiece { get; }
    public ISquare CapturedPieceSquare { get; }
    public ISquare FromSquare { get; }
    public IPiece MovingPiece { get; }
    public ISquare ToSquare { get; }
  }

  public class PromotionMove : IMove
  {
    public PromotionMove(ISquare fromSquare, ISquare toSquare, IPiece movingPiece, PieceType promotedToPieceType, bool promoted = true)
    {
      FromSquare = fromSquare;
      ToSquare = toSquare;
      MovingPiece = movingPiece;
      PromotedToPieceType = promotedToPieceType;
      Promoted = promoted;
    }

    public override bool Equals(object obj)
    {
      PromotionMove promotionMove = obj as PromotionMove;
      return promotionMove != null
        && promotionMove.FromSquare == FromSquare
        && promotionMove.ToSquare == ToSquare
        && promotionMove.MovingPiece == MovingPiece
        && promotionMove.PromotedToPieceType == PromotedToPieceType
        && promotionMove.Promoted == Promoted;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(FromSquare, ToSquare, MovingPiece, PromotedToPieceType, Promoted);
    }

    public override string ToString()
    {
      string pieceTypeChar = "";
      switch (PromotedToPieceType)
      {
        case PieceType.Queen: pieceTypeChar = "q"; break;
        case PieceType.Rook: pieceTypeChar = "r"; break;
        case PieceType.Knight: pieceTypeChar = "n"; break;
        case PieceType.Bishop: pieceTypeChar = "b"; break;
      }
      return FromSquare.UciCode + ToSquare.UciCode + pieceTypeChar;
    }

    public ISquare FromSquare { get; }
    public IPiece MovingPiece { get; }
    public bool Promoted { get; }
    public PieceType PromotedToPieceType { get; }
    public ISquare ToSquare { get; }
  }

  public class StandardMove : IMove
  {
    public StandardMove(ISquare fromSquare, ISquare toSquare, IPiece movingPiece)
    {
      FromSquare = fromSquare;
      ToSquare = toSquare;
      MovingPiece = movingPiece;
    }

    public override bool Equals(object obj)
    {
      StandardMove standardMove = obj as StandardMove;
      return standardMove != null && standardMove.FromSquare == FromSquare && standardMove.ToSquare == ToSquare;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(FromSquare, ToSquare, MovingPiece);
    }

    public override string ToString()
    {
      return FromSquare.UciCode + ToSquare.UciCode;
    }

    public ISquare FromSquare { get; }

    public IPiece MovingPiece { get; }

    public ISquare ToSquare { get; }
  }
}