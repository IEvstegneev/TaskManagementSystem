using TaskManagementSystem.Core.Domain;

namespace TaskManagementSystem.DataAccess.Data
{
    internal class DataFactory
    {
        internal static IEnumerable<IssueNode> GetPreconfiguredIssues()
        {
            var list = new List<IssueNode>();
            list.Add(new IssueNode(
               id: Guid.Parse("19f1c4a7-aecd-4e0b-ad27-a3e0f5740664"),
               title: "Основная задача 1",
               description: "Описание задачи",
               performers: "Исполнитель И.О",
               estimatedLaborCost: TimeSpan.FromHours(10),
               parentId: null));

            list.Add(new IssueNode(
               id: Guid.Parse("9d2e6c1f-8185-4136-81bf-6c8d5bce1ae0"),
               title: "Подзадача основной задачи 1.1",
               description: "Описание подзадачи основной задачи",
               performers: "Исполнитель И.О",
               estimatedLaborCost: TimeSpan.FromHours(10),
               parentId: Guid.Parse("19f1c4a7-aecd-4e0b-ad27-a3e0f5740664")));

            list.Add(new IssueNode(
               title: "Ещё подзадачка к подзадачке основной задачи 1.1.1",
               description: "Описание  подзадачки подзадачи основной задачи",
               performers: "Исполнитель И.О",
               estimatedLaborCost: TimeSpan.FromHours(10),
               parentId: Guid.Parse("9d2e6c1f-8185-4136-81bf-6c8d5bce1ae0")));
            list.Add(new IssueNode(
               title: "Ещё подзадачка к подзадачке основной задачи 1.1.2",
               description: "Описание подзадачки подзадачи основной задачи",
               performers: "Исполнитель И.О",
               estimatedLaborCost: TimeSpan.FromHours(10),
               parentId: Guid.Parse("9d2e6c1f-8185-4136-81bf-6c8d5bce1ae0")));

            list.Add(new IssueNode(
               title: "Подзадача основной задачи 1.2",
               description: "Описание второй подзадачи основной задачи",
               performers: "Исполнитель И.О",
               estimatedLaborCost: TimeSpan.FromHours(10),
               parentId: Guid.Parse("19f1c4a7-aecd-4e0b-ad27-a3e0f5740664")));


            list.Add(new IssueNode(
               title: "Основная задача 2",
               description: "Описание основной задачи 2",
               performers: "Первый И.О, Второй И.О",
               estimatedLaborCost: TimeSpan.FromHours(5)));
            list.Add(new IssueNode(
               title: "Основная задача 3",
               description: "Описание основной задачи 3",
               performers: "Первый И.О, Второй И.О, Третий И.О.",
               estimatedLaborCost: TimeSpan.FromHours(5)));

            return list;
        }
    }
}