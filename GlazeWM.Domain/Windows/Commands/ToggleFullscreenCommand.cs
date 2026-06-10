using GlazeWM.Infrastructure.Bussing;

namespace GlazeWM.Domain.Windows.Commands
{
  public class ToggleFullscreenCommand : Command
  {
    public Window Window { get; }

    public ToggleFullscreenCommand(Window window)
    {
      Window = window;
    }
  }
}
