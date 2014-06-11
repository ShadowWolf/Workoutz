using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workoutz.Model
{
    internal sealed class AudioManager : IDisposable
    {
        Action<IDisposable> safeDispose = disp => { if (disp != null) disp.Dispose(); };

        IWavePlayer WaveOutDevice { get; set; }
        AudioFileReader AudioReader { get; set; }

        Mp3FileReader TickFile { get; set; }
        Mp3FileReader IntervalFile { get; set; }

        public TimeSpan LastPlayedTime { get; private set; }

        public AudioManager()
        {
            WaveOutDevice = new WaveOut();
            TickFile = new Mp3FileReader(new MemoryStream(Properties.Resources.Tick));
            IntervalFile = new Mp3FileReader(new MemoryStream(Properties.Resources.Interval));
            LastPlayedTime = new TimeSpan();
        }

        public void Dispose()
        {
            safeDispose(WaveOutDevice);
            safeDispose(TickFile);
            safeDispose(IntervalFile);
        }

        public void PlayTick()
        {
            Play(TickFile);
        }

        public void PlayInterval()
        {
            Play(IntervalFile);
        }

        private void Play(Mp3FileReader provider)
        {
            LastPlayedTime = provider.TotalTime;
            provider.Seek(0, SeekOrigin.Begin);
            WaveOutDevice.Init(provider);
            WaveOutDevice.Play();
        }
    }
}
