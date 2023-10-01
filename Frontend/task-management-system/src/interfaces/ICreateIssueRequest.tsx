export interface ICreateIssueRequest {
    parentId?: string;
    title: string;
    description: string;
    performers: string;
    estimatedLaborCost: number;
}

export interface ITimeSpan {
    ticks: number;
}
