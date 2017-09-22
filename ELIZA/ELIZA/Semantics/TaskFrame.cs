using System;
using System.Collections.Generic;
using System.Linq;
using ELIZA.Semantics.Patterns;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;
using OGESolver.Factories;

namespace ELIZA.Semantics
{
    [Serializable]
    public class TaskFrame: Frame
    {
        protected int commonSlots;

        public TaskFrame(string name = "") : base(name)
        {
            Slots.Add(new Slot<List<ComplexPattern>>(new List<ComplexPattern>(), "Patterns")); //список паттернов
            Slots.Add(new Slot<object[]>(null, "Params")); //список параметров
            Slots.Last().ValueNeeded += ParamsValueNeeded;
            Slots.Add(new SlotWithDefaultValue<bool>(false, "Match")); //слот, показывающий соотвествие одному из паттернов
            Slots.Last().ValueNeeded += MatchValueNeeded;
            Slots.Add(new SlotWithDefaultValue<ComplexPattern>(null, "MatchingPattern")); //паттерн, которому соотвествует вход
            //слот, в котором хранится входная информация
            Slots.Add(new SlotWithDefaultValue<List<Tree<DForm,
                DeepRelationName>>>(new List<Tree<DForm, DeepRelationName>>(), "Input"));
            //слот с объяснениями задачи
            Slots.Add(new SlotWithDefaultValue<string>(string.Empty, "Explanations"));
            Slots.Last().ValueNeeded += ExplanationsValueNeeded;
            commonSlots = Slots.Count;
        }
        protected void ExplanationsValueNeeded(object sender, SlotEventArgs e)
        {
            if (this["Match"].GetValue(this).Equals(true))
            {
                this["Params"].SetValue(GetParams(), this);
                AlgorithmFactory.Execute(Name, (object[])this["Params"].GetValue(this));
                this["Explanations"].SetValue(AlgorithmFactory.GetDescription(), this);
            }            
        }
        protected void MatchValueNeeded(object sender, SlotEventArgs e)
        {
            ClearOtherSlots();
            //получаем значение слота "Patterns"
            var patterns = (List<ComplexPattern>) this["Patterns"].GetValue(this);
            var input = (List<Tree<DForm, DeepRelationName>>) this["Input"].GetValue(this);
            this["Input"].RemoveValue(this); //значение не нужно
            foreach (var pattern in patterns)
            {
                if (pattern.Match(input))
                {
                    this["MatchingPattern"].SetValue(pattern, this);
                    this["Match"].SetValue(true, this);
                    foreach (var slot in Slots)
                    {
                        var outValue = string.Empty;
                        if (pattern.TryGetSavedValue(slot.Name, out outValue))
                            slot.SetValue(outValue, this);
                    }
                }
            }
        }
        protected void ClearOtherSlots()
        {
            for (var i = commonSlots; i < slots.Count; i++)
            {
                slots[i].RemoveValue(this);
            }
            this["Match"].RemoveValue(this); //значение пока не определено
            this["MatchingPattern"].RemoveValue(this); //значение пока не определено
            this["Explanations"].RemoveValue(this); //значение пока не определено
            this["Params"].RemoveValue(this); //значение пока не определено
        }
        protected void ParamsValueNeeded(object sender, SlotEventArgs e)
        {
            //когда требуется значение слота "Params"
            //проверяется значение слота "Match"
        }
        protected object[] GetParams()
        {
            var result = new object[Slots.Count - commonSlots];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Slots[commonSlots + i].GetValue(this);
            }
            return result;
        }
    }
}
