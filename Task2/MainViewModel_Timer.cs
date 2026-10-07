using System.Windows.Threading;

namespace Task2
{
    public partial class MainViewModel
    {
        private DispatcherTimer _gameTimer;
        private DateTime _startTime;

        private void SetupTimer()
        {
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += GameTimer_Tick;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            var elapsed = DateTime.Now - _startTime;
            TimeDisplay = elapsed.ToString(@"mm\:ss");
        }

        public void StartTimer()
        {
            _startTime = DateTime.Now;
            _gameTimer.Start();
        }

        public void StopTimer()
        {
            _gameTimer?.Stop();
        }

        private void ResetTimerDisplay()
        {
            TimeDisplay = "00:00";
        }
    }
}