namespace App.Models;

public class Reference<TObject> : IDisposable
{
	public delegate void DestroyedHandle(Reference<TObject> reference);
	public event DestroyedHandle? Destroyed;

	public Guid Id { get; private set; }

	public Reference(Guid id)
	{
		this.Id = id;
	}

	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	~Reference() => this.Dispose(false);

	public void Dispose(bool disposing)
	{
		this.Destroyed?.Invoke(this);
	}
}