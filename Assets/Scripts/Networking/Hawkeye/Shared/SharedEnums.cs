using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeye
{
    public static class SharedEnums
    {
        //---- Connection Enum ----
        //-------------------------
        public enum ConnectionState
        {
            None,
            Connect,
            Disconnect,
            Close
        }
    }
}
