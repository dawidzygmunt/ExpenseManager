using ExpensesManager.Application.Commands;
using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Application.Factories;

public interface IExpenseFactory
{
    Expense Create(AddExpenseCommand command);
}
