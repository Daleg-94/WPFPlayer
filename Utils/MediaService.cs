using Microsoft.Win32;
using System.Text.RegularExpressions;
using WPFPlayer.Interface;

namespace WPFPlayer.Utils
{
    internal class MediaService : IMediaService
    {
        public string OpenMediaFile()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Media Files|*.mp3;*.mp4;*.wav;*.avi;*.mkv|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return string.Empty;
        }

        public string FilterString(string input)
        {
            string pattern = @".*\\(.*)(\\[^\\]+)$";

            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return input;
            }
        }
    }
}
