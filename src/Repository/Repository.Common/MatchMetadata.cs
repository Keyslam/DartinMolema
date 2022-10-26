namespace App.Repository;

public class MatchMetadata
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public bool IsDone { get; set; }

	public MatchMetadata(Guid id, string name, bool isDone)
	{
		this.Id = id;
		this.Name = name;
		this.IsDone = isDone;
	}
}