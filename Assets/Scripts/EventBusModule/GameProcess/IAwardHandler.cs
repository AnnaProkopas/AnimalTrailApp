using Awards;
using UnityEngine;

namespace EventBusModule.GameProcess
{
    public interface IAwardHandler : IGlobalSubscriber
    {
        void HandleGettingAward(AwardType type);
    }
}