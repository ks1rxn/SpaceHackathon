using UnityEngine;

namespace SpaceHacathon.Helpers.PidControllers {

    public class VectorPid {
        private float pFactor;
        private float iFactor;
        private float dFactor;

        private Vector3 integral;
        private Vector3 lastError;

        public void Initiate(Vector3 factors) {
            pFactor = factors.x;
            iFactor = factors.y;
            dFactor = factors.z;
        }
 
        public Vector3 Update(Vector3 currentError, float timeFrame) {
            integral += currentError * timeFrame;
            Vector3 deriv = (currentError - lastError) / timeFrame;
            lastError = currentError;
            return currentError * pFactor
                + integral * iFactor
                + deriv * dFactor;
        }
    }

}