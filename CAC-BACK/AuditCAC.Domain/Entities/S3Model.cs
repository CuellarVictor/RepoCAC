using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    public class S3Model
    {
        public S3Model()
        {

        }

        public string S3AccessKeyId { get; set; }
        public string S3SecretAccessKey { get; set; }
        public string S3RegionEndpoint { get; set; }
        public string S3BucketName { get; set; }
        public string S3StorageClass { get; set; }
        public string UrlFileS3Uploaded { get; set; }
    }
}
