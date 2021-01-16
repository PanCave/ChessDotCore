namespace ChessDotCore.Engine.Interfaces
{
  public interface ISquare
  {
    int File { get; }
    IPiece Piece { get; set; }
    int Rank { get; }
    string UciCode { get; }
  }
}