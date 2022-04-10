using UnityEngine;

namespace GameCore
{
    public class FixedWaitForSeconds : CustomYieldInstruction
    {
        float seconds;

        public override bool keepWaiting
        {
            get
            {
                seconds -= FixedTime.DeltaTime;
                return seconds > 0;
            }
        }
        public FixedWaitForSeconds(float seconds)
        {
            this.seconds = seconds;
        }
    }
}