using System.ComponentModel;
using System.Data;

namespace Paradaim.BaseGateway.Infrastructures.Extensions
{
    public static class ListExtensions
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType.Name.ToLower().Contains("null") ? typeof(string) : prop.PropertyType);
            }
            object?[] values = new object?[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static DataTable ToDataTable<T>(this IList<object> data)
        {
            DataTable table = new DataTable();
            if (data.FirstOrDefault() != null)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(data.FirstOrDefault()!.GetType());
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType.Name.ToLower().Contains("null") ? typeof(string) : prop.PropertyType);
                }
                object?[] values = new object?[props.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
            }
            return table;
        }
    }
}
