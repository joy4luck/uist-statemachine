
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeserializeJSONFromNetwork
{
    class DrawClass
    {
        public enum State
        {
            IDLE,
            SCULPT,
            ROLL,
            ZOOM,
        }

        public enum EventType
        {
            APPEAR,
            MOVE,
            VANISH
        }

        public State state = State.IDLE;
        double[,] v;
        static int WIDTH = 50;
        static int HEIGHT = 37;
        static float ZOOM_THRESHOLD = 0.1f;
        float orig_dist = 0;

        public DrawClass()
        {
            v = new double[WIDTH,HEIGHT];
        }

        public override string ToString()
        {
            StringBuilder outstring = new StringBuilder();
            for (int i = 0; i < WIDTH; i++){
                for (int j = 0; j < HEIGHT; j++){
                    outstring.Append(' ');
                    outstring.Append(v[i,j]);
                }
                outstring.Append('\n');
            }
            return outstring.ToString();
        }

        private bool ZoomyDistance(double new_dist)
        {
            double diff = System.Math.Abs(new_dist - orig_dist);
            if (diff / orig_dist > ZOOM_THRESHOLD) return true;
            return false;
        }

        private void ChangeState(int fingers, double dist){
            if (fingers == 0)
            {
                state = State.IDLE;
            }
            else if (fingers == 1)
            {
                if (state == State.IDLE)
                {
                    state = State.SCULPT;
                }
            }
            else if (fingers == 2)
            {
                if (state == State.IDLE || state == State.SCULPT)
                {
                    state = State.ROLL;
                    orig_dist = (float)dist;
                }
                else if (state == State.ROLL)
                {
                    if (ZoomyDistance(dist))
                    {
                        state = State.ZOOM;
                    }
                }
            }
        }

        public void HandleSensorData(SensorData sd)
        {
            ChangeState(sd.FingerCount(), sd.Distance());
        }

        public void UpdatePlot(SensorData sd){
            int x, y;
            if (sd.touched[0])
            {
                x = (int)sd.f0[0] / 100;
                y = (int)sd.f0[1] / 100;
                v[x,y] = sd.f0[2];
            }
        }
    }
}
