using System;
using System.Collections.Generic;
using System.Text;
using static MoneyHeist.Models.Enums;

namespace MoneyHeist.Models.Dtos
{
	public class MemberDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public EnMemberSex Sex { get; set; }
		public string Email { get; set; }
		public SkillsDto[] Skills { get; set; }
		public SkillsDto MainSkill { get; set; }
		public EnMemberStatus Status { get; set; }
	}
}
