using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WikiDataProvider.Data.Extensions;
using WikiDataProvider.Data.Interfaces;
using WikiWebStarter.Example.Models;

namespace WikiWebStarter.Example.Services
{
    public class PeopleService
    {
        //SELECT ALL        
        public List<Person> SelectAll()
        {
            List<Person> people = new List<Person>();
            DataProvider.ExecuteCmd(
                GetConnection,
                "WanderLustFeature_SelectAll",
                inputParamMapper: null,
                map: delegate (IDataReader reader, short set)
                {
                    Person p = MapPerson(reader, 0);
                    people.Add(p);
                });
            return people;
        }
   

        //INSERT
        public int Insert(Person p)
        {
            int i = 0;
            DataProvider.ExecuteNonQuery(
                GetConnection,
                "WanderLustFeature_Insert",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", p.UserId);
                    paramCollection.AddWithValue("@Location", p.Location);
                    paramCollection.AddWithValue("@Activity", p.Activity);
                    paramCollection.AddWithValue("@ImageUrl", p.ImageUrl);

                    SqlParameter parm = new SqlParameter("@Id", SqlDbType.Int);
                    parm.Direction = ParameterDirection.Output;
                    paramCollection.Add(parm);
                },
                returnParameters: delegate (SqlParameterCollection paramCollection)
                {
                    int.TryParse(paramCollection["@Id"].Value.ToString(), out i);
                });
            return i;
        }


        //UPDATE
        public void Update(Person p)
        {
            DataProvider.ExecuteNonQuery(
                GetConnection,
                "WanderLustFeature_Update",
                inputParamMapper: delegate (SqlParameterCollection paramCollection) {
                    paramCollection.AddWithValue("@Id", p.Id);
                    paramCollection.AddWithValue("@UserId", p.UserId);
                    paramCollection.AddWithValue("@Location", p.Location);
                    paramCollection.AddWithValue("@Activity", p.Activity);
                    paramCollection.AddWithValue("@ImageUrl", p.Location);
                },
                returnParameters: null);
        }


        //DELETE BY ID
        public void Delete(int id)
        {
            DataProvider.ExecuteNonQuery(
                GetConnection,
                "WanderLustFeature_DeleteById",
                inputParamMapper: delegate (SqlParameterCollection paramCollection) {
                    paramCollection.AddWithValue("@Id", id);
                },
                returnParameters: null);
        }

        //GET BY ID
        public Person GetById(int id)
        {
            Person newPerson = null;

            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@Id", id);
            };

            Action<IDataReader, short> resultMapper = delegate (IDataReader reader, short set)
            {
                newPerson = MapPerson(reader, 0); // pass in a startingIndex of 0
            };

            DataProvider.ExecuteCmd(
                GetConnection,
                "dbo.WanderLustFeature_SelectById",
                inputMapper,
                resultMapper
            );
            return newPerson;
        }


        private Person MapPerson(IDataReader reader, int startingIndex)
        {
            Person p = new Person();
            //int startingIndex = 0;
            p.Id = reader.GetInt32(startingIndex++);
            p.UserId = reader.GetSafeString(startingIndex++);
            p.Location = reader.GetSafeString(startingIndex++);
            p.Activity = reader.GetSafeString(startingIndex++);
            p.ImageUrl = reader.GetSafeString(startingIndex++);

            //string s = reader.GetSafeString(startingIndex++);
            //if (!string.IsNullOrWhiteSpace(s))
            //    p.UserId = s[0];

            return p;
        }

        // Alternatively, create a BaseService class
        // add this method to the base class
        protected static IDao DataProvider
        {
            get { return WikiDataProvider.Data.DataProvider.Instance; }
        }

        // Alternatively, create a BaseService class
        // add this method to the base class
        protected static SqlConnection GetConnection()
        {
            return new System.Data.SqlClient.SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
    }
}