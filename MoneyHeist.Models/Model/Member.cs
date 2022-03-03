using System;
using System.Collections.Generic;

#nullable disable

namespace MoneyHeist.Models.Model
{
	public partial class Member
	{
		public Member()
		{
			Skills = new HashSet<Skills>();
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Sex { get; set; }
		public string Email { get; set; }
		public Skills MainSkill { get; set; }
		public byte Status { get; set; }

		public virtual ICollection<Skills> Skills { get; set; }
	}
}
