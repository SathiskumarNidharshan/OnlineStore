using OnlineStore.Database;
using OnlineStore.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace OnlineStore.Services
{
    public class AuthService
    {
        protected ApplicationDbContext _dbContext;

        public AuthService()
        {
            _dbContext = new ApplicationDbContext();
        }

        public List<User> getUsers()
        {
            return _dbContext.Users.ToList();
        }

        public List<User> getUserByRole(string role)
        {
            // Fetch users with the role "Admin" from the database
            List<User> users = _dbContext.Users.Where(i => i.Role == role).ToList();
            return users;
        }



        //Authenticate user login
        public async Task<ResultModel> Authenticate(LoginModel model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(i => i.Username == model.Username);

            if (user == null)
            {
                return new ResultModel
                {
                    IsSuccessful = false,
                    Message = "Your username or password is invalid",
                    Code = (int)HttpStatusCode.Unauthorized
                };

            }
            if (!HashManager.VerifyPassword(model.Password, user.Password))
            {
                return new ResultModel
                {
                    IsSuccessful = false,
                    Message = "Your username or password is invalid",
                    Code = (int)HttpStatusCode.Unauthorized

                };
            }
            return new ResultModel
            {
                IsSuccessful = true,
                Data = user,
                Message = "User logged in successfully",
                Code = (int)HttpStatusCode.OK

            };
        }

        public async Task<ResultModel> Register(RegisterModel model)
        {
            var isUseExist = _dbContext.Users.Any(i => i.Username == model.Username);

            if (!isUseExist)
            {
                Console.WriteLine("User model:",model.Role);
                var password = HashManager.HashPassword(model.Password);
                var user = _dbContext.Users.Add(new Entities.User
                {
                    Username = model.Username,
                    Password = password,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role,

                });
                await _dbContext.SaveChangesAsync();

                return new ResultModel
                {
                    IsSuccessful = true,
                    Data = user,
                    Message = "User Successfully registered",
                    Code = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ResultModel
                {
                    IsSuccessful = false,
                    Data = null,
                    Message = "Username is already exist",
                    Code = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}