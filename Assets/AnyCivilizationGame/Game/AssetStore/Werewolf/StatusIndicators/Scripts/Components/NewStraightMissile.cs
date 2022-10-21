using UnityEngine;
using System.Linq;
using Werewolf.StatusIndicators.Services;
using System.Collections;

namespace Werewolf.StatusIndicators.Components {
	public class NewStraightMissile : SpellIndicator {

		// Properties

		public override ScalingType Scaling { get { return ScalingType.LengthOnly; } }

		// Methods

		public override void Update() {
            if (Manager != null)
            {
                Vector3 v = FlattenVector(Manager.Get3DMousePosition()) - Manager.transform.position;
                if (v != Vector3.zero)
                {
                    //Manager.transform.rotation = Quaternion.LookRotation(v);
                    Manager.transform.rotation = Quaternion.LookRotation(transform.GetComponentInParent<PlayerAttack>().lookPos);
                }
            }

        }
	}
}
