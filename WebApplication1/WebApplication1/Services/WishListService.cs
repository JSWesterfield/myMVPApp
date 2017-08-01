using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Models.Requests;
using WikiDataProvider.Data.Extensions;

namespace WebApplication1.Services
{
    public class WishListService : BaseService
    {
        //CREATE
        public int Create(WishCreateRequest model)
        {

            int id = 0;

            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@location", model.Location);
                parameters.AddWithValue("@userId", User.Identity.GetId() ?? (object)DBNull.Value);
                parameters.AddWithValue("@activity", model.Activity);
                parameters.AddWithValue("@image", model.ImageUrl);

                SqlParameter idParam = new SqlParameter("@Id", id);
                idParam.Direction = ParameterDirection.Output;
                parameters.Add(idParam);
            };

            Action<SqlParameterCollection> returnMapper = delegate (SqlParameterCollection parameters)
            {
                id = (int)parameters["@Id"].Value;
            };

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.", inputMapper, returnMapper);

            return id;
        }


        //UPDATE
        public void Update(WishUpdateRequest model)
        {

            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@id", model.Id);
                parameters.AddWithValue("@location", model.Location);
                parameters.AddWithValue("@userId", model.UserId ?? (object)DBNull.Value);
                parameters.AddWithValue("@activity", model.Activity);
                parameters.AddWithValue("@imageUrl", model.ImageUrl);
            };

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.WanderLustFeature_Update", inputMapper);
        }


        //GET ALL
        public List<Wish> GetAll()
        {

            List<Wish> list = null;

            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                //NONE
            };

            Action<IDataReader, short> resultMapper = delegate (IDataReader reader, short set)
            {
                Wish testSR = WishReader(reader);

                if (list == null)
                {
                    list = new List<Wish>();
                }

                list.Add(testSR);
            };

            DataProvider.ExecuteCmd(GetConnection, "dbo.WanderLustFeature_SelectAll", inputMapper, resultMapper);

            return list;
        }


        //GET BY ID
        public Wish GetById(int id)
        {
            Wish modelWish = null;

            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@id", id);
            };

            Action<IDataReader, short> resultMapper = delegate (IDataReader reader, short set)
            {
                modelWish = WishReader(reader);
            };

            DataProvider.ExecuteCmd(GetConnection, "dbo.WanderLustFeature_SelectById", inputMapper, resultMapper);

            return modelWish;
        }

        //WISH READER FOR GET CALLS
        private Wish WishReader(IDataReader reader)
        {
            Wish model = new Wish();

            int index = 0;
            model.Id = reader.GetInt32(index++);
            model.Location = reader.GetSafeString(index++);
            model.UserId = reader.GetSafeString(index++);
            model.Activity = reader.GetSafeString(index++);
            model.ImageUrl = reader.GetSafeString(index++);
            model.DateCreated = reader.GetDateTime(index++);
            model.DateModified = reader.GetDateTime(index++);

            return model;
        }


        //DELETE
        public void Delete(int id)
        {
            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@id", id);
            };

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.WanderLustFeature_Delete", inputMapper);
        }

    }
}