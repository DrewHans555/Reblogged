using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Core
{
    public class BlogUserFileAdapter : IBlogUserDataAccessAdapter
    {
        private readonly IConfiguration _configuration;
        private readonly IFileDataAccess<BlogUser> _fileDataAccess;

        public BlogUserFileAdapter(IConfiguration configuration, IFileDataAccess<BlogUser> fileDataAccess)
        {
            _configuration = configuration;
            _fileDataAccess = fileDataAccess;
        }

        public void Add(BlogUser entity)
        {
            var filePath = _configuration[KeyChain.FileDataAccess_BlogUser_DatabasePath];
            _fileDataAccess.WriteToDatabase(filePath, entity);
        }

        public void Delete(BlogUser entity)
        {
            var listFromDatabase = List();
            var newList = GetNewListWithoutBlogUser(listFromDatabase, entity);
            var filePath = _configuration[KeyChain.FileDataAccess_BlogUser_DatabasePath];
            _fileDataAccess.OverwriteDatabase(filePath, newList);
        }

        public void Edit(BlogUser entity)
        {
            var listFromDatabase = List();
            var newList = GetNewListWithoutBlogUser(listFromDatabase, entity);
            newList.Add(entity);
            var filePath = _configuration[KeyChain.FileDataAccess_BlogUser_DatabasePath];
            _fileDataAccess.OverwriteDatabase(filePath, newList);
        }

        public BlogUser GetById(Guid id)
        {
            return List()
                .FirstOrDefault(blogUser =>
                    blogUser.UserId.Equals(id));
        }
        public List<BlogUser> List()
        {
            var filePath = _configuration[KeyChain.FileDataAccess_BlogUser_DatabasePath];
            return _fileDataAccess.ReadDatabase(filePath);
        }

        private List<BlogUser> GetNewListWithoutBlogUser(List<BlogUser> listOfBlogUser, BlogUser entity)
        {
            return listOfBlogUser
                .Where(blogUser =>
                    (blogUser.UserId.Equals(entity.UserId)) == false)
                .ToList();
        }
    }
}