using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyHeist.Models
{
	public class Enums
	{
		public enum EnMemberSex : byte
		{
			F = 0,
			M = 1,
		}

		public enum EnMemberStatus : byte
		{
			AVAILABLE = 0,
			EXPIRED = 1,
			INCARCERATED = 2,
			RETIRED = 3,
		}

		public enum EnHeistStatus : byte
		{
			PLANNING = 0,
			CONFIRMED = 1,
			READY = 2,
			IN_PROGRESS = 3,
			FINISHED = 4,
		}

		public enum EnHeistOutcome : byte
		{
			FAILED = 0,
			SUCCEEDED = 1,
		}

	}
}
