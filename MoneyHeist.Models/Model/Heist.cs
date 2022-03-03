using System;
using System.Collections.Generic;

#nullable disable

namespace MoneyHeist.Models.Model
{
	public partial class Heist
	{
		public Heist()
		{
			Members = new HashSet<Member>();
			Skills = new HashSet<Skills>();
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public byte Status { get; set; }

		public virtual ICollection<Member> Members { get; set; }
		public virtual ICollection<Skills> Skills { get; set; }
	}
}
