using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    internal class Organization
    {
        public string OrganizationName { get; set; }

        public List<string> Emails { get; set; } = new List<string>();

        public List<string> Phones { get; set; } = new List<string>();

        public DateTime TimeOfContact { get; set; }

        public void ParsingLine(string line)
        {
            string[] parts = line.Split(';');
            OrganizationName = parts[0];
            string[] contacts = parts[1].Split(',');
            TimeOfContact = DateTime.Parse(parts[2]);
            foreach (var contact in contacts)
            {
                if (Regex.IsMatch(contact, "[A-Za-z]"))
                {
                    if (!Emails.Contains(contact, StringComparer.OrdinalIgnoreCase))
                    {
                        Emails.Add(contact);
                    }
                }
                else
                {
                    string phone = FormatNumber(contact);
                    if(phone.Length - 1 == 10)
                    {
                        phone = String.Format("{0:# (###) ###-####}", Convert.ToInt64(phone));

                        if (!Phones.Contains(phone))
                            Phones.Add(phone);
                    }
                }
            }
        }

        public static List<Organization> ReadFile(string filePath)
        {
            List<Organization> resultData = new List<Organization>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    Organization model = new Organization();
                    model.ParsingLine(line);
                    int indexOfmodel = resultData.FindIndex(x => x.OrganizationName == model.OrganizationName);

                    if (indexOfmodel == -1)
                        resultData.Add(model);
                    else
                    {
                        foreach (var email in model.Emails)
                        {
                            if (!resultData[indexOfmodel].Emails.Contains(email, StringComparer.OrdinalIgnoreCase))
                                resultData[indexOfmodel].Emails.Add(email);
                        }

                        foreach (var phone in model.Phones)
                        {
                            if (phone.Length - 1 == 10 && !resultData[indexOfmodel].Phones.Contains(phone))
                                resultData[indexOfmodel].Phones.Add(phone);
                        }

                        if (resultData[indexOfmodel].TimeOfContact.CompareTo(model.TimeOfContact) < 0)
                            resultData[indexOfmodel].TimeOfContact = model.TimeOfContact;
                    }
                }
            }
            return resultData;
        }

        public string FormatNumber(string numberPhone)
        {
            numberPhone = numberPhone.Replace("-", "");
            numberPhone = numberPhone.Replace("+7", "8");
            numberPhone = numberPhone.Replace("(", "");
            numberPhone = numberPhone.Replace(")", "");
            numberPhone = numberPhone.Replace(" ", "");
            return numberPhone;
        }

        public static string TranformToString(List<string> line)
        {
            string resultLine = "";
            foreach (var item in line)
            {
                resultLine += item + ",";
            }
            resultLine = resultLine.TrimEnd(',');
            return resultLine;
        }

        public static void WriteFile(List<Organization> modelList, string filePath)
        {
            List<string> lines = new List<string>();
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                foreach(var model in modelList)
                {
                    sw.WriteLine($"{model.OrganizationName};{TranformToString(model.Emails)};{TranformToString(model.Phones)};{model.TimeOfContact}");
                }
                
            }
        }

       
    }
}
