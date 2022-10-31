using UnityEngine;
using System.Linq;
using Werewolf.StatusIndicators.Services;
using System.Collections;
using DG.Tweening;

namespace Werewolf.StatusIndicators.Components
{
    public class ReducableStraightMissile : SpellIndicator
    {

        // Fields

        private float arrowHeadScale;
        private Projector arrowHeadProjector;

        public GameObject ArrowHead;
        public float MinimumRange;

 
        private RaycastHit hit;

        // Properties
        public float offSetValue= -0.0438f;

        public override ScalingType Scaling { get { return ScalingType.LengthOnly; } }

        // Methods
        public LayerMask layerMask;
        public override void Initialize()
        {
            base.Initialize();
            arrowHeadProjector = ArrowHead.GetComponent<Projector>();
            arrowHeadScale = arrowHeadProjector.orthographicSize;
        }
        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.GetComponentInParent<PlayerAttack>().transform.position + new Vector3(0, .5f, 0), transform.GetComponentInParent<PlayerAttack>().lookPos.normalized * (Range + offSetValue + (ArrowHead.transform.localScale.x)), Color.green);

        }
        public override void Update()
        {
            if (Manager != null)
            {


                if (Physics.Raycast(transform.GetComponentInParent<PlayerAttack>().transform.position + new Vector3(0, .5f, 0), (transform.GetComponentInParent<PlayerAttack>().lookPos.normalized), out hit, (Range + offSetValue + (ArrowHead.transform.localScale.x)), layerMask))
                {
                    Debug.Log(hit.distance);
                   // Scale = ;


                }
                else
                {

                  //  Scale = (Range - ArrowHeadDistance()) * 2;

                }


                ArrowHead.transform.localPosition = new Vector3(0, (Scale * 0.5f) + ArrowHeadDistance()+ offSetValue , 0);
            }
        }

        public override void OnValueChanged()
        {
            base.OnValueChanged();
            arrowHeadProjector.aspectRatio = 1f;
            arrowHeadProjector.orthographicSize = arrowHeadScale;
        
        }

        /// <summary>
        /// Calculate distance of the Arrow Head from the centre point when scaling.
        /// </summary>
        private float ArrowHeadDistance()
        {
            //1 yerine 0.96 idi.
            return (float)arrowHeadProjector.orthographicSize ;
        }
    }
}
