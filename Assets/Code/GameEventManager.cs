using System.Collections.Generic;

namespace GameEvents
{
    public abstract class GameEvent
    {
        public virtual bool isValid() { return true; }
    }
    public static class GameEventManager
    {
        public delegate void EventDelegate<in T>(T e) where T : GameEvent;
        private delegate void EventDelegate(GameEvent e);

        private static readonly Dictionary<System.Type, EventDelegate> delegates;
        private static readonly Dictionary<System.Delegate, EventDelegate> delegateLookup;

        static GameEventManager()
        {
            delegates = new Dictionary<System.Type, EventDelegate>();
            delegateLookup = new Dictionary<System.Delegate, EventDelegate>();
        }

        public static void AddListener<T>(EventDelegate<T> del) where T: GameEvent
        {
            if (delegateLookup.ContainsKey(del))
                return;

            void InternalDelegate(GameEvent e) => del((T)e);
            delegateLookup[del] = InternalDelegate;

            EventDelegate tempDel;
            if(delegates.TryGetValue(typeof(T), out tempDel))
            {
                delegates[typeof(T)] = tempDel += InternalDelegate;
            }
            else
            {
                delegates[typeof(T)] = InternalDelegate;
            }
        }

        public static void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
        {
            EventDelegate internalDelegate;
            if (delegateLookup.TryGetValue(del, out internalDelegate))
            {
                EventDelegate tempDel;
                if(delegates.TryGetValue(typeof(T), out tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        delegates.Remove(typeof(T));
                    }
                    else
                    {
                        delegates[typeof(T)] = tempDel;
                    }
                }
                delegateLookup.Remove(del);
            }
        }

        public static void Raise(GameEvent e)
        {
            EventDelegate del;
            if(delegates.TryGetValue(e.GetType(),out del))
            {
                del.Invoke(e);
            }
        }
        //i don't know how to use unity events so i will use this one instead... :o
    }
}

