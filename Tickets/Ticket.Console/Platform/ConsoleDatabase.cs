using System.Data.SQLite;
using Ticket.Console.Repository.Sqlite;
using Ticket.Console.Repository.SQLite;
using TicketLib;
using TicketLib.Models;
using TicketLib.Platform;
using TicketLib.Repository;

namespace Ticket.Console.Platform;

public class ConsoleDatabase : IDatabase
{
    IConnectionHelper<SQLiteConnection> _connectionHelper;
    public IRepository<Person> Persons { get; private set; }
    public IRepository<Incident> Incidents { get; private set; }
    public IRepository<IncidentLog> IncidentLogs { get; private set; }
    public IRepository<Staff> Staffs { get; private set; }
    public IRepository<ContactMethod> ContactMethods { get; private set; }

    public ConsoleDatabase(string connection_string)
    {
        _connectionHelper = new ConnectionHelper<SQLiteConnection>(connection_string);
        Persons = new PersonRepository(_connectionHelper);
        Incidents = new IncidentRepository(_connectionHelper);
        IncidentLogs = new IncidentLogsRepository(_connectionHelper);
        Staffs = new StaffRepository(_connectionHelper);
        ContactMethods = new ContactMethodRepository(_connectionHelper);
    }
}