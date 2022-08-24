using System;
using System.Collections.Generic;
using B.Inrerface;

namespace B.Note
{
    public abstract class Note
    {
        protected int _noteNumber;
        protected float _noteFrequency;

        public virtual int Number { get; set; }
        public virtual float Frequency { get; set; }
        public static bool operator ==(Note n1, Note n2) => n1?.Number == n2?.Number;
        public static bool operator !=(Note n1, Note n2) => n1.Number == n2.Number;
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            Note n2 = obj as Note;
            if (n2 == null) { return false; }
            return (Number == n2.Number);

        }
        public override int GetHashCode()
        {
            return Number;
        }

    }
    public abstract class NotStandartNote : Note
    {
        protected static float _rootFrequency;
        protected int _noteLogTemper12ClosestAnalog;
        protected static Dictionary<int, int> _noteLogTemper12ClosestAnalogs;
        protected static Dictionary<int, double> _note12Freq;

        public float RootFrequency { get => _rootFrequency; }
        public virtual int NoteLogTemper12ClosestAnalog
        {
            get => _noteLogTemper12ClosestAnalog;
        } 
        public virtual Dictionary<int, double> Notes12Freq 
        {
            get
            {
                if (_note12Freq != null)
                {
                    return _note12Freq;
                }
                else
                {
                   _note12Freq = new Dictionary<int, double>()
                       {{ 0 , 8.18 },
                        { 1 , 8.66 },
                        { 2 , 9.18 },
                        { 3 , 9.72 },
                        { 4 , 10.30 },
                        { 5 , 10.91 },
                        { 6 , 11.56 },
                        { 7 , 12.25 },
                        { 8 , 12.98 },
                        { 9 , 13.75 },
                        { 10 , 14.57 },
                        { 11 , 15.43 },
                        { 12 , 16.35 },
                        { 13 , 17.32 },
                        { 14 , 18.35 },
                        { 15 , 19.45 },
                        { 16 , 20.60 },
                        { 17 , 21.83 },
                        { 18 , 23.12 },
                        { 19 , 24.50 },
                        { 20 , 25.96 },
                        { 21 , 27.50 },
                        { 22 , 29.14 },
                        { 23 , 30.87 },
                        { 24 , 32.70 },
                        { 25 , 34.65 },
                        { 26 , 36.71 },
                        { 27 , 38.89 },
                        { 28 , 41.20 },
                        { 29 , 43.65 },
                        { 30 , 46.25 },
                        { 31 , 49.00 },
                        { 32 , 51.91 },
                        { 33 , 55.00 },
                        { 34 , 58.27 },
                        { 35 , 61.74 },
                        { 36 , 65.41 },
                        { 37 , 69.30 },
                        { 38 , 73.42 },
                        { 39 , 77.78 },
                        { 40 , 82.41 },
                        { 41 , 87.31 },
                        { 42 , 92.50 },
                        { 43 , 98.00 },
                        { 44 , 103.83 },
                        { 45 , 110.00 },
                        { 46 , 116.54 },
                        { 47 , 123.47 },
                        { 48 , 130.81 },
                        { 49 , 138.59 },
                        { 50 , 146.83 },
                        { 51 , 155.56 },
                        { 52 , 164.81 },
                        { 53 , 174.61 },
                        { 54 , 185.00 },
                        { 55 , 196.00 },
                        { 56 , 207.65 },
                        { 57 , 220.00 },
                        { 58 , 233.08 },
                        { 59 , 246.94 },
                        { 60 , 261.63 },
                        { 61 , 277.18 },
                        { 62 , 293.66 },
                        { 63 , 311.13 },
                        { 64 , 329.63 },
                        { 65 , 349.23 },
                        { 66 , 369.99 },
                        { 67 , 392.00 },
                        { 68 , 415.30 },
                        { 69 , 440.00 },
                        { 70 , 466.16 },
                        { 71 , 493.88 },
                        { 72 , 523.25 },
                        { 73 , 554.37 },
                        { 74 , 587.33 },
                        { 75 , 622.25 },
                        { 76 , 659.26 },
                        { 77 , 698.46 },
                        { 78 , 739.99 },
                        { 79 , 783.99 },
                        { 80 , 830.61 },
                        { 81 , 880.00 },
                        { 82 , 932.33 },
                        { 83 , 987.77 },
                        { 84 , 1046.50 },
                        { 85 , 1108.73 },
                        { 86 , 1174.66 },
                        { 87 , 1244.51 },
                        { 88 , 1318.51 },
                        { 89 , 1396.91 },
                        { 90 , 1479.98 },
                        { 91 , 1567.98 },
                        { 92 , 1661.22 },
                        { 93 , 1760.00 },
                        { 94 , 1864.66 },
                        { 95 , 1975.53 },
                        { 96 , 2093.00 },
                        { 97 , 2217.46 },
                        { 98 , 2349.32 },
                        { 99 , 2489.02 },
                        { 100 , 2637.02 },
                        { 101 , 2793.83 },
                        { 102 , 2959.96 },
                        { 103 , 3135.96 },
                        { 104 , 3322.44 },
                        { 105 , 3520.00 },
                        { 106 , 3729.31 },
                        { 107 , 3951.07 },
                        { 108 , 4186.01 },
                        { 109 , 4434.92 },
                        { 110 , 4698.64 },
                        { 111 , 4978.03 },
                        { 112 , 5274.04 },
                        { 113 , 5587.65 },
                        { 114 , 5919.91 },
                        { 115 , 6271.93 },
                        { 116 , 6644.88 },
                        { 117 , 7040.00 },
                        { 118 , 7458.62 },
                        { 119 , 7902.13 },
                        { 120 , 8372.02 },
                        { 121 , 8869.84 },
                        { 122 , 9397.27 },
                        { 123 , 9956.06 },
                        { 124 , 10548.08 },
                        { 125 , 11175.30 },
                        { 126 , 11839.82 },
                        { 127 , 12543.85 }};
                    return _note12Freq;
                }
            }
        }
    }
    public class NoteLogTemper53 : NotStandartNote
    {
        private static Dictionary<int, float> NotesFrequency;
        private static Dictionary<float, int> FrequencysNote;

        public override int Number
        {

            get => _noteNumber;
            set
            {
                if ((value >= 0) && (value <= NotesFrequency.Count))
                {
                    _noteNumber = value;
                    _noteFrequency = NotesFrequency[value];
                }
            }
        }
        public override float Frequency
        {

            get => _noteFrequency;
            set
            {
                if ((value >= 20) && (value <= 20_000))
                {
                    _noteFrequency = value;
                    Number = FrequencysNote[value];
                }
            }
        }

        public NoteLogTemper53(int num, float root = 7.83f)
        {
            _rootFrequency = root;
            if (NotesFrequency == null) 
            { CreateNoteFreqTebles(root); 
              CreateClosestAnalogTables();
            }
            Number = num;
            _noteLogTemper12ClosestAnalog = _noteLogTemper12ClosestAnalogs[Number];
        }
        public NoteLogTemper53(float freq, float root = 7.83f)
        {
            _rootFrequency = root;
            if (NotesFrequency == null) { CreateNoteFreqTebles(root); CreateClosestAnalogTables(); }
            Frequency = freq;
            _noteLogTemper12ClosestAnalog = _noteLogTemper12ClosestAnalogs[FrequencysNote[freq]];
        }

        static private void CreateNoteFreqTebles(float root)
        {
            NotesFrequency = new Dictionary<int, float>();
            FrequencysNote = new Dictionary<float, int>();
            while (root > 40) { root /= 2; }
            int id = 0;
            while (root < 20_000)
            {
                for (int i = 0; i < 53; i++)
                {
                    float CurNoteFreq = ((root - root / 2) * ((i + 1) % 53)) / 53 + root / 2;
                    if ((CurNoteFreq > 20) && (CurNoteFreq < 20_000))
                    {
                        NotesFrequency.Add(id, CurNoteFreq);
                        FrequencysNote.Add(CurNoteFreq, id);
                        id++;
                    }
                }
                root *= 2;
            }
        }
        private void CreateClosestAnalogTables()
        {
            _noteLogTemper12ClosestAnalogs = new Dictionary<int, int>();
            
            foreach (var key in NotesFrequency.Keys)
            {
                for (int i = 0; i < 126; i++)
                {
                    if ((Math.Abs(NotesFrequency[key] - Notes12Freq[i])) < (Math.Abs(NotesFrequency[key] - Notes12Freq[i + 1])))
                    {
                        _noteLogTemper12ClosestAnalogs.Add(key, i);
                        break;
                    }
                }
                if (_noteLogTemper12ClosestAnalogs.Count < (key - 1))
                {
                    _noteLogTemper12ClosestAnalogs.Add(key, 127);
                }
            }
        }
    }
}