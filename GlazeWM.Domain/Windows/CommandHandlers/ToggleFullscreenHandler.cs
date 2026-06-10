using GlazeWM.Domain.Windows.Commands;
using GlazeWM.Infrastructure.Bussing;

namespace GlazeWM.Domain.Windows.CommandHandlers
{
  internal sealed class ToggleFullscreenHandler : ICommandHandler<ToggleFullscreenCommand>
  {
    private readonly Bus _bus;

    public ToggleFullscreenHandler(Bus bus)
    {
      _bus = bus;
    }

    public CommandResponse Handle(ToggleFullscreenCommand command)
    {
      var window = command.Window;

      if (window is FullscreenWindow fullscreenWindow)
      {
        if (fullscreenWindow.PreviousState == WindowType.Floating)
          _bus.Invoke(new SetFloatingCommand(window));
        else
          _bus.Invoke(new SetTilingCommand(window));
      }
      else
      {
        _bus.Invoke(new SetFullscreenCommand(window));
      }

      return CommandResponse.Ok;
    }
  }
}
