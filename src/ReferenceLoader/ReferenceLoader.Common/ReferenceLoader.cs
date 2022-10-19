using App.Models;
using App.Repository;

namespace App.ReferenceLoader;

public class ReferenceLoader<TObject, TRepository>
	where TObject : notnull
	where TRepository : IRepository<TObject>
{
	private Dictionary<Guid, TObject> Objects { get; }
	private Dictionary<Guid, HashSet<Reference<TObject>>> References { get; }

	private TRepository Repository { get; }

	public ReferenceLoader(TRepository repository)
	{
		this.Repository = repository;

		this.Objects = new Dictionary<Guid, TObject>();
		this.References = new Dictionary<Guid, HashSet<Reference<TObject>>>();
	}

	public TObject Resolve(Reference<TObject> reference)
	{
		// Fetch object from cache
		if (this.Objects.ContainsKey(reference.Id))
		{
			// Keep track of this reference object referring to this object
			if (!this.References.ContainsKey(reference.Id))
			{
				var references = this.References[reference.Id];
				references.Add(reference);

				reference.Destroyed += this.OnReferenceDestroyed;
			}

			return this.Objects[reference.Id];
		}

		// Fetch object from repository
		{
			var @object = this.Repository.Read(reference.Id)!;

			// Add object to cache
			this.Objects.Add(reference.Id, @object);

			// Keep track of this reference object referring to this object
			var references = new HashSet<Reference<TObject>>() {
				reference
			};
			this.References.Add(reference.Id, references);
			reference.Destroyed += this.OnReferenceDestroyed;

			return @object;
		}
	}

	private void Unload(Reference<TObject> reference)
	{
		// Remove tracking of this reference object referring to this object
		var references = this.References[reference.Id];
		references.Remove(reference);
		reference.Destroyed -= OnReferenceDestroyed;

		// If there's no more references to the object, remove it
		if (references.Count == 0)
		{
			this.References.Remove(reference.Id);
			this.Objects.Remove(reference.Id);
		}
	}

	private void OnReferenceDestroyed(Reference<TObject> reference)
	{
		this.Unload(reference);
	}
}