namespace ChessDotCore.Engine.Interfaces
{
  public interface IMove
  {
    ISquare FromSquare { get; }
    IPiece MovingPiece { get; }
    ISquare ToSquare { get; }
  }
}