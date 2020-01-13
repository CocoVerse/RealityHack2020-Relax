internal class ConditionMetForDurationTracker {
    private readonly float windowSize;
    private bool conditionMet;
    private float conditionMetStartTime;


    public ConditionMetForDurationTracker(float windowSize) {
        this.windowSize = windowSize;
    }

    public bool Ready(float time) {
        return conditionMet && conditionMetStartTime + windowSize <= time;
    }

    public void Yes(float time) {
        if(!conditionMet) {
            conditionMet = true;
            conditionMetStartTime = time;
        }
    }

    public void No() {
        conditionMet = false;
    }
}