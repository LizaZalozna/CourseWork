using System;
using System.Collections.Generic;

namespace CourseWork
{
	public class Settings
	{
		int reservedReputation;
		int reservedTime;
		int returnReputation;
		int returnTime;

        public int reservedReputation_
		{ 
            get { return reservedReputation; }
        }

        public int reservedTime_
        {
            get { return reservedTime; }

        }

        public int returnReputation_
        {
            get { return returnReputation; }

        }

        public int returnTime_
        {
            get { return returnTime; }

        }

        public Settings(int reservedReputation, int reservedTime, int returnReputation, int returnTime)
        {
            this.reservedReputation = reservedReputation;
            this.reservedTime = reservedTime;
            this.returnReputation = returnReputation;
            this.returnTime = returnTime;
        }

        public SettingsDTO ToDTO()
        {
            return new SettingsDTO()
            {
                ReservedReputation = reservedReputation,
                ReservedTime = reservedTime,
                ReturnReputation = returnReputation,
                ReturnTime = returnTime
            };
        }

        public Settings(SettingsDTO dto)
        {
            this.reservedReputation = dto.ReservedReputation;
            this.reservedTime = dto.ReservedTime;
            this.returnReputation = dto.ReturnReputation;
            this.returnTime = dto.ReturnTime;
        }
    }
}

