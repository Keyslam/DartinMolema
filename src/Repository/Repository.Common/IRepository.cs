namespace App.Repository;

public interface IRepository<T>
{
	void Save(T t);
	T? Read(Guid id);
	IReadOnlyList<T> ReadAll();
}