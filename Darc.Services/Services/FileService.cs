using Darc.Models.Domain;
using Darc.Models.Domain.File;
using Darc.Models.Domain.Requests;
using Darc.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;
using Darc.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using Amazon;
using Darc.Models;
using Microsoft.Extensions.Options;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace Darc.Services.Services
{
    public class FileService : IFileService
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _env;
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest1;
        private readonly string _AWSBucketName;
        private readonly string _AWSAcessKey;
        private readonly string _AWSSecretKey;
        private readonly string _AWSDomain;

        public FileService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _connectionString = configuration.GetConnectionString("Default");
            _AWSBucketName = configuration.GetSection("AppKeys:AWSBucketName").Value;
            _AWSAcessKey = configuration.GetSection("AppKeys:AWSAcessKey").Value;
            _AWSSecretKey = configuration.GetSection("AppKeys:AWSSecretKey").Value;
            _AWSDomain = configuration.GetSection("AppKeys:AWSDomain").Value;
            _env = env;
        }

        public FilesInfo AddFile(FileAddRequest model, int userId)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "[dbo].[Files_InsertFile]";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FileName", model.FileName);
                cmd.Parameters.AddWithValue("@FileUrl", model.FileUrl);
                cmd.Parameters.AddWithValue("@CreatedBy", userId);

                SqlParameter idParam = cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                idParam.Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                FilesInfo addedFiles = new FilesInfo()
                {
                    Id = (int)idParam.Value,
                    FileName = model.FileName,
                    FileUrl = model.FileUrl
                };
                return addedFiles;
            }
        }

        public FilesInfo Upload (IFormFile file, int userId)
        {
            //FilesInfo newFile = new FilesInfo();
            FileAddRequest fileReq = new FileAddRequest();

            var s3Client = new AmazonS3Client(_AWSAcessKey, _AWSSecretKey, bucketRegion);
            var fileTransferUtility = new TransferUtility(s3Client);

            string keyName = "darcsocials/" + Guid.NewGuid() + "/" + file.FileName;
            fileTransferUtility.UploadAsync(file.OpenReadStream(), _AWSBucketName, keyName).Wait();


            //newFile.FileName = file.FileName;
            //newFile.FileUrl = _AWSDomain + keyName;

            fileReq.FileName = file.FileName;
            fileReq.FileUrl = _AWSDomain + keyName;

            var fileToAdd = AddFile(fileReq, userId);

            return fileToAdd;
        }

    }
}
