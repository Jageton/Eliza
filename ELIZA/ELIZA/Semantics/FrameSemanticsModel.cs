using System;
using System.Collections.Generic;
using ELIZA.Semantics.Patterns;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;
using OGESolver;

namespace ELIZA.Semantics
{
    [Serializable]
    public class FrameSemanticsModel: ISemanticsModel
    {
        protected List<TaskFrame> frames;

        public FrameSemanticsModel()
        {
            frames = new List<TaskFrame>();
            var frame = new TaskFrame();
            frame.Name = "Перевести из одной системы в другую";
            var patterns = new List<ComplexPattern>();
            patterns.Add(Pattern.CreateAny("...переводить - (число-(Число)PROP, A3 - (В)ATTR)"));
            frame["Patterns"].SetValue(patterns, frame);
            frame.Slots.Add(new Slot<string>("", "Число"));
            frame.Slots.Add(new SlotWithConvertibleDefaultValue<ReferenceOf<int>>(10, "Из"));
            frame.Slots.Add(new SlotWithConvertibleDefaultValue<ReferenceOf<int>>(2, "В"));
            frames.Add(frame);
        }

        public FrameSemanticsModel(IEnumerable<TaskFrame> frames)
        {
            this.frames = new List<TaskFrame>(frames);
        }

        public string GetAnswer(List<Tree<DForm, DeepRelationName>> trees)
        {
            foreach (var frame in frames)
            {
                frame.SetValue("Input", trees);
                //если фрейм принимает вход
                string explanations = (string)frame.GetValue("Explanations");
                if (explanations != string.Empty)
                {
                    return explanations;
                }
            }
            return "Ваш запрос не может быть обработан. Попробуйте переформулировать задачу.";
        }
    }
}
