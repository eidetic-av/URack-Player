using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eidetic.URack.Scene
{

    public class LightCheck : MonoBehaviour
    {
        public bool UseManualSwitch = false;

        Light defaultLight;
        Light DefaultLight => defaultLight ?? (defaultLight = GetComponent<Light>());
        
        void CheckActivate()
        {
            if (UseManualSwitch) return;
            // only enable the default light if every other module instance
            // in the scene contains no Light components
            bool otherLight = false;
            foreach (var module in UModule.Instances.Values)
                if (otherLight = module.gameObject.GetComponentsInChildren<Light>().Length > 0)
                    break;

            DefaultLight.enabled = !otherLight;
        }


        void Start()
        {
            // do the check when new modules are created,
            // and when modules are activated/deactivated
            Osc.Server.OnAddModule += m => CheckActivate();
            Osc.Server.OnRemoveModule += () => CheckActivate();
            Osc.Server.OnModuleSetActive += (m, a) => CheckActivate();
        }

        void Update()
        {
            if (!UseManualSwitch) return;
            if (Input.GetKeyDown(KeyCode.L)) DefaultLight.enabled = !DefaultLight.enabled;
        }
    }

}
