import { IIssueItem } from "./IIssueItem";
import { IssueStatus } from "./IssueStatus";

export interface IIssue {
    id: string;
    title: string;
    description: string;
    performers: string;
    status: IssueStatus;
    createdAt: string;
    finishedAt?: string;
    estimatedLaborCost: string;
    actualLaborCost: string;
    children?: IIssueItem[];
    canStart: boolean;
    canStop: boolean;
    canFinish: boolean;
}


