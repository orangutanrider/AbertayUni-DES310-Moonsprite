using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsBurstLerper : MonoBehaviour, ITimelineEvent
{
    #region Burst Parameter Classes
    [System.Serializable]
    public class PsBurstParameters
    {
        public int burstIndexOnParticleSystem = 0;
        [Space]
        public bool burstCountLerpEnabled = false;
        public PsBurstCount burstCountLerp;
        [Space]
        public bool intervalLerpEnabled = false;
        public PsBurstInterval burstIntervalLerp;
        [Space]
        public bool probabilityLerpEnabled = false;
        public PsBurstProbability burstProbabilityLerp;

        public PsBurstParameters(int _burstIndex, bool _countEnabled, bool _intervalEnabled, bool _probabilityEnabled, PsBurstCount _count = null, PsBurstInterval _interval = null, PsBurstProbability _probability = null)
        {
            burstIndexOnParticleSystem = _burstIndex;

            burstCountLerpEnabled = _countEnabled;
            burstCountLerp = _count;

            intervalLerpEnabled = _intervalEnabled;
            burstIntervalLerp = _interval;

            probabilityLerpEnabled = _probabilityEnabled;
            burstProbabilityLerp = _probability;
        }
    }

    [System.Serializable]
    public class PsBurstCount
    {
        public short randomMinCountMin = 0;
        public short randomMinCountMax = 1;
        [Space]
        public short randomMaxCountMin = 0;
        public short randomMaxCountMax = 1;
        [Space]
        public AnimationCurve curve;

        public PsBurstCount(short _randomMinCountMin, short _randomMinCountMax, short _randomMaxCountMin, short _randomMaxCountMax, AnimationCurve _curve = null)
        {
            randomMinCountMin = _randomMinCountMin;
            randomMinCountMax = _randomMinCountMax;

            randomMaxCountMin = _randomMaxCountMin;
            randomMaxCountMax = _randomMaxCountMax;

            curve = _curve;
        }
    }

    [System.Serializable]
    public class PsBurstInterval
    {
        public float min = 0;
        public float max = 1;
        public AnimationCurve curve;

        public PsBurstInterval(float _min, float _max, AnimationCurve _curve = null)
        {
            min = _min;
            max = _max;
            curve = _curve;
        }
    }

    [System.Serializable]
    public class PsBurstProbability
    {
        public float min = 0;
        public float max = 1;
        public AnimationCurve curve;

        public PsBurstProbability(float _min, float _max, AnimationCurve _curve = null)
        {
            min = _min;
            max = _max;
            curve = _curve;
        }
    }
    #endregion

    public ParticleSystem ps;

    [Header("Settings")]
    public bool startLerpingOnStart = false;
    public bool startLerpingOnTimelineEvent = false;
    [Space]
    public float changeTime = 10;
    [Space]
    public bool overrideAllCurvesToLinear = false;
    public List<PsBurstParameters> lerpParametersForBursts = new List<PsBurstParameters>();

    [Header("These are exposed so you can test your changes, they're set to 0 and false, when the game starts")]
    public float changeTimer = 0;
    public bool active = false;

    [Header("ReadMe")]
    [TextArea(10, 100)]
    public string ReadMeText = "If you want to trigger this via a timeline event you have to manually add the game object (that this script is attached to) to the timeline event master to register it."
       + System.Environment.NewLine + "Also, use 0 as the start and 1 as the end, for the time values on the curves.";

    void Start()
    {
        if (startLerpingOnStart == false)
        {
            return;
        }
        active = true;
    }

    void Update()
    {
        if (active == false)
        {
            return;
        }

        if (changeTimer > changeTime)
        {
            return;
        }

        changeTimer = changeTimer + Time.deltaTime;

        for (int loop = 0; loop < lerpParametersForBursts.Count; loop++)
        {
            ParticleSystem.Burst newBurst = CreateBurstFromParametersAndLerp(lerpParametersForBursts[loop]);
            ps.emission.SetBurst(lerpParametersForBursts[loop].burstIndexOnParticleSystem, newBurst);
        }
    }

    void ITimelineEvent.TimelineEvent(int eventIndex)
    {
        if (startLerpingOnTimelineEvent == false)
        {
            return;
        }
        active = true;
    }

    ParticleSystem.Burst CreateBurstFromParametersAndLerp(PsBurstParameters burstParameters)
    {
        #region Error Handling
        if (burstParameters == null)
        {
            Debug.LogError("burst parameters recieved was null");
            return ErrorBurst();
        }

        int burstIndex = burstParameters.burstIndexOnParticleSystem;

        if (burstIndex < 0 || burstIndex > ps.emission.burstCount - 1)
        {
            Debug.LogError("burstIndex recieved is outside the bounds of the particle system's array of bursts");
            return ErrorBurst();
        }
        #endregion

        ParticleSystem.Burst burstBase = ps.emission.GetBurst(burstIndex);
        ParticleSystem.Burst returnBurst = burstBase;

        if (burstParameters.burstCountLerpEnabled == true)
        {
            returnBurst = WriteCountLerpToBurst(returnBurst, burstParameters.burstCountLerp);
        }
        if (burstParameters.intervalLerpEnabled == true)
        {
            returnBurst = WriteIntervalLerpToBurst(returnBurst, burstParameters.burstIntervalLerp);
        }
        if (burstParameters.probabilityLerpEnabled == true)
        {
            returnBurst = WriteProbabilityLerpToBurst(returnBurst, burstParameters.burstProbabilityLerp);
        }

        return returnBurst;
    }

    ParticleSystem.Burst WriteCountLerpToBurst(ParticleSystem.Burst burstToWriteTo, PsBurstCount countLerp)
    {
        if (countLerp == null)
        {
            Debug.LogError("countLerp was null");
        }

        short newBurstCountMin = 0;
        short newBurstCountMax = 0;
        ParticleSystem.Burst newBurst = burstToWriteTo;

        if (overrideAllCurvesToLinear == false && countLerp.curve != null)
        {
            float curveTime = countLerp.curve.Evaluate(changeTimer / changeTime);

            newBurstCountMin = FloatToShort(Mathf.LerpUnclamped(countLerp.randomMinCountMin, countLerp.randomMinCountMax, curveTime));
            newBurst.minCount = newBurstCountMin;

            newBurstCountMax = FloatToShort(Mathf.LerpUnclamped(countLerp.randomMaxCountMin, countLerp.randomMaxCountMax, curveTime));
            newBurst.maxCount = newBurstCountMax;

            return newBurst;
        }
        if(overrideAllCurvesToLinear == false && countLerp.curve == null)
        {
            Debug.LogWarning("Curve was null, falling back on linear override");
        }

        newBurstCountMin = FloatToShort(Mathf.Lerp(countLerp.randomMinCountMin, countLerp.randomMinCountMax, changeTimer / changeTime));
        newBurst.minCount = newBurstCountMin;

        newBurstCountMax = FloatToShort(Mathf.Lerp(countLerp.randomMaxCountMin, countLerp.randomMaxCountMax, changeTimer / changeTime));
        newBurst.maxCount = newBurstCountMax;

        return newBurst;
    }

    ParticleSystem.Burst WriteIntervalLerpToBurst(ParticleSystem.Burst burstToWriteTo, PsBurstInterval intervalLerp)
    {
        if (intervalLerp == null)
        {
            Debug.LogError("intervalLerp was null");
        }

        float newBurstInterval = 0;
        ParticleSystem.Burst newBurst = burstToWriteTo;

        if (overrideAllCurvesToLinear == false && intervalLerp.curve != null)
        {
            float curveTime = intervalLerp.curve.Evaluate(changeTimer / changeTime);

            newBurstInterval = Mathf.Lerp(intervalLerp.min, intervalLerp.max, curveTime);
            newBurst.repeatInterval = newBurstInterval;

            return newBurst;
        }
        if (overrideAllCurvesToLinear == false && intervalLerp.curve == null)
        {
            Debug.LogWarning("Curve was null, falling back on linear override");
        }

        newBurstInterval = Mathf.Lerp(intervalLerp.min, intervalLerp.max, changeTimer / changeTime);
        newBurst.repeatInterval = newBurstInterval;

        return newBurst;
    }

    ParticleSystem.Burst WriteProbabilityLerpToBurst(ParticleSystem.Burst burstToWriteTo, PsBurstProbability probabilityLerp)
    {
        if (probabilityLerp == null)
        {
            Debug.LogError("probabilityLerp was null");
        }

        short newBurstProbability = 0;
        ParticleSystem.Burst newBurst = burstToWriteTo;

        if (overrideAllCurvesToLinear == false && probabilityLerp.curve != null)
        {
            float curveTime = probabilityLerp.curve.Evaluate(changeTimer / changeTime);

            newBurstProbability = FloatToShort(Mathf.LerpUnclamped(probabilityLerp.min, probabilityLerp.max, curveTime));
            newBurst.probability = newBurstProbability;

            return newBurst;
        }
        if (overrideAllCurvesToLinear == false && probabilityLerp.curve == null)
        {
            Debug.LogWarning("Curve was null, falling back on linear override");
        }

        newBurstProbability = FloatToShort(Mathf.LerpUnclamped(probabilityLerp.min, probabilityLerp.max, changeTimer / changeTime));
        newBurst.probability = newBurstProbability;

        return newBurst;
    }

    ParticleSystem.Burst ErrorBurst()
    {
        // usually I would just return null, but it doesn't let you do that for bursts
        Debug.LogWarning("ErrorBurst");
        return new ParticleSystem.Burst(0, 0, 0, 0, 0);
    }

    short FloatToShort(float x)
    {
        //https://stackoverflow.com/questions/25201304/convert-float-to-short-with-minimal-loss-of-precision
        if (x < short.MinValue)
        {
            return short.MinValue;
        }
        if (x > short.MaxValue)
        {
            return short.MaxValue;
        }
        return (short)Mathf.Round(x);
    }
}
