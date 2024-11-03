using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using SQLite;
using Velura.Models;
using Velura.Services.Abstract;

namespace Velura.Services;

public sealed class Database
{
	readonly ILogger<Database> logger;
	readonly IPathResolver pathResolver;

	public Database(
		ILogger<Database> logger,
		IPathResolver pathResolver)
	{
		this.logger = logger;
		this.pathResolver = pathResolver;

		logger.LogInformation("[Database-.ctor] Database has been initialized.");
	}
	
	
	SQLiteAsyncConnection connection = default!;

	public async Task EnsureLoadedAsync()
	{
		if (connection is not null)
			return;
		
		logger.LogInformation("[Database-EnsureLoadedAsync] Loading database...");
		connection = new(pathResolver.Database, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
		
		await connection.CreateTableAsync<Movie>();
		await connection.CreateTableAsync<Show>();
		await connection.CreateTableAsync<Episode>();
	}


	public async Task<int> InsertAsync<T>(
		T item) where T : new()
	{
		await EnsureLoadedAsync();
		
		logger.LogInformation("[Database-InsertAsync] Inserting item...");
		return await connection.InsertAsync(item);
	}

	
	public async Task<T> GetAsync<T>(
		int id) where T : new()
	{
		await EnsureLoadedAsync();
		
		logger.LogInformation("[Database-GetAsync] Getting item with id...");
		return await connection.FindAsync<T>(id);
	}

	public async Task<T[]> GetAsync<T>(
		Expression<Func<T, bool>>? predicate = null) where T : new()
	{
		await EnsureLoadedAsync();
		
		logger.LogInformation("[Database-GetAsync] Getting items...");
		return predicate is null 
			? await connection.Table<T>().ToArrayAsync() 
			: await connection.Table<T>().Where(predicate).ToArrayAsync();
	}
	
	
	public async Task<int> DeleteAsync<T>(
		int id) where T : new()
	{
		await EnsureLoadedAsync();

		logger.LogInformation("[Database-DeleteAsync] Deleting item with id...");
		return await connection.DeleteAsync<T>(id);
	}
	
	public async Task<int> DeleteAsync<T>(
		Expression<Func<T, bool>>? predicate = null) where T : new()
	{
		await EnsureLoadedAsync();

		logger.LogInformation("[Database-DeleteAsync] Deleting items...");
		return predicate is null 
			? await connection.DeleteAllAsync<T>()
			: await connection.Table<T>().DeleteAsync(predicate);
	}
}