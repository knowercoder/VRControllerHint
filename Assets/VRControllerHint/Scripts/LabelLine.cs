using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace krisnart.ControllerTut
{
    public class LabelLine : MonoBehaviour
    {                
        public Labelparam[] labelparams;        
        public Material[] ButtonMaterials;

        [HideInInspector]
        public List<bool> labelStates = new List<bool> { false, false, false, false, false, false };
        [HideInInspector]
        public Coroutine HapticCOR;

        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        float vertexCount = 6;          
        
        // Start is called before the first frame update
        void Start()
        {
            Application.onBeforeRender += Update;
            for(int i = 0; i < labelparams.Length; i++)
            {
                if (labelparams[i].linerenderer != null)
                    initializeLinerenderer(labelparams[i].linerenderer);
            }            
        }        

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < labelparams.Length; i++)
            {
                if (labelparams[i].islabelline)
                {
                    labelparams[i].Startpos = labelparams[i].labelPos.position;
                    var speed = labelparams[i].Startpos - labelparams[i].Prevpos;
                    labelparams[i].labelObject.position -= speed;
                    labelparams[i].labelObject.position = Vector3.Lerp(labelparams[i].labelObject.position, labelparams[i].labelPos.position, 0.2f);
                    ShootLaser(labelparams[i].labelObject.localPosition, labelparams[i].targetObject.localPosition, labelparams[i].linerenderer);
                }
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < labelparams.Length; i++)
            {
                if (labelparams[i].islabelline)
                {
                    labelparams[i].Prevpos = labelparams[i].labelPos.position;
                }
            }
        }

        public void Setlabel(string ButtonName, string label, bool state)
        {
            var index = ControllerButton.ControllerButtons.IndexOf(ButtonName);
            if(index >= 0 && index < labelparams.Length)
            {
                labelparams[index].labelObject.GetChild(0).GetComponentInChildren<Text>().text = label;
                labelparams[index].labelObject.gameObject.SetActive(state);
                labelparams[index].linerenderer.enabled = state;
                labelparams[index].islabelline = state;
                if (ControllerButton.instance.isColor)
                {
                    if (state)
                        labelparams[index].targetMesh.material = ButtonMaterials[1];
                    else
                        labelparams[index].targetMesh.material = ButtonMaterials[0];
                }
            }
            else
            {
                Debug.LogError("You are trying to acess an incorrect device button!");
            }                       
        }

        public void Setlabel(bool state)
        {
            for(int i = 0; i < labelparams.Length; i++)
            {
                labelparams[i].labelObject.gameObject.SetActive(state);
                labelparams[i].linerenderer.enabled = state;
                labelparams[i].islabelline = state;
                labelStates[i] = state;
                transform.GetChild(0).GetChild(i).GetComponent<HighlightController>().enabled = ControllerButton.instance.isHighlight ? state : false;
                transform.GetChild(0).GetChild(i).GetComponent<HighlightController>().HighlightAnimate = ControllerButton.instance.isHighlightAnimate ? state : false;
                if (ControllerButton.instance.isColor)
                {
                    if (state)
                        labelparams[i].targetMesh.material = ButtonMaterials[1];
                    else
                        labelparams[i].targetMesh.material = ButtonMaterials[0];
                }
            }
        }

        void initializeLinerenderer(LineRenderer linerenderer)
        {
            Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
            linerenderer.SetPositions(initLaserPositions);
            linerenderer.startWidth = 0.0005f;
            linerenderer.endWidth = 0.002f;
        }

        void ShootLaser(Vector3 targetPosition, Vector3 endPosition, LineRenderer lineRenderer)
        {
            Vector3 Point1, Point2, Point3;            
            Point1 = targetPosition;
            Point3 = endPosition;

            var median = (Point1 + Point3) / 2;            
            Point2 = new Vector3(median.x, median.y-0.02f, median.z);
            var pointList = new List<Vector3>();

            for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
            {
                var tangent1 = Vector3.Lerp(Point1, Point2, ratio);
                var tangent2 = Vector3.Lerp(Point2, Point3, ratio);
                var curve = transform.TransformPoint(Vector3.Lerp(tangent1, tangent2, ratio));

                pointList.Add(curve);
            }

            lineRenderer.positionCount = pointList.Count;
            lineRenderer.SetPositions(pointList.ToArray());
        }

        public void SetHaptic(string hand, bool hapticState)
        {
            if (hapticState && HapticCOR == null)
            {
                HapticCOR = StartCoroutine(ButtonHintHaptic(hand));
            }
            else if (!hapticState && HapticCOR != null)
            {
                StopCoroutine(HapticCOR);
                HapticCOR = null;
            }
        }

        IEnumerator ButtonHintHaptic(string hand)
        {
            InputDevices.GetDevicesWithCharacteristics((hand == "Right") ? InputDeviceCharacteristics.Right : InputDeviceCharacteristics.Left, devices);
            while (true)
            {
                foreach (var device in devices)
                {
                    HapticCapabilities capabilities;
                    if (device.TryGetHapticCapabilities(out capabilities))
                    {
                        if (capabilities.supportsImpulse)
                        {
                            uint channel = 0;
                            float amplitude = 0.8f;
                            float duration = 0.3f;
                            device.SendHapticImpulse(channel, amplitude, duration);
                        }
                    }
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }

    [System.Serializable]
    public class Labelparam
    {        
        public RectTransform labelObject;
        public Transform targetObject;
        public Transform labelPos;
        public LineRenderer linerenderer;
        public Renderer targetMesh;
        public bool islabelline;
        public Vector3 Startpos, Prevpos;
    }
}
