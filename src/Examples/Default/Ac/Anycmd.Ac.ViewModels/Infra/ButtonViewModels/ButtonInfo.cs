
namespace Anycmd.Ac.ViewModels.Infra.ButtonViewModels
{
    using Model;
    using System.Collections.Generic;
    using ViewModel;

    public class ButtonInfo : Dictionary<string, object>
    {
        private ButtonInfo() { }

        public static ButtonInfo Create(DicReader dic)
        {
            if (dic == null)
            {
                return null;
            }
            var data = new ButtonInfo();
            foreach (var item in dic)
            {
                data.Add(item.Key, item.Value);
            }
            if (!data.ContainsKey("IsEnabledName"))
            {
                data.Add("IsEnabledName", dic.Host.Translate("Ac", "Button", "IsEnabledName", data["IsEnabled"].ToString()));
            }

            return data;
        }
    }
}
