using System;
using GlazeWM.Domain.Monitors;
using GlazeWM.Domain.UserConfigs;
using GlazeWM.Domain.Workspaces;
using GlazeWM.Infrastructure;
using GlazeWM.Infrastructure.WindowsApi;
using GlazeWM.Infrastructure.Utils;

namespace GlazeWM.Domain.Windows
{
  public sealed class FullscreenWindow : Window
  {
    public readonly WindowType PreviousState;

    private readonly UserConfigService _userConfigService =
      ServiceLocator.GetRequiredService<UserConfigService>();

    private Monitor Monitor =>
      WorkspaceService.GetWorkspaceFromChildContainer(this).Parent as Monitor;

    private BarConfig BarForMonitor => _userConfigService.GetBarConfigForMonitor(Monitor);

    private int LogicalBarHeight
    {
      get
      {
        if (!BarForMonitor.Enabled)
          return 0;

        var barHeight = UnitsHelper.TrimUnits(BarForMonitor.Height);
        return Convert.ToInt32(barHeight * Monitor.ScaleFactor);
      }
    }

    public override int Width => Monitor.Width;

    public override int Height => Monitor.Height - LogicalBarHeight;

    public override int X => Monitor.X;

    public override int Y =>
      BarForMonitor.Position == BarPosition.Top
        ? Monitor.Y + LogicalBarHeight
        : Monitor.Y;

    public FullscreenWindow(
      IntPtr handle,
      Rect floatingPlacement,
      RectDelta borderDelta
    ) : this(handle, floatingPlacement, borderDelta, WindowType.Tiling)
    {
    }

    public FullscreenWindow(
      IntPtr handle,
      Rect floatingPlacement,
      RectDelta borderDelta,
      WindowType previousState
    ) : base(handle, floatingPlacement, borderDelta)
    {
      PreviousState = previousState;
    }
  }
}
