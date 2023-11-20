using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFPlayer.Infrastructure.Commands;
using WPFPlayer.Utils;

namespace WPFPlayer.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public ICommand CloseApplicationCommand { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ICommand MouseDownCommand { get; private set; }
        public ICommand PlayButtonCommand { get; private set; }
        public ICommand PauseButtonCommand { get; private set; }
        public ICommand StopButtonCommand { get; private set; }
        public ICommand SpeedLowCommand { get; private set; }
        public ICommand SpeedHightCommand { get; private set; }
        public ICommand ToggleMaxCommand { get; private set; }
        public ICommand ToggleMinCommand { get; private set; }


        private MainWindow _Window { get; }
        private MediaService _MediaService { get; }
        public TimeSpan TotalDuration { get; private set; }
        public TimeSpan CurrentPosition { get; private set; }


        private Uri _MediaSource;
        public Uri MediaSource
        {
            get => _MediaSource;
            set => Set(ref _MediaSource, value);
        }
        private string _Title;
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        private string _timeText;
        public string TimeText
        {
            get => _timeText;
            set => Set(ref _timeText, value);
        }

        private string _remainingTime;
        public string RemainingTime
        {
            get => _remainingTime;
            set => Set(ref _remainingTime, value);
        }
        private string _volume;
        public string Volume
        {
            get => _volume;
            set => Set(ref _volume, value);
        }

        public MainWindowViewModel(MainWindow window)
        {
            _Window = window ?? throw new ArgumentNullException(nameof(window));
            _Window.MouseLeftButtonDown += _Window_MouseLeftButtonDown;
            _MediaService = new MediaService();
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted);
            OpenFileCommand = new LambdaCommand(OnOpenFile);
            PlayButtonCommand = new LambdaCommand(PlayButton);
            PauseButtonCommand = new LambdaCommand(PauseButton);
            StopButtonCommand = new LambdaCommand(StopButton);
            SpeedLowCommand = new LambdaCommand(SpeedLow);
            SpeedHightCommand = new LambdaCommand(SpeedHight);
            ToggleMaxCommand = new LambdaCommand(ToggleMax);
            ToggleMinCommand = new LambdaCommand(ToggleMin);
        }


        private void PlayButton(object p)
        {
            _Window.mediaElement.PlayClick();
        }
        private void PauseButton(object p)
        {
            _Window.mediaElement.PauseClick();
        }
        private void StopButton(object p)
        {
            _Window.mediaElement.StopClick();
        }
        private void SpeedLow(object p)
        {
            _Window.mediaElement.SpeedLowClick();
        }
        private void SpeedHight(object p)
        {
            _Window.mediaElement.SpeedHighClick();
        }
        private void Timer_tick(object sender, EventArgs e)
        {
            if (_Window.mediaElement.NaturalDuration.HasTimeSpan)
            {
                _Window.sliderPosition.Maximum = _Window.mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                _Window.sliderPosition.Value = _Window.mediaElement.Position.TotalSeconds;
                UpdateTimeProperties();
            }
        }


        private void SliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_Window.mediaElement != null)
            {
                TimeSpan newPosition = TimeSpan.FromSeconds(_Window.sliderPosition.Value);
                if (Math.Abs(e.NewValue - e.OldValue) > 1)
                {
                    _Window.mediaElement.TogglePlayPause();
                    _Window.mediaElement.Position = newPosition;
                    Task.Delay(100);
                    _Window.mediaElement.TogglePlayPause();
                    UpdateTimeProperties();
                }
            }
        }

        public void UpdatePosition()
        {
            if (_Window.mediaElement != null)
            {
                TimeSpan newPosition = TimeSpan.FromSeconds(_Window.sliderPosition.Value);
                _Window.mediaElement.Position = newPosition;
                _Window.sliderPosition.Value = 0;
                _Window.mediaElement.SpeedRatio = 1;
                _Window.mediaElement.PlayClick();
                _Window.mediaElement.Timer.Start();
            }
        }

        private void OnOpenFile(object p)
        {
            string selectedFilePath = _MediaService.OpenMediaFile();
            if (!string.IsNullOrEmpty(selectedFilePath))
            {
                MediaSource = new Uri(selectedFilePath);
                Title = _MediaService.FilterString(selectedFilePath);
                UpdatePosition();
                _Window.sliderPosition.ValueChanged += SliderPosition_ValueChanged;
                _Window.mediaElement.Timer.Tick += Timer_tick;
            }
        }

        private void _Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _Window.mediaElement.MouseDownTime = DateTime.Now;
            TimeSpan duration = DateTime.Now - _Window.mediaElement.MouseDownTime;
            if (e.ChangedButton == MouseButton.Left)
            {
                _Window.DragMove();
                _Window.mediaElement.MouseUpTime = DateTime.Now - _Window.mediaElement.MouseDownTime;
            }
            if (duration.TotalSeconds < 0.2 && _Window.mediaElement.MouseUpTime.TotalSeconds < 0.2)
            {
                _Window.mediaElement.TogglePlayPause();
                ToggleMaxMin();
            }
        }

        public void ToggleMaxMin()
        {
            DateTime now = DateTime.Now;
            int doubleClickTime = 250;

            if ((now - _Window.mediaElement.LastClickTime).TotalMilliseconds < doubleClickTime)
            {
                if (_Window.WindowState == WindowState.Maximized)
                {
                    _Window.WindowState = WindowState.Normal;
                }
                else
                {
                    _Window.WindowState = WindowState.Maximized;
                }
            }
            _Window.mediaElement.LastClickTime = now;
        }

        public void ToggleMin(object p)
        {
            _Window.WindowState = WindowState.Minimized;
        }

        public void ToggleMax(object p)
        {
            _Window.WindowState = WindowState.Maximized;
        }

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        private void UpdateTimeProperties()
        {
            if (_Window.mediaElement.NaturalDuration.HasTimeSpan)
            {
                TimeText = _Window.mediaElement.Position.ToString(@"hh\:mm\:ss");
                TotalDuration = _Window.mediaElement.NaturalDuration.TimeSpan;
                CurrentPosition = _Window.mediaElement.Position;
                RemainingTime = (TotalDuration - CurrentPosition).ToString(@"hh\:mm\:ss");
            }
        }
    }
}
