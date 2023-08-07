using UnityEngine;
using System.Collections.Generic;

namespace EricGames.Runtime.System
{
    public interface IGameLoopSubscriber
    {
        void FixedUpdateHandler();
        void UpdateHandler();
    }
}