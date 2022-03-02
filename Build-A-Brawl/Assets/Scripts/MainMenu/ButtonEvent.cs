using UnityEngine;

namespace BuildABrawl.Events
{
    public class ButtonEvent : BaseGameEvent<int>
    {
        public void Raise() => Raise(new int());
    }
}
