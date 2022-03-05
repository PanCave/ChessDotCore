using ChessDotCore.Engine.Interfaces;
using System;

namespace ChessDotCore.Engine
{
  internal class GameInformation : IGameInformation
  {
    public GameInformation(
      string @event = "Unknown Event",
      string site = "Unknown Site",
      DateTime date = new DateTime(),
      string round = "Unknown Round",
      string white = "Unknown Player",
      string black = "Unknown Player"
      )
    {
      Event = @event;
      Site = site;
      Date = date;
      Round = round;
      White = white;
      Black = black;
    }

    public string Event { get; }
    public string Site { get; }
    public DateTime Date { get; }
    public string Round { get; }
    public string White { get; }
    public string Black { get; }
  }
}