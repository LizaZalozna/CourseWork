using System;
using System.Collections.Generic;

namespace CourseWork
{
	public class Settings
	{
		int reservedReputation;
		int reservedTime;
        int maxReserved;
		int returnReputation;
		int returnTime;
        int maxLended;
        
        public int reservedReputation_
		{ 
            get { return reservedReputation; }
        }

        public int reservedTime_
        {
            get { return reservedTime; }

        }

        public int maxReserved_
        {
            get { return maxReserved; }
        }

        public int returnReputation_
        {
            get { return returnReputation; }

        }

        public int returnTime_
        {
            get { return returnTime; }

        }

        public int maxLended_
        {
            get { return maxLended; }
        }

        public Settings(int reservedReputation, int reservedTime, int maxReserved, int returnReputation, int returnTime, int maxLended)
        {
            this.reservedReputation = reservedReputation;
            this.reservedTime = reservedTime;
            this.maxReserved = maxReserved;
            this.returnReputation = returnReputation;
            this.returnTime = returnTime;
            this.maxLended = maxLended;
        }

        public SettingsDTO ToDTO()
        {
            return new SettingsDTO()
            {
                ReservedReputation = reservedReputation,
                ReservedTime = reservedTime,
                MaxReserved = maxReserved,
                ReturnReputation = returnReputation,
                ReturnTime = returnTime,
                MaxLended = maxLended
            };
        }

        public Settings(SettingsDTO dto)
        {
            this.reservedReputation = dto.ReservedReputation;
            this.reservedTime = dto.ReservedTime;
            this.maxReserved = dto.MaxReserved;
            this.returnReputation = dto.ReturnReputation;
            this.returnTime = dto.ReturnTime;
            this.maxLended = dto.MaxLended;
        }
    }
}