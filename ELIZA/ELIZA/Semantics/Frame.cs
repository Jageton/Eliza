using System;
using System.Collections.Generic;
using System.Linq;

namespace ELIZA.Semantics
{
    [Serializable]
    public abstract class Frame
    {
        protected Slot<Frame> aKindfOf;
        protected List<AbstractSlot> slots;

        public string Name { get; set; }

        public AbstractSlot this[string name]
        {
            get
            {
                return slots.FirstOrDefault((slot) => slot.Name == name);                
            }
            set
            {
                for (var i = 0; i < slots.Count; i++)
                {
                    if (slots[i].Name == value.Name)
                        slots[i] = value;
                }
            }
        }

        public object GetValue(string slotName)
        {
            var slot = this[slotName];
            if (slot != null)
            {
                return slot.GetValue(this);
            }
            return null;
        }

        public void SetValue(string slotName, object value)
        {
            var slot = this[slotName];
            if (slot != null)
            {
                slot.SetValue(value, this);
            }
        }

        public Frame AKindOf
        {
            get { return (Frame)aKindfOf.GetValue(this); }
            set
            {
                aKindfOf.SetValue(value, this);
            }
        }

        public List<AbstractSlot> Slots
        {
            get { return slots; }
            set { slots = value; }
        }

        protected Frame()
        {
            aKindfOf = new Slot<Frame>();
            slots = new List<AbstractSlot>();
        }

        protected Frame(string name = ""): base()
        {
            Name = name;
            slots = new List<AbstractSlot>();
        }
    }
}
