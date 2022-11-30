using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using System.Timers;

namespace SlipNTrip
{
    public class TestResults
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string PatientName { get; set; }
        public int PatientID { get; set; }
        public string TestName { get; set; }
        public DateTime Date { get; set; }
        public string Direction { get; set; }    
        public double Distance { get; set; }
        public double MotorSpeed { get; set; }
        public bool StepTaken { get; set; }
        public TimeSpan TimeBetweenStep { get; set; }
        public double DistanceBetweenStep { get; set; }

        public string WasAStepTaken()
        {
            if(StepTaken)
                return "Yes";
            return "No";
        }

        public override string ToString()
        {
            return this.TestName + " (" + this.Date.ToString() + ")";
        }
    }
}
