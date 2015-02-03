
namespace Anycmd.Edi.ViewModels.InfoConstraintViewModels
{
    using Engine.Edi;
    using System;

    // TODO:使用InfoRuleState来组合IInfoRule和CreateOn和IsEnabled
    /// <summary>
    /// 
    /// </summary>
    public class InfoRuleTr
    {
        private InfoRuleTr() { }

        public static InfoRuleTr Create(IAcDomain acDomain, InfoRuleState infoRule)
        {
            return new InfoRuleTr
            {
                Id = infoRule.Id,
                Author = infoRule.InfoRule.Author,
                Description = infoRule.InfoRule.Description,
                FullName = infoRule.InfoRule.GetType().Name,
                Name = infoRule.InfoRule.Name,
                Title = infoRule.InfoRule.Title,
                CreateOn = infoRule.CreateOn,
                IsEnabled = infoRule.IsEnabled
            };
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        public int IsEnabled { get; set; }

        public DateTime? CreateOn { get; set; }
    }
}
