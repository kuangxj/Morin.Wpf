using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Morin.Wpf.Views.Players;
public partial class PlayerView : Window, IDisposable
{
    private DateTime lastMouseMoveTime;
    private Point lastMousePosition;
    private DispatcherTimer mouseMoveTimer;
    public PlayerView()
    {
        InitializeComponent();

        Host.Surface.MouseMove += (s, e) =>
        {
            if (s is Window window)
            {
                var currentPosition = e.GetPosition(window);
                if (Math.Abs(currentPosition.X - lastMousePosition.X) > double.Epsilon ||
                    Math.Abs(currentPosition.Y - lastMousePosition.Y) > double.Epsilon)
                    lastMouseMoveTime = DateTime.UtcNow;

                lastMousePosition = currentPosition;
            }
        };

        Host.Surface.MouseLeave += (s, e) =>
        {
            lastMouseMoveTime = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(10));
        };

        mouseMoveTimer = new DispatcherTimer(DispatcherPriority.Background)
        {
            Interval = TimeSpan.FromMilliseconds(150),
            IsEnabled = true
        };
        mouseMoveTimer.Tick += MouseMoveTimer_Tick;
        mouseMoveTimer.Start();

        Closing += (s, e) =>
        {
            mouseMoveTimer.Stop();
        };
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    private void MouseMoveTimer_Tick(object? sender, EventArgs e)
    {
        var expireCount = 3000;
        var elapsedSinceMouseMove = DateTime.UtcNow.Subtract(lastMouseMoveTime);

        if (elapsedSinceMouseMove.TotalMilliseconds >= expireCount && Host.Player.IsPlaying && Host.Surface.IsMouseOver)
        {
            if (GridController.Visibility == Visibility.Visible)
            {
                Mouse.OverrideCursor = Cursors.None;
                GridController.Visibility = Visibility.Hidden;
            }
        }
        else
        {
            if (GridController.Visibility != Visibility.Visible)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                GridController.Visibility = Visibility.Visible;
            }
        }
    }
}