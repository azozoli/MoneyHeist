using MoneyHeist.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoneyHeist.Models.Interfaces.IRepositories
{
	public interface IHeistRepository
	{
		Task<IEnumerable<Heist>> GetHeistsAsync();

		Task<Heist> GetHeistByIdAsync(int heistId);

		Task<int> AddHeistAsync(Heist heist);

		Task<int> UpdateHeistAsync(Heist heist);

		Task<bool> HeistAlreadyExistsAsync(string name);
	}
}
