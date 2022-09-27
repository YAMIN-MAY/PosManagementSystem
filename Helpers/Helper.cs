using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NetTestSolution.Domain.Models;
using Newtonsoft.Json;
using QRCoder;

namespace NetTestSolution.Helpers
{
    public static class Helper
    {

        public static string GenerateQRImage(GenerateQrRequestModel requestModel)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(JsonConvert.SerializeObject(requestModel), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var coloring = Color.FromArgb(71, 170, 136);
            var logoPath = "Images/qr-code.png";
            Bitmap qrCodeImage = qrCode.GetGraphic(50, coloring, Color.White, (Bitmap)Bitmap.FromFile(logoPath), 30, 100, true);
            var InternalImageUrl = "Images/CouponQR/" + Guid.NewGuid() + ".png";
            if (!File.Exists(InternalImageUrl))
            {
                qrCodeImage.Save(InternalImageUrl, ImageFormat.Png);
            }

            return InternalImageUrl;
        }
        public static string GeneratePassword()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            string confirmationcode = BitConverter.ToInt32(buffer, 0).ToString();
            confirmationcode = confirmationcode.Replace("-", "");
            confirmationcode = confirmationcode.Substring(0, 6);

            return confirmationcode;
        }

        public static string SHA256HexHashString(string stringIn)
        {
            string hashString;
            using (var sha256 = SHA256Managed.Create())
            {
                var hash = sha256.ComputeHash(Encoding.Default.GetBytes(stringIn));
                hashString = ToHex(hash, false);
            }

            return hashString;
        }

        public static string ToHex(byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            return result.ToString();
        }


        public static string GenerateMemberQr(GenerateMemberQrRequestModel requestModel)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(JsonConvert.SerializeObject(requestModel), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var coloring = Color.FromArgb(71, 170, 136);
            var logoPath = "Images/qr-code.png";
            Bitmap qrCodeImage = qrCode.GetGraphic(50, coloring, Color.White, (Bitmap)Bitmap.FromFile(logoPath), 30, 100, true);
            var InternalImageUrl = "Images/CouponQR/" + Guid.NewGuid() + ".png";
            if (!File.Exists(InternalImageUrl))
            {
                qrCodeImage.Save(InternalImageUrl, ImageFormat.Png);
            }

            return InternalImageUrl;
        }
    }
}
