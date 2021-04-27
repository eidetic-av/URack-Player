using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eidetic.URack.Scene
{

    public class CameraCheck : MonoBehaviour
    {
        Camera defaultCamera;
        Camera DefaultCamera => defaultCamera ?? (defaultCamera = GetComponent<Camera>());

        void Start()
        {
            void checkActivate()
            {
                // only enable the default camera if every other module instance
                // in the scene contains no Camera components
                bool otherCamera = false;
                foreach (var module in UModule.Instances.Values)
                    if (otherCamera = module.gameObject.GetComponentsInChildren<Camera>().Length > 0)
                        break;

                DefaultCamera.enabled = !otherCamera;
            }

            // do the check when new modules are created,
            // and when modules are activated/deactivated
            Osc.Server.OnAddModule += m => checkActivate();
            Osc.Server.OnRemoveModule += () => checkActivate();
            Osc.Server.OnModuleSetActive += (m, a) => checkActivate();
        }
    }

}
