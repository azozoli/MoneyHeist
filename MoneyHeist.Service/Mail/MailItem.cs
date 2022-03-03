using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyHeist.Models.Dtos;
using MoneyHeist.Service.TaskScheduler;

namespace MoneyHeist.Service.Mail
{
	public class MailItem : TaskItemBase
	{
		public MailItem(MailSenderItemType itemType, string heistName, DateTime heistStartTime, DateTime heistEndTime, string to)
		{
			ItemType = itemType;
			HeistName = heistName;
			HeistStartTime = heistStartTime;
			HeistEndTime = heistEndTime;
			To = to;
		}

		public override string Name => $"Mail item for sending From: {From}, To: {To}, Subject: {Subject}";
		public string To { get; set; }
		public string Body { get; set; }
		public string From { get; set; }
		public string Subject { get; set; }
		public MailSenderItemType ItemType { get; private set; }
		public string HeistName { get; set; }
		public DateTime HeistStartTime { get; set; }
		public DateTime HeistEndTime { get; set; }
		public MemberDto[] Members { get; set; }

		public IMailSender MailSender => SP.GetService<IMailSender>();

		public override async Task<bool> ExecuteAsync()
		{
			try
			{
				await MailSender.SendMail( this );
				return true;
			}
			catch ( Exception )
			{
				return false;
			}
		}
	}

	public enum MailSenderItemType : byte
	{
		NewMemberAdded = 0,
		ConfirmedToParticipate = 1,
		HaistHasStarted = 2,
		HaistHasFinished = 3,
	}
}
