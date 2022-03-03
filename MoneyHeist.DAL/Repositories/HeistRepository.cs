using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoneyHeist.Models;
using MoneyHeist.Models.Interfaces.IRepositories;
using MoneyHeist.Models.Model;
using MoneyHeist.Service.BackgroundTasks;
using MoneyHeist.Service.TaskScheduler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyHeist.DAL.Repositories
{
	public class HeistRepository : IHeistRepository
	{
		private readonly MoneyHeistContext _context;
		private readonly string _connectionString;
		private readonly ITaskCreatorService _taskCreator;
		private readonly IBackgroundWorker _backgroundWorker;


		public HeistRepository(IOptions<ConnectionSettingsMoneyHeist> connectionSettings, MoneyHeistContext context, ITaskCreatorService taskCreator, IBackgroundWorker backgroundWorker)
		{
			_connectionString = connectionSettings.Value.MoneyHeistConnectionSettings;
			_context = context;
			_taskCreator = taskCreator;
			_backgroundWorker = backgroundWorker;
		}

		public HeistRepository(MoneyHeistContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Heist>> GetHeistsAsync()
		{
			return await _context.Heists
				.Include( x => x.Skills )
				.Include( x => x.Members )
				.ToArrayAsync();
		}

		public async Task<Heist> GetHeistByIdAsync(int heistId)
		{
			return await _context.Heists.Where( x => x.Id == heistId ).FirstOrDefaultAsync();
		}

		public async Task<int> AddHeistAsync(Heist heist)
		{
			await _context.Heists.AddAsync( heist );
			await _context.SaveChangesAsync();

			TaskPutEventAutomatic task1 = _taskCreator.CreatePutEventTask( true, heist.Id, heist.StartTime );
			TaskPutEventAutomatic task2 = _taskCreator.CreatePutEventTask( false, heist.Id, heist.EndTime );
			_backgroundWorker.AddToQueue( task1 );
			_backgroundWorker.AddToQueue( task2 );
			return heist.Id;
		}

		public async Task<int> UpdateHeistAsync(Heist heist)
		{
			_context.Heists.Update( heist );
			await _context.SaveChangesAsync();
			return heist.Id;
		}

		public async Task<bool> HeistAlreadyExistsAsync(string name)
		{
			return await _context.Heists.AnyAsync( x => x.Name == name );
		}
	}
}
