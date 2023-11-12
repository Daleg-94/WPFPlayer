using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WPFPlayer.Controls
{
    internal class WPFPlayer : MediaElement
    {
        private readonly DispatcherTimer timer;
        private readonly Slider positionSlider;
        public string TimeText { get; set; }
        private bool IsPlaying { get; set; }
        private DateTime LastClickTime { get; set; }
        private DateTime MouseDownTime { get; set; }

        public WPFPlayer()
        {
            IsPlaying = false;
            timer = new DispatcherTimer();
            TimeText = string.Empty;
            LastClickTime = DateTime.Now;
            positionSlider = new Slider();
        }

        internal void TogglePlayPause()
        {
            IsPlaying = !IsPlaying;
            if (IsPlaying)
            {
                Play();
                timer.Start();
            }
            else
            {
                Pause();
                timer.Stop();
            }
        }

        public void UpdatePosition()
        {
            TimeSpan newPositionTimeSpan = TimeSpan.FromSeconds(0);
            Position = newPositionTimeSpan;
            positionSlider.Value = 0;
            Play();
            timer.Start();
        }

        private void PlayButton_Click()
        {
            TogglePlayPause();
            timer.Start();
        }

        private void PauseButton_Click()
        {
            TogglePlayPause();
            timer.Stop();
        }

        private void StopButton_Click()
        {
            Stop();
            timer.Stop();
        }
        private void SpeedLow_Click()
        {
            if (SpeedRatio > 0.25)
            {
                TogglePlayPause();
                SpeedRatio -= 0.5;
                Task.Delay(100);
                TogglePlayPause();
            }
        }
        private void SpeedHigh_Click()
        {
            if (SpeedRatio < 2)
            {
                TogglePlayPause();
                SpeedRatio += 0.25;
                Task.Delay(100);
                TogglePlayPause();
            }
        }
    }
}
