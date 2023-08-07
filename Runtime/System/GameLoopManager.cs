using UnityEngine;
using System.Collections.Generic;

namespace EricGames.Runtime.System
{
    public class GameLoopManager : MonoBehaviour
    {
        static private GameLoopManager instance;

        static public GameLoopManager Instance
        {
            get
            {
                if (instance == null)
                {
                    var instanceName = "System";
                    var system = GameObject.Find(instanceName);

                    if (system == null)
                    {
                        system = new GameObject(instanceName, typeof(GameLoopManager));
                    }

                    if (!system.TryGetComponent(out instance))
                    {
                        instance = system.AddComponent<GameLoopManager>();
                    }
                }

                return instance;
            }
        }

        private List<IGameLoopSubscriber> looper;

        private void Update()
        {
        }
    }
}
