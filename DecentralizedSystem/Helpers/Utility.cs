using AutoMapper;
using Dapper;
using DecentralizedSystem.Consts;
using DecentralizedSystem.Core.Configuration;
using DecentralizedSystem.Models.Account;
using DecentralizedSystem.Models.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;

namespace DecentralizedSystem.Helpers
{
    public static class Utility
    {
        public static T DeserializeObject<T>(this string str)
        {
            if (str == null)
                str = string.Empty;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        public static string SerializeObject(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static IEnumerable<IDictionary<string, object>> ToDict(this IEnumerable<dynamic> datas)
        {
            return datas.Select(s => (IDictionary<string, object>)s);
        }
        public static bool IsPropExist(dynamic obj, string property)
        {
            return ((Type)obj.GetType()).GetProperties().Where(p => p.Name.Equals(property)).Any();
        }
        public static void RegisterTypeMaps()
        {
            var mappedTypes = Assembly.GetAssembly(typeof(Initiator)).GetTypes().Where(
                f =>
                f.GetProperties().Any(
                    p =>
                    p.GetCustomAttributes(false).Any(
                        a => a.GetType().Name == ColumnAttributeTypeMapper<dynamic>.ColumnAttributeName)));

            var mapper = typeof(ColumnAttributeTypeMapper<>);
            foreach (var mappedType in mappedTypes)
            {
                var genericType = mapper.MakeGenericType(new[] { mappedType });
                SqlMapper.SetTypeMap(mappedType, Activator.CreateInstance(genericType) as SqlMapper.ITypeMap);
            }
        }
        public static T MapProp<T>(this object item)
        {
            return item.SerializeObject().DeserializeObject<T>();
        }
        public static D MapProp<T, D>(this T item)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, D>();
            });

            var mapper = configuration.CreateMapper();
            return mapper.Map<T, D>(item);
        }

        public static T LowerCaseField<T>(this object item)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            var json = JsonConvert.SerializeObject(item, Formatting.Indented, settings);
            return json.DeserializeObject<T>();
        }

        public static T GetValue<T>(this object item, string property)
        {
            Dictionary<string, object> obj;
            if (item is string)
                obj = ((string)item).DeserializeObject<Dictionary<string, object>>();
            else
            {
                var json = JsonConvert.SerializeObject(item);
                obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            if (obj.ContainsKey(property))
                return obj[property].ToString().DeserializeObject<T>();
            return default(T);
        }
        public static string GetValue(this object item, string property)
        {
            Dictionary<string, object> obj;
            if (item is string)
                obj = ((string)item).DeserializeObject<Dictionary<string, object>>();
            else
            {
                var json = JsonConvert.SerializeObject(item);
                obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            if (obj.ContainsKey(property))
                return obj[property].ToString();
            return null;
        }

        public static IEnumerable<GroupResult<TElement>> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            params Func<TElement, object>[] groupSelectors)
        {
            if (groupSelectors.Length > 0)
            {
                var selector = groupSelectors.First();

                //reduce the list recursively until zero
                var nextSelectors = groupSelectors.Skip(1).ToArray();
                return
                    elements.GroupBy(selector).Select(
                        g => new GroupResult<TElement>
                        {
                            Key = g.Key,
                            Count = g.Count(),
                            Item = g,
                            SubGroups = g.GroupByMany(nextSelectors)
                        });
            }
            return null;
        }
        public static SessionModel GetSession(this ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Sid).Value;
            var userName = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier).Value.ToUpper();
            var userFullName = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Name).Value;
            var userEmail = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email).Value;
            var deptId = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.DEPARTMENT_ID).Value;
            var deptCode = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.DEPARTMENT_CODE).Value;
            var deptName = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.DEPARTMENT_NAME).Value;
            var branchId = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_ID).Value;
            var branchCode = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_CODE).Value;
            var branchName = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_NAME).Value;
            var branchAddress = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_ADDRESS).Value;
            var branchCodeFCC = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_CODE_FCC).Value;
            var branchNameFCC = user.Claims.FirstOrDefault(f => f.Type == ClaimTypesCustom.BRANCH_NAME_FCC).Value;
            var roles = user.Claims.Where(f => f.Type == ClaimTypes.Role).Select(s => s.Value).ToList();

            return new SessionModel
            {
                UserId = userName, //userId,
                UserName = userName,
                UserFullName = userFullName,
                DeptpartmentId = deptId,
                DeptpartmentCode = deptCode,
                DeptpartmentName = deptName,
                BranchId = branchId,
                BranchCode = branchCode,
                BranchName = branchName,
                BranchAddress = branchAddress,
                Roles = roles,
                BranchCodeFCC = branchCodeFCC,
                BranchNameFCC = branchNameFCC
            };
        }

        public static string GetLetterValue(int pos)
        {
            int letterPos = 65 + pos;

            string letter = ((char)letterPos).ToString();

            return letter.ToLower();
        }


        public static string GetLetterRange(string letter, int range)
        {
            char s = letter.ToCharArray()[0];

            int index = char.ToUpper(s) + range;

            string letterDes = ((char)index).ToString();

            return letterDes;
        }


        public static string NumberToTextValue(this decimal inputNumber, string ccy)
        {
            var result = "";

            var isNegative = false;

            try
            {
                string currencyName;
                var fractionalName = "cent";

                switch (ccy?.Substring(0, 3))
                {
                    case "VND":
                        currencyName = "đồng";
                        fractionalName = "";
                        break;
                    case "USD":
                        currencyName = "đô la Mỹ";
                        break;
                    case "XAU":
                        currencyName = "chỉ vàng SJC ";
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

                // -12345678.3445435 => "-12345678"
                var sNumber = split[0]; //Số nguyên

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

                if (isFloat)
                {
                    result += result?.Length > 0 && ccy?.Substring(0, 3) != "XAU" ? " và " : "";
                    result += $"{ConvertNumToTextInternal(split[1])?.TrimEnd()} {fractionalName}";
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

        /// <summary>
        /// Format currency to thousand separator (if any)
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static string FormatThousandSeparator(this decimal currency)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            return currency == 0 ? "-" : currency.ToString("#,##0.##", cul.NumberFormat);
        }

        public static string NumberToTextVN(decimal total)
        {
            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();

                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = System.Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }

                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " ";//" đồng";
                else
                    rs += "";//"đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười");
            }
            catch
            {
                return "";
            }

        }
    }
    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
