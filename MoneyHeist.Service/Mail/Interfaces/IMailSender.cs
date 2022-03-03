using System.Threading.Tasks;

namespace MoneyHeist.Service.Mail
{
	public interface IMailSender
	{
		Task SendMail(MailItem mailItem);
	}
}
