using AutoMapper;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Data.Utilities;
using ComparisonShoppingEngineAPI.DTOs.Image;
using ComparisonShoppingEngineAPI.DTOs.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace ComparisonShoppingEngineAPI.Data.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId => int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public async Task<ServiceResponse<GetImageDto>> GetProfileImage()
        {
            ServiceResponse<GetImageDto> service = new ServiceResponse<GetImageDto>();

            try
            {
                if (_context.Images.Any(x => x.UserId == GetUserId && x.ImageType == ImageTypes.Profile))
                {
                    Image image = await _context.Images.FirstAsync(x => x.UserId == GetUserId && x.ImageType == ImageTypes.Profile);
                    Console.WriteLine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + image.ImagePath);
                    GetImageDto imageData = new GetImageDto { ImageData = await File.ReadAllBytesAsync(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + image.ImagePath) };
                    service.Data = imageData;
                    service.Success = true;
                } else
                {
                    service.Message = "Could not find profile image in database!";
                }
            } catch (Exception)
            {
                service.Message = "Unexpected server response.";
            }

            return service;
        }

        public async Task<ServiceResponse<GetUserUpdate>> Update(UserUpdateDto userUpdate)
        {
            ServiceResponse<GetUserUpdate> service = new ServiceResponse<GetUserUpdate>();

            try
            {
                int userId = GetUserId;
                // Update user
                switch((UserUpdateTypes) Enum.Parse(typeof(UserUpdateTypes), userUpdate.UserUpdateType, true))
                {
                    case UserUpdateTypes.Password:
                        service.Success = await ChangePassword(userId, userUpdate.Password);
                        service.Data = new GetUserUpdate { IsPasswordChanged = true };
                        break;
                    case UserUpdateTypes.Image:
                        string imagePath = await UploadImage(userId, userUpdate.Image, userUpdate.ImageType);
                        if (imagePath != null)
                        {
                            service.Message = "Image has been uploaded successfully!";
                            service.Success = true;
                            break;
                        }
                        service.Data = null;
                        service.Message = "Could not upload image!";
                        service.Success = false;
                        break;
                    case UserUpdateTypes.Deactivate:
                        service.Success = await Deactivate(userId);
                        service.Data = new GetUserUpdate { IsDeactivated = true };
                        service.Message = (service.Success) ? null : "Could not deactivate account!";
                        break;
                    case UserUpdateTypes.Gender:
                        service.Success = await ChangeGender(userId, (GenderTypes) Enum.Parse(typeof(GenderTypes), userUpdate.Gender, true));
                        service.Message = (service.Success) ? "Gender has been changed!" : "Could not change gender!";
                        break;
                    default:
                        service.Message = ($"Unrecognized user update {userUpdate.UserUpdateType} type!");
                        break;
                }
            }
            catch (Exception e)
            {
                service.Data = null;
                service.Message = $"We are sorry... Unable to proceed {userUpdate.UserUpdateType.ToUpper()} type action!";
                service.Success = false;
            }

            return service;
        }

        public async Task<ServiceResponse<string>> GetGender()
        {
            ServiceResponse<string> service = new ServiceResponse<string>();

            try
            {
                User foundUser = await _context.Users.Where(x => x.UserId == GetUserId).FirstOrDefaultAsync();
                if (foundUser != null)
                {
                    service.Data = foundUser.Gender.ToString();
                    service.Success = true;
                }
            } catch (Exception)
            {
                service.Message = "Unexpected error from server! Try again later.";
            }
            return service;
        }

        private async Task<bool> ChangeGender(int userId, GenderTypes newGender)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.UserId == userId);
                if (user != null)
                {
                    Console.WriteLine(user.Gender);
                    user.Gender = newGender;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            } catch (Exception e)
            {
                return false;
            }
        }

        private async Task<string> UploadImage(int userId, string imageEncode, string imageType)
        {
            // PI - Profile Image
            Image image = new Image { UserId = userId, ImageName = ($"U{userId}_PI.jpg"), ImageType = ((ImageTypes)Enum.Parse(typeof(ImageTypes), imageType)) };
            try
            {
                var imageDataByteArray = Convert.FromBase64String(imageEncode);

                if (!File.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\UserData"))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\UserData");
                    Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\UserData\\Images");
                }
                else if (!File.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $"\\UserData\\Images"))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\UserData\\Images");
                }

                var filePath = (Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $"\\UserData\\Images\\{userId}\\U{userId}_PI.jpg");

                Console.WriteLine("Value: "+File.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $"\\UserData\\Images\\{userId}"));

                if (File.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $"\\UserData\\Images\\{userId}"))
                {
                    File.WriteAllBytes(filePath, imageDataByteArray);
                    image.ImagePath = $"\\UserData\\Images\\{userId}\\U{userId}_PI.jpg";
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + $"\\UserData\\Images\\{userId}");
                    File.WriteAllBytes(filePath, imageDataByteArray);
                    image.ImagePath = $"\\UserData\\Images\\{userId}\\U{userId}_PI.jpg";
                }

                if (await _context.Images.AnyAsync(x => x.UserId == GetUserId && x.ImageType == ImageTypes.Profile))
                {
                    Image obj = await _context.Images.FirstOrDefaultAsync(x => x.UserId == GetUserId && x.ImageType == ImageTypes.Profile);
                    image.ImageId = obj.ImageId;
                    _context.Images.Update(image);
                }
                else
                {
                    _context.Images.Add(image);
                }

                await _context.SaveChangesAsync();
            } catch (InvalidOperationException e)
            {

            } catch (Exception e)
            {
                image.ImagePath = null;
            }
            return image.ImagePath;
        }

        private async Task<bool> ChangePassword(int userId, string newPassword)
        {
            User user = await _context.Users.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            if (user == null) return false;

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            } catch (Exception)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> Deactivate(int userId)
        {
            User user = await _context.Users.Where(x => x.UserId.Equals(userId)).FirstOrDefaultAsync();

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
