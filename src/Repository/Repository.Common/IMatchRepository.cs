using App.Models;

namespace App.Repository;

public interface IMatchRepository : IRepository<Match>
{
	IReadOnlyList<(Guid, string)> ReadAllNames();
}