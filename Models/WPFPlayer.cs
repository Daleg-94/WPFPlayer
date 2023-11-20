using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WPFPlayer.Controls
{
    internal class WPFPlayer : MediaElement
    {
        internal DispatcherTimer Timer { get; set; }
        internal string TimeText { get; set; }
        private bool IsPlaying { get; set; }
        internal DateTime LastClickTime { get; set; }
        internal DateTime MouseDownTime { get; set; }
        internal TimeSpan MouseUpTime { get; set; }


        public WPFPlayer()
        {
            IsPlaying = false;
            Timer = new DispatcherTimer();
            TimeText = string.Empty;
            LastClickTime = DateTime.Now;
            Timer.Interval = TimeSpan.FromSeconds(0.1);

            LoadedBehavior = MediaState.Manual;
            UnloadedBehavior = MediaState.Manual;
        }

        internal void TogglePlayPause()
        {
            IsPlaying = !IsPlaying;
            if (IsPlaying)
            {
                Play();
                Timer.Start();
            }
            else
            {
                Pause();
                Timer.Stop();
            }
        }

        internal void PlayClick()
        {
            if (!IsPlaying)
            {
                TogglePlayPause();
            }
            Timer.Start();
        }

        internal void PauseClick()
        {
            if (IsPlaying)
            {
                TogglePlayPause();
            }
            Timer.Stop();
        }

        internal void StopClick()
        {
            Stop();
            Timer.Stop();
        }
        internal void SpeedLowClick()
        {
            if (SpeedRatio > 0.25)
            {
                TogglePlayPause();
                SpeedRatio -= 0.25;
                Task.Delay(100);
                TogglePlayPause();
            }
        }
        internal void SpeedHighClick()
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