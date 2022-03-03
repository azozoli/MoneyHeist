using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MoneyHeist.Service.Mail
{
	public class MailSender : IMailSender
	{
		private ILogger _logger;
		private SmtpClient _smtpClient = new SmtpClient();

		private string _smtpServer;
		private int _port;
		private bool _useTLS;
		private string _userName;
		private string _password;
		private string _sender;

		public MailSender(IConfiguration configuration)
		{
			_smtpServer = configuration.GetValue<string>( "SMTPCredentials:SeverName" );
			_userName = configuration.GetValue<string>( "SMTPCredentials:SMTPUsername" );
			_password = configuration.GetValue<string>( "SMTPCredentials:SMTPPassword" );
			_port = configuration.GetValue<int>( "SMTPCredentials:Port" );
			_useTLS = configuration.GetValue<bool>( "SMTPCredentials:TLS" );
			_sender = configuration.GetValue<string>( "SMTPCredentials:From" );
		}

		public async Task SendMail(MailItem msg)
		{
			MailMessage mm = CreateMsg( msg );

			try
			{
				if ( !_smtpClient.IsConnected )
					await _smtpClient.ConnectAsync( _smtpServer, _port, _useTLS ? SecureSocketOptions.StartTls : SecureSocketOptions.None );
			}
			catch ( SmtpCommandException ex )
			{
				_logger.LogError( $"Error trying to connect {ex.Message}.\tStatusCode: {ex.StatusCode}", ex );
				throw ex;
			}
			catch ( SmtpProtocolException ex )
			{
				_logger.LogError( $"Protocol error while trying to connect: {ex.Message}.", ex );
				throw ex;
			}

			if ( _smtpClient.Capabilities.HasFlag( SmtpCapabilities.Authentication ) )
			{
				try
				{
					if ( !string.IsNullOrEmpty( _userName ) && !string.IsNullOrEmpty( _password ) )
					{
						var credentials = new NetworkCredential( _userName, _password );

						this._smtpClient.Authenticate( credentials );
					}
				}
				catch ( AuthenticationException ex )
				{
					_logger.LogError( "Invalid user name or password.", ex );
					throw ex;
				}
				catch ( SmtpCommandException ex )
				{
					_logger.LogError( $"Error trying to authenticate.\tStatusCode: {ex.StatusCode}", ex );
					throw ex;
				}
				catch ( SmtpProtocolException ex )
				{
					_logger.LogError( $"Protocol error while trying to authenticate: {ex.Message}.", ex );
					throw ex;
				}
			}

			try
			{
				await _smtpClient.SendAsync( mm );
			}
			catch ( SmtpCommandException ex )
			{
				_logger.LogError( $"Error sending message: {ex.Message}\tStatusCode: {ex.StatusCode}", ex );

				switch ( ex.ErrorCode )
				{
					case SmtpErrorCode.RecipientNotAccepted:
						_logger.LogError( $"\tRecipient not accepted: {ex.Mailbox}" );
						break;
					case SmtpErrorCode.SenderNotAccepted:
						_logger.LogError( $"\tSender not accepted: {ex.Mailbox}" );
						break;
					case SmtpErrorCode.MessageNotAccepted:
						_logger.LogError( $"\tMessage not accepted." );
						break;
				}

				throw ex;
			}
			catch ( SmtpProtocolException ex )
			{
				_logger.LogError( $"Protocol error while sending message: {ex.Message }", ex );
				throw ex;
			}

			_smtpClient.Disconnect( true );
		}

		private MailMessage CreateMsg(MailItem msg)
		{
			MailMessage mm = new MailMessage();
			mm.From = new MailAddress( msg.From );
			mm.Subject = GetSubject( msg );
			mm.Body = GetBody( msg );
			return mm;
		}

		private string GetSubject(MailItem msg)
		{
			switch ( msg.ItemType )
			{
				case MailSenderItemType.NewMemberAdded:
					return string.Format( "You have been added as member" );
				case MailSenderItemType.ConfirmedToParticipate:
					return string.Format( "You have been confirmed to participate in a heist {0} ", msg.HeistName );
				case MailSenderItemType.HaistHasStarted:
				case MailSenderItemType.HaistHasFinished:
					return string.Format( "Heist {0} has {1} ", msg.HeistName, msg.ItemType == MailSenderItemType.HaistHasStarted ? "started" : "finished" );
				default:
					return "New subject";
			}
		}

		private string GetBody(MailItem msg)
		{
			switch ( msg.ItemType )
			{
				case MailSenderItemType.NewMemberAdded:
					return string.Format( "You have been added as member in a heist {0} which will start on {1} and end on {2}", msg.HeistName, msg.HeistStartTime, msg.HeistEndTime );
				case MailSenderItemType.ConfirmedToParticipate:
					return string.Format( "Yo have been confirmed to participate in a heist {0} which will start on {1} and end on {2}", msg.HeistName, msg.HeistStartTime, msg.HeistEndTime );
				case MailSenderItemType.HaistHasStarted:
				case MailSenderItemType.HaistHasFinished:
					return string.Format( "Heist {0} has {1}", msg.HeistName, msg.ItemType == MailSenderItemType.HaistHasStarted ? "started." : "finished. Thank you for participating!" );
				default:
					return "";
			}
		}

	}
}
