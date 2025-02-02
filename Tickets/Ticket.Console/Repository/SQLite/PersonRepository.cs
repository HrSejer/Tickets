using TicketLib.Repository;
using TicketLib.Models;
using System.Data.SQLite;
using System.Data.Common;

namespace Ticket.Console.Repository.SQLite;

public class PersonRepository : IRepository<Person>
{
    IConnectionHelper<SQLiteConnection> _connectionHelper;
    const string TABLE = "person";
    public PersonRepository(IConnectionHelper<SQLiteConnection> connectionHelper) 
    {
        _connectionHelper = connectionHelper;
    }

    public IEnumerable<Person> GetAll() 
    {
        using SQLiteConnection conn = _connectionHelper.GetConnection();
        SQLiteCommand command = conn.CreateCommand();
        command.CommandText = $"SELECT personId, firstName, middleName,lastName, registeredDate, addressId, preferredContactMethodId FROM {TABLE}";
        
        List<Person> result = new();
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read()) {
            Person p = new() {
                PersonId = reader.GetInt32(0),
                Firstname = reader.GetString(1),
                Middlename = reader.GetString(2),
                Lastname = reader.GetString(3),
                RegisterdDate = reader.GetDateTime(4),
                Address = reader.GetInt32(5),
                PreferredContactMethod = reader.GetInt32(6),
            };
            result.Add(p);
        }
        return result;
    }
    public Person? GetById(int id) 
    {
        using SQLiteConnection conn = _connectionHelper.GetConnection();
        SQLiteCommand command = conn.CreateCommand();
        command.CommandText = $"SELECT personId, firstName,middleName,lastName, registeredDate, addressId, preferredContactMethodId FROM {TABLE} WHERE personId=@id";
        command.Parameters.AddWithValue("@id", id);
        SQLiteDataReader reader = command.ExecuteReader();
        if (reader.Read()) {
            Person p = new() {
                PersonId = reader.GetInt32(0),
                Firstname = reader.GetString(1),
                Middlename = reader.GetString(2),
                Lastname = reader.GetString(3),
                RegisterdDate = reader.GetDateTime(4),
                Address = reader.GetInt32(5),
                PreferredContactMethod = reader.GetInt32(6),
            };
            return p;
        }
        
        return null;
    }
    public void Add(Person model)
    {
        using SQLiteConnection conn = _connectionHelper.GetConnection();
        SQLiteCommand command = conn.CreateCommand();
        command.CommandText = $"INSERT INTO {TABLE} (firstName,middleName,lastName, registeredDate, addressId, preferredContactMethodId) VALUES (@firstName, @middleName, @lastName, @registeredDate, @addressId, @preferredContactMethodId)";

        command.Parameters.AddWithValue("@firstName", model.Firstname);
        command.Parameters.AddWithValue("@middleName", model.Middlename);
        command.Parameters.AddWithValue("@lastName", model.Lastname);
        command.Parameters.AddWithValue("@registeredDate", model.RegisterdDate);
        command.Parameters.AddWithValue("@addressId", model.Address);
        command.Parameters.AddWithValue("@preferredContactMethodId", model.PreferredContactMethod);
        command.ExecuteNonQuery();

    }
    public void Update(Person model) 
    {
        using SQLiteConnection conn = _connectionHelper.GetConnection();
        SQLiteCommand command = conn.CreateCommand();
        command.CommandText = @$"UPDATE {TABLE} SET 
                    firstName = @firstName,
                    middleName = @middleName, 
                    lastName = @lastName, 
                    registeredDate = @registeredDate,
                    addressId = @addressId,
                    preferredContactMethodId = @preferredContactMethodId
                WHERE personId=@id";
        command.Parameters.AddWithValue("@firstName", model.Firstname);
        command.Parameters.AddWithValue("@middleName", model.Middlename);
        command.Parameters.AddWithValue("@lastName", model.Lastname);
        command.Parameters.AddWithValue("@registeredDate", model.RegisterdDate);
        command.Parameters.AddWithValue("@addressId", model.Address);
        command.Parameters.AddWithValue("@preferredContactMethodId", model.PreferredContactMethod);
        command.Parameters.AddWithValue("@id", model.PersonId);
        command.ExecuteNonQuery();
    }
    public void Delete(int id) 
    {
        using SQLiteConnection conn = _connectionHelper.GetConnection();
        SQLiteCommand command = conn.CreateCommand();
        command.CommandText = $"DELETE FROM {TABLE} WHERE personId=@id";
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }
}