using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using B.Note;
namespace B.Inrerface
{
    
    public interface INSNEvent
    {
        


        public NotStandartNote Note { get; set; }
        public int Velocity { get; set; }
        public float StartTime { get; set; }
        public float Duration { get; set; }

    }
    public interface IWorkWithXyidi
    {
        public void AddNSNEvent(float startTime, float playTime, int noteNumber);
        public void RemoveNSNEvent(float startTime, float playTime, int noteNumber);
        //public 
    }
    public interface IGUI
    {
        public void Start(List<INSNEvent> NSNEvents);
        public void PrintError(string ErrorMessage);
    }
    public interface IAudio
    {
        public void SoundIt(List<List<INSNEvent>> NSNEvents);
    }
}
