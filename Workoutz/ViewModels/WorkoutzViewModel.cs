using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Workoutz.Model;

namespace Workoutz.ViewModels
{
    internal class WorkoutzViewModel : ReactiveObject
    {
        private bool _acceptIntervalWorkout;
        public bool AcceptIntervalWorkout
        {
            get { return _acceptIntervalWorkout; }
            set { this.RaiseAndSetIfChanged(ref _acceptIntervalWorkout, value); }
        }

        private TimeSpan _totalWorkoutTime;
        public TimeSpan TotalWorkoutTime
        {
            get { return _totalWorkoutTime; }
            set { this.RaiseAndSetIfChanged(ref _totalWorkoutTime, value); }
        }

        private TimeSpan _interval;
        public TimeSpan Interval
        {
            get { return _interval; }
            set { this.RaiseAndSetIfChanged(ref _interval, value); }
        }

        private TimeSpan _timeLeft;
        public TimeSpan TimeLeft
        {
            get { return _timeLeft; }
            set { this.RaiseAndSetIfChanged(ref _timeLeft, value); }
        }

        private TimeSpan _intervalLeft;
        public TimeSpan IntervalLeft
        {
            get { return _intervalLeft; }
            set { this.RaiseAndSetIfChanged(ref _intervalLeft, value); }
        }

        private bool _workoutFinished;
        public bool WorkoutFinished
        {
            get { return _workoutFinished; }
            set { this.RaiseAndSetIfChanged(ref _workoutFinished, value); }
        }

        public ReactiveCommand StartWorkout
        {
            get;
            private set;
        }

        public ReactiveCommand RestartWorkout
        {
            get;
            private set;
        }

        public ReactiveCommand ExitWorkout
        {
            get;
            private set;
        }

        private TimeSpan _tickInterval = TimeSpan.FromMilliseconds(10);
        private DispatcherTimer _workoutTimer;
        private AudioManager _audioManager = new AudioManager();

        public WorkoutzViewModel()
        {
            AcceptIntervalWorkout = true;
            TotalWorkoutTime = new TimeSpan();
            Interval = new TimeSpan();

            var enableWorkout = this.WhenAny(
                vm => vm.TotalWorkoutTime,
                vm => vm.Interval,
                (workout, interval) => ValidateEnableWorkout(workout.Value, interval.Value));

            StartWorkout = new ReactiveCommand(enableWorkout);
            StartWorkout.Subscribe(param => OnStartWorkout());

            RestartWorkout = new ReactiveCommand(this.WhenAny(vm => vm.WorkoutFinished, obs => obs.Value));
            RestartWorkout.Subscribe(param => { AcceptIntervalWorkout = true; WorkoutFinished = false; });

            ExitWorkout = new ReactiveCommand();
            ExitWorkout.Subscribe(param => App.Current.Shutdown(0));
        }

        private static bool ValidateEnableWorkout(TimeSpan workout, TimeSpan interval)
        {
            return workout.TotalSeconds > interval.TotalSeconds && workout.TotalSeconds > 0 && interval.TotalSeconds > 0;
        }

        private object OnStartWorkout()
        {
            AcceptIntervalWorkout = false;
            WorkoutFinished = false;
            IntervalLeft = Interval;
            TimeLeft = TotalWorkoutTime;

            if (_workoutTimer != null && _workoutTimer.IsEnabled)
            {
                throw new InvalidOperationException("Attempting to start a timer when the old one is already running");
            }

            _workoutTimer = new DispatcherTimer
            {
                Interval = _tickInterval
            };

            _workoutTimer.Tick += timer_Tick;
            _workoutTimer.Start();
            return null;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            TimeLeft = TimeLeft.Subtract(_tickInterval);
            IntervalLeft = IntervalLeft.Subtract(_tickInterval);

            if (IntervalLeft.TotalMilliseconds <= 0)
            {
                PlayInterval();
                IntervalLeft = Interval;
            }
            else if (IntervalLeft.TotalMilliseconds % 500 == 0)
            {
                PlayTick();
            }

            if (TimeLeft.TotalMilliseconds <= 0)
            {
                WorkoutFinished = true;
                _workoutTimer.Stop();
            }
        }

        void PlayTick()
        {
            _audioManager.PlayTick();
        }

        void PlayInterval()
        {
            _audioManager.PlayInterval();
        }

    }
}
