using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace GreenOnions.NT.ExportGroupMembers
{
    public static class XmlExtensions
    {
        public static void WriteExcelXml(this DataSet ds, string fileName)
        {
            using FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            ds.WriteExcelXml(fileStream);
        }

        public static void WriteExcelXml(this DataSet ds, Stream stream)
        {
            using XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true });
            writer.WriteStartDocument();
            // 写入根元素及命名空间
            writer.WriteStartElement("Workbook", "urn:schemas-microsoft-com:office:spreadsheet");
            writer.WriteAttributeString("xmlns", "o", null, "urn:schemas-microsoft-com:office:office");
            writer.WriteAttributeString("xmlns", "x", null, "urn:schemas-microsoft-com:office:excel");
            writer.WriteAttributeString("xmlns", "ss", null, "urn:schemas-microsoft-com:office:spreadsheet");
            writer.WriteAttributeString("xmlns", "html", null, "http://www.w3.org/TR/REC-html40");

            foreach (DataTable dt in ds.Tables)
            {
                writer.WriteStartElement("Worksheet");
                writer.WriteAttributeString("ss", "Name", null, dt.TableName.SanitizeSheetName());
                writer.WriteStartElement("Table");

                writer.WriteHeaderRow(dt); // 列头

                writer.WriteDataRows(dt);

                writer.WriteEndElement(); // Table
                writer.WriteEndElement(); // Worksheet
            }

            writer.WriteEndElement(); // Workbook
            writer.WriteEndDocument();
        }

        private static string SanitizeSheetName(this string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return "Sheet";

            Regex invalidCharRegex = new Regex(@"[^\u4e00-\u9fa5a-zA-Z0-9]", RegexOptions.Compiled);

            var sanitized = invalidCharRegex.Replace(originalName, "");

            if (sanitized.Length == 0)
                return "Sheet";

            return sanitized.Length <= 31 ? sanitized : sanitized.Substring(0, 31);
        }

        private static void WriteHeaderRow(this XmlWriter writer, DataTable dt)
        {
            writer.WriteStartElement("Row");
            foreach (DataColumn col in dt.Columns)
            {
                writer.WriteStartElement("Cell");
                writer.WriteStartElement("Data");
                writer.WriteAttributeString("ss", "Type", null, "String");
                writer.WriteString(col.ColumnName);
                writer.WriteEndElement(); // Data
                writer.WriteEndElement(); // Cell
            }
            writer.WriteEndElement(); // Row
        }

        private static void WriteDataRows(this XmlWriter writer, DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                writer.WriteStartElement("Row");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    DataColumn col = dt.Columns[i];
                    object item = row[i];

                    writer.WriteStartElement("Cell");
                    writer.WriteStartElement("Data");

                    string type = "String";
                    string value = "";

                    if (item != DBNull.Value)
                    {
                        Type dataType = col.DataType;
                        value = GetValueByType(item, dataType, out type);
                    }

                    writer.WriteAttributeString("ss", "Type", null, type);
                    writer.WriteString(value.SanitizeXmlString());
                    writer.WriteEndElement(); // Data
                    writer.WriteEndElement(); // Cell
                }
                writer.WriteEndElement(); // Row
            }
        }

        private static string GetValueByType(object? item, Type dataType, out string type)
        {
            type = "String";
            string value;
            if (item is null)
            {
                value = "";
            }
            else if (dataType == typeof(int) || dataType == typeof(decimal) || dataType == typeof(double) ||
                dataType == typeof(float) || dataType == typeof(long) || dataType == typeof(short))
            {
                type = "Number";
                value = Convert.ToString(item, CultureInfo.InvariantCulture) ?? "0";
            }
            else if (dataType == typeof(DateTime))
            {
                type = "DateTime";
                DateTime dtValue = (DateTime)item;
                value = dtValue.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            else if (dataType == typeof(bool))
            {
                type = "Boolean";
                value = (bool)item ? "1" : "0";
            }
            else
            {
                value = item.ToString()!;
            }
            return value;
        }

        private static string SanitizeXmlString(this string value)
        {
            if (string.IsNullOrEmpty(value)) 
                return "";

            var buffer = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                // 根据XML规范过滤非法字符
                if (IsLegalXmlChar(c))
                    buffer.Append(c);
            }
            return buffer.ToString();
        }

        // XML 1.0合法字符检查
        private static bool IsLegalXmlChar(int character)
        {
            return
                character == 0x9 ||          // Tab
                character == 0xA ||          // 换行符
                character == 0xD ||          // 回车
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF);
        }
    }
}
