using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolProject.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace SchoolProject.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the Teachers table of our school database.
        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <param name="SearchKey"> Search key (optional) of tearcher
        /// name</param>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>
        /// A list of teachers object (including 
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
          
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();
            
            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teacher Names
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Teacher NewTeacher = new Teacher();
                //NewTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                //NewTeacher.TeacherFname = ResultSet["teacherfname"].ToString();
                //NewTeacher.TeacherLname = ResultSet["teacherlname"].ToString();
                //string TeacherName = ResultSet["teacherfname"] + " "+ ResultSet["teacherlname"];
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string Teachelname = (string)ResultSet["teacherlname"];
                string TeacherNumber = (string)ResultSet["employeenumber"];
               // decimal TeacherSalary = (decimal)ResultSet["salary"];


                Teacher newTeacher = new Teacher();
                newTeacher.TeacherId = TeacherId;
                newTeacher.TeacherFname=TeacherFname;
                newTeacher.TeacherLname = Teachelname;
                newTeacher.TeacherNumber = TeacherNumber;
                //newTeacher.TeacherSalary = TeacherSalary.ToString();

                //Add the Teacher Name to the List
                //Teachers.Add(NewTeacher);
                Teachers.Add(newTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teacher names
            return Teachers;
        }
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher newTeacher = new Teacher();

            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where teacherid = "+id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string Teachelname = (string)ResultSet["teacherlname"];
                string TeacherNumber = (string)ResultSet["employeenumber"];
                //decimal TeacherSalary = (decimal)ResultSet["salary"];



                newTeacher.TeacherId = TeacherId;
                newTeacher.TeacherFname = TeacherFname;
                newTeacher.TeacherLname = Teachelname;
                newTeacher.TeacherNumber = TeacherNumber;
                //newTeacher.TeacherSalary = TeacherSalary.ToString();
            }

                return newTeacher;
        }
      /// <summary>
      /// Add new teacher 
      /// </summary>
      /// <paramref name="NewTeacher"></paramref>
        public void AddTeacher(Teacher NewTeacher)
        {
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "insert into teachers(teacherfname, teacherlname, employeenumber) values (@teacherfname,@teacherlname,@employeenumber)";
            cmd.CommandText = query;
           
                cmd.Parameters.AddWithValue("@teacherfname", NewTeacher.TeacherFname);
            
            cmd.Parameters.AddWithValue("@teacherlname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@employeenumber", NewTeacher.TeacherNumber);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }
        /// <summary>
        /// this function will delete the teacher from the database
        /// </summary>
        /// <param name="TeacherId">the primary key teacherid</param>
        
        public void DeleteTeacher(int TeacherId)
        {

            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id",TeacherId);
            cmd.CommandText= query;
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

        /// <summary>
        /// It will update the Teacher info if needed
        /// </summary>
        /// <param name="TeacherId">Teacher primary key so that we know that this is the teacher that need to be updated</param>
        /// <param name="TeacherInfo">teacher firstName lastname or Number</param>

        public void UpdateTeacher(int TeacherId, Teacher TeacherInfo)
        {
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //string query = "insert into teachers(teacherfname, teacherlname, employeenumber) values (@teacherfname,@teacherlname,@employeenumber)";
            cmd.CommandText = "update teachers set teacherfname=@teacherfname,teacherlname =@teacherlname, employeenumber =@employeenumber WHERE teacherid =@teacherid";
            cmd.Parameters.AddWithValue("@teacherfname", TeacherInfo.TeacherFname);

            cmd.Parameters.AddWithValue("@teacherlname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@employeenumber", TeacherInfo.TeacherNumber);

            cmd.Parameters.AddWithValue("@teacherid", TeacherId);
            

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

    }
}
