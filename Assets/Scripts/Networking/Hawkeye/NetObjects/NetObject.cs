using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeye
{
    /// <summary>
    /// Base class that all net objects derive from
    /// </summary>
    public class NetObject
    {
        public int NetId;

        public NetObject(int id)
        {
            NetId = id;
        }
    }
} // end namespace
