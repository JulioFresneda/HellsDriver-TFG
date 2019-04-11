using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSystem
{
    [RequireComponent(typeof(WheelCollider))]

    public class CarSuspension : MonoBehaviour
    {

        public GameObject _wheelModel;
        private WheelCollider _wheelCollider;
#pragma warning disable 0649
        [SerializeField] Vector3 localRotOffset;
#pragma warning restore 0649
        private float lastUpdate;

        void Start()
        {
            lastUpdate = Time.realtimeSinceStartup;

            _wheelCollider = GetComponent<WheelCollider>();
        }


        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag == "Border")
            {
                GetComponentInParent<CarFitnessTest>().SetDoneCalculatingFitness(true);
            }
        }

        void FixedUpdate()
        {
            if (Time.realtimeSinceStartup - lastUpdate < 1f / 60f)
            {
                return;
            }
            lastUpdate = Time.realtimeSinceStartup;

            if (_wheelModel && _wheelCollider)
            {
                Vector3 pos = new Vector3(0, 0, 0);
                Quaternion quat = new Quaternion();
                _wheelCollider.GetWorldPose(out pos, out quat);

                
                _wheelModel.transform.rotation = quat;
                if( _wheelCollider.gameObject.name[1] == 'L' ) _wheelModel.transform.Rotate(0, 90, 0);
                else _wheelModel.transform.Rotate(0, -90, 0);
                _wheelModel.transform.localRotation *= Quaternion.Euler(localRotOffset);
                _wheelModel.transform.position = pos;

                WheelHit wheelHit;
                _wheelCollider.GetGroundHit(out wheelHit);
            }
        }
    }
}
