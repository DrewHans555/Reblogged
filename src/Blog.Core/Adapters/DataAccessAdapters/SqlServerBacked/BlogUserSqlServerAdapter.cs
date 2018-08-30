using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Blog.Core
{
    public class BlogUserSqlServerAdapter : IBlogUserDataAccessAdapter
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlServerDataAccess _sqlServerDataAccess;
        private readonly ISqlParameterBuilder _sqlParameterBuilder;
        private readonly string _sqlConnectionStringConfigKey;

        public BlogUserSqlServerAdapter(IConfiguration configuration, ISqlServerDataAccess sqlServerDataAccess,
            ISqlParameterBuilder sqlParameterBuilder)
        {
            _configuration = configuration;
            _sqlServerDataAccess = sqlServerDataAccess;
            _sqlParameterBuilder = sqlParameterBuilder;
            _sqlConnectionStringConfigKey = _configuration["sqlServerDataAccess_connectionstring"];
        }

        public void Add(BlogUser entity)
        {
            var sqlConnectionString = _configuration[_sqlConnectionStringConfigKey];
            var storedProcedure = _configuration["sqlServerDataAccess_storedprocedure_bloguser_add"];
            var listOfSqlParameters = new List<SqlParameter>();
            listOfSqlParameters.Add(_sqlParameterBuilder.BuildSqlParameter<BlogUser>("Permissions", entity.Permissions));
            listOfSqlParameters.Add(_sqlParameterBuilder.BuildSqlParameter<BlogUser>("TimeRegistered", entity.TimeRegistered));
            listOfSqlParameters.Add(_sqlParameterBuilder.BuildSqlParameter<BlogUser>("UserId", entity.UserId));
            listOfSqlParameters.Add(_sqlParameterBuilder.BuildSqlParameter<BlogUser>("UserName", entity.UserName));
            var rowsAffected = _sqlServerDataAccess.ExecuteNonQueryStoredProcedure(sqlConnectionString, storedProcedure, listOfSqlParameters);
            if (rowsAffected < 1)
                throw new Exception("BlogUserSqlRepo failed to Add");
        }

        public void Delete(BlogUser entity)
        {
            var sqlConnectionString = _configuration[_sqlConnectionStringConfigKey];
            var storedProcedure = _configuration["sqlServerDataAccess_storedprocedure_bloguser_delete"];
            var listOfSqlParameters = new List<SqlParameter>();
            listOfSqlParameters.Add(_sqlParameterBuilder.BuildSqlParameter<BlogUser>("UserId", entity.UserId));
            var rowsAffected = _sqlServerDataAccess.ExecuteNonQueryStoredProcedure(sqlConnectionString, storedProcedure, listOfSqlParameters);
            if (rowsAffected < 1)
                throw new Exception("BlogUserSqlRepo failed to Delete");
        }

        public void Edit(BlogUser entity)
        {
            var sqlConnectionString = _configuration[_sqlConnectionStringConfigKey];
            var storedProcedure = _configuration["sqlServerDataAccess_storedprocedure_bloguser_edit"];
            var listOfSqlParameters = new List<SqlParameter>();
            listOfSqlParameters.Add(_sqlParameterBuilder.BuildSqlParameter<BlogUser>("Permissions", entity.Permissions));
            listOfSqlParameters.Add(_sqlParameterBuilder.BuildSqlParameter<BlogUser>("TimeRegistered", entity.TimeRegistered));
            listOfSqlParameters.Add(_sqlParameterBuilder.BuildSqlParameter<BlogUser>("UserId", entity.UserId));
            listOfSqlParameters.Add(_sqlParameterBuilder.BuildSqlParameter<BlogUser>("UserName", entity.UserName));
            var rowsAffected = _sqlServerDataAccess.ExecuteNonQueryStoredProcedure(sqlConnectionString, storedProcedure, listOfSqlParameters);
            if (rowsAffected < 1)
                throw new Exception("BlogUserSqlRepo failed to Edit");
        }

        public BlogUser GetById(Guid id)
        {
            var sqlConnectionString = _configuration[_sqlConnectionStringConfigKey];
            var storedProcedure = _configuration["sqlServerDataAccess_storedprocedure_bloguser_getbyid"];
            var listOfSqlParameters = new List<SqlParameter>();
            listOfSqlParameters.Add(_sqlParameterBuilder.BuildSqlParameter<BlogUser>("UserId", id));
            var listReturned = _sqlServerDataAccess.ExecuteReaderStoredProcedure<BlogUser>(sqlConnectionString, storedProcedure, listOfSqlParameters);
            if (listReturned.Count != 1)
                return null;
            return listReturned[0];
        }

        public List<BlogUser> List()
        {
            var sqlConnectionString = _configuration[_sqlConnectionStringConfigKey];
            var storedProcedure = _configuration["sqlServerDataAccess_storedprocedure_bloguser_list"];
            return _sqlServerDataAccess.ExecuteReaderStoredProcedure<BlogUser>(sqlConnectionString, storedProcedure);
        }
    }
}