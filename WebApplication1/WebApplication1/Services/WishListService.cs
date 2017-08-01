using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication1.Models.Requests;

namespace WebApplication1.Services
{
    public class WishListService : BaseService
    {
        public SupportRequestsService(IUsersService usersService, IMessagingService messagingService)
        {
            _messagingService = messagingService;
            _usersService = usersService;
        }

        //CREATE
        public int CreateSupportRequest(WishCreateRequest model)
        {

            int id = 0;

            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@location", model.Location);
                parameters.AddWithValue("@userId", User.Identity.GetId() ?? (object)DBNull.Value);
                parameters.AddWithValue("@activity", model.Activity);
                parameters.AddWithValue("@image", model.Image);

                SqlParameter idParam = new SqlParameter("@Id", id);
                idParam.Direction = ParameterDirection.Output;
                parameters.Add(idParam);
            };

            Action<SqlParameterCollection> returnMapper = delegate (SqlParameterCollection parameters)
            {
                id = (int)parameters["@Id"].Value;
            };

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.SupportRequests_Insert", inputMapper, returnMapper);

            return id;
        }

        //UPDATE

        public void UpdateSupportRequest(SupportRequestUpdateRequest model)
        {

            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@id", model.Id);
                parameters.AddWithValue("@subject", model.Subject);
                parameters.AddWithValue("@email", model.Email ?? (object)DBNull.Value);
                parameters.AddWithValue("@body", model.Body);
                parameters.AddWithValue("@userId", User.Identity.GetId() ?? (object)DBNull.Value);
            };

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.SupportRequests_Update", inputMapper);
        }

        //GET ALL

        public List<SupportRequest> GetAll()
        {

            List<SupportRequest> list = null;

            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                //NONE
            };

            Action<IDataReader, short> resultMapper = delegate (IDataReader reader, short set)
            {
                SupportRequest testSR = SupportRequestReader(reader);

                if (list == null)
                {
                    list = new List<SupportRequest>();
                }

                list.Add(testSR);
            };

            DataProvider.ExecuteCmd(GetConnection, "dbo.SupportRequests_SelectAll", inputMapper, resultMapper);

            return list;
        }

        //GET BY ID

        public SupportRequest GetById(int id)
        {

            SupportRequest testSR = null;

            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@id", id);
            };

            Action<IDataReader, short> resultMapper = delegate (IDataReader reader, short set)
            {
                testSR = SupportRequestReader(reader);
            };

            DataProvider.ExecuteCmd(GetConnection, "dbo.SupportRequests_SelectById", inputMapper, resultMapper);

            return testSR;
        }

        //SUPPORT REQUEST READER FOR GET CALLS

        private SupportRequest SupportRequestReader(IDataReader reader)
        {
            SupportRequest model = new SupportRequest();

            int index = 0;
            model.Id = reader.GetInt32(index++);
            model.Subject = reader.GetString(index++);
            model.Email = reader.GetSafeString(index++);
            model.Body = reader.GetString(index++);
            model.Response = reader.GetSafeString(index++);
            model.AdminUserId = reader.GetSafeInt32Nullable(index++);
            model.UserId = reader.GetSafeInt32Nullable(index++);
            model.DateCreated = reader.GetDateTime(index++);
            model.DateModified = reader.GetDateTime(index++);

            return model;
        }


        //UPDATE RESPONSE FROM ADMIN USER
        public void UpdateResponse(UpdateSupportRequestResponseRequest model)
        {
            Action<SqlParameterCollection> inputMapper = delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@id", model.Id);
                parameters.AddWithValue("@response", model.Response);
                parameters.AddWithValue("@adminUserId", User.Identity.GetId().Value);
            };

            DataProvider.ExecuteNonQuery(
                GetConnection,
                "dbo.SupportRequests_UpdateResponse",
                inputMapper
                );

            SupportRequest supportRequest = GetById(model.Id);

            String email = supportRequest.Email;

            if (supportRequest.Email == null)
            {

                User user = _usersService.GetById(supportRequest.UserId.Value);


                email = user.Email;
            }

            _messagingService.SendSupportRequestEmail(email, supportRequest.Subject, supportRequest.Response);

            //return;
            //do not need return if i'm getting nothing back and it's the last line.
        }
    }
}