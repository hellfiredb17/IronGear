using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeye
{
    public interface ILog
    {
        void Output(string msg);
        void Warn(string msg);
        void Error(string msg);
    }
} // end namespace
