using B.Inrerface;
using B.Note;
using System;

namespace B
{
    public class NSNEvent : INSNEvent
    {
        private NotStandartNote _note;
        private float _startTime;
        private float _duration;
        private int _velocity;


        public NotStandartNote Note { get => _note; set => _note = value; }
        public float StartTime
        {
            get => _startTime;
            set
            {
                if (value > 0) { _startTime = value; }
            }
        }
        public float Duration
        {
            get => _duration;
            set
            {
                if (value > 0) { _duration = value; }
            }
        }
        public int Velocity
        {
            get => _velocity;
            set
            {
                if ((value >= 0) && (value <= 127)) { _velocity = value; }
            }
        }
        public NSNEvent(NotStandartNote n, int vel, float st, float dr)
        {
            if (n == null) { throw new Exception("Error: Can't create NSNEvent, becose Note is null"); }
            Note = n;
            Velocity = vel;
            StartTime = st;
            Duration = dr;
        }
    }
}