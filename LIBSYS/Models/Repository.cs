using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace LIBSYS.Models
{
    public class Repository
    {
        private string ConnectionString { get; }
        private IDbConnection connection { get; }

        //Ändra connectionstringen så den blir hashad
        public Repository()
        {
            ConnectionString = "Data Source=syss3-grupp1.database.windows.net;Initial Catalog=libsys;User Id=Grupp1;Password=Hunter12;";
            connection = new SqlConnection(ConnectionString);
            connection.Open();
        }
        private IDbConnection Connection
        {
            get
            {
                IDbConnection con;
                con = new SqlConnection(ConnectionString);
                con.Open();
                return con;
            }
        }

        #region Members Stored Procedures
        public int CreateMember(string _email, string _nickName, string _pwd, string _role)
        {
            using (IDbConnection con = Connection)
            {
                var memberID = connection.Query<int>("create_member", new { email = _email, nickname = _nickName, pwd = _pwd, role = _role }, commandType: CommandType.StoredProcedure);
                return memberID.First();
            }
        }

        public async  Task<List<Member>> ReadMembers()
        {
            using (IDbConnection con = Connection)
            {
                List<Member> members = (await connection.QueryAsync<Member>("read_members", commandType: CommandType.StoredProcedure)).ToList();
                return members;
            }
        }

        #endregion

        #region Events Stored Procedures
        public int CreateEvent(string _eventType, DateTime _timeStart, DateTime _timeEnd, string _location, int _owner)
        {
            using (IDbConnection con = Connection)
            {
                var eventID = connection.Query<int>("create_event", new { event_type = _eventType, time_start = _timeStart, time_end = _timeEnd , location = _location, owner = _owner }, commandType: CommandType.StoredProcedure);
                return eventID.First();
            }
        }

        //public List<Event> ReadEvents



        #endregion

    }
}
