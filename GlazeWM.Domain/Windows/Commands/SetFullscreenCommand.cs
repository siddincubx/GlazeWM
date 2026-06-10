using GlazeWM.Infrastructure.Bussing;

namespace GlazeWM.Domain.Windows.Commands
{
  public class SetFullscreenCommand : Command
  {
    public Window Window { get; }

    public SetFullscreenCommand(Window window)
    {
      Window = window;
    }
  }
}
