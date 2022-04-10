using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    [DisallowMultipleComponent]
    public class InteractionManager : MonoBehaviour
    {
        public List<Interaction> possibleInteractions = new List<Interaction>();
        private Interaction closestInteraction;

        [SerializeField] private float interactTime = 0.1f;
        private float interactTimerC;

        public Transform playerTrans;

        private void Update()
        {
            if (interactTimerC > 0)
                interactTimerC -= FixedTime.DeltaTime;
        }

        public void TryInteract()
        {
            if (interactTimerC <= 0 && closestInteraction && closestInteraction.CanInteract)
            {
                interactTimerC = interactTime;
                closestInteraction.Interact();
            }
        }

        public Interaction GetClosestInteraction()
        {
            for (int i = 0; i < possibleInteractions.Count; i++)
            {
                Interaction newInteraction = possibleInteractions[i];
                if (SetNewClosest(newInteraction.transform.position))
                    closestInteraction = newInteraction;
            }
            return closestInteraction;
        }

        public bool SetNewClosest(Vector2 newIntPos)
        {
            Vector2 clampPos = (Vector2)playerTrans.position + Vector2.ClampMagnitude(GameUtils.Instance.MousePos - (Vector2)playerTrans.position, 3);
            return !closestInteraction || Utils.GetDistance(newIntPos, clampPos) < Utils.GetDistance(closestInteraction.transform.position, clampPos);
        }

        public void RemoveInteraction(Interaction interaction)
        {
            bool isClosest = interaction == GetClosestInteraction();
            possibleInteractions.Remove(interaction);

            if (isClosest)
            closestInteraction = null;
        }

        public bool HasInteraction(Interaction interaction)
        {
            return possibleInteractions.Contains(interaction);
        }

        public void RemoveAllInteractions()
        {
            possibleInteractions.Clear();
            closestInteraction = null;
        }
    }
}