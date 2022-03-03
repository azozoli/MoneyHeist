using System;
using static MoneyHeist.Models.Enums;

namespace MoneyHeist.Models.Dtos
{
	public class HeistDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public EnHeistStatus Status { get; set; }
		public SkillsDto[] Skills { get; set; }
		public MemberDto[] Members { get; set; }
		public EnHeistOutcome Outcome { get; set; }

	}
}
