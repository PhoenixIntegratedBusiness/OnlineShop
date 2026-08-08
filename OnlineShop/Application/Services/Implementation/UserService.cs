using Application.DTOs;
using Application.Enums.Account;
using Application.Generator;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Domain.ViewModel.AccountViewModel;
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

        #region UpdateUserRoleAsync
        public async Task<EditUserRoleResult> UpdateUserRoleAsync(EditUserRoleViewModel model)
        {
            var res = await _userRepository.GetUserByIdlAsync(model.UserId);

            if (res == null)
            {
                return EditUserRoleResult.Fauiler;
            }

            var userres = await _userRepository.IsUserExistanceAsync(
                model.Username,
                model.Email.Trim().ToLowerInvariant(),
                model.Mobile,
                model.UserId);

            if (userres != null)
            {
                if (userres.Username == model.Username)
                {
                    return EditUserRoleResult.DuplicateUsername;
                }
                else if (userres.Email == model.Email.Trim().ToLowerInvariant())
                {
                    return EditUserRoleResult.DuplicateEmail;
                }
                else if (userres.Mobile == model.Mobile)
                {
                    return EditUserRoleResult.DuplicateMobile;
                }
            }

            // Update the existing user
            res.Email = model.Email.Trim().ToLowerInvariant();
            res.Mobile = model.Mobile;
            res.Username = model.Username;
            res.IsAdmin = model.IsAdmin;
            res.isDelete = model.isDelete;

            // Only update the password if a new password was entered
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                res.Password = _passwordHasher.HashPassword(res, model.Password);
            }

            res.userInRoles = model.SelectedRoles
                .Select(roleId => new UserInRole
                {
                    RoleId = roleId,
                    UserId = res.UserId
                })
                .ToList();

            await _userRepository.SaveChangeAsync();

            return EditUserRoleResult.Success;
        }
        #endregion

        #region GetUserByIdlAsync
        public async Task<EditUserRoleViewModel> GetUserByIdlAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdlAsync(userId);
            return new EditUserRoleViewModel()
            {
                UserId = userId,
                CreateDate = user.CreateDate,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                isDelete = user.isDelete,
                Mobile = user.Mobile,
                Username = user.Username,

                SelectedRoles = user.userInRoles.Select(u => u.RoleId).ToList(),
            };
        }
        #endregion

        #region CreateUserRoleAsync
        public async Task<CreateUserRoleResult> CreateUserRoleAsync(CreateUserRoleViewModel model)
        {
            var userres = await _userRepository.IsUserExistAsync(model.Username, model.Email.Trim().ToLowerInvariant(), model.Mobile);

            if (userres != null)
            {
                if (userres.Username == model.Username)
                {
                    return CreateUserRoleResult.DuplicateUsername;
                }
                else if (userres.Email == model.Email)
                {
                    return CreateUserRoleResult.DuplicateEmail;
                }
                else if (userres.Mobile == model.Mobile)
                {
                    return CreateUserRoleResult.DuplicateMobile;
                }
            }
            else
            {
                var user = new Users()
                {
                    Email = model.Email.ToLowerInvariant().Trim(),
                    Mobile = model.Mobile,
                    Username = model.Username,
                    ActiveCode = "123",
                    CreateDate = DateTime.Now,
                    isDelete = false,
                };
                user.Password = _passwordHasher.HashPassword(user, model.Password);

                user.userInRoles = model.SelectedRoles
              .Select(roleId => new UserInRole
              {
                  RoleId = roleId
              }).ToList();

                await _userRepository.CreateUserRoleAsync(user);
                await _userRepository.SaveChangeAsync();
                return CreateUserRoleResult.Success;
            }
            return CreateUserRoleResult.Fauiler;
        }
        #endregion

        #region  GetRoles  
        public async Task<List<Role>> GetRoles()
        {
            return await _userRepository.GetRoles();
        }
        #endregion

        #region GetUsersWithRoleAsync
        public async Task<List<UserRoleViewModel>> GetUsersWithRoleAsync()
        {
            var users = await _userRepository.GetUsersWithRoleAsync();
            return users.Select(user => new UserRoleViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                CreateDate = DateTime.Now,
                IsAdmin = user.IsAdmin,
                isDelete = user.isDelete,

                Roles = user.userInRoles.Select(r => r.Role.RoleName).ToList()
            }).ToList();
        }
        #endregion

        #region ChangePasswordAsync
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
            }


            return ChangePassResult.Failure;
        }
        #endregion

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
        public async Task<LoginResultDto> LoginUserAsync(LoginViewModel model)
        {

            var existUser = await _userRepository.GetUserByEmailAsync(model.Email.Trim().ToLowerInvariant());

            if (existUser == null || existUser.isDelete)
            {
                return new LoginResultDto
                {
                    Result = LoginResult.UserNotFound,
                    User = null
                };
            }



            var result = _passwordHasher.VerifyHashedPassword(existUser, existUser.Password, model.Password);

            switch (result)
            {
                case PasswordVerificationResult.Success:
                    return new LoginResultDto
                    {
                        Result = LoginResult.Success,
                        User = existUser
                    };

                case PasswordVerificationResult.SuccessRehashNeeded:
                    existUser.Password = _passwordHasher.HashPassword(existUser, model.Password);
                    await _userRepository.SaveChangeAsync();
                    return new LoginResultDto
                    {
                        Result = LoginResult.Success,
                        User = existUser

                    };

                case PasswordVerificationResult.Failed:
                    return new LoginResultDto
                    {
                        Result = LoginResult.Failure,
                        User = existUser
                    };

                default:
                    return new LoginResultDto
                    {
                        Result = LoginResult.Failure,
                        User = existUser
                    };
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
