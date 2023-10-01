
export enum IssueStatus {
    Assigned,
    InProgress,
    Stopped,
    Finished
}

export const getDisplayStatusName = (status: IssueStatus) => {
    switch (status) {
        case IssueStatus.Assigned: return "Назначена";
        case IssueStatus.InProgress: return "Выполняется";
        case IssueStatus.Stopped: return "Приостановлена";
        case IssueStatus.Finished: return "Завершена";
    }
}
