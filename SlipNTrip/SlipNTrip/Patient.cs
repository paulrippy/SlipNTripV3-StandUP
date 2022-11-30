using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SlipNTrip
{
    public class Patient
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string PatientID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public double Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double ShoeSize { get; set; }

        private int maxAge = 120;
        private int maxHeight = 8;  //feet
        private int maxWeight = 300; // lb
        private int maxShoeSize = 15; //US Shoe Size

        public override string ToString()
        {
            return this.PatientID;
        }

        public bool isAgeWithinRange()
        {
            if(this.Age < 0 || this.Age > maxAge || (int)(((decimal)this.Age % 1) * 10) != 0)
            {
                return false;
            }
            return true;
        }

        public bool isHeightWithinRange()
        {
            int size = this.Height.ToString().Length;
            if (size <= 3)
            {
                if (this.Height < 0 || this.Height > maxHeight)
                    return false;
            }
            else
            {
                if (this.Height < 0 || this.Height > maxHeight || (int)(((decimal)this.Height % 1) * 100) > 11)
                    return false;
            }
            return true;
        }

        public bool isWeightWithinRange()
        {
            int size = this.Weight.ToString().Length;
            if (size <= 3)
            {
                if (this.Weight < 0 || this.Weight > maxWeight)
                    return false;
            }
            else
            {
                if (this.Weight < 0 || this.Weight > maxWeight || ((int)(((decimal)this.Weight % 1) * 10) != 5 && (int)(((decimal)this.Weight % 1) * 10) != 0))
                    return false;
            }
            return true;
        }

        public bool isShoeSizeWithinRange()
        {
            int size = this.ShoeSize.ToString().Length;
            if(ShoeSize <= 2)
            {
                if (this.ShoeSize < 0 || this.ShoeSize > maxShoeSize)
                    return false;
            }
            else
            {
                if (this.ShoeSize < 0 || this.ShoeSize > maxShoeSize || ((int)(((decimal)this.ShoeSize % 1) * 10) != 5 && (int)(((decimal)this.ShoeSize % 1) * 10) != 0))
                    return false;
            }
            return true;

        }
    }

    
}
