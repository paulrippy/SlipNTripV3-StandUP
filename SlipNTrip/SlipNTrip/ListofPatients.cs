using SlipNTrip.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SQLite;

namespace SlipNTrip
{
    class ListofPatients
    {
        string dbPathPatients = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "patients.db3");
        string dbPathTestResults = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "testResults.db3");

        public void GenerateListofPatients()
        {
            var dbPatients = new SQLiteConnection(dbPathPatients);
            var dbTestResults = new SQLiteConnection(dbPathTestResults);
            dbPatients.CreateTable<Patient>();
            dbPatients.DeleteAll<Patient>();
            dbTestResults.CreateTable<TestResults>();
            dbTestResults.DeleteAll<TestResults>();
            Random rnd = new Random();
            
            string[] patientGender = { "Male","Female","Male","Male","Female",
                                        "Female","Male","Male","Male","Male",
                                        "Male","Male","Male","Female","Male",
                                        "Male","Male","Female","Male","Female",
                                        "Male","Female","Female","Male","Male",
                                        "Female","Female","Female","Female","Male",
                                        "Female","Male","Male","Male","Male",
                                        "Male","Male","Female","Female","Male",
                                        "Male","Female","Male","Female","Male",
                                        "Male","Female","Male","Female","Female"};
            string[] patientName = {"Patrick Farrell","Evelyn Williams","Chester Williams","Andrew Elliott","Melanie Crawford",
                                    "Isabella Harrison","Arnold Armstrong","Fenton Payne","Preston Bennett","Garry Campbell",
                                    "Marcus Baker","Justin Foster","James Watson","Catherine Stewart","Arnold Perry",
                                    "Stuart Alexander","Paul Johnston","Chelsea Morgan","Adam Harris","Savana Ferguson",
                                    "Edgar Harris","Tess Wells","Lilianna Dixon","Aldus Tucker","Abraham Harper",
                                    "Olivia Holmes","Aida Owens","Lucia Williams","Cherry Higgins","David Elliott",
                                    "Rebecca Myers","Martin Smith","John Ryan","John Farrell","Maximilian Johnson",
                                    "Max Richards","Dale Baker","Kelsey Robinson","Brooke Riley","Adrian Cunningham",
                                    "Oliver Stevens","Abigail Adams","Paul Perry","Chelsea Roberts","Michael Miller",
                                    "James Hunt","Valeria Hill","Kristian Williams","Victoria Anderson","Gianna Alexander"};
            string[] directionArray = { "f", "F", "forward", "Forward", "b", "B", "backwards", "Backwards" };
            double[] decimalArray = { 0, 0.5, 0, 0.5 };
            double[] heightDecimalArray = { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.10, 0.11 };

            string tempPatientID;
            for (int i = 0; i < 50; i++)
            {
                if (i + 1 < 10)
                {
                    tempPatientID = "M_00" + (i + 1);
                }
                else
                    tempPatientID = "M_0" + (i + 1);
                Patient patient = new Patient()
                {
                    PatientID = tempPatientID,
                    Name = patientName[i],
                    Gender = patientGender[i],
                    Age = rnd.Next(50, 80),
                    Height = rnd.Next(5, 6) + heightDecimalArray[rnd.Next(0, 11)],
                    Weight = rnd.Next(120, 300) + decimalArray[rnd.Next(0, 3)],
                    ShoeSize = rnd.Next(6, 12) + decimalArray[rnd.Next(0, 3)]
                };

                dbPatients.Insert(patient);

                for (int j = 0; j < 5; j++)
                {
                    TestResults testResults = new TestResults()
                    {
                        PatientID = patient.ID,
                        PatientName = patientName[i],
                        TestName = "Test #" + (j + 1),
                        Date = DateTime.Now,
                        Direction = directionArray[rnd.Next(0, 7)],
                        Distance = rnd.Next(0, 15),
                        MotorSpeed = rnd.Next(15, 35),
                        StepTaken = false,
                        TimeBetweenStep = DateTime.Now - DateTime.Now,
                        DistanceBetweenStep = 0.0
                    };

                    dbTestResults.Insert(testResults);
                }
            }
        }
    }
}
