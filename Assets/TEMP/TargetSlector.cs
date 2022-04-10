using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    public class TargetSlector : MonoBehaviour
    {
        // Define a posiçao onde será realizada a busca pelos possiveis alvos mais próximos
        public enum TargetPosition { MousePosition, ShooterPosition, SpawnPosition }

        // Define a forma de busca.
        // Final position: Vai até a posiçao e permanece.
        // ClosestTargetable: Busca o alvo mais próximo ao ser criado.
        // DynamicClosestTargetable: Busca o alvo mais próximo em tempo real, mudando conforme novos alvos se aproximem.
        public enum TargetObject { FinalPosition, ClosestTargetable, DynamicClosestTargetable }

        // Define como será a perseguiçao
        // Follow: Persegue o alvo sem parar
        // AinToPos: apenas mira em direçao ao alvo.
        // GoToPos:
        // GoToPosAndStay
        public enum TargetBehavior { Follow, AinToPos, GoToPos, GoToPosAndStay }

        TargetPosition targetPositionType;
        TargetObject targetObject;
        TargetBehavior targetBehavior;

        List<ITargetable> allTargets;

        #region Target

        public bool isSearchEnabled;
        public bool isSearchingTarget;
        public bool hasTargetDefined;
        public Vector3 targetPosition;

        public Vector3 GetFinalTargetPosition(Vector3 searchInitialPosition)
        {
            Vector3 targetPosition = Vector3.zero;

            switch (targetBehavior)
            {
                case TargetBehavior.GoToPos:
                case TargetBehavior.Follow:
                    targetPosition = GetTargetPosition(searchInitialPosition);
                    break;
                case TargetBehavior.AinToPos:
                    targetPosition = hasTargetDefined ? Vector3.zero : GetTargetPosition(searchInitialPosition);
                    break;
                case TargetBehavior.GoToPosAndStay:
                    targetPosition = this.targetPosition == null ? GetTargetPosition(searchInitialPosition) : this.targetPosition;
                    break;
            }

            return targetPosition;
        }

        Vector3 GetTargetPosition(Vector3 searchInitialPosition)
        {
            switch (targetObject)
            {
                case TargetObject.FinalPosition:
                    return searchInitialPosition;
                case TargetObject.ClosestTargetable:
                    return hasTargetDefined ? targetPosition : GetClosestTarget(allTargets, searchInitialPosition).GetTransform().position;
                case TargetObject.DynamicClosestTargetable:
                    return GetClosestTarget(allTargets, searchInitialPosition).GetTransform().position;
            }

            return Vector3.zero;
        }


        #endregion

        #region Target Finder

        public ITargetable GetClosestTarget(List<ITargetable> targets, Vector2 closeOfPos, float area = float.MaxValue)
        {
            List<ITargetable> avaliableTargets = GetTargetsInArea(targets, closeOfPos, area);
            return GetClosest(avaliableTargets, closeOfPos);
        }

        public ITargetable GetPriorityClosestTarget(List<ITargetable> targets, Vector2 closeOfPos, float area = float.MaxValue)
        {
            List<ITargetable> avaliableTargets = GetTargetsInArea(targets, closeOfPos, area);
            return GetPriorityClosest(avaliableTargets, closeOfPos);
        }

        public List<ITargetable> GetTargetsInArea(List<ITargetable> targets, Vector2 closeOfPos, float area = float.MaxValue)
        {
            List<ITargetable> avaliableTargets = targets.
                 FindAll(x => Utils.GetDistance(closeOfPos, x.GetTransform().position) < area).
                 Select(x => x).ToList();

            return avaliableTargets;
        }

        private ITargetable GetClosest(List<ITargetable> searchList, Vector2 closeOfPos)
        {
            return searchList.OrderBy(x => Utils.GetDistance(closeOfPos, x.GetTransform().position)).First();
        }

        private ITargetable GetPriorityClosest(List<ITargetable> searchList, Vector2 closeOfPos)
        {
            return searchList.OrderBy(x => x.GetPriority()).ThenBy(x => Utils.GetDistance(closeOfPos, x.GetTransform().position)).First();
        }
        #endregion
    }
}