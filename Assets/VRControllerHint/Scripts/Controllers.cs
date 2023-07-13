using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace krisnart.ControllerTut
{
    public class Controllers : MonoBehaviour
    {
        public LabelLine[] ControllerList;

        private void Start()
        {
            var parentObj = transform.parent.parent.parent;
            if (parentObj != null)
            {                 
                if (parentObj.name == "OVRCameraRig")
                {
                    ControllerList[1].transform.localPosition = Vector3.zero;
                    ControllerList[1].transform.localEulerAngles = Vector3.zero;
                    ControllerList[2].transform.localPosition = Vector3.zero;
                    ControllerList[2].transform.localEulerAngles = Vector3.zero;
                }
            }
        }

    }
}