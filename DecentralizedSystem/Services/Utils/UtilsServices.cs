using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DecentralizedSystem.Services.Utils
{
    public static class UtilsServices
    {
        public static string EncryptData(this string textData, string Encryptionkey)
        {
            var rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.KeySize = 128;
            rijndaelManaged.BlockSize = 128;
            var bytes1 = Encoding.UTF8.GetBytes(Encryptionkey);
            var numArray = new byte[16];
            var length1 = bytes1.Length;
            if (length1 > numArray.Length)
                length1 = numArray.Length;
            Array.Copy(bytes1, numArray, length1);
            rijndaelManaged.Key = numArray;
            rijndaelManaged.IV = numArray;
            var encryptor = rijndaelManaged.CreateEncryptor();
            var bytes2 = Encoding.UTF8.GetBytes(textData);
            var inputBuffer = bytes2;
            var length2 = bytes2.Length;
            return Convert.ToBase64String(encryptor.TransformFinalBlock(inputBuffer, 0, length2));
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }

        public static bool IsDateInRange(this DateTime dateToCheck, DateTime fromDate, DateTime toDate)
        {
            return dateToCheck >= fromDate.Add(new TimeSpan(00, 00, 00)) &&
                   dateToCheck <= toDate.Add(new TimeSpan(23, 59, 59));
        }

        public static string Encrypt(string key, string data)
        {
            string encData = null;
            var keys = GetHashKeys(key);

            try
            {
                encData = EncryptStringToBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException)
            {
            }
            catch (ArgumentNullException)
            {
            }

            return encData;
        }

        public static string Decrypt(string key, string data)
        {
            string decData = null;
            var keys = GetHashKeys(key);

            try
            {
                decData = DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException)
            {
            }
            catch (ArgumentNullException)
            {
            }

            return decData;
        }

        private static byte[][] GetHashKeys(string key)
        {
            var result = new byte[2][];
            var enc = Encoding.UTF8;

            SHA256 sha2 = new SHA256CryptoServiceProvider();

            var rawKey = enc.GetBytes(key);
            var rawIV = enc.GetBytes(key);

            var hashKey = sha2.ComputeHash(rawKey);
            var hashIV = sha2.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            result[0] = hashKey;
            result[1] = hashIV;

            return result;
        }

        private static string EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt =
                        new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            var cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt =
                        new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static string NumberToTextValue(this decimal inputNumber, string ccy)
        {
            var result = "";
            
            var isNegative = false;

            try
            {
                string currencyName;
                var fractionalName = "cent";
                var goldUnit = "";

                switch (ccy?.Substring(0,3))
                {
                    case "VND":
                        currencyName = "đồng";
                        fractionalName = "";
                        break;
                    case "USD":
                        currencyName = "đô la Mỹ";
                        break;
                    case "XAU":
                        if (inputNumber >= 1)
                            currencyName = "chỉ vàng SJC";
                        else
                            currencyName = "phân vàng SJC";
                        fractionalName = "phân";
                        break;
                    case "EUR":
                        currencyName = "euro";
                        break;
                    case "NZD":
                        currencyName = "đô la New Zealand";
                        break;
                    case "GBP":
                        currencyName = "bảng Anh";
                        break;
                    case "AUD":
                        currencyName = "đô la Úc";
                        break;
                    case "THB":
                        currencyName = "baht Thái";
                        break;
                    case "CAD":
                        currencyName = "đô la Canada";
                        break;
                    case "HKD":
                        currencyName = "đô la Hongkong";
                        break;
                    case "JPY":
                        currencyName = "yên Nhật";
                        break;
                    case "SGD":
                        currencyName = "đô la Singapore";
                        break;
                    case "CNY":
                        currencyName = "nhân dân tệ";
                        break;
                    case "TWD":
                        currencyName = "tân đài tệ";
                        break;
                    case "CHF":
                        currencyName = "franc Thụy Sĩ";
                        break;
                    case "WON":
                        currencyName = "won Hàn Quốc";
                        break;
                    case "NOK":
                        currencyName = "krone Na Uy";
                        break;
                    case "SEK":
                        currencyName = "krona Thụy Điển";
                        break;
                    default:
                        currencyName = "";
                        break;
                }

                var split = inputNumber.ToString(CultureInfo.InvariantCulture).Split('.');
                bool isFloat = split.Length > 1;
                if (ccy?.Substring(0, 3) == "XAU" && split[0] != "0" && split.Length == 2)
                {
                    currencyName = "chỉ lẻ ";
                    goldUnit = "vàng SJC";
                }
                // -12345678.3445435 => "-12345678"

                var sNumber = split[0]; //Số nguyên
                
                if(ccy?.Substring(0, 3) == "XAU" && split[0] == "0")
                {
                    sNumber = split[1];
                }

                var number = Convert.ToDouble(!string.IsNullOrEmpty(sNumber) ? sNumber : "0");

                if (number < 0)
                {
                    number = -number;
                    sNumber = number.ToString(CultureInfo.InvariantCulture);
                    isNegative = true;
                }

                if (Math.Abs(number) > 0)
                {
                    result = ConvertNumToTextInternal(sNumber);
                    //$"{result} {currencyName}" words = words?.length > 0 ? `${words} ${currency_name}` : '';
                    result = result?.Length > 0 ? $"{result?.TrimEnd()} {currencyName}" : string.Empty;
                }
                else
                {
                    result = $"Không {currencyName}";
                }

                if (isFloat && !(ccy?.Substring(0, 3) == "XAU" && split[0] == "0"))
                {
                    result += result?.Length > 0 && ccy?.Substring(0, 3) != "XAU" ? " và " : "";
                    result += $"{ConvertNumToTextInternal(split[1])?.TrimEnd()} {fractionalName} {goldUnit}";
                }

                result = result?.Trim()?.TrimEnd();
                if (isNegative) result = "Âm " + result;
                return $"{FirstCharToUpper(result)}./.";
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Có lỗi xảy ra trong quá trình chuyển số thành chữ");
            }
        }

        private static string ConvertNumToTextInternal(string inputNumber)
        {
            var result = string.Empty;
            int ones, tens, hundreds;
            string[] unitNumbers = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = { "", "nghìn", "triệu", "tỷ" };
            //var sNumber = Convert.ToString(inputNumber);

            var positionDigit = inputNumber.Length; // last -> first
            
            if (positionDigit == 0)
            {
                result = unitNumbers[0] + result;
            }
            else
            {
                var placeValue = 0;

                while (positionDigit > 0)
                {
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(inputNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(inputNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(inputNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if (ones > 0 || tens > 0 || hundreds > 0 || placeValue == 3)
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if (ones == 1 && tens > 1)
                    {
                        result = "mốt " + result;
                    }
                    else
                    {
                        if (ones == 5 && tens > 0)
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }

                    if (tens < 0) break;

                    if (tens == 0 && ones > 0) result = "lẻ " + result;
                    if (tens == 1) result = "mười " + result;
                    if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    if (hundreds < 0)
                    {
                        break;
                    }

                    if (hundreds > 0 || tens > 0 || ones > 0)
                        result = unitNumbers[hundreds] + " trăm " + result;
                    result = " " + result;
                }
            }

            return result;
        }
        public static string FirstCharToUpper(string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input[0].ToString().ToUpper() + input.Substring(1)
            };
        }

        public static DateTime ParseDate(string date)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            var formatStrings = new string[] { "dd/MM/yyyy", "dd/MM/yyyy hh:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "MM/dd/yyyy hh:mm:ss tt", "yyyy-MM-dd hh:mm:ss", "MM/dd/yyyy", "MMM/dd/yyyy", "dd/MM/yy", "dd/MMM/yyyy", "dd-MM-yyyy", "dd-MMM-yy", "MM-dd-yyyy", "yyyy-MM-dd", "yyyy/MM/dd", "yyyy-MMM-dd", "yyyy/MMM/dd" };
            if (DateTime.TryParseExact(date, formatStrings, enUS, DateTimeStyles.None, out var dateValue))
                return dateValue;
            throw new ArgumentException($"Không xác định được định dạng ngày: {date}");
        }

        private static bool IsFloat(double number)
        {
            if (Decimal.TryParse(number.ToString(CultureInfo.InvariantCulture), out _))
                return true;
            return false;
        }
    }

    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters is null) return;
            var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
            //foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
            //{
            //    var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
            //    var routeInfo = description.RouteInfo;

            //    if (parameter.Description == null)
            //    {
            //        parameter.Description = description.ModelMetadata?.Description;
            //    }

            //    if (routeInfo == null)
            //    {
            //        continue;
            //    }

            //    if (parameter.Default == null)
            //    {
            //        parameter.Default = routeInfo.DefaultValue;
            //    }

            //    parameter.Required |= !routeInfo.IsOptional;
            //}
        }
    }

    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}