using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace krisnart.ControllerTut
{
    public class HighlightController : MonoBehaviour
    {
        public float maximumWidth = 7;
        public bool HighlightAnimate;
        float HighlightValue;
        bool Highlight, peakReached;

        void Update()
        {
            if (Highlight)
            {
                Highlighter();
            }
        }

        private void OnEnable()
        {
            HighlightStart();
        }

        private void OnDisable()
        {
            HighlightStop();
        }

        public void HighlightStop()
        {
            Highlight = false;
            HighlightValue = 0;
            GetComponent<Outline>().OutlineWidth = 0;
        }

        public void HighlightStart()
        {
            Highlight = true;
        }

        public void Highlighter()
        {
            if (HighlightAnimate)
            {
                if (HighlightValue < 1)
                    peakReached = false;
                if (HighlightValue > maximumWidth)
                    peakReached = true;

                if (!peakReached)
                    HighlightValue += 0.14f * 2;
                else
                    HighlightValue -= 0.14f * 2;
            }
            else
            {
                HighlightValue = maximumWidth;
            }

            GetComponent<Outline>().OutlineWidth = HighlightValue;

        }
    }
}
