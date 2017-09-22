using System;

namespace ELIZA.Semantics
{
    /// <summary>
    /// Представляет абстрактный слот фреймовой модели.
    /// </summary>
    [Serializable]
    public abstract class AbstractSlot
    {
        public string Name { get; set; }

        /// <summary>
        /// Возникает, когда изменяется значение слота.
        /// </summary>
        public event EventHandler<SlotEventArgs> ValueChanged;
        /// <summary>
        /// Возникает, когда какой-либо фрейм пытается получить значение слота.
        /// </summary>
        public event EventHandler<SlotEventArgs> ValueNeeded;
        /// <summary>
        /// Возникает, когда значение слота удаляется.
        /// </summary>
        public event EventHandler<SlotEventArgs> ValueRemoved;


        /// <summary>
        /// Получает значение слота.
        /// </summary>
        /// <param name="f">Фрейм, которому требуется значение.</param>
        /// <returns>Возвращает значение слота.</returns>
        public abstract object GetValue(Frame f);
        /// <summary>
        /// Устанавливает значение слота.
        /// </summary>
        /// <param name="value">Значение слота.</param>
        /// <param name="f">Фрейм, устанавливающий значение.</param>
        public abstract void SetValue(object value, Frame f);
        /// <summary>
        /// Удаляет значение слота.
        /// </summary>
        /// <param name="f">Фрейм, удаляющий значение.</param>
        public abstract void RemoveValue(Frame f);


        //безопасный вызов событий
        /// <summary>
        /// Вызывается, когда изменяется значение слота..
        /// </summary>
        /// <param name="f">Фрейм, изменяющий значение.</param>
        protected virtual void OnValueChanged(Frame f)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, new SlotEventArgs(f));
            }
        }
        /// <summary>
        /// Вызывается, когда требуется значение слота.
        /// </summary>
        /// <param name="f">Фрейм, пытающийся получить значение.</param>
        protected virtual void OnValueNeeded(Frame f)
        {
            if (ValueNeeded != null)
            {
                ValueNeeded(this, new SlotEventArgs(f));
            }
        }
        /// <summary>
        /// Вызывается, когда удаляется значение слота.
        /// </summary>
        /// <param name="f">Фрейм, удаляющий значение.</param>
        protected virtual void OnValueRemoved(Frame f)
        {
            if (ValueRemoved != null)
            {
                ValueRemoved(this, new SlotEventArgs(f));
            }
        }
    }
}
