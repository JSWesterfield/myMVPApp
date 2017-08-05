using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WikiDataProvider.Data.Extensions;
using WikiDataProvider.Data.Interfaces;
using WikiWebStarter.Example.Models;

namespace WikiWebStarter.Example.Services
{
    public class WishesService
    {
        //SELECT ALL        
        public List<Wish> SelectAll()
        {
            List<Wish> wishes = new List<Wish>();
            DataProvider.ExecuteCmd(
                GetConnection,
                "WanderLustFeature_SelectAll",
                inputParamMapper: null,
                map: delegate (IDataReader reader, short set)
                {
                    Wish w = MapWish(reader, 0);
                    wishes.Add(w);
                });
            return wishes;
        }
   

        //INSERT
        public int Insert(Wish p)
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
        public void Update(Wish w)
        {
            DataProvider.ExecuteNonQuery(
                GetConnection,
                "WanderLustFeature_Update",
                inputParamMapper: delegate (SqlParameterCollection paramCollection) {
                    paramCollection.AddWithValue("@Id", w.Id);
                    paramCollection.AddWithValue("@UserId", w.UserId);
                    paramCollection.AddWithValue("@Location", w.Location);
                    paramCollection.AddWithValue("@Activity", w.Activity);
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
        public Wish GetById(int id)
        {
            Wish newWish = null;

            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@Id", id);
            };

            Action<IDataReader, short> resultMapper = delegate (IDataReader reader, short set)
            {
                newWish = MapWish(reader, 0); // pass in a startingIndex of 0
            };

            DataProvider.ExecuteCmd(
                GetConnection,
                "dbo.WanderLustFeature_SelectById",
                inputMapper,
                resultMapper
            );
            return newWish;
        }


        private Wish MapWish(IDataReader reader, int startingIndex)
        {
            Wish w = new Wish();
            //int startingIndex = 0;
            w.Id = reader.GetInt32(startingIndex++);
            w.UserId = reader.GetSafeString(startingIndex++);
            w.Location = reader.GetSafeString(startingIndex++);
            w.Activity = reader.GetSafeString(startingIndex++);

            //string s = reader.GetSafeString(startingIndex++);
            //if (!string.IsNullOrWhiteSpace(s))
            //    w.UserId = s[0];

            return w;
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