using System;

namespace ChessDotCore.Engine.Interfaces
{
  public interface IGameInformation
  {
    string Event { get; }
    string Site { get; }
    DateTime Date { get; }
    string Round { get; }
    string White { get; }
    string Black { get; }
  }
}