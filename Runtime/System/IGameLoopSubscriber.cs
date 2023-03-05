using UnityEngine;
using System.Collections.Generic;

namespace EricGames.Core.System
{
    public interface IGameLoopSubscriber
    {
        void FixedUpdateHandler();
        void UpdateHandler();
    }
}