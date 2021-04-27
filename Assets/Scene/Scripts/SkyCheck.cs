using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Eidetic.URack.Scene
{

    public class SkyCheck : MonoBehaviour
    {
        Volume defaultSky;
        Volume DefaultSky => defaultSky ?? (defaultSky = GetComponent<Volume>());

        void Start()
        {
            void checkActivate()
            {
                // only enable the default camera if every other module instance
                // in the scene contains no Camera components
                bool otherSky = false;
                foreach (var module in UModule.Instances.Values)
                {
                    foreach (var volume in module.GetComponentsInChildren<Volume>())
                    {
                        if (otherSky = volume.profile.TryGet<GradientSky>(out GradientSky g))
                            break;
                        else if (otherSky = volume.profile.TryGet<HDRISky>(out HDRISky h))
                            break;
                        else if (otherSky = volume.profile.TryGet<PhysicallyBasedSky>(out PhysicallyBasedSky pb))
                            break;
                    }
                    if (otherSky) break;
                }

                DefaultSky.enabled = !otherSky;
            }

            // do the check when new modules are created,
            // and when modules are activated/deactivated
            Osc.Server.OnAddModule += m => checkActivate();
            Osc.Server.OnRemoveModule += () => checkActivate();
            Osc.Server.OnModuleSetActive += (m, a) => checkActivate();
        }
    }

}
