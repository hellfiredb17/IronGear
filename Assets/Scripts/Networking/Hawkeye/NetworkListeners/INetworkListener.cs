using System;

namespace Hawkeye
{
    /// <summary>
    /// Base interface for all listeners
    /// This sets a standard function to process
    /// net messages across interfaces
    /// </summary>
    public interface INetworkListener
    {   
        void OnProcess(object netMessage, Type type);     
    }
} // end namespace
