using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WeavingTestData.UnsupportedTypes.Contracts
{
    public class NotAContract : MonoBehaviour, IActiveJointHost
    {
        public Part GetHostPart()
        {
            throw new NotImplementedException();
        }

        public Transform GetLocalTransform()
        {
            throw new NotImplementedException();
        }

        public void OnJointInit(ActiveJoint joint)
        {
            throw new NotImplementedException();
        }

        public void OnDriveModeChanged(ActiveJoint.DriveMode mode)
        {
            throw new NotImplementedException();
        }
    }
}
