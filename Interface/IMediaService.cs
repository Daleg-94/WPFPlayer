using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFPlayer.Interface
{
    internal interface IMediaService
    {
        string OpenMediaFile();
        string FilterString(string input);

    }
}
