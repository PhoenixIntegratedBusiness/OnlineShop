using Application.Generator;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Domain.ViewModel.AccountViewModel;
using Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender emailSender;
        private readonly IPasswordHasher<Users> _passwordHasher;
        public UserService(IUserRepository userRepository, IEmailSender emailSender, IPasswordHasher<Users> passwordHasher)
        {
            _userRepository = userRepository;
            this.emailSender = emailSender;
            _passwordHasher = passwordHasher;
        }

        public async Task<ChangePassResult> ChangePasswordAsync(ChangePasswordViewModel model, int UserId)
        {
            var user = await _userRepository.GetUserByIdAsync(UserId);
            if (user == null)
            {
                return ChangePassResult.Unauthorized;
            }
            var passverification = _passwordHasher.VerifyHashedPassword(user, user.Password, model.CurrentPassword);
            switch (passverification)
            {
                case PasswordVerificationResult.Failed:
                    return ChangePassResult.WrongCurrentPass;
                case PasswordVerificationResult.Success:
                    if (model.NewPassword == model.ReNewPassword)
                    {
                        user.Password = _passwordHasher.HashPassword(user, model.NewPassword);
                        _userRepository.UserUpdate(user);
                        await _userRepository.SaveChangeAsync();
                        return ChangePassResult.Success;
                    }
                    else
                    {
                        return ChangePassResult.NewPassNotMaching;
                    }

                case PasswordVerificationResult.SuccessRehashNeeded:
                    _passwordHasher.HashPassword(user, model.NewPassword);
                    _userRepository.UserUpdate(user);
                    await _userRepository.SaveChangeAsync();
                    return ChangePassResult.Success;
            }

            return ChangePassResult.Failure;
        }

        #region CheckActiveCodeAsync
        public async Task<ResetPasswordResult> CheckActiveCodeAsync(ResetPasswordViewModel model)
        {


            var resuser = await _userRepository.CheckActiveCodeAsync(model.Email.ToLowerInvariant().Trim(), model.ActiveCode);
            if (resuser != null)
            {
                resuser.ActiveCode = UniqCodeGenerator.GeneratUniqCode();

                resuser.Password = _passwordHasher.HashPassword(resuser, model.Password);
                _userRepository.UserUpdate(resuser);
                await _userRepository.SaveChangeAsync();

                return ResetPasswordResult.Success;

            }
            return ResetPasswordResult.Failure;
        }
        #endregion


        #region ForgetPasswordAsync
        public async Task<ForgetPassResult> ForgetPasswordAsync(ForgetPasswordViewModel model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email.ToLowerInvariant().Trim());
            if (user == null || user.isDelete)
            {
                return ForgetPassResult.UserNotFound;
            }
            else
            {
                user.ActiveCode = UniqCodeGenerator.GeneratUniqCode();
                _userRepository.UserUpdate(user);
                await _userRepository.SaveChangeAsync();
                //string body1 = $"Dear {user.Username}, your verification code is: {user.ActiveCode}";
                string body = $@"
                                  <h2>Password Reset</h2>
                                  <p>Dear <strong>{user.Username}</strong>,</p>
                                  <p>Your verification code is:</p>
                                  <h1>{user.ActiveCode}</h1>
                                  <p>If you didn't request a password reset, please ignore this email.</p>";

                emailSender.SendEmail(user.Email, "verificationCode", body);
                return ForgetPassResult.Success;
            }
        }
        #endregion

        #region GetUserByEmailAsync
        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email.ToLower().Trim());
        }
        #endregion

        #region GetUsersAsync  
        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var list = await _userRepository.GetUsersAsync();

            var users = list.Select(u => new UserViewModel
            {
                UserId = u.UserId,
                Username = u.Username,
                IsAdmin = u.IsAdmin,
                Email = u.Email,
                Mobile = u.Mobile,
                CreateDate = u.CreateDate,
                isDelete = u.isDelete

            }).ToList();

            return users;
        }
        #endregion

        #region LoginUserAsync 
        public async Task<LoginResult> LoginUserAsync(LoginViewModel model)
        {

            var existUser = await _userRepository.GetUserByEmailAsync(model.Email.Trim().ToLowerInvariant());
            if (existUser == null || existUser.isDelete)
            {
                return LoginResult.UserNotFound;
            }
            var result = _passwordHasher.VerifyHashedPassword(existUser, existUser.Password, model.Password);

            switch (result)
            {
                case PasswordVerificationResult.Success:
                    return LoginResult.Success;

                case PasswordVerificationResult.SuccessRehashNeeded:
                    existUser.Password = _passwordHasher.HashPassword(existUser, model.Password);
                    await _userRepository.SaveChangeAsync();
                    return LoginResult.Success;

                case PasswordVerificationResult.Failed:
                    return LoginResult.Failure;

                default:
                    return LoginResult.Failure;
            }
        }
        #endregion

        #region RegisterUserAsync
        public async Task<ResultRegister> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                var existUser = await _userRepository.GetUserByEmailAsync(model.Email.Trim().ToLower());

                if (existUser != null)
                {
                    return ResultRegister.EmailExists;
                }

                var user = new Users
                {
                    Email = model.Email.Trim().ToLowerInvariant(),
                    Username = model.UserName,
                    Mobile = model.Mobile,
                    CreateDate = DateTime.Now,
                    isDelete = false,
                    IsAdmin = false
                };


                user.Password = _passwordHasher.HashPassword(user, model.Password);

                await _userRepository.AddUserAsync(user);
                await _userRepository.SaveChangeAsync();

                return ResultRegister.Success;
            }
            catch (Exception)
            {
                return ResultRegister.Failed;
            }
        }
        #endregion
    }
}
