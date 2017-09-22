using PrLanguages.Expressions;

namespace PrLanguages.Interpreters.VariableManagers
{
    public interface IVariableManager
    {
        dynamic GetValue(string name);
        void SetValue(string name, dynamic value);
        void Declare(string name, dynamic value = null);
        void Reset();
        void SetAllVariables(Expression exp);
    }
}
