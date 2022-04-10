using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    public interface ITargetable
    {
        public void SelectTarget();
        public Transform GetTransform();
        public int GetPriority();
    }
}