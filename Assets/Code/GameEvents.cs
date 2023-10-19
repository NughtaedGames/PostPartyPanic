using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace GameEvents
{
    public class TestEvent : GameEvent
    {
        public TestEvent()
        {            
        }

        public override bool isValid()
        {
            return base.isValid(); //check if event is valid. I actually didn't use it in my past projects lol
        }
    }

    public class SimpleTestEvent : GameEvent { }

    public class DayPassedEvent : GameEvent
    {
        public int NumberofDays;
        public DayPassedEvent(int days)
        {
            NumberofDays = days;
        }
    }

    public class HighlightObjects : GameEvent {}

    public class IncreaseMaxAmountOfCleanupObjects : GameEvent {}
    public class DecreaseMaxAmountOfCleanupObjects : GameEvent {}
    public class IncreaseAmountOfCleanedObjects : GameEvent {}
    public class DecreaseAmountOfCleanedObjects : GameEvent {}
    public class TimeIsOver : GameEvent {}
    public class RestartGame : GameEvent {}
    public class StartFire : GameEvent {}
    public class FireIsGameover : GameEvent {}
    public class StartGame : GameEvent {}
    public class DestroyCup : GameEvent {}
    public class SpawnCup : GameEvent {}

    public class StartUFO : GameEvent {}

    public class SwitchPauseMenu : GameEvent {}

    public class PlayerJoined : GameEvent
    {
        public Transform player;
        public PlayerJoined(Transform pl)
        {
            player = pl;
        }
    }
    
    public class InReachEvent : GameEvent {
        public int pickableObj;
        public InReachEvent(int obj)
        {
            pickableObj = obj;
        }
    }


    public class UIOpened : GameEvent //ideally, this could be merged with the community board looking event but it's used in too many other scripts
    {
        public bool UIisopened;
        public GameObject UIOrigin;
        public UIOpened(bool ui, GameObject obj)
        {
            UIisopened = ui;
            UIOrigin = obj;
        }
    }

    //--save system--//
    public class LoadEvent : GameEvent { }
}