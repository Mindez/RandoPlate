using Kitchen;
using KitchenMods;
using KitchenLib.Logging;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace RandoPlate
{
    public class RandoPlate : GenericSystemBase, IModSystem
    {
        protected override void Initialise()
        {
            base.Initialise();
            KitchenLogger logger = new KitchenLogger("Randoplate");
            logger.LogInfo("Initialise Function");
        }

        protected override void OnUpdate()
        {

        }
    }
}