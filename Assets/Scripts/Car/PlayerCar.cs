using System.Collections.Generic;
using UnityEngine;

namespace Car
{
    public class PlayerCar : MonoBehaviour
    {
        // Wheels Setting /////////////////////////////////

        public CarWheels carWheels;

        [System.Serializable]
        public class CarWheels
        {
            public ConnectWheel wheels;
            public WheelSetting setting;
        }
        
        [System.Serializable]
        public class ConnectWheel
        {
            public bool frontWheelDrive = true;

            public Transform frontRight;
            public Transform frontLeft;

            public Transform frontRightAxle;
            public Transform frontLeftAxle;



            public bool backWheelDrive = true;
            public Transform backRight;
            public Transform backLeft;


            public Transform backRightAxle;
            public Transform backLeftAxle;

        }
        
        [System.Serializable]
        public class WheelSetting
        {
            public float FrontWheelsRadius = 0.4f;
            public float BackWheelsRadius = 0.4f;
            public float Weight = 1000.0f;
            public float Distance = 0.2f;
        }
        
        // Car Engine Setting /////////////////////////////////

        public CarSetting carSetting;

        [System.Serializable]
        public class CarSetting
        {

            public bool showNormalGizmos = false;
            public Transform carSteer;
            public HitGround[] hitGround;

            public List<Transform> cameraSwitchView;

            public float springs = 25000.0f;
            public float dampers = 1500.0f;

            public float carPower = 120f;
            public float shiftPower = 150f;
            public float brakePower = 8000f;

            public Vector3 shiftCentre = new Vector3(0.0f, -0.8f, 0.0f);

            public float maxSteerAngle = 25.0f;

            public float shiftDownRPM = 1500.0f;
            public float shiftUpRPM = 2500.0f;
            public float idleRPM = 500.0f;

            public float stiffness = 2.0f;

            public bool automaticGear = true;

            public float[] gears = { -10f, 9f, 6f, 4.5f, 3f, 2.5f };


            public float LimitBackwardSpeed = 60.0f;
            public float LimitForwardSpeed = 220.0f;

        }
        
        [System.Serializable]
        public class HitGround
        {
       
            public string tag = "street";
            public bool grounded = false;
            public AudioClip brakeSound;
            public AudioClip groundSound;
            public Color brakeColor;
        }
        
           // Lights Setting ////////////////////////////////

    public CarLights carLights;

    [System.Serializable]
    public class CarLights
    {
        public Light[] brakeLights;
        public Light[] reverseLights;
    }

    // Car sounds /////////////////////////////////

    public CarSounds carSounds;

    [System.Serializable]
    public class CarSounds
    {
        public AudioSource IdleEngine, LowEngine, HighEngine;

        public AudioSource crash;

        public AudioSource nitro;
        public AudioSource switchGear;
    }

    // Car Particle /////////////////////////////////

    public CarParticles carParticles;

    [System.Serializable]
    public class CarParticles
    {
        public GameObject brakeParticlePerfab;
        public ParticleSystem shiftParticle1, shiftParticle2;
        private GameObject[] wheelParticle = new GameObject[4];
    }

    // Car Engine Setting /////////////////////////////////

    [HideInInspector]
    public float speed = 0.0f;
    [HideInInspector]
    public bool brake;
    [HideInInspector]
    public float steer = 0;
    [HideInInspector]
    public float curTorque = 100f;
    [HideInInspector]
    public float powerShift = 100;
    [HideInInspector]
    public bool shift;

    [HideInInspector]
    public int currentGear = 0;
    [HideInInspector]
    public bool NeutralGear = true;
    [HideInInspector]
    public float motorRPM = 0.0f;
    [HideInInspector]
    public bool Backward = false;


    private float lastSpeed = -10.0f;
    private float accel = 0.0f;
    private float torque = 100f;

    private bool shifmotor;
    private bool shifting = false;


    private float[] efficiencyTable = { 0.6f, 0.65f, 0.7f, 0.75f, 0.8f, 0.85f, 0.9f, 1.0f, 1.0f, 0.95f, 0.80f, 0.70f, 0.60f, 0.5f, 0.45f, 0.40f, 0.36f, 0.33f, 0.30f, 0.20f, 0.10f, 0.05f };
    private float efficiencyTableStep = 250.0f;


    private float Pitch;
    private float PitchDelay;
    private float shiftTime = 0.0f;
    private float shiftDelay = 0.0f;
        
        private float wantedRPM = 0.0f;
        private float w_rotate;
        private float slip, slip2 = 0.0f;
        
        private GameObject[] Particle = new GameObject[4];
        private Vector3 steerCurAngle;
        private Rigidbody myRigidbody;

        void Awake()
        {
            if (carSetting.automaticGear) NeutralGear = false;

            myRigidbody = transform.GetComponent<Rigidbody>();
        }

        void OnCollisionEnter(Collision collision)
        {

            if (!carSounds.crash.isPlaying)
                carSounds.crash.Play();

            if (collision.transform.root.GetComponent<PlayerCar>())
            {
                collision.transform.root.GetComponent<PlayerCar>().slip2 = Mathf.Clamp(collision.relativeVelocity.magnitude, 0.0f, 5.0f);

                myRigidbody.angularVelocity = new Vector3(-myRigidbody.angularVelocity.x * 0.5f, myRigidbody.angularVelocity.y * 0.5f, -myRigidbody.angularVelocity.z * 0.5f);
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, myRigidbody.velocity.y * 0.5f, myRigidbody.velocity.z);
            
            }

        }
    
        void OnCollisionStay(Collision collision)
        {

            if (collision.transform.root.GetComponent<PlayerCar>())
                collision.transform.root.GetComponent<PlayerCar>().slip2 = 5.0f;
        }
    }
}
