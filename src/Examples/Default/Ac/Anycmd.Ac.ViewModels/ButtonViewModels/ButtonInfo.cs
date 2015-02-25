
namespace Anycmd.Ac.ViewModels.ButtonViewModels
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
                data.Add("IsEnabledName", dic.AcDomain.Translate("Ac", "Button", "IsEnabledName", (int)data["IsEnabled"]));
            }

            return data;
        }
    }
}
