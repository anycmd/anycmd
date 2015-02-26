
namespace Anycmd.Ac.ViewModels
{
    using Engine.Ac.Dsd;
    using Engine.Ac.Groups;
    using Engine.Ac.Roles;
    using Engine.Ac.Ssd;
    using System.Collections.Generic;

    public static class ModelExtension
    {
        public static Dictionary<string, object> ToTableRowData(this IDsdSet model)
        {
            if (model == null)
            {
                return null;
            }
            return new Dictionary<string, object>
            {
                { "DsdCard", model.DsdCard },
                { "CreateOn", model.CreateOn },
                { "Id", model.Id },
                { "IsEnabled", model.IsEnabled },
                { "Name", model.Name }
            };
        }

        public static Dictionary<string, object> ToTableRowData(this ISsdSet model)
        {
            if (model == null)
            {
                return null;
            }
            return new Dictionary<string, object>
            {
                {"SsdCard",model.SsdCard},
                {"CreateOn",model.CreateOn},
                {"Id",model.Id},
                {"IsEnabled",model.IsEnabled},
                {"Name",model.Name}
            };
        }

        public static Dictionary<string, object> ToTableRowData(this IGroup model)
        {
            if (model == null)
            {
                return null;
            }
            return new Dictionary<string, object>
            {
                {"Id", model.Id},
                {"CategoryCode", model.CategoryCode},
                {"Name", model.Name},
                {"SortCode", model.SortCode},
                {"IsEnabled", model.IsEnabled},
                {"CreateOn", model.CreateOn}
            };
        }

        public static Dictionary<string, object> ToTableRowData(this IRole model)
        {
            if (model == null)
            {
                return null;
            }
            return new Dictionary<string, object>
            {
                {"CategoryCode", model.CategoryCode},
                {"CreateOn", model.CreateOn},
                {"Icon", model.Icon},
                {"Id", model.Id},
                {"IsEnabled", model.IsEnabled},
                {"Name", model.Name},
                {"SortCode", model.SortCode}
            };
        }
    }
}
