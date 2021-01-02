using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeye
{
    /// <summary>
    /// The game data that is passed into each netmessage for processing
    /// </summary>
    public class GameState
    {
        //---- Enum
        //---------
        public enum State
        {
            None,
            Connecting,
            Disconnecting,
            Lobby,
            Game
        }

        //---- Variables
        //--------------
        protected MenuManager menuManager;
        //protected Dictionary<int, NetObject> netObjects;
        protected State state;

        //---- Ctor
        //---------
        public GameState()
        {
            //netObjects = new Dictionary<int, NetObject>();
            menuManager = MenuManager.UIManager;
        }

        //---- Properties
        //---------------
        public State CurrentState => state;
        public MenuManager Menus => menuManager;

        //---- State Change
        //-----------------
        public virtual void StateChange(State state)
        {
            this.state = state;
        }

        //---- Processing
        //---------------
        public void ProcessNetMessage(string typeStr, string json)
        {
            /*System.Type type;
            if(!NetMessage.NetMessageTypes.TryGetValue(typeStr, out type))
            {
                Debug.LogError($"Unable to parse {typeStr} to object");
                return;
            }
            var netmessage = JsonUtility.FromJson(json, type) as NetMessage;
            netmessage.Process(this);*/
        }

        /*public void ProcessDirectMessage(NetMessage netMessage)
        {
            netMessage.Process(this);
        }*/

        //---- Messages
        //-------------
        /*public void DirectMessage(NetMessage netMessage)
        {
            ProcessDirectMessage(netMessage);
        }*/

        //---- NetObject
        //--------------
        /*public T GetNetObject<T>(int id) where T : NetObject
        {
            NetObject netObject;
            if(!netObjects.TryGetValue(id, out netObject))
            {                
                return null;
            }
            return netObject as T;
        }*/

        /*public T FindObject<T>() where T : NetObject
        {
            foreach(var netObject in netObjects)
            {
                if(netObject.Value is T)
                {
                    return netObject.Value as T;
                }
            }
            return null;
        }*/

        /*public List<T> FindAllObjects<T>() where T : NetObject
        {
            List<T> objects = new List<T>();
            foreach (var netObject in netObjects)
            {
                if (netObject.Value is T)
                {
                    objects.Add(netObject.Value as T);
                }
            }
            return objects;
        }*/

        /*public void Add(NetObject netObject)
        {
            netObjects.Add(netObject.NetId, netObject);
        }*/

        public void Remove(int netId)
        {
            //netObjects.Remove(netId);
        }

    }
} // end namespace
