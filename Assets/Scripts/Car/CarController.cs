using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace VehicleSystem
{
    public class CarController : MonoBehaviour
    {
        
        private CarAI carAI;

        public bool IsPlayer { get { return isPlayer; } set { isPlayer = value; } }
        [SerializeField] bool isPlayer = true;


       

        CarRaycast carRaycast;




        [Header("Inputs")]
                                               // If is player and not AI
        // Inputs for the car (only used if the car is controlled by players, not AI)

             
        [SerializeField] string throttleInput = "Vertical";                                     
        [SerializeField] string brakeInput = "Fire3";                                           
        [SerializeField] string turnInput = "Horizontal";
        [SerializeField] string boostInput = "Fire2";



        // Inputs for the car, outputs by the NN (Only with AI)
        float throttleInputAI;
        public float ThrottleInputAI { get { return throttleInputAI; } set { throttleInputAI = value; } }

        float brakeInputAI;
        public float BrakeInputAI { get { return brakeInputAI; } set { brakeInputAI = value; } }

        float boostInputAI;
        public float BoostInputAI { get { return boostInputAI; } set { boostInputAI = value; } }

        float turnInputAI;
        public float TurnInputAI { get { return turnInputAI; } set { turnInputAI = value; } }

        double lockSteeringAI;
        public double LockSteeringAI { get { return lockSteeringAI; } set { lockSteeringAI = value; } }


        // The turn action is not uniform
#pragma warning disable 0649
        [SerializeField] AnimationCurve turnInputCurve;

        [Header("Wheels")]
        [SerializeField] WheelCollider[] driveWheel;    // Wheels for drive
        [SerializeField] WheelCollider[] turnWheel;     // Wheels for turn


        #region BEHAVIOUR
        [Header("Behaviour")]
        // Car
        [SerializeField] AnimationCurve motorTorque;    // The throttle is not uniform
        [SerializeField] float brakeForce = 1500.0f;    
        [Range(0f, 100.0f)]
        [SerializeField] float steerAngle = 30.0f;      // The maximum angle for turn
        [Range(0.001f, 10.0f)]
        [SerializeField] float steerSpeed = 0.2f;       // The speed of the steering



        [SerializeField] float throttlePower = 4f;

        //Reset
        Vector3 spawnPosition;
        Quaternion spawnRotation;

        [SerializeField] Transform centerOfMass;
        [Range(0.5f, 3f)]
        [SerializeField] float downforce = 1.0f;


        public float AntiRoll = 20000.0f;
        #endregion


        float steering;
        public float Steering { get { return steering; } set { steering = value; } }

        float throttle;
        public float Throttle { get { return throttle; } set { throttle = value; } }

        [SerializeField] bool handbrake;
        public bool Handbrake { get { return handbrake; } set { handbrake = value; } }

        [SerializeField] float speed = 0.0f;
        public float Speed { get { return speed; } }


        #region BOOST
        [Header("Boost")]
        [SerializeField] float maxBoost = 10f;
        public float MaxBoost { get { return maxBoost; } }
        [SerializeField] float boost = 10f;
        public float Boost { get { return boost; } }
        [Range(0f, 1f)]
        [SerializeField] float boostRegen = 0.2f;
        public float BoostRegen { get { return boostRegen; } }
        [SerializeField] float boostForce = 5000;
        public float BoostForce { get { return boostForce; } }
        public bool boosting = false;
        #endregion
#pragma warning restore 0649 

        Rigidbody _rb;

        WheelCollider[] wheels;

        void Awake()
        {
            

            if (!isPlayer)
            {
                carAI = GetComponentInParent<CarAI>();

                throttleInputAI = 0f;
                brakeInputAI = 0f;
                boostInputAI = 0f;
                turnInputAI = 0f;
                lockSteeringAI = 0f;
            }
            else
            {
                carAI = null;
                carRaycast = GetComponentInParent<CarRaycast>();
                carRaycast.GenerateRays();
            }
    

            boost = maxBoost;

            _rb = GetComponent<Rigidbody>();
            spawnPosition = transform.position;
            spawnRotation = transform.rotation;

            if (centerOfMass)
            {
                _rb.centerOfMass = centerOfMass.localPosition;
            }

            wheels = GetComponentsInChildren<WheelCollider>();

            // Ignore border collissions (Border is for AI only)

            /*if (IsPlayer)
            {
                foreach (Collider collider in this.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Border").GetComponent<Collider>(), collider);

                }
            }*/
        

        }


      




        void FixedUpdate()
        {

            

            if (!isPlayer)
            {
                
                carAI.Speed = speed;
                //carAI.WheelSteerAngle = turnWheel[0].steerAngle;
                carAI.Boost = boost;

                carAI.AIUpdate();


                foreach (Tuple<string, double> o in carAI.GetOutputs())
                {
                    if (o.Item1 == "throttle") ThrottleInputAI = (float)o.Item2;
                    if (o.Item1 == "brake") BrakeInputAI = (float)o.Item2;
                    if (o.Item1 == "turn") TurnInputAI = (float)o.Item2;
                    if (o.Item1 == "locksteering") LockSteeringAI = o.Item2;
                }
            }
            else
            {
                //carRaycast.CalculateDistances();
            }

            
            speed = transform.InverseTransformDirection(_rb.velocity).z * 3.6f;

            #region GET_INPUTS

            boost += Time.deltaTime * boostRegen;
            if (boost > maxBoost) { boost = maxBoost; }

            if (isPlayer)
            {
                // Accelerate & brake
                if (throttleInput != "" && throttleInput != null)
                {
                    throttle = GetInput(throttleInput) - GetInput(brakeInput);
                    //Debug.Log(throttle);
                }
                // Boost
                boosting = (GetInput(boostInput) > 0.5f);
                // Turn
                steering = turnInputCurve.Evaluate(GetInput(turnInput)) * steerAngle;

            }
            else
            {
                
                // Accelerate & brake
                throttle = (float)(throttleInputAI) - (brakeInputAI);     
                // Boost
                boosting = (boostInputAI  > 0.5f);
                // Turn
                int ls = 0;
                if (lockSteeringAI > 0) ls = 1;
                steering = (1-ls) * turnInputCurve.Evaluate((float)(turnInputAI)) * steerAngle;
            }

            #endregion


            // Not roll
            DoRollBar(driveWheel[0], driveWheel[1]);
            DoRollBar(turnWheel[0], turnWheel[1]);





            #region APPLY_INPUTS

            // Direction
            foreach (WheelCollider wheel in turnWheel)
            {
                wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, steering, steerSpeed);
            }

            foreach (WheelCollider wheel in wheels)
            {
                wheel.brakeTorque = 0;
            }

            // Handbrake
            if (handbrake)
            {
                foreach (WheelCollider wheel in wheels)
                {
                    wheel.motorTorque = 0;
                    wheel.brakeTorque = brakeForce;
                }
            }
            // Throttle
            else if (Mathf.Abs(speed) < 4 || Mathf.Sign(speed) == Mathf.Sign(throttle))
            {
                foreach (WheelCollider wheel in driveWheel)
                {
                    wheel.brakeTorque = 0;
                    wheel.motorTorque = throttle * motorTorque.Evaluate(speed) * throttlePower;
                }
            }
            // Brake
            else
            {
                foreach (WheelCollider wheel in wheels)
                {
                    wheel.motorTorque = 0;
                    wheel.brakeTorque = Mathf.Abs(throttle) * brakeForce;
                }
            }


            // Boost
            if (boosting && boost > 0.1f)
            {
                _rb.AddForce(transform.forward * boostForce);

                boost -= Time.fixedDeltaTime;
                if (boost < 0f) { boost = 0f; }
            }


            #endregion


            // Downforce
            _rb.AddForce(transform.up * speed * downforce);


            
                

        }







        public void ResetPos()
        {
            transform.position = spawnPosition;
            transform.rotation = spawnRotation;

            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        public void toogleHandbrake(bool h)
        {
            handbrake = h;
        }


        private float GetInput(string input)
        {

            return Input.GetAxis(input);

        }


        public double WheelSteerAngle()
        {
            return turnWheel[0].steerAngle;
        }




        void DoRollBar(WheelCollider WheelL, WheelCollider WheelR)
        {
            WheelHit hit;
            float travelL = 1.0f;
            float travelR = 1.0f;

            bool groundedL = WheelL.GetGroundHit(out hit);
            if (groundedL)
                travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

            bool groundedR = WheelR.GetGroundHit(out hit);
            if (groundedR)
                travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

            float antiRollForce = (travelL - travelR) * AntiRoll;

            if (groundedL)
                GetComponent<Rigidbody>().AddForceAtPosition(WheelL.transform.up * -antiRollForce,
                                             WheelL.transform.position);
            if (groundedR)
                GetComponent<Rigidbody>().AddForceAtPosition(WheelR.transform.up * antiRollForce,
                                             WheelR.transform.position);
        }


     


    }
}
