using GlazeWM.Domain.Containers;
using GlazeWM.Domain.Containers.Commands;
using GlazeWM.Domain.Windows.Commands;
using GlazeWM.Domain.Workspaces;
using GlazeWM.Infrastructure.Bussing;

namespace GlazeWM.Domain.Windows.CommandHandlers
{
  internal sealed class SetFullscreenHandler : ICommandHandler<SetFullscreenCommand>
  {
    private readonly Bus _bus;

    public SetFullscreenHandler(Bus bus)
    {
      _bus = bus;
    }

    public CommandResponse Handle(SetFullscreenCommand command)
    {
      var window = command.Window;

      if (window is FullscreenWindow)
        return CommandResponse.Ok;

      var workspace = WorkspaceService.GetWorkspaceFromChildContainer(window);
      var previousState = WindowService.GetWindowType(window);

      if (window is IResizable)
        _bus.Invoke(new MoveContainerWithinTreeCommand(window, workspace, true));
      else
        _bus.Invoke(new MoveContainerWithinTreeCommand(window, workspace, false));

      var fullscreenWindow = new FullscreenWindow(
        window.Handle,
        window.FloatingPlacement,
        window.BorderDelta,
        previousState
      );

      _bus.Invoke(new ReplaceContainerCommand(fullscreenWindow, window.Parent, window.Index));
      _bus.Invoke(new RedrawContainersCommand());

      return CommandResponse.Ok;
    }
  }
}
